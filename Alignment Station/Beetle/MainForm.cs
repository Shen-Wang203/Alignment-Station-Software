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
            Parameters.Log("\r\n");
            Parameters.Log("***************************");
            Parameters.Log("Aligment Starts");
            Console.WriteLine("A New Alignemnt Starts");
            runThread = new Thread(BA.AlignmentRun);
            runThread.Start();
            IL.Text = Parameters.lossCurrentMax.ToString();
        }

        private void ButtonPreAlign_Click(object sender, EventArgs e)
        {
            Parameters.Log("\r\n");
            Parameters.Log("***************************");
            Parameters.Log("PreAligment Starts");
            Console.WriteLine("PreAlignemnt Starts");
            runThread = new Thread(BA.PreAlignRun);
            runThread.Start();
        }

        private void ButtonCuring_Click(object sender, EventArgs e)
        {
            Parameters.Log("\r\n");
            Parameters.Log("***************************");
            Parameters.Log("Curing Starts");
            Console.WriteLine("Curing Starts");
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
            Parameters.LoadAll();
        }

        private void Test_Click(object sender, EventArgs e)
        {
            List<double> loss = new List<double>();
            List<double> pos = new List<double>();
            loss.Add(-0.2);
            loss.Add(-0.1);
            loss.Add(-0.3);
            pos.Add(12);
            pos.Add(31);
            pos.Add(54);
            Console.WriteLine(pos[loss.IndexOf(loss.Max())]);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Parameters.SaveAll();
        }
    }
}
