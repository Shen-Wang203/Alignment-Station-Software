using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;

namespace Beetle
{
    public partial class MainForm : Form
    {
        private Thread runThread;
        private bool stopReadPM = false;
        private bool chartsOn = false;
        private bool tabBC = false; // whether on Beetle Control tab
        private DateTime thisTime, startTime;
        private TimeSpan timeElapsed;

        private FilterInfoCollection filterInfoCollecion;
        private VideoCaptureDevice videoCaptureDevice;

        private BeetleSystemObject bt;
        private BeetleSystemObject beetle1 = new BeetleSystemObject();
        private BeetleSystemObject beetle2;

        public MainForm()
        {
            InitializeComponent();

            bt = beetle1;
            ChartsInit();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            comboBoxBeetleSelect.SelectedIndex = 0;

            // Find all available cameras
            filterInfoCollecion = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterInfo in filterInfoCollecion)
                comboBoxCamSelect.Items.Add(filterInfo.Name);
            if (comboBoxCamSelect.Items.Count != 0)
                comboBoxCamSelect.SelectedIndex = 0; // this will triger comboBoxCamSelect_SelectedIndexChanged function

            bt.parameters.LoadAll();
            if (!bt.Connection())
            {
                buttonCalibration.Enabled = false;
                buttonClearError.Enabled = false;
                //buttonTest.Enabled = false;
                buttonClose.Enabled = false;
                buttonCancleRun.Enabled = false;
                ButtonReset.Enabled = false;
                ButtonAlignment.Enabled = false;
                ButtonPreCuring.Enabled = false;
                ButtonCuring.Enabled = false;
                // disable buttons on stage tab
                buttonSetPosition.Enabled = false;
                buttonSetInitial.Enabled = false;
                buttonClearErrorBC.Enabled = false;
                buttonPiezoReset.Enabled = false;
                buttonPiezoSearch.Enabled = false;
            }

            // comboBox Initial index
            comboBoxProductSelect.SelectedIndex = 0;
            comboBoxUsePiezo.SelectedIndex = 1;
            comboBoxMotorSelectTop.SelectedIndex = 6;
            comboBoxMotorSelectMid.SelectedIndex = 6;
            comboBoxMotorSelectBot.SelectedIndex = 6;
            comboBoxPMChl.SelectedIndex = 0;

            ReloadBeetleInfo();
        }

