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
        private bool numericUpDownEditting = false;
        private bool beetle1Connected = false;
        private bool beetle2Connected = false;
        private DateTime thisTime, startTime;
        private TimeSpan timeElapsed;
        private bool clickGo = false;
        private int dofIndicator = 0; // indicate which dof is controlled by keys now, 0-5 means xyzRxRyRz

        private FilterInfoCollection filterInfoCollecion;
        private VideoCaptureDevice videoCaptureDevice;

        private BeetleSystemObject bt;
        private BeetleSystemObject beetle1 = new BeetleSystemObject();
        private BeetleSystemObject beetle2;

        public MainForm()
        {
            InitializeComponent();

            bt = beetle1;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            comboBoxBeetleSelect.SelectedIndex = 0;

            // Find all available cameras
            filterInfoCollecion = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterInfo in filterInfoCollecion)
                comboBoxCamSelect.Items.Add(filterInfo.Name);
            //if (comboBoxCamSelect.Items.Count > 1)
            //    comboBoxCamSelect.SelectedIndex = 1; // this will triger comboBoxCamSelect_SelectedIndexChanged function
            comboBoxCamSelect.SelectedIndex = 0;

            bt.parameters.LoadAll();

            if (!bt.Connection())
            {
                ButtonEnables(false);
                beetle1Connected = false;
                labelControlBoxNum.Text = "Connected to Control Box *";
            }
            else
            {
                beetle1Connected = true;
                labelControlBoxNum.Text = "Connected to Control Box " + bt.parameters.beetleControlBoxNum;
            }

            // comboBox Initial index
            comboBoxProductSelect.SelectedIndex = 0;
            comboBoxUsePiezo.SelectedIndex = 1;
            comboBoxMotorSelectTop.SelectedIndex = 6;
            comboBoxMotorSelectMid.SelectedIndex = 6;
            comboBoxMotorSelectBot.SelectedIndex = 6;
            comboBoxPMChl.SelectedIndex = 0;
            comboBoxStepSize.SelectedIndex = 0;

            labelX.ForeColor = Color.Red;

            ReloadBeetleInfo();
        }

        private void VideoCaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap pic = (Bitmap)eventArgs.Frame.Clone();
            // Rotate or flip. Other options include Rotate180FlipX, Rotate180FlipXY etc.
            //pic.RotateFlip(RotateFlipType.Rotate180FlipX);
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
            if (comboBoxCamSelect.SelectedIndex != 0)
            {
                videoCaptureDevice = new VideoCaptureDevice(filterInfoCollecion[comboBoxCamSelect.SelectedIndex - 1].MonikerString);

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
            else
            {
                pictureBoxCam.Image = pictureBoxCam.InitialImage;
                pictureBoxCamBC.Image = pictureBoxCam.InitialImage;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Always only save beetle1's parameters
            beetle1.parameters.Save();
            beetle1.beetleControl.ClosePorts();
            beetle1.piezoControl.ClosePort();
            if (beetle2 != null)
            {
                beetle2.parameters.Save_2();
                beetle2.beetleControl.ClosePorts();
                beetle2.piezoControl.ClosePort();
            }
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
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

                if (!numericUpDownEditting)
                {
                    numericUpDownX.Value = (decimal)bt.parameters.position[0];
                    numericUpDownY.Value = (decimal)bt.parameters.position[1];
                    numericUpDownZ.Value = (decimal)bt.parameters.position[2];
                    numericUpDownRx.Value = (decimal)bt.parameters.position[3];
                    numericUpDownRy.Value = (decimal)bt.parameters.position[4];
                    numericUpDownRz.Value = (decimal)bt.parameters.position[5];
                }
                
                if (chartInitFlag)
                    MotorChartsUpdate();
            }

        }

        private void ButtonReset_Click(object sender, EventArgs e)
        {
            bt.parameters.errorFlag = false;
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
            bt.parameters.productName = comboBoxProductSelect.Text;

            if (PowerMeter.loss > -40 || bt.parameters.productName == "WOA")
            {
                Parameters.Log("\r\n");
                Parameters.Log("***************************");
                Parameters.Log("Aligment Starts");
                Console.WriteLine("A New Alignemnt Starts");
                richTextBoxErrorMsg.Text += "Alignment Running\n";

                stopReadPM = false;
                startTime = DateTime.Now;
                Thread.Sleep(500);

                //bt.piezoControl.Reset();
                //Thread.Sleep(500);

                if (bt.parameters.productName != "WOA")
                {
                    if (bt.ba == null)
                        bt.AlignCuringInit();
                    runThread = new Thread(bt.ba.AlignmentRun);
                }
                else
                {
                    if (bt.woa == null)
                        bt.WOAInit();
                    runThread = new Thread(bt.woa.SingleRun);
                }
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
                Thread.Sleep(500);

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
            Thread.Sleep(500);

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
            beetle1.beetleControl.ClosePorts();
            beetle1.piezoControl.ClosePort();
            if (beetle2 != null)
            {
                beetle2.beetleControl.ClosePorts();
                beetle2.piezoControl.ClosePort();
            }
            bt.Detect();
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

                if (comboBoxBeetleSelect.SelectedIndex <= 1)
                    beetle1Connected = true;
                else
                    beetle2Connected = true;
                labelControlBoxNum.Text = "Connected to Control Box " + bt.parameters.beetleControlBoxNum;
            }

            if (!beetle1.beetleControl.PortsIsOpen())
                beetle1.beetleControl.OpenPorts();
            if (!beetle1.piezoControl.PortIsOpen()) 
                beetle1.piezoControl.OpenPort();
            if (beetle2 != null)
            {
                if (!beetle2.beetleControl.PortsIsOpen())
                    beetle2.beetleControl.OpenPorts();
                if (!beetle2.piezoControl.PortIsOpen())
                    beetle2.piezoControl.OpenPort();
            }
        }

        private void buttonReference_Click(object sender, EventArgs e)
        {
            PowerMeter.lossReference = PowerMeter.Read("dBm");
            richTextBoxErrorMsg.Text += "Reference Set\n";
            bt.parameters.SaveReference();
            numericUpDownReference.Value = (decimal)PowerMeter.lossReference;
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
            //double[] pp = new double[6];
            //bt.parameters.position.CopyTo(pp, 0);
            //pp[2] += 4;
            //bt.beetleControl.GotoPosition(pp, mode: 't', checkOnTarget: false);
        }

        private void comboBoxProductSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            bt.parameters.productName = comboBoxProductSelect.Text;
            if (bt.ba == null)
                bt.AlignCuringInit();
            bt.ba.ProductSelect();
        }

        private void comboBoxUsePiezo_SelectedIndexChanged(object sender, EventArgs e) => bt.parameters.usePiezo = comboBoxUsePiezo.SelectedIndex == 0;

        private void buttonChartOnOff_Click(object sender, EventArgs e)
        {
            if (!chartInitFlag)
                ChartsInit();
            chartsOn = !chartsOn;
            if (chartsOn)
                buttonChartOnOff.Text = "Live Chart On";
            else
                buttonChartOnOff.Text = "Live Chart Off";
        }

        private void buttonSetPosition_Click(object sender, EventArgs e) => StepAction('p');

        private void StepAction(char mode)
        {
            bt.parameters.errorFlag = false;
            if (runThread != null && runThread.IsAlive)
            {
                MessageBox.Show("Another Process Runing");
                return;
            }

            bt.beetleControl.tempP = new double[6] { (double)numericUpDownX.Value, (double)numericUpDownY.Value, (double)numericUpDownZ.Value,
                    (double)numericUpDownRx.Value, (double)numericUpDownRy.Value, (double)numericUpDownRz.Value };
            if (mode == 't')
                runThread = new Thread(bt.beetleControl.GotoTempSyn);
            else if (mode == 'j')
                runThread = new Thread(bt.beetleControl.GotoTempTraj);
            else
                runThread = new Thread(bt.beetleControl.GotoTemp);
            runThread.Start();

            numericUpDownEditting = false;
        }

        private void buttonSetPositionSyn_Click(object sender, EventArgs e) => StepAction('t');

        private void numericUpDownNotXY_ValueChanged(object sender, EventArgs e)
        {
            if (clickGo && numericUpDownEditting)
                StepAction('t');
        }

        private void numericUpDownXY_ValueChanged(object sender, EventArgs e)
        {
            if (clickGo && numericUpDownEditting)
                StepAction('j');
        }

        private void tabControl1_KeyUp(object sender, KeyEventArgs e)
        {
            //Console.WriteLine(e.KeyCode);
            if (e.KeyCode == Keys.Space)
            {
                dofIndicator += 1;
                dofIndicator = dofIndicator <= 5 ? (dofIndicator >= 0 ? dofIndicator : 5) : 0;
                labelX.ForeColor = Color.Black;
                labelY.ForeColor = Color.Black;
                labelZ.ForeColor = Color.Black;
                labelRx.ForeColor = Color.Black;
                labelRy.ForeColor = Color.Black;
                labelRz.ForeColor = Color.Black;
                switch (dofIndicator)
                {
                    case 0:
                        labelX.ForeColor = Color.Red;
                        break;
                    case 1:
                        labelY.ForeColor = Color.Red;
                        break;
                    case 2:
                        labelZ.ForeColor = Color.Red;
                        break;
                    case 3:
                        labelRx.ForeColor = Color.Red;
                        break;
                    case 4:
                        labelRy.ForeColor = Color.Red;
                        break;
                    case 5:
                        labelRz.ForeColor = Color.Red;
                        break;
                }
            }
            else if ((e.KeyCode == Keys.Oemplus || e.KeyCode == Keys.Add) && clickGo)
            {
                numericUpDownEditting = true;
                switch (dofIndicator)
                {
                    case 0:
                        numericUpDownX.Value += numericUpDownX.Increment;
                        break;
                    case 1:
                        numericUpDownY.Value += numericUpDownY.Increment;
                        break;
                    case 2:
                        numericUpDownZ.Value += numericUpDownZ.Increment;
                        break;
                    case 3:
                        numericUpDownRx.Value += numericUpDownRx.Increment;
                        break;
                    case 4:
                        numericUpDownRy.Value += numericUpDownRy.Increment;
                        break;
                    case 5:
                        numericUpDownRz.Value += numericUpDownRz.Increment;
                        break;
                }
            }
            else if ((e.KeyCode == Keys.OemMinus || e.KeyCode == Keys.Subtract) && clickGo)
            {
                numericUpDownEditting = true;
                switch (dofIndicator)
                {
                    case 0:
                        numericUpDownX.Value -= numericUpDownX.Increment;
                        break;
                    case 1:
                        numericUpDownY.Value -= numericUpDownY.Increment;
                        break;
                    case 2:
                        numericUpDownZ.Value -= numericUpDownZ.Increment;
                        break;
                    case 3:
                        numericUpDownRx.Value -= numericUpDownRx.Increment;
                        break;
                    case 4:
                        numericUpDownRy.Value -= numericUpDownRy.Increment;
                        break;
                    case 5:
                        numericUpDownRz.Value -= numericUpDownRz.Increment;
                        break;
                }
            }
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
                MessageBox.Show("Another Process is Runing");
                return;
            }

            if (bt.ba == null)
                bt.AlignCuringInit();
            runThread = new Thread(bt.ba.PiezoSearchXYZRun);
            runThread.Start();
        }

        private void comboBoxPiezoStep_SelectedIndexChanged(object sender, EventArgs e) => bt.parameters.piezoStepSize = ushort.Parse(comboBoxPiezoStep.SelectedItem.ToString());

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

        private void comboBoxBeetleSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (runThread != null && runThread.IsAlive)
            {
                MessageBox.Show("Another Process is Runing");
                return;
            }

            // when switch system, clickGo needs to be false to avoid unexpected errors and move the switched system
            if (clickGo)
            {
                clickGo = false;
                buttonClickGo.Text = "ClickGo: False";
                numericUpDownX.Increment = (decimal)0.001;
                numericUpDownY.Increment = (decimal)0.001;
                numericUpDownZ.Increment = (decimal)0.001;
                numericUpDownRx.Increment = (decimal)0.05;
                numericUpDownRy.Increment = (decimal)0.05;
                numericUpDownRz.Increment = (decimal)0.05;
            }

            if (comboBoxBeetleSelect.SelectedIndex == 2 && beetle2 == null)
            {
                beetle2 = new BeetleSystemObject();
                //beetle2.parameters.LoadAll();
                beetle2.parameters.LoadAll_2();

                if (beetle2.Connection())
                {
                    ButtonEnables(true);
                    beetle2Connected = true;
                }
                else
                {
                    // Close all ports first then reconnect
                    beetle1.beetleControl.ClosePorts();
                    beetle1.piezoControl.ClosePort();
                    if (beetle2.Detect())
                    {
                        //beetle2.parameters.SaveCOMPorts();
                        richTextBoxErrorMsg.Text += "COM Ports Detected\n";
                        if (beetle2.Connection())
                        {
                            ButtonEnables(true);
                            beetle2Connected = true;
                        }
                        else
                        {
                            ButtonEnables(false);
                            beetle2Connected = false;
                        }
                    }
                    try
                    {
                        beetle1.beetleControl.OpenPorts();
                        beetle1.piezoControl.OpenPort();
                    }
                    catch (Exception)
                    {
                        beetle1Connected = false;
                    }
                }
            }

            if (comboBoxBeetleSelect.SelectedIndex == 1)
            {
                bt = beetle1;
                ButtonEnables(beetle1Connected);
            }
            else if (comboBoxBeetleSelect.SelectedIndex == 2)
            {
                bt = beetle2;
                ButtonEnables(beetle2Connected);
            }
            else
                return;
            labelControlBoxNum.Text = "Connected to Control Box " + bt.parameters.beetleControlBoxNum;
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

            numericUpDownReference.Value = (decimal)PowerMeter.lossReference;

            comboBoxFixtureNum.SelectedIndex = bt.parameters.beetleFixtureNumber;
            comboBoxPiezoStep.SelectedIndex = bt.parameters.piezoStepSize / 2 - 1;
        }

        private void numericUpDown_Click(object sender, EventArgs e) => numericUpDownEditting = true;

        private void comboBoxPMChl_SelectedIndexChanged(object sender, EventArgs e)
        {
            PowerMeter.channel = (byte)(comboBoxPMChl.SelectedIndex + 1);
            PowerMeter.Open();
            bt.parameters.SavePMChl();
        }

        private void buttonSetRef_Click(object sender, EventArgs e)
        {
            PowerMeter.lossReference = (double)numericUpDownReference.Value;
            richTextBoxErrorMsg.Text += "Reference Set\n";
            bt.parameters.SaveReference();
        }

        private void buttonClickGo_Click(object sender, EventArgs e)
        {
            clickGo = !clickGo;
            if (clickGo)
            { 
                buttonClickGo.Text = "ClickGo: True";
                numericUpDownRx.Increment = (decimal)0.1;
                numericUpDownRy.Increment = (decimal)0.1;
                numericUpDownRz.Increment = (decimal)0.1;
                decimal stp = decimal.Parse(comboBoxStepSize.SelectedItem.ToString());
                stp *= (decimal)0.001;
                numericUpDownX.Increment = stp;
                numericUpDownY.Increment = stp;
                numericUpDownZ.Increment = stp;
            }
            else
            {
                buttonClickGo.Text = "ClickGo: False";
                numericUpDownX.Increment = (decimal)0.001;
                numericUpDownY.Increment = (decimal)0.001;
                numericUpDownZ.Increment = (decimal)0.001;
                numericUpDownRx.Increment = (decimal)0.05;
                numericUpDownRy.Increment = (decimal)0.05;
                numericUpDownRz.Increment = (decimal)0.05;
            }
        }

        private void ButtonEnables(bool enable)
        {
            if (enable)
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
            else
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

        private void comboBoxStepSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            decimal stp = decimal.Parse(comboBoxStepSize.SelectedItem.ToString());
            stp *= (decimal)0.001;
            numericUpDownX.Increment = stp;
            numericUpDownY.Increment = stp;
            numericUpDownZ.Increment = stp;
        }

        // WOA tab
        private void buttonInitPos_Click(object sender, EventArgs e)
        {
            bt.beetleControl.tempP = new double[6] { -2, -2, 139, 0, 0, 0};
            bt.beetleControl.GotoTemp();
            numericUpDownEditting = false;
        }

        private void buttonRightAway_Click(object sender, EventArgs e)
        {
            bt.beetleControl.tempP[2] = 140;
            bt.beetleControl.tempP[3] = 0;
            bt.beetleControl.tempP[4] = 0;
            bt.beetleControl.tempP[5] = 0;
            bt.beetleControl.tempP[0] = -4;
            bt.beetleControl.tempP[1] = -4;
            bt.beetleControl.GotoTempTraj();
            numericUpDownEditting = false;
        }

        private void buttonRightEngage_Click(object sender, EventArgs e)
        {
            bt.beetleControl.tempP = new double[6] { -0.44, .48, 138.21, 0, 0, 0 };
            bt.beetleControl.GotoTempTraj();
            numericUpDownEditting = false;
        }

        private void buttonLeftAway_Click(object sender, EventArgs e)
        {
            bt.beetleControl.tempP[2] = 140;
            bt.beetleControl.tempP[3] = 0;
            bt.beetleControl.tempP[4] = 0;
            bt.beetleControl.tempP[5] = 0;
            bt.beetleControl.GotoTempTraj();
            bt.beetleControl.tempP[0] = -4;
            bt.beetleControl.tempP[1] = -4;
            bt.beetleControl.GotoTempTraj();
            numericUpDownEditting = false;
        }

        private void buttonLeftEngage_Click(object sender, EventArgs e)
        {
            // -0.81, 3.99, 137.8
            bt.beetleControl.tempP[2] = 139;
            bt.beetleControl.GotoTempTraj();
            bt.beetleControl.tempP[0] = -0.81;
            bt.beetleControl.tempP[1] = 3.99;
            bt.beetleControl.GotoTempTraj();
            bt.beetleControl.tempP[2] = 137.8;
            bt.beetleControl.GotoTempTraj();
            numericUpDownEditting = false;
        }

        private void buttonAngleEdgePos_Click(object sender, EventArgs e)
        {
            // -.35, -2.5, 137.5
            bt.beetleControl.tempP[2] = 139;
            bt.beetleControl.GotoTempTraj();
            bt.beetleControl.tempP[0] = -0.8;
            bt.beetleControl.tempP[1] = -2.5;
            bt.beetleControl.GotoTempTraj();
            bt.beetleControl.tempP[2] = 138;
            bt.beetleControl.GotoTempTraj();
            numericUpDownEditting = false;
        }

        private void PivotPointRightPumpSet()
        {
            double[] p = { 122.5, 16.5, 37, 0 };
            bt.mathModel.SetPivotPoint = p; // this line will copy the correct value to parameter.pivotpoint, so no need to copy to parameter again.

            numericUpDownPx.Value = (decimal)p[0];
            numericUpDownPy.Value = (decimal)p[1];
            numericUpDownPz.Value = (decimal)p[2];
        }

        private void buttonPPRightSignal_Click(object sender, EventArgs e)
        {
            double[] p = { 122.5, 19.5, 40, 0 };
            bt.mathModel.SetPivotPoint = p; // this line will copy the correct value to parameter.pivotpoint

            numericUpDownPx.Value = (decimal)p[0];
            numericUpDownPy.Value = (decimal)p[1];
            numericUpDownPz.Value = (decimal)p[2];
        }

        private void PivotPointLeftSet()
        {
            double[] p = { 127.7, 2.4, 45, 0 };
            bt.mathModel.SetPivotPoint = p; // this line will copy the correct value to parameter.pivotpoint

            numericUpDownPx.Value = (decimal)p[0];
            numericUpDownPy.Value = (decimal)p[1];
            numericUpDownPz.Value = (decimal)p[2];
        }

        private void buttonRightConfig_Click(object sender, EventArgs e)
        {
            bt.parameters.beetleFixtureNumber = 5;
            comboBoxFixtureNum.SelectedIndex = bt.parameters.beetleFixtureNumber;
            comboBoxProductSelect.SelectedIndex = 2;

            PivotPointRightPumpSet();
        }

        private void buttonLeftConfig_Click(object sender, EventArgs e)
        {
            bt.parameters.beetleFixtureNumber = 0;
            comboBoxFixtureNum.SelectedIndex = bt.parameters.beetleFixtureNumber;
            comboBoxProductSelect.SelectedIndex = 2;

            PivotPointLeftSet();
        }


    }
}
