using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Beetle
{
    public partial class MainForm : Form
    {
        private static BeetleAlignment BA;
        private static BeetleCuring BC;
        private static Thread runThread;
        public MainForm()
        {
            InitializeComponent();
            BA = BeetleAlignment.GetInstance();
            BC = BeetleCuring.GetInstance();
        }

        private void ButtonReset_Click(object sender, EventArgs e)
        {
            runThread = new Thread(BeetleControl.GotoReset);
            runThread.Start();
        }

        private void ButtonAlignment_Click(object sender, EventArgs e)
        {
            runThread = new Thread(BA.AlignmentRun);
            runThread.Start();
            IL.Text = BA.ReadLossCurrentMax;
        }

        private void ButtonPreAlign_Click(object sender, EventArgs e)
        {
            runThread = new Thread(BA.PreAlignRun);
            runThread.Start();
        }

        private void ButtonCuring_Click(object sender, EventArgs e)
        {
            runThread = new Thread(BC.Run);
            runThread.Start();
        }

        private void ButtonClearError_Click(object sender, EventArgs e)
        {
            BeetleControl.ClearErrors();
        }

        private void ButtonCalibration_Click(object sender, EventArgs e)
        {
            BeetleControl.Calibration();
        }

        private void IL_Click(object sender, EventArgs e)
        {
            PowerMeter.Read();
            IL.Text = Parameters.loss.ToString();
        }

        private void ControlBoxDetection_Click(object sender, EventArgs e)
        {
            if (BeetleConnection.AssignPorts())
                Parameters.SaveCOMPorts();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Parameters.LoadParameters();
        }

        private void Test_Click(object sender, EventArgs e)
        {
            BeetleConnection.AssignPorts();
        }
    }
}
