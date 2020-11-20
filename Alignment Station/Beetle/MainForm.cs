﻿using System;
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
        public MainForm()
        {
            InitializeComponent();
            BA = BeetleAlignment.GetInstance();
            BC = BeetleCuring.GetInstance();
        }

        private void ButtonReset_Click(object sender, EventArgs e)
        {
            BeetleControl.GotoReset();
        }

        private void ButtonBackAlign_Click(object sender, EventArgs e)
        {
            BA.Run(criteriaSelect: "currentMax", backDistanceAfterSearching: 0, runFromContact: false, useScanMode: false);
        }

        private void ButtonAlignment_Click(object sender, EventArgs e)
        {
            //Thread runThread = new Thread(BA.Run);
            BA.Run(criteriaSelect: "global");
            //runThread.Start(criteriaSelect: "global");
            IL.Text = BA.ReadLossCurrentMax;
        }

        private void ButtonCuring_Click(object sender, EventArgs e)
        {
            BC.Run();
        }

        private void ButtonClearError_Click(object sender, EventArgs e)
        {
            BeetleControl.ClearErrors();
        }

        private void ButtonCalibration_Click(object sender, EventArgs e)
        {
            BeetleControl.Calibration();
        }
    }
}