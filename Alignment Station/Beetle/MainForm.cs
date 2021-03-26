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
        private static BeetleAlignment BA;
        private static BeetleCuring BC;
        private static Thread runThread;
        private static bool stopReadPM = false;
        private static int timerCount = 0;
        private static bool chartsOn = true;

        private static FilterInfoCollection filterInfoCollecion;
        private static VideoCaptureDevice videoCaptureDevice;

        public MainForm()
        {
            InitializeComponent();
            BA = BeetleAlignment.GetInstance();
            BC = BeetleCuring.GetInstance();

            ChartsInit();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Parameters.LoadAll();
            if (Parameters.usePiezo)
                PiezoControl.Connection();
            if (!BeetleControl.Connection())
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
            }
            // Find all available cameras
            filterInfoCollecion = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterInfo in filterInfoCollecion)
                comboBoxCamSelect.Items.Add(filterInfo.Name);
            comboBoxCamSelect.SelectedIndex = 0; // this will triger comboBoxCamSelect_SelectedIndexChanged function

            comboBoxProductSelect.SelectedIndex = 0;
            comboBoxUsePiezo.SelectedIndex = 1;

            numericUpDownX.Value = (decimal)Parameters.initialPosition[0];
            numericUpDownY.Value = (decimal)Parameters.initialPosition[1];
            numericUpDownZ.Value = (decimal)Parameters.initialPosition[2];
            numericUpDownRx.Value = (decimal)Parameters.initialPosition[3];
            numericUpDownRy.Value = (decimal)Parameters.initialPosition[4];
            numericUpDownRz.Value = (decimal)Parameters.initialPosition[5];
            numericUpDownPx.Value = (decimal)Parameters.pivotPoint[0];
            numericUpDownPy.Value = (decimal)Parameters.pivotPoint[1];
            numericUpDownPz.Value = (decimal)Parameters.pivotPoint[2];
        }

        private void VideoCaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap pic = (Bitmap)eventArgs.Frame.Clone();
            // Rotate or flip. Other options include Rotate180FlipX, Rotate180FlipXY etc.
            pic.RotateFlip(RotateFlipType.RotateNoneFlipX); 
            pictureBoxCam.Image = pic;
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

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPageIndex == 0)
            {
                if (videoCaptureDevice != null && !videoCaptureDevice.IsRunning)
                    videoCaptureDevice.Start();
            }
            else
            {
                if (videoCaptureDevice != null && videoCaptureDevice.IsRunning)
                    videoCaptureDevice.Stop();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Parameters.Save();
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                labelTargetValue.Text = Math.Round(Parameters.lossCurrentMax, 4).ToString();
                labelILValue.Text = Math.Round(Parameters.loss, 4).ToString();

                if (Parameters.errors != "")
                {
                    richTextBoxErrorMsg.Text += Parameters.errors;
                    Parameters.errors = "";
                }

                // if thread is running then start the time count on the GUI
                if (runThread != null && runThread.IsAlive)
                    timerCount += 1;
                //else if (!stopReadPM)
                //    PowerMeter.ReadNoPrint();

                int second = timerCount / 4; // 250ms interval
                int minutes = second / 60;

                labelTimeValue.Text = minutes.ToString() + "min " + second.ToString() + "s";

                sbyte sum = 0;
                foreach (var i in BeetleControl.motorEngageFlag)
                    sum += i;
                if (sum == 0)
                {
                    labelStatusValue.Text = "IDLE";
                    labelStatusValue.ForeColor = Color.Lime;
                }
                else
                {
                    labelStatusValue.Text = "BUSY";
                    labelStatusValue.ForeColor = Color.Red;
                }

                labelPositionXYZ.Text = Math.Round(Parameters.position[0], 3).ToString() + ", " + Math.Round(Parameters.position[1], 3).ToString() + 
                                        ", " + Math.Round(Parameters.position[2], 3).ToString();
                labelPositionAngles.Text = Math.Round(Parameters.position[3], 3).ToString() + ", " + Math.Round(Parameters.position[4], 3).ToString() + 
                                        ", " + Math.Round(Parameters.position[5], 3).ToString();

                if (Parameters.usePiezo)
                    labelPiezoPositionValue.Text = Parameters.piezoPosition[0].ToString() + ", " + Parameters.piezoPosition[1].ToString() + 
                                                    ", " + Parameters.piezoPosition[2].ToString();

                if (chartsOn)
                    ChartsDataUpdate();
            }
            else
            {
                sbyte sum = 0;
                foreach (sbyte i in BeetleControl.motorEngageFlag)
                    sum += i;
                if (sum == 0 && !Parameters.errorFlag)
                {
                    labelStatusValueBC.Text = "IDLE";
                    labelStatusValueBC.ForeColor = Color.Lime;
                }
                else if (sum != 0 && !Parameters.errorFlag)
                {
                    labelStatusValueBC.Text = "BUSY";
                    labelStatusValueBC.ForeColor = Color.Red;
                }
                else
                {
                    labelStatusValueBC.Text = "ERROR";
                    labelStatusValueBC.ForeColor = Color.Red;
                }
            }

        }


        private void ButtonReset_Click(object sender, EventArgs e)
        {
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
            timerCount = 0;

            runThread = new Thread(BeetleControl.GotoReset);
            runThread.Start();
        }

        private void ButtonAlignment_Click(object sender, EventArgs e)
        {
            if (Parameters.loss > -40)
            {
                Parameters.Log("\r\n");
                Parameters.Log("***************************");
                Parameters.Log("Aligment Starts");
                Console.WriteLine("A New Alignemnt Starts");
                richTextBoxErrorMsg.Text += "Alignment Running\n";

                stopReadPM = false;
                timerCount = 0;

                runThread = new Thread(BA.AlignmentRun);
                runThread.Start();

                ButtonAlignment.BackColor = Color.Yellow;
                ButtonPreCuring.Enabled = true;
            }
            else
                MessageBox.Show(text: "Make The Loss Lower Than -40dB");
        }

        private void ButtonPreCuring_Click(object sender, EventArgs e)
        {
            if (Parameters.loss > -8)
            {
                Parameters.Log("\r\n");
                Parameters.Log("***************************");
                Parameters.Log("PreAligment Starts");
                Console.WriteLine("PreAlignemnt Starts");
                richTextBoxErrorMsg.Text += "PreCuring Running\n";

                stopReadPM = false;
                timerCount = 0;

                runThread = new Thread(BA.PreCuringRun);
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
            Parameters.Log("\r\n");
            Parameters.Log("***************************");
            Parameters.Log("Curing Starts");
            Console.WriteLine("Curing Starts");
            richTextBoxErrorMsg.Text += "Curing Running\n";

            stopReadPM = false;
            timerCount = 0;
            
            runThread = new Thread(BC.Run);
            runThread.Start();

            ButtonPreCuring.BackColor = Color.Green;
            ButtonCuring.BackColor = Color.Yellow;
            ButtonCuring.Enabled = true;
        }

        private void ButtonClearError_Click(object sender, EventArgs e)
        {
            BeetleControl.ClearErrors();
            BeetleControl.NormalTrajSpeed();
            richTextBoxErrorMsg.Text += "Errors are Cleared\n";
        }

        private void ButtonCalibration_Click(object sender, EventArgs e)
        {
            runThread = new Thread(BeetleControl.Calibration);
            runThread.Start();
            richTextBoxErrorMsg.Text += "Calibrating\n";
        }

        private void IL_Click(object sender, EventArgs e)
        {
            PowerMeter.Read();
            labelILValue.Text = Parameters.loss.ToString();
        }

        private void ControlBoxDetection_Click(object sender, EventArgs e)
        {
            if (BeetleConnection.AssignPorts())
            {
                Parameters.SaveCOMPorts();
                richTextBoxErrorMsg.Text += "COM Ports Saved\n";
            }
            if (BeetleControl.Connection() && PiezoControl.Connection())
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
            }
        }

        private void buttonReference_Click(object sender, EventArgs e)
        {
            Parameters.lossReference = PowerMeter.Read("dBm");
            richTextBoxErrorMsg.Text += "Reference Set\n";
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            timerCount = 0;
            runThread = new Thread(BeetleControl.GotoClose);
            runThread.Start();
            richTextBoxErrorMsg.Text += "Closing\n";
        }

        private void buttonCancleRun_Click(object sender, EventArgs e)
        {
            // Abort function can cause unexpected errors, so avoid using it
            //runThread.Abort();
            Parameters.errorFlag = true; // set errorFlag to let thread finish
            richTextBoxErrorMsg.Text += "Run Cancelling\n";
            while (runThread.IsAlive)
            {; }
            BeetleControl.DisengageMotors();
            BeetleControl.NormalTrajSpeed();
            Parameters.errorFlag = false;
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
            timerCount = 0;
            runThread = new Thread(PiezoControl.Test);
            ////runThread = new Thread(BA.Test);
            //runThread = new Thread(PiezoControl.TestRun);
            runThread.Start();
        }

        private void comboBoxProductSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            Parameters.productName = comboBoxProductSelect.Text;
        }

        private void comboBoxUsePiezo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Parameters.usePiezo = comboBoxUsePiezo.SelectedIndex == 0;
        }

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
            BeetleControl.tempP = new double[6] { (double)numericUpDownX.Value, (double)numericUpDownY.Value, (double)numericUpDownZ.Value, 
                    (double)numericUpDownRx.Value, (double)numericUpDownRy.Value, (double)numericUpDownRz.Value };
            runThread = new Thread(BeetleControl.GotoTemp);
            runThread.Start();
        }

        private void buttonSetPivot_Click(object sender, EventArgs e)
        {
            double[] p = { (double)numericUpDownPx.Value, (double)numericUpDownPy.Value, (double)numericUpDownPz.Value, 0 };
            p.CopyTo(Parameters.pivotPoint, 0);
            Parameters.SavePivotPoint();
        }

        private void buttonSetInitial_Click(object sender, EventArgs e)
        {
            Parameters.position.CopyTo(Parameters.initialPosition, 0);
            Parameters.SaveInitialPosition();
        }

        private void buttonClearErrorBC_Click(object sender, EventArgs e)
        {
            BeetleControl.ClearErrors();
            Console.WriteLine("Errors Cleared");
            //BeetleControl.NormalTrajSpeed();
            //richTextBoxErrorMsg.Text += "Errors are Cleared\n";
        }

    }
}