        private void VideoCaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap pic = (Bitmap)eventArgs.Frame.Clone();
            // Rotate or flip. Other options include Rotate180FlipX, Rotate180FlipXY etc.
            pic.RotateFlip(RotateFlipType.Rotate180FlipX);
            if (tabBC)
                pictureBoxCamBC.Image = pic;
            else
                pictureBoxCam.Image = pic;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
                tabBC = false;
            else
                tabBC = true;
        }

        private void comboBoxCamSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (videoCaptureDevice != null && videoCaptureDevice.IsRunning)
                videoCaptureDevice.Stop();
            videoCaptureDevice = new VideoCaptureDevice(filterInfoCollecion[comboBoxCamSelect.SelectedIndex].MonikerString);

            VideoCapabilities[] videoCap = videoCaptureDevice.VideoCapabilities;
            int maxWidth = 0;
            foreach (var item in videoCap)
            {
                if (item.FrameSize.Width > maxWidth)
                {
                    maxWidth = item.FrameSize.Width;
                    videoCaptureDevice.VideoResolution = item;
                }
            }

            videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;
            videoCaptureDevice.Start();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            bt.parameters.Save();
            if (beetle2 != null)
                beetle2.parameters.Save();
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            if (!tabBC)
            {
                if (bt.parameters.errors != "")
                {
                    richTextBoxErrorMsg.Text += bt.parameters.errors;
                    bt.parameters.errors = "";
                }

                // if thread is running then start the time count on the GUI
                if (runThread != null && runThread.IsAlive)
                {
                    thisTime = DateTime.Now;
                    labelTargetValue.Text = Math.Round(bt.parameters.lossCurrentMax, 4).ToString();
                    labelILValue.Text = Math.Round(PowerMeter.loss, 4).ToString();
                }
                else if (!stopReadPM)
                    labelILValue.Text = PowerMeter.ReadNoPrint().ToString();

                timeElapsed = thisTime - startTime;
                labelTimeValue.Text = timeElapsed.Minutes.ToString() + "min " + timeElapsed.Seconds.ToString() + "s";

                sbyte sum = 0;
                foreach (var i in bt.beetleControl.motorEngageFlag)
                    sum += i;
                if (sum == 0 && !bt.parameters.piezoRunning)
                {
                    labelStatusValue.Text = "IDLE";
                    labelStatusValue.ForeColor = Color.Lime;
                }
                else if (sum == 0 && bt.parameters.piezoRunning)
                {
                    labelStatusValue.Text = "BUSY";
                    labelStatusValue.ForeColor = Color.Yellow;
                }
                else if (sum != 0)
                {
                    labelStatusValue.Text = "BUSY";
                    labelStatusValue.ForeColor = Color.Red;
                }
                

                labelPositionXYZ.Text = Math.Round(bt.parameters.position[0], 3).ToString() + ", " + Math.Round(bt.parameters.position[1], 3).ToString() + 
                                        ", " + Math.Round(bt.parameters.position[2], 3).ToString();
                labelPositionAngles.Text = Math.Round(bt.parameters.position[3], 3).ToString() + ", " + Math.Round(bt.parameters.position[4], 3).ToString() + 
                                        ", " + Math.Round(bt.parameters.position[5], 3).ToString();

                labelPiezoPositionValue.Text = bt.parameters.piezoPosition[0].ToString() + ", " + bt.parameters.piezoPosition[1].ToString() + 
                                                    ", " + bt.parameters.piezoPosition[2].ToString();

                if (chartsOn)
                    ChartsDataUpdate();
            }
            else
            {
                // if thread is running then start the time count on the GUI
                if (runThread != null && runThread.IsAlive)
                {
                    labelILValueBC.Text = Math.Round(PowerMeter.loss, 4).ToString();
                }
                else if (!stopReadPM)
                    labelILValueBC.Text = PowerMeter.ReadNoPrint().ToString();

                sbyte sum = 0;
                foreach (sbyte i in bt.beetleControl.motorEngageFlag)
                    sum += i;
                if (sum == 0)
                {
                    labelStatusValueBC.Text = "IDLE";
                    labelStatusValueBC.ForeColor = Color.Lime;
                }
                else
                {
                    labelStatusValueBC.Text = "BUSY";
                    labelStatusValueBC.ForeColor = Color.Red;
                }
                if (bt.parameters.errorFlag && bt.parameters.errors != "")
                {
                    labelStatusValueBC.Text = "ERROR";
                    labelStatusValueBC.ForeColor = Color.Red;
                }

                if (bt.parameters.piezoRunning)
                {
                    labelStatusValueBC.Text = "BUSY";
                    labelStatusValueBC.ForeColor = Color.Yellow;
                }
                labelPiezoPositionValueBC.Text = bt.parameters.piezoPosition[0].ToString() + ", " + bt.parameters.piezoPosition[1].ToString() +
                                ", " + bt.parameters.piezoPosition[2].ToString();
                numericUpDownX.Value = (decimal)bt.parameters.position[0];
                numericUpDownY.Value = (decimal)bt.parameters.position[1];
                numericUpDownZ.Value = (decimal)bt.parameters.position[2];
                numericUpDownRx.Value = (decimal)bt.parameters.position[3];
                numericUpDownRy.Value = (decimal)bt.parameters.position[4];
                numericUpDownRz.Value = (decimal)bt.parameters.position[5];

                MotorChartsUpdate();
            }

        }

        private void ButtonReset_Click(object sender, EventArgs e)
        {
            if (runThread != null && runThread.IsAlive)
            {
                MessageBox.Show("Another Process Runing");
                return;
            }
            richTextBoxErrorMsg.Text += "Reset\n";
            Parameters.Log(" ");
            Parameters.Log("Reset");
            Parameters.Log(" ");
            Console.WriteLine("Reset");

            ButtonReset.BackColor = Color.Green;
            ButtonAlignment.BackColor = Color.Red;
            ButtonPreCuring.BackColor = Color.Red;
            ButtonCuring.BackColor = Color.Red;
            ButtonAlignment.Enabled = true;

            stopReadPM = false;
            startTime = DateTime.Now;

            bt.piezoControl.Reset();
            Thread.Sleep(500);

            runThread = new Thread(bt.beetleControl.GotoReset);
            runThread.Start();
        }

        private void ButtonAlignment_Click(object sender, EventArgs e)
        {
            if (PowerMeter.loss > -40)
            {
                Parameters.Log("\r\n");
                Parameters.Log("***************************");
                Parameters.Log("Aligment Starts");
                Console.WriteLine("A New Alignemnt Starts");
                richTextBoxErrorMsg.Text += "Alignment Running\n";

                stopReadPM = false;
                startTime = DateTime.Now;

                bt.piezoControl.Reset();
                Thread.Sleep(500);

                if (bt.ba == null)
                    bt.AlignCuringInit();
                runThread = new Thread(bt.ba.AlignmentRun);
                runThread.Start();

                ButtonAlignment.BackColor = Color.Yellow;
                ButtonPreCuring.Enabled = true;
            }
            else
                MessageBox.Show(text: "Make The Loss Lower Than -40dB");
        }

        private void ButtonPreCuring_Click(object sender, EventArgs e)
        {
            if (PowerMeter.loss > -8)
            {
                if (runThread != null && runThread.IsAlive)
                {
                    MessageBox.Show("Another Process Runing");
                    return;
                }

                Parameters.Log("\r\n");
                Parameters.Log("***************************");
                Parameters.Log("PreAligment Starts");
                Console.WriteLine("PreAlignemnt Starts");
                richTextBoxErrorMsg.Text += "PreCuring Running\n";

                stopReadPM = false;
                startTime = DateTime.Now;

                if (bt.ba == null)
                    bt.AlignCuringInit();
                runThread = new Thread(bt.ba.PreCuringRun);
                runThread.Start();

                ButtonAlignment.BackColor = Color.Green;
                ButtonPreCuring.BackColor = Color.Yellow;
                ButtonCuring.Enabled = true;
            }
            else
                MessageBox.Show(text: "Make The Loss Lower Than -8dB");

        }

        private void ButtonCuring_Click(object sender, EventArgs e)
        {
            if (runThread != null && runThread.IsAlive)
            {
                MessageBox.Show("Another Process Runing");
                return;
            }

            Parameters.Log("\r\n");
            Parameters.Log("***************************");
            Parameters.Log("Curing Starts");
            Console.WriteLine("Curing Starts");
            richTextBoxErrorMsg.Text += "Curing Running\n";

            stopReadPM = false;
            startTime = DateTime.Now;

            if (bt.bc == null)
                bt.AlignCuringInit();
            runThread = new Thread(bt.bc.Run);
            runThread.Start();

            ButtonPreCuring.BackColor = Color.Green;
            ButtonCuring.BackColor = Color.Yellow;
            ButtonCuring.Enabled = true;
        }

        private void ButtonClearError_Click(object sender, EventArgs e)
        {
            bt.beetleControl.ClearErrors();
            richTextBoxErrorMsg.Text += "Errors are Cleared\n";
        }

        private void ButtonCalibration_Click(object sender, EventArgs e)
        {
            if (runThread != null && runThread.IsAlive)
            {
                MessageBox.Show("Another Process Runing");
                return;
            }

            startTime = DateTime.Now;

            runThread = new Thread(bt.beetleControl.Calibration);
            runThread.Start();
            richTextBoxErrorMsg.Text += "Calibrating\n";
        }

        private void IL_Click(object sender, EventArgs e)
        {
            PowerMeter.Read();
            labelILValue.Text = PowerMeter.loss.ToString();
        }

        private void ControlBoxDetection_Click(object sender, EventArgs e)
        {
            if (bt.Detect())
            {
                bt.parameters.SaveCOMPorts();
                richTextBoxErrorMsg.Text += "COM Ports Saved\n";
            }
            if (bt.Connection())
            {
                buttonCalibration.Enabled = true;
                buttonClearError.Enabled = true;
                buttonTest.Enabled = true;
                buttonClose.Enabled = true;
                buttonCancleRun.Enabled = true;
                ButtonReset.Enabled = true;
                ButtonAlignment.Enabled = true;
                ButtonPreCuring.Enabled = true;
                ButtonCuring.Enabled = true;
                // disable buttons on stage tab
                buttonSetPosition.Enabled = true;
                buttonSetInitial.Enabled = true;
                buttonClearErrorBC.Enabled = true;
                buttonPiezoReset.Enabled = true;
                buttonPiezoSearch.Enabled = true;
            }
        }

        private void buttonReference_Click(object sender, EventArgs e)
        {
            PowerMeter.lossReference = PowerMeter.Read("dBm");
            richTextBoxErrorMsg.Text += "Reference Set\n";
            bt.parameters.SaveReference();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            if (runThread != null && runThread.IsAlive)
            {
                MessageBox.Show("Another Process Runing");
                return;
            }

            startTime = DateTime.Now;
            runThread = new Thread(bt.beetleControl.GotoClose);
            runThread.Start();
            richTextBoxErrorMsg.Text += "Closing\n";
        }

        private void buttonCancleRun_Click(object sender, EventArgs e)
        {
            // Abort function can cause unexpected errors, so avoid using it
            //runThread.Abort();
            bt.parameters.errorFlag = true; // set errorFlag to let thread finish
            richTextBoxErrorMsg.Text += "Run Cancelling\n";
            while (runThread.IsAlive)
            {; }
            bt.beetleControl.DisengageMotors();
            bt.beetleControl.NormalTrajSpeed();
            bt.parameters.errorFlag = false;
            richTextBoxErrorMsg.Text += "Run Cancelled\n";
            Parameters.Log("Run Cancelled");
            Console.WriteLine("Run Cancelled");
        }

        private void buttonDisconnectPM_Click(object sender, EventArgs e)
        {
            stopReadPM = true;
            richTextBoxErrorMsg.Text += "PM Disconnected\n";
        }

        private void buttonConnectPM_Click(object sender, EventArgs e)
        {
            stopReadPM = false;
            richTextBoxErrorMsg.Text += "PM Connected\n";
        }

        private void Test_Click(object sender, EventArgs e)
        {
            double[] pp = new double[6];
            bt.parameters.position.CopyTo(pp, 0);
            pp[2] += 4;
            bt.beetleControl.GotoPosition(pp, mode: 't', checkOnTarget: false);
        }

        private void comboBoxProductSelect_SelectedIndexChanged(object sender, EventArgs e) => bt.parameters.productName = comboBoxProductSelect.Text;

        private void comboBoxUsePiezo_SelectedIndexChanged(object sender, EventArgs e) => bt.parameters.usePiezo = comboBoxUsePiezo.SelectedIndex == 0;

        private void buttonChartOnOff_Click(object sender, EventArgs e)
        {
            chartsOn = !chartsOn;
            if (chartsOn)
                buttonChartOnOff.Text = "Live Chart On";
            else
                buttonChartOnOff.Text = "Live Chart Off";
        }

        private void buttonSetPosition_Click(object sender, EventArgs e)
        {
            if (runThread != null && runThread.IsAlive)
            {
                MessageBox.Show("Another Process Runing");
                return;
            }

            bt.beetleControl.tempP = new double[6] { (double)numericUpDownX.Value, (double)numericUpDownY.Value, (double)numericUpDownZ.Value, 
                    (double)numericUpDownRx.Value, (double)numericUpDownRy.Value, (double)numericUpDownRz.Value };
            runThread = new Thread(bt.beetleControl.GotoTemp);
            runThread.Start();
        }

        // The pivot point Z input value should be the distance between pivot point to top moving part top surface
        private void buttonSetPivot_Click(object sender, EventArgs e)
        {
            double[] p = { (double)numericUpDownPx.Value, (double)numericUpDownPy.Value, (double)numericUpDownPz.Value, 0 };
            //p.CopyTo(Parameters.pivotPoint, 0);
            bt.mathModel.SetPivotPoint = p;
            bt.parameters.SavePivotPoint();
            MessageBox.Show("Pivot Point Saved");
        }

        private void buttonSetInitial_Click(object sender, EventArgs e)
        {
            bt.parameters.position.CopyTo(bt.parameters.initialPosition, 0);
            bt.parameters.SaveInitialPosition();
            MessageBox.Show("Initial Position Saved");
        }

        private void buttonClearErrorBC_Click(object sender, EventArgs e)
        {
            bt.beetleControl.ClearErrors();
            Console.WriteLine("Errors Cleared");
            //BeetleControl.NormalTrajSpeed();
            //richTextBoxErrorMsg.Text += "Errors are Cleared\n";
        }

        private void buttonPiezoReset_Click(object sender, EventArgs e) => bt.piezoControl.Reset();

        private void buttonPiezoSearch_Click(object sender, EventArgs e)
        {
            if (runThread != null && runThread.IsAlive)
            {
                MessageBox.Show("Another Process Runing");
                return;
            }

            runThread = new Thread(bt.ba.PiezoSearchRun);
            runThread.Start();
        }

        private void comboBoxPiezoStep_SelectedIndexChanged(object sender, EventArgs e)=> bt.parameters.piezoStepSize = ushort.Parse(comboBoxPiezoStep.SelectedItem.ToString());

        private void richTextBoxErrorMsg_TextChanged(object sender, EventArgs e)
        {
            // set the current caret position to the end
            richTextBoxErrorMsg.SelectionStart = richTextBoxErrorMsg.Text.Length;
            // scroll it automatically
            richTextBoxErrorMsg.ScrollToCaret();
        }

        private void comboBoxFixtureNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            bt.parameters.beetleFixtureNumber = (byte)comboBoxFixtureNum.SelectedIndex;
            bt.beetleControl.FixtureInit();
        }

        private void buttonSetPositionSyn_Click(object sender, EventArgs e)
        {
            if (runThread != null && runThread.IsAlive)
            {
                MessageBox.Show("Another Process Runing");
                return;
            }

            bt.beetleControl.tempP = new double[6] { (double)numericUpDownX.Value, (double)numericUpDownY.Value, (double)numericUpDownZ.Value,
                    (double)numericUpDownRx.Value, (double)numericUpDownRy.Value, (double)numericUpDownRz.Value };
            runThread = new Thread(bt.beetleControl.GotoTempSyn);
            runThread.Start();
        }

        private void comboBoxBeetleSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxBeetleSelect.SelectedIndex == 2 && beetle2 == null)
            {
                beetle2 = new BeetleSystemObject();
                beetle2.parameters.LoadAll();
                if (!beetle2.Connection())
                {
                    buttonCalibration.Enabled = false;
                    buttonClearError.Enabled = false;
                    //buttonTest.Enabled = false;
                    buttonClose.Enabled = false;
                    buttonCancleRun.Enabled = false;
                    ButtonReset.Enabled = false;
                    ButtonAlignment.Enabled = false;
                    ButtonPreCuring.Enabled = false;
                    ButtonCuring.Enabled = false;
                    // disable buttons on stage tab
                    buttonSetPosition.Enabled = false;
                    buttonSetInitial.Enabled = false;
                    buttonClearErrorBC.Enabled = false;
                    buttonPiezoReset.Enabled = false;
                    buttonPiezoSearch.Enabled = false;
                }
            }

            if (comboBoxBeetleSelect.SelectedIndex == 1)
                bt = beetle1;
            else if (comboBoxBeetleSelect.SelectedIndex == 2)
                bt = beetle2;
            else
                return;
            ReloadBeetleInfo();
        }

        private void ReloadBeetleInfo()
        {
            // load inital positions on the GUI
            numericUpDownX.Value = (decimal)bt.parameters.position[0];
            numericUpDownY.Value = (decimal)bt.parameters.position[1];
            numericUpDownZ.Value = (decimal)bt.parameters.position[2];
            numericUpDownRx.Value = (decimal)bt.parameters.position[3];
            numericUpDownRy.Value = (decimal)bt.parameters.position[4];
            numericUpDownRz.Value = (decimal)bt.parameters.position[5];
            numericUpDownPx.Value = (decimal)bt.parameters.pivotPoint[0];
            numericUpDownPy.Value = (decimal)bt.parameters.pivotPoint[1];
            numericUpDownPz.Value = (decimal)bt.parameters.pivotPoint[2] - 8;

            comboBoxFixtureNum.SelectedIndex = bt.parameters.beetleFixtureNumber;
            comboBoxPiezoStep.SelectedIndex = bt.parameters.piezoStepSize / 2 - 1;
        }

        private void comboBoxPMChl_SelectedIndexChanged(object sender, EventArgs e)
        {
            PowerMeter.channel = (byte)(comboBoxPMChl.SelectedIndex + 1);
            PowerMeter.Open();
            bt.parameters.SavePMChl();
        }
    }
}
