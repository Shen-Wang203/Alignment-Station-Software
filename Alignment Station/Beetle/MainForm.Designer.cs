﻿namespace Beetle
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ButtonReset = new System.Windows.Forms.Button();
            this.ButtonAlignment = new System.Windows.Forms.Button();
            this.ButtonPreCuring = new System.Windows.Forms.Button();
            this.ButtonCuring = new System.Windows.Forms.Button();
            this.buttonClearError = new System.Windows.Forms.Button();
            this.buttonCalibration = new System.Windows.Forms.Button();
            this.labelILValue = new System.Windows.Forms.Label();
            this.buttonControlBoxDetection = new System.Windows.Forms.Button();
            this.buttonTest = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageAlignCuring = new System.Windows.Forms.TabPage();
            this.buttonChart = new System.Windows.Forms.Button();
            this.comboBoxProductSelect = new System.Windows.Forms.ComboBox();
            this.labelProductSelect = new System.Windows.Forms.Label();
            this.labelUsePiezo = new System.Windows.Forms.Label();
            this.comboBoxUsePiezo = new System.Windows.Forms.ComboBox();
            this.labelTimeValue = new System.Windows.Forms.Label();
            this.labelTime = new System.Windows.Forms.Label();
            this.labelErrorMessage = new System.Windows.Forms.Label();
            this.labelStatusValue = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.labelTargetValue = new System.Windows.Forms.Label();
            this.labelTarget = new System.Windows.Forms.Label();
            this.buttonCancleRun = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonConnectPM = new System.Windows.Forms.Button();
            this.buttonDisconnectPM = new System.Windows.Forms.Button();
            this.buttonReference = new System.Windows.Forms.Button();
            this.labelIL = new System.Windows.Forms.Label();
            this.richTextBoxErrorMsg = new System.Windows.Forms.RichTextBox();
            this.cartesianChartZ = new LiveCharts.WinForms.CartesianChart();
            this.cartesianChartXY = new LiveCharts.WinForms.CartesianChart();
            this.label_camSelect = new System.Windows.Forms.Label();
            this.comboBoxCamSelect = new System.Windows.Forms.ComboBox();
            this.pictureBoxCam = new System.Windows.Forms.PictureBox();
            this.tabPageStageControl = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonClearErrorBC = new System.Windows.Forms.Button();
            this.labelStatusValueBC = new System.Windows.Forms.Label();
            this.labelStatusBC = new System.Windows.Forms.Label();
            this.buttonSetInitial = new System.Windows.Forms.Button();
            this.numericUpDownPz = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownPy = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownPx = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.numericUpDownRz = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownRy = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownRx = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownZ = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownY = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownX = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.buttonSetPosition = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonSetPivot = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label_Z = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label_Y = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label_X = new System.Windows.Forms.Label();
            this.cartesianChartMotorB = new LiveCharts.WinForms.CartesianChart();
            this.cartesianChartMotorA = new LiveCharts.WinForms.CartesianChart();
            this.pictureBoxFigure = new System.Windows.Forms.PictureBox();
            this.refreshTimer = new System.Windows.Forms.Timer(this.components);
            this.cartesianChartMotorC = new LiveCharts.WinForms.CartesianChart();
            this.tabControl1.SuspendLayout();
            this.tabPageAlignCuring.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCam)).BeginInit();
            this.tabPageStageControl.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPz)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRz)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFigure)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonReset
            // 
            this.ButtonReset.BackColor = System.Drawing.Color.Red;
            this.ButtonReset.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Yellow;
            this.ButtonReset.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ButtonReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonReset.Location = new System.Drawing.Point(1566, 54);
            this.ButtonReset.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ButtonReset.Name = "ButtonReset";
            this.ButtonReset.Size = new System.Drawing.Size(260, 62);
            this.ButtonReset.TabIndex = 0;
            this.ButtonReset.Text = "Reset";
            this.ButtonReset.UseVisualStyleBackColor = false;
            this.ButtonReset.Click += new System.EventHandler(this.ButtonReset_Click);
            // 
            // ButtonAlignment
            // 
            this.ButtonAlignment.BackColor = System.Drawing.Color.Red;
            this.ButtonAlignment.Enabled = false;
            this.ButtonAlignment.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Yellow;
            this.ButtonAlignment.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ButtonAlignment.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonAlignment.Location = new System.Drawing.Point(1566, 165);
            this.ButtonAlignment.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ButtonAlignment.Name = "ButtonAlignment";
            this.ButtonAlignment.Size = new System.Drawing.Size(260, 66);
            this.ButtonAlignment.TabIndex = 1;
            this.ButtonAlignment.Text = "Alignment";
            this.ButtonAlignment.UseVisualStyleBackColor = false;
            this.ButtonAlignment.Click += new System.EventHandler(this.ButtonAlignment_Click);
            // 
            // ButtonPreCuring
            // 
            this.ButtonPreCuring.BackColor = System.Drawing.Color.Red;
            this.ButtonPreCuring.Enabled = false;
            this.ButtonPreCuring.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Yellow;
            this.ButtonPreCuring.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ButtonPreCuring.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonPreCuring.Location = new System.Drawing.Point(1566, 275);
            this.ButtonPreCuring.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ButtonPreCuring.Name = "ButtonPreCuring";
            this.ButtonPreCuring.Size = new System.Drawing.Size(260, 65);
            this.ButtonPreCuring.TabIndex = 2;
            this.ButtonPreCuring.Text = "PreCuring";
            this.ButtonPreCuring.UseVisualStyleBackColor = false;
            this.ButtonPreCuring.Click += new System.EventHandler(this.ButtonPreCuring_Click);
            // 
            // ButtonCuring
            // 
            this.ButtonCuring.BackColor = System.Drawing.Color.Red;
            this.ButtonCuring.Enabled = false;
            this.ButtonCuring.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Yellow;
            this.ButtonCuring.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ButtonCuring.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonCuring.Location = new System.Drawing.Point(1566, 380);
            this.ButtonCuring.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ButtonCuring.Name = "ButtonCuring";
            this.ButtonCuring.Size = new System.Drawing.Size(260, 68);
            this.ButtonCuring.TabIndex = 3;
            this.ButtonCuring.Text = "Curing";
            this.ButtonCuring.UseVisualStyleBackColor = false;
            this.ButtonCuring.Click += new System.EventHandler(this.ButtonCuring_Click);
            // 
            // buttonClearError
            // 
            this.buttonClearError.BackColor = System.Drawing.Color.Black;
            this.buttonClearError.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Yellow;
            this.buttonClearError.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonClearError.Location = new System.Drawing.Point(838, 360);
            this.buttonClearError.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonClearError.Name = "buttonClearError";
            this.buttonClearError.Size = new System.Drawing.Size(144, 46);
            this.buttonClearError.TabIndex = 4;
            this.buttonClearError.Text = "Clear Errors";
            this.buttonClearError.UseVisualStyleBackColor = false;
            this.buttonClearError.Click += new System.EventHandler(this.ButtonClearError_Click);
            // 
            // buttonCalibration
            // 
            this.buttonCalibration.BackColor = System.Drawing.Color.Black;
            this.buttonCalibration.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Yellow;
            this.buttonCalibration.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonCalibration.Location = new System.Drawing.Point(838, 180);
            this.buttonCalibration.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonCalibration.Name = "buttonCalibration";
            this.buttonCalibration.Size = new System.Drawing.Size(144, 43);
            this.buttonCalibration.TabIndex = 5;
            this.buttonCalibration.Text = "Calibration";
            this.buttonCalibration.UseVisualStyleBackColor = false;
            this.buttonCalibration.Click += new System.EventHandler(this.ButtonCalibration_Click);
            // 
            // labelILValue
            // 
            this.labelILValue.AutoSize = true;
            this.labelILValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelILValue.Location = new System.Drawing.Point(830, 595);
            this.labelILValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelILValue.Name = "labelILValue";
            this.labelILValue.Size = new System.Drawing.Size(203, 46);
            this.labelILValue.TabIndex = 7;
            this.labelILValue.Text = "-50.000dB";
            this.labelILValue.Click += new System.EventHandler(this.IL_Click);
            // 
            // buttonControlBoxDetection
            // 
            this.buttonControlBoxDetection.BackColor = System.Drawing.Color.Black;
            this.buttonControlBoxDetection.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonControlBoxDetection.FlatAppearance.BorderSize = 0;
            this.buttonControlBoxDetection.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Yellow;
            this.buttonControlBoxDetection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonControlBoxDetection.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.buttonControlBoxDetection.Location = new System.Drawing.Point(838, 49);
            this.buttonControlBoxDetection.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonControlBoxDetection.Name = "buttonControlBoxDetection";
            this.buttonControlBoxDetection.Size = new System.Drawing.Size(144, 86);
            this.buttonControlBoxDetection.TabIndex = 8;
            this.buttonControlBoxDetection.Text = "Control Box Detection";
            this.buttonControlBoxDetection.UseVisualStyleBackColor = false;
            this.buttonControlBoxDetection.Click += new System.EventHandler(this.ControlBoxDetection_Click);
            // 
            // buttonTest
            // 
            this.buttonTest.BackColor = System.Drawing.Color.Black;
            this.buttonTest.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Yellow;
            this.buttonTest.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonTest.Location = new System.Drawing.Point(838, 454);
            this.buttonTest.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(144, 46);
            this.buttonTest.TabIndex = 9;
            this.buttonTest.Text = "Test";
            this.buttonTest.UseVisualStyleBackColor = false;
            this.buttonTest.Click += new System.EventHandler(this.Test_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tabControl1.Controls.Add(this.tabPageAlignCuring);
            this.tabControl1.Controls.Add(this.tabPageStageControl);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.ItemSize = new System.Drawing.Size(50, 30);
            this.tabControl1.Location = new System.Drawing.Point(-9, -2);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(0, 0);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1944, 1182);
            this.tabControl1.TabIndex = 10;
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            // 
            // tabPageAlignCuring
            // 
            this.tabPageAlignCuring.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tabPageAlignCuring.Controls.Add(this.buttonChart);
            this.tabPageAlignCuring.Controls.Add(this.comboBoxProductSelect);
            this.tabPageAlignCuring.Controls.Add(this.labelProductSelect);
            this.tabPageAlignCuring.Controls.Add(this.labelUsePiezo);
            this.tabPageAlignCuring.Controls.Add(this.comboBoxUsePiezo);
            this.tabPageAlignCuring.Controls.Add(this.labelTimeValue);
            this.tabPageAlignCuring.Controls.Add(this.labelTime);
            this.tabPageAlignCuring.Controls.Add(this.labelErrorMessage);
            this.tabPageAlignCuring.Controls.Add(this.labelStatusValue);
            this.tabPageAlignCuring.Controls.Add(this.labelStatus);
            this.tabPageAlignCuring.Controls.Add(this.labelTargetValue);
            this.tabPageAlignCuring.Controls.Add(this.labelTarget);
            this.tabPageAlignCuring.Controls.Add(this.buttonCancleRun);
            this.tabPageAlignCuring.Controls.Add(this.buttonClose);
            this.tabPageAlignCuring.Controls.Add(this.buttonConnectPM);
            this.tabPageAlignCuring.Controls.Add(this.buttonDisconnectPM);
            this.tabPageAlignCuring.Controls.Add(this.buttonReference);
            this.tabPageAlignCuring.Controls.Add(this.labelIL);
            this.tabPageAlignCuring.Controls.Add(this.richTextBoxErrorMsg);
            this.tabPageAlignCuring.Controls.Add(this.cartesianChartZ);
            this.tabPageAlignCuring.Controls.Add(this.cartesianChartXY);
            this.tabPageAlignCuring.Controls.Add(this.label_camSelect);
            this.tabPageAlignCuring.Controls.Add(this.comboBoxCamSelect);
            this.tabPageAlignCuring.Controls.Add(this.pictureBoxCam);
            this.tabPageAlignCuring.Controls.Add(this.labelILValue);
            this.tabPageAlignCuring.Controls.Add(this.ButtonReset);
            this.tabPageAlignCuring.Controls.Add(this.buttonTest);
            this.tabPageAlignCuring.Controls.Add(this.ButtonAlignment);
            this.tabPageAlignCuring.Controls.Add(this.ButtonPreCuring);
            this.tabPageAlignCuring.Controls.Add(this.buttonControlBoxDetection);
            this.tabPageAlignCuring.Controls.Add(this.ButtonCuring);
            this.tabPageAlignCuring.Controls.Add(this.buttonCalibration);
            this.tabPageAlignCuring.Controls.Add(this.buttonClearError);
            this.tabPageAlignCuring.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tabPageAlignCuring.Location = new System.Drawing.Point(4, 34);
            this.tabPageAlignCuring.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageAlignCuring.Name = "tabPageAlignCuring";
            this.tabPageAlignCuring.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageAlignCuring.Size = new System.Drawing.Size(1936, 1144);
            this.tabPageAlignCuring.TabIndex = 2;
            this.tabPageAlignCuring.Text = "Alignment & Curing";
            // 
            // buttonChart
            // 
            this.buttonChart.BackColor = System.Drawing.Color.Black;
            this.buttonChart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Yellow;
            this.buttonChart.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonChart.Location = new System.Drawing.Point(22, 1063);
            this.buttonChart.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonChart.Name = "buttonChart";
            this.buttonChart.Size = new System.Drawing.Size(212, 46);
            this.buttonChart.TabIndex = 33;
            this.buttonChart.Text = "Live Charts On/Off";
            this.buttonChart.UseVisualStyleBackColor = false;
            this.buttonChart.Click += new System.EventHandler(this.buttonChart_Click);
            // 
            // comboBoxProductSelect
            // 
            this.comboBoxProductSelect.FormattingEnabled = true;
            this.comboBoxProductSelect.Items.AddRange(new object[] {
            "MM 1xN",
            "SM 1xN"});
            this.comboBoxProductSelect.Location = new System.Drawing.Point(1119, 49);
            this.comboBoxProductSelect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBoxProductSelect.Name = "comboBoxProductSelect";
            this.comboBoxProductSelect.Size = new System.Drawing.Size(122, 33);
            this.comboBoxProductSelect.TabIndex = 32;
            this.comboBoxProductSelect.SelectedIndexChanged += new System.EventHandler(this.comboBoxProductSelect_SelectedIndexChanged);
            // 
            // labelProductSelect
            // 
            this.labelProductSelect.AutoSize = true;
            this.labelProductSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProductSelect.Location = new System.Drawing.Point(1006, 52);
            this.labelProductSelect.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelProductSelect.Name = "labelProductSelect";
            this.labelProductSelect.Size = new System.Drawing.Size(115, 30);
            this.labelProductSelect.TabIndex = 31;
            this.labelProductSelect.Text = "Product: ";
            // 
            // labelUsePiezo
            // 
            this.labelUsePiezo.AutoSize = true;
            this.labelUsePiezo.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUsePiezo.Location = new System.Drawing.Point(1006, 137);
            this.labelUsePiezo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelUsePiezo.Name = "labelUsePiezo";
            this.labelUsePiezo.Size = new System.Drawing.Size(84, 30);
            this.labelUsePiezo.TabIndex = 30;
            this.labelUsePiezo.Text = "Piezo:";
            // 
            // comboBoxUsePiezo
            // 
            this.comboBoxUsePiezo.FormattingEnabled = true;
            this.comboBoxUsePiezo.Items.AddRange(new object[] {
            "Yes",
            "No"});
            this.comboBoxUsePiezo.Location = new System.Drawing.Point(1119, 137);
            this.comboBoxUsePiezo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBoxUsePiezo.Name = "comboBoxUsePiezo";
            this.comboBoxUsePiezo.Size = new System.Drawing.Size(68, 33);
            this.comboBoxUsePiezo.TabIndex = 2;
            this.comboBoxUsePiezo.SelectedIndexChanged += new System.EventHandler(this.comboBoxUsePiezo_SelectedIndexChanged);
            // 
            // labelTimeValue
            // 
            this.labelTimeValue.AutoSize = true;
            this.labelTimeValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTimeValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.labelTimeValue.Location = new System.Drawing.Point(1646, 492);
            this.labelTimeValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTimeValue.Name = "labelTimeValue";
            this.labelTimeValue.Size = new System.Drawing.Size(159, 46);
            this.labelTimeValue.TabIndex = 28;
            this.labelTimeValue.Text = "0min 0s";
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTime.Location = new System.Drawing.Point(1512, 492);
            this.labelTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(136, 46);
            this.labelTime.TabIndex = 27;
            this.labelTime.Text = "Time: ";
            // 
            // labelErrorMessage
            // 
            this.labelErrorMessage.AutoSize = true;
            this.labelErrorMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelErrorMessage.Location = new System.Drawing.Point(1389, 663);
            this.labelErrorMessage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelErrorMessage.Name = "labelErrorMessage";
            this.labelErrorMessage.Size = new System.Drawing.Size(89, 36);
            this.labelErrorMessage.TabIndex = 26;
            this.labelErrorMessage.Text = "Logs:";
            // 
            // labelStatusValue
            // 
            this.labelStatusValue.AutoSize = true;
            this.labelStatusValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatusValue.ForeColor = System.Drawing.Color.Lime;
            this.labelStatusValue.Location = new System.Drawing.Point(1677, 572);
            this.labelStatusValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelStatusValue.Name = "labelStatusValue";
            this.labelStatusValue.Size = new System.Drawing.Size(130, 69);
            this.labelStatusValue.TabIndex = 25;
            this.labelStatusValue.Text = "Idle";
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatus.Location = new System.Drawing.Point(1442, 572);
            this.labelStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(241, 69);
            this.labelStatus.TabIndex = 24;
            this.labelStatus.Text = "Status: ";
            // 
            // labelTargetValue
            // 
            this.labelTargetValue.AutoSize = true;
            this.labelTargetValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTargetValue.Location = new System.Drawing.Point(1110, 595);
            this.labelTargetValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTargetValue.Name = "labelTargetValue";
            this.labelTargetValue.Size = new System.Drawing.Size(203, 46);
            this.labelTargetValue.TabIndex = 23;
            this.labelTargetValue.Text = "-50.000dB";
            // 
            // labelTarget
            // 
            this.labelTarget.AutoSize = true;
            this.labelTarget.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTarget.Location = new System.Drawing.Point(1131, 535);
            this.labelTarget.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTarget.Name = "labelTarget";
            this.labelTarget.Size = new System.Drawing.Size(164, 46);
            this.labelTarget.TabIndex = 22;
            this.labelTarget.Text = "Target: ";
            // 
            // buttonCancleRun
            // 
            this.buttonCancleRun.BackColor = System.Drawing.Color.Black;
            this.buttonCancleRun.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Yellow;
            this.buttonCancleRun.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonCancleRun.Location = new System.Drawing.Point(1323, 405);
            this.buttonCancleRun.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonCancleRun.Name = "buttonCancleRun";
            this.buttonCancleRun.Size = new System.Drawing.Size(170, 43);
            this.buttonCancleRun.TabIndex = 21;
            this.buttonCancleRun.Text = "Cancle Run";
            this.buttonCancleRun.UseVisualStyleBackColor = false;
            this.buttonCancleRun.Click += new System.EventHandler(this.buttonCancleRun_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.BackColor = System.Drawing.Color.Black;
            this.buttonClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Yellow;
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonClose.Location = new System.Drawing.Point(1323, 315);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(170, 43);
            this.buttonClose.TabIndex = 20;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonConnectPM
            // 
            this.buttonConnectPM.BackColor = System.Drawing.Color.Black;
            this.buttonConnectPM.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Yellow;
            this.buttonConnectPM.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonConnectPM.Location = new System.Drawing.Point(1323, 188);
            this.buttonConnectPM.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonConnectPM.Name = "buttonConnectPM";
            this.buttonConnectPM.Size = new System.Drawing.Size(170, 82);
            this.buttonConnectPM.TabIndex = 19;
            this.buttonConnectPM.Text = "Connect PM";
            this.buttonConnectPM.UseVisualStyleBackColor = false;
            this.buttonConnectPM.Click += new System.EventHandler(this.buttonConnectPM_Click);
            // 
            // buttonDisconnectPM
            // 
            this.buttonDisconnectPM.BackColor = System.Drawing.Color.Black;
            this.buttonDisconnectPM.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Yellow;
            this.buttonDisconnectPM.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonDisconnectPM.Location = new System.Drawing.Point(1323, 52);
            this.buttonDisconnectPM.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonDisconnectPM.Name = "buttonDisconnectPM";
            this.buttonDisconnectPM.Size = new System.Drawing.Size(170, 86);
            this.buttonDisconnectPM.TabIndex = 18;
            this.buttonDisconnectPM.Text = "Disconnect PM";
            this.buttonDisconnectPM.UseVisualStyleBackColor = false;
            this.buttonDisconnectPM.Click += new System.EventHandler(this.buttonDisconnectPM_Click);
            // 
            // buttonReference
            // 
            this.buttonReference.BackColor = System.Drawing.Color.Black;
            this.buttonReference.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Yellow;
            this.buttonReference.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonReference.Location = new System.Drawing.Point(838, 275);
            this.buttonReference.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonReference.Name = "buttonReference";
            this.buttonReference.Size = new System.Drawing.Size(144, 43);
            this.buttonReference.TabIndex = 17;
            this.buttonReference.Text = "Reference";
            this.buttonReference.UseVisualStyleBackColor = false;
            this.buttonReference.Click += new System.EventHandler(this.buttonReference_Click);
            // 
            // labelIL
            // 
            this.labelIL.AutoSize = true;
            this.labelIL.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelIL.Location = new System.Drawing.Point(898, 535);
            this.labelIL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelIL.Name = "labelIL";
            this.labelIL.Size = new System.Drawing.Size(78, 46);
            this.labelIL.TabIndex = 16;
            this.labelIL.Text = "IL: ";
            // 
            // richTextBoxErrorMsg
            // 
            this.richTextBoxErrorMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxErrorMsg.Location = new System.Drawing.Point(1396, 706);
            this.richTextBoxErrorMsg.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.richTextBoxErrorMsg.Name = "richTextBoxErrorMsg";
            this.richTextBoxErrorMsg.Size = new System.Drawing.Size(460, 346);
            this.richTextBoxErrorMsg.TabIndex = 15;
            this.richTextBoxErrorMsg.Text = "";
            this.richTextBoxErrorMsg.WordWrap = false;
            // 
            // cartesianChartZ
            // 
            this.cartesianChartZ.Location = new System.Drawing.Point(717, 663);
            this.cartesianChartZ.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cartesianChartZ.Name = "cartesianChartZ";
            this.cartesianChartZ.Size = new System.Drawing.Size(638, 391);
            this.cartesianChartZ.TabIndex = 14;
            this.cartesianChartZ.Text = "cartesianChart2";
            // 
            // cartesianChartXY
            // 
            this.cartesianChartXY.Location = new System.Drawing.Point(21, 663);
            this.cartesianChartXY.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cartesianChartXY.Name = "cartesianChartXY";
            this.cartesianChartXY.Size = new System.Drawing.Size(638, 391);
            this.cartesianChartXY.TabIndex = 13;
            this.cartesianChartXY.Text = "cartesianChart1";
            // 
            // label_camSelect
            // 
            this.label_camSelect.AutoSize = true;
            this.label_camSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_camSelect.Location = new System.Drawing.Point(16, 18);
            this.label_camSelect.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_camSelect.Name = "label_camSelect";
            this.label_camSelect.Size = new System.Drawing.Size(207, 30);
            this.label_camSelect.TabIndex = 12;
            this.label_camSelect.Text = "Camera Source: ";
            // 
            // comboBoxCamSelect
            // 
            this.comboBoxCamSelect.FormattingEnabled = true;
            this.comboBoxCamSelect.Location = new System.Drawing.Point(243, 15);
            this.comboBoxCamSelect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBoxCamSelect.Name = "comboBoxCamSelect";
            this.comboBoxCamSelect.Size = new System.Drawing.Size(250, 33);
            this.comboBoxCamSelect.TabIndex = 11;
            this.comboBoxCamSelect.SelectedIndexChanged += new System.EventHandler(this.comboBoxCamSelect_SelectedIndexChanged);
            // 
            // pictureBoxCam
            // 
            this.pictureBoxCam.Location = new System.Drawing.Point(12, 54);
            this.pictureBoxCam.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBoxCam.Name = "pictureBoxCam";
            this.pictureBoxCam.Size = new System.Drawing.Size(750, 577);
            this.pictureBoxCam.TabIndex = 10;
            this.pictureBoxCam.TabStop = false;
            // 
            // tabPageStageControl
            // 
            this.tabPageStageControl.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPageStageControl.Controls.Add(this.cartesianChartMotorC);
            this.tabPageStageControl.Controls.Add(this.panel1);
            this.tabPageStageControl.Controls.Add(this.cartesianChartMotorB);
            this.tabPageStageControl.Controls.Add(this.cartesianChartMotorA);
            this.tabPageStageControl.Controls.Add(this.pictureBoxFigure);
            this.tabPageStageControl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tabPageStageControl.Location = new System.Drawing.Point(4, 34);
            this.tabPageStageControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageStageControl.Name = "tabPageStageControl";
            this.tabPageStageControl.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageStageControl.Size = new System.Drawing.Size(1936, 1144);
            this.tabPageStageControl.TabIndex = 1;
            this.tabPageStageControl.Text = "Stage Control";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gainsboro;
            this.panel1.Controls.Add(this.buttonClearErrorBC);
            this.panel1.Controls.Add(this.labelStatusValueBC);
            this.panel1.Controls.Add(this.labelStatusBC);
            this.panel1.Controls.Add(this.buttonSetInitial);
            this.panel1.Controls.Add(this.numericUpDownPz);
            this.panel1.Controls.Add(this.numericUpDownPy);
            this.panel1.Controls.Add(this.numericUpDownPx);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.numericUpDownRz);
            this.panel1.Controls.Add(this.numericUpDownRy);
            this.panel1.Controls.Add(this.numericUpDownRx);
            this.panel1.Controls.Add(this.numericUpDownZ);
            this.panel1.Controls.Add(this.numericUpDownY);
            this.panel1.Controls.Add(this.numericUpDownX);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.buttonSetPosition);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.buttonSetPivot);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label_Z);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label_Y);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label_X);
            this.panel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(672, 1125);
            this.panel1.TabIndex = 25;
            // 
            // buttonClearErrorBC
            // 
            this.buttonClearErrorBC.BackColor = System.Drawing.Color.Silver;
            this.buttonClearErrorBC.FlatAppearance.BorderSize = 0;
            this.buttonClearErrorBC.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            this.buttonClearErrorBC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClearErrorBC.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonClearErrorBC.Location = new System.Drawing.Point(458, 1002);
            this.buttonClearErrorBC.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonClearErrorBC.Name = "buttonClearErrorBC";
            this.buttonClearErrorBC.Size = new System.Drawing.Size(172, 71);
            this.buttonClearErrorBC.TabIndex = 37;
            this.buttonClearErrorBC.Text = "Clear Error";
            this.buttonClearErrorBC.UseVisualStyleBackColor = false;
            this.buttonClearErrorBC.Click += new System.EventHandler(this.buttonClearErrorBC_Click);
            // 
            // labelStatusValueBC
            // 
            this.labelStatusValueBC.AutoSize = true;
            this.labelStatusValueBC.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatusValueBC.ForeColor = System.Drawing.Color.Lime;
            this.labelStatusValueBC.Location = new System.Drawing.Point(250, 1002);
            this.labelStatusValueBC.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelStatusValueBC.Name = "labelStatusValueBC";
            this.labelStatusValueBC.Size = new System.Drawing.Size(167, 69);
            this.labelStatusValueBC.TabIndex = 36;
            this.labelStatusValueBC.Text = "IDLE";
            // 
            // labelStatusBC
            // 
            this.labelStatusBC.AutoSize = true;
            this.labelStatusBC.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatusBC.Location = new System.Drawing.Point(14, 1002);
            this.labelStatusBC.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelStatusBC.Name = "labelStatusBC";
            this.labelStatusBC.Size = new System.Drawing.Size(241, 69);
            this.labelStatusBC.TabIndex = 35;
            this.labelStatusBC.Text = "Status: ";
            // 
            // buttonSetInitial
            // 
            this.buttonSetInitial.BackColor = System.Drawing.Color.Silver;
            this.buttonSetInitial.FlatAppearance.BorderSize = 0;
            this.buttonSetInitial.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            this.buttonSetInitial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSetInitial.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSetInitial.Location = new System.Drawing.Point(435, 300);
            this.buttonSetInitial.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonSetInitial.Name = "buttonSetInitial";
            this.buttonSetInitial.Size = new System.Drawing.Size(214, 60);
            this.buttonSetInitial.TabIndex = 34;
            this.buttonSetInitial.Text = "Set Initial";
            this.buttonSetInitial.UseVisualStyleBackColor = false;
            this.buttonSetInitial.Click += new System.EventHandler(this.buttonSetInitial_Click);
            // 
            // numericUpDownPz
            // 
            this.numericUpDownPz.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownPz.DecimalPlaces = 1;
            this.numericUpDownPz.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownPz.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownPz.Location = new System.Drawing.Point(464, 871);
            this.numericUpDownPz.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numericUpDownPz.Name = "numericUpDownPz";
            this.numericUpDownPz.Size = new System.Drawing.Size(146, 62);
            this.numericUpDownPz.TabIndex = 33;
            // 
            // numericUpDownPy
            // 
            this.numericUpDownPy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownPy.DecimalPlaces = 1;
            this.numericUpDownPy.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownPy.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownPy.Location = new System.Drawing.Point(243, 871);
            this.numericUpDownPy.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numericUpDownPy.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownPy.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.numericUpDownPy.Name = "numericUpDownPy";
            this.numericUpDownPy.Size = new System.Drawing.Size(146, 62);
            this.numericUpDownPy.TabIndex = 32;
            // 
            // numericUpDownPx
            // 
            this.numericUpDownPx.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownPx.DecimalPlaces = 1;
            this.numericUpDownPx.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownPx.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownPx.Location = new System.Drawing.Point(26, 871);
            this.numericUpDownPx.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numericUpDownPx.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownPx.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.numericUpDownPx.Name = "numericUpDownPx";
            this.numericUpDownPx.Size = new System.Drawing.Size(146, 62);
            this.numericUpDownPx.TabIndex = 31;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Agency FB", 33F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label9.Location = new System.Drawing.Point(134, 188);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(360, 79);
            this.label9.TabIndex = 30;
            this.label9.Text = "Beetle Control";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(148, 37);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(248, 112);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 29;
            this.pictureBox1.TabStop = false;
            // 
            // numericUpDownRz
            // 
            this.numericUpDownRz.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownRz.DecimalPlaces = 1;
            this.numericUpDownRz.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownRz.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownRz.Location = new System.Drawing.Point(435, 582);
            this.numericUpDownRz.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numericUpDownRz.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.numericUpDownRz.Minimum = new decimal(new int[] {
            6,
            0,
            0,
            -2147483648});
            this.numericUpDownRz.Name = "numericUpDownRz";
            this.numericUpDownRz.Size = new System.Drawing.Size(184, 62);
            this.numericUpDownRz.TabIndex = 28;
            // 
            // numericUpDownRy
            // 
            this.numericUpDownRy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownRy.DecimalPlaces = 1;
            this.numericUpDownRy.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownRy.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownRy.Location = new System.Drawing.Point(234, 582);
            this.numericUpDownRy.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numericUpDownRy.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.numericUpDownRy.Minimum = new decimal(new int[] {
            6,
            0,
            0,
            -2147483648});
            this.numericUpDownRy.Name = "numericUpDownRy";
            this.numericUpDownRy.Size = new System.Drawing.Size(184, 62);
            this.numericUpDownRy.TabIndex = 27;
            // 
            // numericUpDownRx
            // 
            this.numericUpDownRx.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownRx.DecimalPlaces = 1;
            this.numericUpDownRx.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownRx.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownRx.Location = new System.Drawing.Point(33, 582);
            this.numericUpDownRx.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numericUpDownRx.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.numericUpDownRx.Minimum = new decimal(new int[] {
            6,
            0,
            0,
            -2147483648});
            this.numericUpDownRx.Name = "numericUpDownRx";
            this.numericUpDownRx.Size = new System.Drawing.Size(184, 62);
            this.numericUpDownRx.TabIndex = 26;
            // 
            // numericUpDownZ
            // 
            this.numericUpDownZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownZ.DecimalPlaces = 4;
            this.numericUpDownZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownZ.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numericUpDownZ.Location = new System.Drawing.Point(422, 429);
            this.numericUpDownZ.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numericUpDownZ.Maximum = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.numericUpDownZ.Minimum = new decimal(new int[] {
            132,
            0,
            0,
            -2147483648});
            this.numericUpDownZ.Name = "numericUpDownZ";
            this.numericUpDownZ.Size = new System.Drawing.Size(249, 62);
            this.numericUpDownZ.TabIndex = 25;
            this.numericUpDownZ.Value = new decimal(new int[] {
            140,
            0,
            0,
            0});
            // 
            // numericUpDownY
            // 
            this.numericUpDownY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownY.DecimalPlaces = 4;
            this.numericUpDownY.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownY.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numericUpDownY.Location = new System.Drawing.Point(214, 429);
            this.numericUpDownY.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numericUpDownY.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.numericUpDownY.Minimum = new decimal(new int[] {
            9,
            0,
            0,
            -2147483648});
            this.numericUpDownY.Name = "numericUpDownY";
            this.numericUpDownY.Size = new System.Drawing.Size(202, 62);
            this.numericUpDownY.TabIndex = 24;
            // 
            // numericUpDownX
            // 
            this.numericUpDownX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownX.DecimalPlaces = 4;
            this.numericUpDownX.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownX.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numericUpDownX.Location = new System.Drawing.Point(4, 429);
            this.numericUpDownX.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numericUpDownX.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.numericUpDownX.Minimum = new decimal(new int[] {
            9,
            0,
            0,
            -2147483648});
            this.numericUpDownX.Name = "numericUpDownX";
            this.numericUpDownX.Size = new System.Drawing.Size(206, 62);
            this.numericUpDownX.TabIndex = 23;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 746);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(290, 58);
            this.label2.TabIndex = 15;
            this.label2.Text = "Pivot Point: ";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(15, 806);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(176, 58);
            this.label8.TabIndex = 16;
            this.label8.Text = "X(mm)";
            // 
            // buttonSetPosition
            // 
            this.buttonSetPosition.BackColor = System.Drawing.Color.Silver;
            this.buttonSetPosition.FlatAppearance.BorderSize = 0;
            this.buttonSetPosition.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            this.buttonSetPosition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSetPosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSetPosition.Location = new System.Drawing.Point(236, 300);
            this.buttonSetPosition.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonSetPosition.Name = "buttonSetPosition";
            this.buttonSetPosition.Size = new System.Drawing.Size(160, 60);
            this.buttonSetPosition.TabIndex = 13;
            this.buttonSetPosition.Text = "Go";
            this.buttonSetPosition.UseVisualStyleBackColor = false;
            this.buttonSetPosition.Click += new System.EventHandler(this.buttonSetPosition_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 300);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(230, 58);
            this.label1.TabIndex = 14;
            this.label1.Text = "Position: ";
            // 
            // buttonSetPivot
            // 
            this.buttonSetPivot.BackColor = System.Drawing.Color.Silver;
            this.buttonSetPivot.FlatAppearance.BorderSize = 0;
            this.buttonSetPivot.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            this.buttonSetPivot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSetPivot.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSetPivot.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.buttonSetPivot.Location = new System.Drawing.Point(300, 746);
            this.buttonSetPivot.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonSetPivot.Name = "buttonSetPivot";
            this.buttonSetPivot.Size = new System.Drawing.Size(330, 60);
            this.buttonSetPivot.TabIndex = 22;
            this.buttonSetPivot.Text = "Set Pivot Point";
            this.buttonSetPivot.UseVisualStyleBackColor = false;
            this.buttonSetPivot.Click += new System.EventHandler(this.buttonSetPivot_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(231, 806);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(176, 58);
            this.label7.TabIndex = 17;
            this.label7.Text = "Y(mm)";
            // 
            // label_Z
            // 
            this.label_Z.AutoSize = true;
            this.label_Z.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Z.Location = new System.Drawing.Point(447, 365);
            this.label_Z.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Z.Name = "label_Z";
            this.label_Z.Size = new System.Drawing.Size(178, 55);
            this.label_Z.TabIndex = 3;
            this.label_Z.Text = "Z (mm)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(42, 520);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(152, 55);
            this.label3.TabIndex = 4;
            this.label3.Text = "θx ( °)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(244, 520);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(152, 55);
            this.label4.TabIndex = 5;
            this.label4.Text = "θy ( °)";
            // 
            // label_Y
            // 
            this.label_Y.AutoSize = true;
            this.label_Y.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Y.Location = new System.Drawing.Point(224, 365);
            this.label_Y.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Y.Name = "label_Y";
            this.label_Y.Size = new System.Drawing.Size(181, 55);
            this.label_Y.TabIndex = 2;
            this.label_Y.Text = "Y (mm)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(446, 520);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(152, 55);
            this.label5.TabIndex = 6;
            this.label5.Text = "θz ( °)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(453, 806);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(174, 58);
            this.label6.TabIndex = 18;
            this.label6.Text = "Z(mm)";
            // 
            // label_X
            // 
            this.label_X.AutoSize = true;
            this.label_X.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_X.Location = new System.Drawing.Point(14, 365);
            this.label_X.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_X.Name = "label_X";
            this.label_X.Size = new System.Drawing.Size(181, 55);
            this.label_X.TabIndex = 1;
            this.label_X.Text = "X (mm)";
            // 
            // cartesianChartMotorB
            // 
            this.cartesianChartMotorB.Location = new System.Drawing.Point(1434, 314);
            this.cartesianChartMotorB.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cartesianChartMotorB.Name = "cartesianChartMotorB";
            this.cartesianChartMotorB.Size = new System.Drawing.Size(452, 306);
            this.cartesianChartMotorB.TabIndex = 24;
            this.cartesianChartMotorB.Text = "cartesianChart4";
            // 
            // cartesianChartMotorA
            // 
            this.cartesianChartMotorA.Location = new System.Drawing.Point(1434, 0);
            this.cartesianChartMotorA.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cartesianChartMotorA.Name = "cartesianChartMotorA";
            this.cartesianChartMotorA.Size = new System.Drawing.Size(452, 291);
            this.cartesianChartMotorA.TabIndex = 23;
            this.cartesianChartMotorA.Text = "cartesianChart3";
            // 
            // pictureBoxFigure
            // 
            this.pictureBoxFigure.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxFigure.Image = global::Beetle.Properties.Resources.Beetle_Indicate;
            this.pictureBoxFigure.Location = new System.Drawing.Point(668, 0);
            this.pictureBoxFigure.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBoxFigure.Name = "pictureBoxFigure";
            this.pictureBoxFigure.Size = new System.Drawing.Size(758, 1123);
            this.pictureBoxFigure.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxFigure.TabIndex = 0;
            this.pictureBoxFigure.TabStop = false;
            // 
            // refreshTimer
            // 
            this.refreshTimer.Enabled = true;
            this.refreshTimer.Interval = 250;
            this.refreshTimer.Tick += new System.EventHandler(this.RefreshTimer_Tick);
            // 
            // cartesianChartMotorC
            // 
            this.cartesianChartMotorC.Location = new System.Drawing.Point(1434, 718);
            this.cartesianChartMotorC.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cartesianChartMotorC.Name = "cartesianChartMotorC";
            this.cartesianChartMotorC.Size = new System.Drawing.Size(452, 310);
            this.cartesianChartMotorC.TabIndex = 26;
            this.cartesianChartMotorC.Text = "cartesianChart4";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1926, 1171);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Beetle";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageAlignCuring.ResumeLayout(false);
            this.tabPageAlignCuring.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCam)).EndInit();
            this.tabPageStageControl.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPz)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRz)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFigure)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ButtonReset;
        private System.Windows.Forms.Button ButtonAlignment;
        private System.Windows.Forms.Button ButtonPreCuring;
        private System.Windows.Forms.Button ButtonCuring;
        private System.Windows.Forms.Button buttonClearError;
        private System.Windows.Forms.Button buttonCalibration;
        private System.Windows.Forms.Label labelILValue;
        private System.Windows.Forms.Button buttonControlBoxDetection;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageStageControl;
        private System.Windows.Forms.TabPage tabPageAlignCuring;
        private System.Windows.Forms.PictureBox pictureBoxCam;
        private System.Windows.Forms.Timer refreshTimer;
        private System.Windows.Forms.ComboBox comboBoxCamSelect;
        private System.Windows.Forms.Label label_camSelect;
        private LiveCharts.WinForms.CartesianChart cartesianChartXY;
        private System.Windows.Forms.Label labelIL;
        private System.Windows.Forms.RichTextBox richTextBoxErrorMsg;
        private LiveCharts.WinForms.CartesianChart cartesianChartZ;
        private System.Windows.Forms.PictureBox pictureBoxFigure;
        private System.Windows.Forms.Label label_Y;
        private System.Windows.Forms.Label label_X;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label_Z;
        private LiveCharts.WinForms.CartesianChart cartesianChartMotorB;
        private LiveCharts.WinForms.CartesianChart cartesianChartMotorA;
        private System.Windows.Forms.Button buttonSetPivot;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonSetPosition;
        private System.Windows.Forms.Button buttonCancleRun;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonConnectPM;
        private System.Windows.Forms.Button buttonDisconnectPM;
        private System.Windows.Forms.Button buttonReference;
        private System.Windows.Forms.Label labelTargetValue;
        private System.Windows.Forms.Label labelTarget;
        private System.Windows.Forms.Label labelTimeValue;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.Label labelErrorMessage;
        private System.Windows.Forms.Label labelStatusValue;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Label labelUsePiezo;
        private System.Windows.Forms.ComboBox comboBoxUsePiezo;
        private System.Windows.Forms.ComboBox comboBoxProductSelect;
        private System.Windows.Forms.Label labelProductSelect;
        private System.Windows.Forms.Button buttonChart;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown numericUpDownRz;
        private System.Windows.Forms.NumericUpDown numericUpDownRy;
        private System.Windows.Forms.NumericUpDown numericUpDownRx;
        private System.Windows.Forms.NumericUpDown numericUpDownZ;
        private System.Windows.Forms.NumericUpDown numericUpDownY;
        private System.Windows.Forms.NumericUpDown numericUpDownX;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown numericUpDownPz;
        private System.Windows.Forms.NumericUpDown numericUpDownPy;
        private System.Windows.Forms.NumericUpDown numericUpDownPx;
        private System.Windows.Forms.Button buttonSetInitial;
        private System.Windows.Forms.Label labelStatusValueBC;
        private System.Windows.Forms.Label labelStatusBC;
        private System.Windows.Forms.Button buttonClearErrorBC;
        private LiveCharts.WinForms.CartesianChart cartesianChartMotorC;
    }
}

