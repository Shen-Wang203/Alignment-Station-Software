namespace Beetle
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
            this.comboBoxPMChl = new System.Windows.Forms.ComboBox();
            this.labelPMChl = new System.Windows.Forms.Label();
            this.labelPiezoPositionValue = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.labelPiezoPosition = new System.Windows.Forms.Label();
            this.labelPositionAngles = new System.Windows.Forms.Label();
            this.labelPositionXYZ = new System.Windows.Forms.Label();
            this.labelBeetlePosition = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.comboBoxFixtureNum = new System.Windows.Forms.ComboBox();
            this.labelBeetleFixture = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.buttonChartOnOff = new System.Windows.Forms.Button();
            this.labelErrorMessage = new System.Windows.Forms.Label();
            this.comboBoxProductSelect = new System.Windows.Forms.ComboBox();
            this.buttonReference = new System.Windows.Forms.Button();
            this.labelProductSelect = new System.Windows.Forms.Label();
            this.labelUsePiezo = new System.Windows.Forms.Label();
            this.comboBoxUsePiezo = new System.Windows.Forms.ComboBox();
            this.richTextBoxErrorMsg = new System.Windows.Forms.RichTextBox();
            this.buttonCancleRun = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.labelTimeValue = new System.Windows.Forms.Label();
            this.labelTime = new System.Windows.Forms.Label();
            this.labelStatusValue = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.labelTargetValue = new System.Windows.Forms.Label();
            this.labelTarget = new System.Windows.Forms.Label();
            this.labelIL = new System.Windows.Forms.Label();
            this.cartesianChartZ = new LiveCharts.WinForms.CartesianChart();
            this.buttonDisconnectPM = new System.Windows.Forms.Button();
            this.cartesianChartXY = new LiveCharts.WinForms.CartesianChart();
            this.label_camSelect = new System.Windows.Forms.Label();
            this.buttonConnectPM = new System.Windows.Forms.Button();
            this.comboBoxCamSelect = new System.Windows.Forms.ComboBox();
            this.pictureBoxCam = new System.Windows.Forms.PictureBox();
            this.tabPageStageControl = new System.Windows.Forms.TabPage();
            this.labelRy = new System.Windows.Forms.Label();
            this.labelRz = new System.Windows.Forms.Label();
            this.labelRx = new System.Windows.Forms.Label();
            this.labelZ = new System.Windows.Forms.Label();
            this.labelT = new System.Windows.Forms.Label();
            this.labelX = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.comboBoxMotorSelectTop = new System.Windows.Forms.ComboBox();
            this.buttonSetInitial = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.numericUpDownPx = new System.Windows.Forms.NumericUpDown();
            this.buttonSetPivot = new System.Windows.Forms.Button();
            this.numericUpDownPy = new System.Windows.Forms.NumericUpDown();
            this.buttonClearErrorBC = new System.Windows.Forms.Button();
            this.numericUpDownPz = new System.Windows.Forms.NumericUpDown();
            this.buttonSetPosition = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBoxMotorSelectMid = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.comboBoxMotorSelectBot = new System.Windows.Forms.ComboBox();
            this.cartesianChartMotorC = new LiveCharts.WinForms.CartesianChart();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelILValueBC = new System.Windows.Forms.Label();
            this.labelILBC = new System.Windows.Forms.Label();
            this.labelPiezoPositionBC = new System.Windows.Forms.Label();
            this.labelPiezoPositionValueBC = new System.Windows.Forms.Label();
            this.buttonPiezoReset = new System.Windows.Forms.Button();
            this.comboBoxPiezoStep = new System.Windows.Forms.ComboBox();
            this.buttonPiezoSearch = new System.Windows.Forms.Button();
            this.labelPiezoStep = new System.Windows.Forms.Label();
            this.pictureBoxCamBC = new System.Windows.Forms.PictureBox();
            this.labelStatusValueBC = new System.Windows.Forms.Label();
            this.labelStatusBC = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.numericUpDownY = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownRz = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.cartesianChartMotorB = new LiveCharts.WinForms.CartesianChart();
            this.numericUpDownRy = new System.Windows.Forms.NumericUpDown();
            this.cartesianChartMotorA = new LiveCharts.WinForms.CartesianChart();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDownZ = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownRx = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownX = new System.Windows.Forms.NumericUpDown();
            this.pictureBoxFigure = new System.Windows.Forms.PictureBox();
            this.refreshTimer = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPageAlignCuring.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCam)).BeginInit();
            this.tabPageStageControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPz)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCamBC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRz)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFigure)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonReset
            // 
            this.ButtonReset.BackColor = System.Drawing.Color.Red;
            this.ButtonReset.FlatAppearance.BorderSize = 0;
            this.ButtonReset.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Orange;
            this.ButtonReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonReset.ForeColor = System.Drawing.Color.Black;
            this.ButtonReset.Location = new System.Drawing.Point(2725, 235);
            this.ButtonReset.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.ButtonReset.Name = "ButtonReset";
            this.ButtonReset.Size = new System.Drawing.Size(450, 150);
            this.ButtonReset.TabIndex = 0;
            this.ButtonReset.Text = "Reset";
            this.ButtonReset.UseVisualStyleBackColor = false;
            this.ButtonReset.Click += new System.EventHandler(this.ButtonReset_Click);
            // 
            // ButtonAlignment
            // 
            this.ButtonAlignment.BackColor = System.Drawing.Color.Red;
            this.ButtonAlignment.Enabled = false;
            this.ButtonAlignment.FlatAppearance.BorderSize = 0;
            this.ButtonAlignment.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Orange;
            this.ButtonAlignment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonAlignment.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonAlignment.ForeColor = System.Drawing.Color.Black;
            this.ButtonAlignment.Location = new System.Drawing.Point(2725, 452);
            this.ButtonAlignment.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.ButtonAlignment.Name = "ButtonAlignment";
            this.ButtonAlignment.Size = new System.Drawing.Size(450, 150);
            this.ButtonAlignment.TabIndex = 1;
            this.ButtonAlignment.Text = "Alignment";
            this.ButtonAlignment.UseVisualStyleBackColor = false;
            this.ButtonAlignment.Click += new System.EventHandler(this.ButtonAlignment_Click);
            // 
            // ButtonPreCuring
            // 
            this.ButtonPreCuring.BackColor = System.Drawing.Color.Red;
            this.ButtonPreCuring.Enabled = false;
            this.ButtonPreCuring.FlatAppearance.BorderSize = 0;
            this.ButtonPreCuring.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Orange;
            this.ButtonPreCuring.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonPreCuring.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonPreCuring.ForeColor = System.Drawing.Color.Black;
            this.ButtonPreCuring.Location = new System.Drawing.Point(2725, 660);
            this.ButtonPreCuring.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.ButtonPreCuring.Name = "ButtonPreCuring";
            this.ButtonPreCuring.Size = new System.Drawing.Size(450, 150);
            this.ButtonPreCuring.TabIndex = 2;
            this.ButtonPreCuring.Text = "PreCuring";
            this.ButtonPreCuring.UseVisualStyleBackColor = false;
            this.ButtonPreCuring.Click += new System.EventHandler(this.ButtonPreCuring_Click);
            // 
            // ButtonCuring
            // 
            this.ButtonCuring.BackColor = System.Drawing.Color.Red;
            this.ButtonCuring.Enabled = false;
            this.ButtonCuring.FlatAppearance.BorderSize = 0;
            this.ButtonCuring.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Orange;
            this.ButtonCuring.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonCuring.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonCuring.ForeColor = System.Drawing.Color.Black;
            this.ButtonCuring.Location = new System.Drawing.Point(2728, 872);
            this.ButtonCuring.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.ButtonCuring.Name = "ButtonCuring";
            this.ButtonCuring.Size = new System.Drawing.Size(450, 150);
            this.ButtonCuring.TabIndex = 3;
            this.ButtonCuring.Text = "Curing";
            this.ButtonCuring.UseVisualStyleBackColor = false;
            this.ButtonCuring.Click += new System.EventHandler(this.ButtonCuring_Click);
            // 
            // buttonClearError
            // 
            this.buttonClearError.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.buttonClearError.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonClearError.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Indigo;
            this.buttonClearError.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClearError.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonClearError.Location = new System.Drawing.Point(402, 498);
            this.buttonClearError.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.buttonClearError.Name = "buttonClearError";
            this.buttonClearError.Size = new System.Drawing.Size(400, 140);
            this.buttonClearError.TabIndex = 4;
            this.buttonClearError.Text = "Clear Errors";
            this.buttonClearError.UseVisualStyleBackColor = false;
            this.buttonClearError.Click += new System.EventHandler(this.ButtonClearError_Click);
            // 
            // buttonCalibration
            // 
            this.buttonCalibration.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.buttonCalibration.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonCalibration.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Indigo;
            this.buttonCalibration.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCalibration.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCalibration.Location = new System.Drawing.Point(402, 355);
            this.buttonCalibration.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.buttonCalibration.Name = "buttonCalibration";
            this.buttonCalibration.Size = new System.Drawing.Size(400, 140);
            this.buttonCalibration.TabIndex = 5;
            this.buttonCalibration.Text = "Calibration";
            this.buttonCalibration.UseVisualStyleBackColor = false;
            this.buttonCalibration.Click += new System.EventHandler(this.ButtonCalibration_Click);
            // 
            // labelILValue
            // 
            this.labelILValue.AutoSize = true;
            this.labelILValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelILValue.ForeColor = System.Drawing.Color.Yellow;
            this.labelILValue.Location = new System.Drawing.Point(2070, 765);
            this.labelILValue.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelILValue.Name = "labelILValue";
            this.labelILValue.Size = new System.Drawing.Size(512, 113);
            this.labelILValue.TabIndex = 7;
            this.labelILValue.Text = "-50.000dB";
            this.labelILValue.Click += new System.EventHandler(this.IL_Click);
            // 
            // buttonControlBoxDetection
            // 
            this.buttonControlBoxDetection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.buttonControlBoxDetection.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonControlBoxDetection.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Indigo;
            this.buttonControlBoxDetection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonControlBoxDetection.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonControlBoxDetection.ForeColor = System.Drawing.Color.Silver;
            this.buttonControlBoxDetection.Location = new System.Drawing.Point(5, 355);
            this.buttonControlBoxDetection.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.buttonControlBoxDetection.Name = "buttonControlBoxDetection";
            this.buttonControlBoxDetection.Size = new System.Drawing.Size(400, 140);
            this.buttonControlBoxDetection.TabIndex = 8;
            this.buttonControlBoxDetection.Text = "Control Box Detection";
            this.buttonControlBoxDetection.UseVisualStyleBackColor = false;
            this.buttonControlBoxDetection.Click += new System.EventHandler(this.ControlBoxDetection_Click);
            // 
            // buttonTest
            // 
            this.buttonTest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.buttonTest.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonTest.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Indigo;
            this.buttonTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonTest.Location = new System.Drawing.Point(5, 782);
            this.buttonTest.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(400, 140);
            this.buttonTest.TabIndex = 9;
            this.buttonTest.Text = "Test";
            this.buttonTest.UseVisualStyleBackColor = false;
            this.buttonTest.Click += new System.EventHandler(this.Test_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabControl1.Controls.Add(this.tabPageAlignCuring);
            this.tabControl1.Controls.Add(this.tabPageStageControl);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.HotTrack = true;
            this.tabControl1.ItemSize = new System.Drawing.Size(50, 30);
            this.tabControl1.Location = new System.Drawing.Point(-8, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(0, 0);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(3225, 1800);
            this.tabControl1.TabIndex = 10;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPageAlignCuring
            // 
            this.tabPageAlignCuring.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(64)))));
            this.tabPageAlignCuring.Controls.Add(this.comboBoxPMChl);
            this.tabPageAlignCuring.Controls.Add(this.labelPMChl);
            this.tabPageAlignCuring.Controls.Add(this.labelPiezoPositionValue);
            this.tabPageAlignCuring.Controls.Add(this.label13);
            this.tabPageAlignCuring.Controls.Add(this.labelPiezoPosition);
            this.tabPageAlignCuring.Controls.Add(this.labelPositionAngles);
            this.tabPageAlignCuring.Controls.Add(this.labelPositionXYZ);
            this.tabPageAlignCuring.Controls.Add(this.labelBeetlePosition);
            this.tabPageAlignCuring.Controls.Add(this.panel2);
            this.tabPageAlignCuring.Controls.Add(this.labelTimeValue);
            this.tabPageAlignCuring.Controls.Add(this.labelTime);
            this.tabPageAlignCuring.Controls.Add(this.labelStatusValue);
            this.tabPageAlignCuring.Controls.Add(this.labelStatus);
            this.tabPageAlignCuring.Controls.Add(this.labelTargetValue);
            this.tabPageAlignCuring.Controls.Add(this.labelTarget);
            this.tabPageAlignCuring.Controls.Add(this.labelIL);
            this.tabPageAlignCuring.Controls.Add(this.cartesianChartZ);
            this.tabPageAlignCuring.Controls.Add(this.buttonDisconnectPM);
            this.tabPageAlignCuring.Controls.Add(this.cartesianChartXY);
            this.tabPageAlignCuring.Controls.Add(this.label_camSelect);
            this.tabPageAlignCuring.Controls.Add(this.buttonConnectPM);
            this.tabPageAlignCuring.Controls.Add(this.comboBoxCamSelect);
            this.tabPageAlignCuring.Controls.Add(this.pictureBoxCam);
            this.tabPageAlignCuring.Controls.Add(this.labelILValue);
            this.tabPageAlignCuring.Controls.Add(this.ButtonReset);
            this.tabPageAlignCuring.Controls.Add(this.ButtonAlignment);
            this.tabPageAlignCuring.Controls.Add(this.ButtonPreCuring);
            this.tabPageAlignCuring.Controls.Add(this.ButtonCuring);
            this.tabPageAlignCuring.ForeColor = System.Drawing.Color.Silver;
            this.tabPageAlignCuring.Location = new System.Drawing.Point(4, 34);
            this.tabPageAlignCuring.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageAlignCuring.Name = "tabPageAlignCuring";
            this.tabPageAlignCuring.Size = new System.Drawing.Size(3217, 1762);
            this.tabPageAlignCuring.TabIndex = 2;
            this.tabPageAlignCuring.Text = "Alignment & Curing";
            // 
            // comboBoxPMChl
            // 
            this.comboBoxPMChl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(64)))));
            this.comboBoxPMChl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxPMChl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxPMChl.ForeColor = System.Drawing.Color.Silver;
            this.comboBoxPMChl.FormattingEnabled = true;
            this.comboBoxPMChl.Items.AddRange(new object[] {
            "CH1",
            "CH2",
            "CH3"});
            this.comboBoxPMChl.Location = new System.Drawing.Point(2392, 58);
            this.comboBoxPMChl.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.comboBoxPMChl.Name = "comboBoxPMChl";
            this.comboBoxPMChl.Size = new System.Drawing.Size(139, 46);
            this.comboBoxPMChl.TabIndex = 42;
            this.comboBoxPMChl.SelectedIndexChanged += new System.EventHandler(this.comboBoxPMChl_SelectedIndexChanged);
            // 
            // labelPMChl
            // 
            this.labelPMChl.AutoSize = true;
            this.labelPMChl.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPMChl.Location = new System.Drawing.Point(2070, 55);
            this.labelPMChl.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelPMChl.Name = "labelPMChl";
            this.labelPMChl.Size = new System.Drawing.Size(327, 58);
            this.labelPMChl.TabIndex = 41;
            this.labelPMChl.Text = "PM Channel: ";
            // 
            // labelPiezoPositionValue
            // 
            this.labelPiezoPositionValue.AutoSize = true;
            this.labelPiezoPositionValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPiezoPositionValue.Location = new System.Drawing.Point(2562, 1565);
            this.labelPiezoPositionValue.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelPiezoPositionValue.Name = "labelPiezoPositionValue";
            this.labelPiezoPositionValue.Size = new System.Drawing.Size(297, 39);
            this.labelPiezoPositionValue.TabIndex = 40;
            this.labelPiezoPositionValue.Text = "2048, 2048, 2048";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(2600, 1545);
            this.label13.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(0, 39);
            this.label13.TabIndex = 39;
            // 
            // labelPiezoPosition
            // 
            this.labelPiezoPosition.AutoSize = true;
            this.labelPiezoPosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPiezoPosition.Location = new System.Drawing.Point(2560, 1500);
            this.labelPiezoPosition.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelPiezoPosition.Name = "labelPiezoPosition";
            this.labelPiezoPosition.Size = new System.Drawing.Size(538, 46);
            this.labelPiezoPosition.TabIndex = 38;
            this.labelPiezoPosition.Text = "Piezo DAC Value (0 - 4096)";
            // 
            // labelPositionAngles
            // 
            this.labelPositionAngles.AutoSize = true;
            this.labelPositionAngles.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPositionAngles.Location = new System.Drawing.Point(2562, 1372);
            this.labelPositionAngles.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelPositionAngles.Name = "labelPositionAngles";
            this.labelPositionAngles.Size = new System.Drawing.Size(231, 39);
            this.labelPositionAngles.TabIndex = 37;
            this.labelPositionAngles.Text = "-0.0, -0.0, 0.0";
            // 
            // labelPositionXYZ
            // 
            this.labelPositionXYZ.AutoSize = true;
            this.labelPositionXYZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPositionXYZ.Location = new System.Drawing.Point(2562, 1315);
            this.labelPositionXYZ.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelPositionXYZ.Name = "labelPositionXYZ";
            this.labelPositionXYZ.Size = new System.Drawing.Size(451, 39);
            this.labelPositionXYZ.TabIndex = 36;
            this.labelPositionXYZ.Text = "-0.0000, -0.0000, 138.0000";
            // 
            // labelBeetlePosition
            // 
            this.labelBeetlePosition.AutoSize = true;
            this.labelBeetlePosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBeetlePosition.Location = new System.Drawing.Point(2560, 1252);
            this.labelBeetlePosition.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelBeetlePosition.Name = "labelBeetlePosition";
            this.labelBeetlePosition.Size = new System.Drawing.Size(583, 46);
            this.labelBeetlePosition.TabIndex = 35;
            this.labelBeetlePosition.Text = "Beetle Position (XYZ; θxθyθz)";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(35)))), ((int)(((byte)(64)))));
            this.panel2.Controls.Add(this.comboBoxFixtureNum);
            this.panel2.Controls.Add(this.labelBeetleFixture);
            this.panel2.Controls.Add(this.pictureBox2);
            this.panel2.Controls.Add(this.buttonControlBoxDetection);
            this.panel2.Controls.Add(this.buttonChartOnOff);
            this.panel2.Controls.Add(this.labelErrorMessage);
            this.panel2.Controls.Add(this.buttonCalibration);
            this.panel2.Controls.Add(this.comboBoxProductSelect);
            this.panel2.Controls.Add(this.buttonReference);
            this.panel2.Controls.Add(this.labelProductSelect);
            this.panel2.Controls.Add(this.labelUsePiezo);
            this.panel2.Controls.Add(this.buttonClearError);
            this.panel2.Controls.Add(this.comboBoxUsePiezo);
            this.panel2.Controls.Add(this.richTextBoxErrorMsg);
            this.panel2.Controls.Add(this.buttonTest);
            this.panel2.Controls.Add(this.buttonCancleRun);
            this.panel2.Controls.Add(this.buttonClose);
            this.panel2.Location = new System.Drawing.Point(-5, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(802, 1690);
            this.panel2.TabIndex = 34;
            // 
            // comboBoxFixtureNum
            // 
            this.comboBoxFixtureNum.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.comboBoxFixtureNum.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxFixtureNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxFixtureNum.ForeColor = System.Drawing.Color.Silver;
            this.comboBoxFixtureNum.FormattingEnabled = true;
            this.comboBoxFixtureNum.Items.AddRange(new object[] {
            "#0",
            "#1",
            "#2",
            "#3",
            "#4",
            "#5"});
            this.comboBoxFixtureNum.Location = new System.Drawing.Point(238, 988);
            this.comboBoxFixtureNum.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.comboBoxFixtureNum.Name = "comboBoxFixtureNum";
            this.comboBoxFixtureNum.Size = new System.Drawing.Size(196, 46);
            this.comboBoxFixtureNum.TabIndex = 36;
            this.comboBoxFixtureNum.SelectedIndexChanged += new System.EventHandler(this.comboBoxFixtureNum_SelectedIndexChanged);
            // 
            // labelBeetleFixture
            // 
            this.labelBeetleFixture.AutoSize = true;
            this.labelBeetleFixture.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.labelBeetleFixture.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBeetleFixture.Location = new System.Drawing.Point(5, 985);
            this.labelBeetleFixture.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelBeetleFixture.Name = "labelBeetleFixture";
            this.labelBeetleFixture.Size = new System.Drawing.Size(247, 58);
            this.labelBeetleFixture.TabIndex = 35;
            this.labelBeetleFixture.Text = "Fixture #: ";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(38, 25);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(730, 295);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 34;
            this.pictureBox2.TabStop = false;
            // 
            // buttonChartOnOff
            // 
            this.buttonChartOnOff.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.buttonChartOnOff.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonChartOnOff.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Indigo;
            this.buttonChartOnOff.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonChartOnOff.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonChartOnOff.Location = new System.Drawing.Point(402, 782);
            this.buttonChartOnOff.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.buttonChartOnOff.Name = "buttonChartOnOff";
            this.buttonChartOnOff.Size = new System.Drawing.Size(400, 140);
            this.buttonChartOnOff.TabIndex = 33;
            this.buttonChartOnOff.Text = "Live Charts Off";
            this.buttonChartOnOff.UseVisualStyleBackColor = false;
            this.buttonChartOnOff.Click += new System.EventHandler(this.buttonChartOnOff_Click);
            // 
            // labelErrorMessage
            // 
            this.labelErrorMessage.AutoSize = true;
            this.labelErrorMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelErrorMessage.ForeColor = System.Drawing.Color.Silver;
            this.labelErrorMessage.Location = new System.Drawing.Point(12, 1055);
            this.labelErrorMessage.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelErrorMessage.Name = "labelErrorMessage";
            this.labelErrorMessage.Size = new System.Drawing.Size(181, 69);
            this.labelErrorMessage.TabIndex = 26;
            this.labelErrorMessage.Text = "Logs:";
            // 
            // comboBoxProductSelect
            // 
            this.comboBoxProductSelect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.comboBoxProductSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxProductSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxProductSelect.ForeColor = System.Drawing.Color.Silver;
            this.comboBoxProductSelect.FormattingEnabled = true;
            this.comboBoxProductSelect.Items.AddRange(new object[] {
            "MM 1xN",
            "SM 1xN"});
            this.comboBoxProductSelect.Location = new System.Drawing.Point(230, 927);
            this.comboBoxProductSelect.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.comboBoxProductSelect.Name = "comboBoxProductSelect";
            this.comboBoxProductSelect.Size = new System.Drawing.Size(204, 46);
            this.comboBoxProductSelect.TabIndex = 32;
            this.comboBoxProductSelect.SelectedIndexChanged += new System.EventHandler(this.comboBoxProductSelect_SelectedIndexChanged);
            // 
            // buttonReference
            // 
            this.buttonReference.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.buttonReference.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonReference.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Indigo;
            this.buttonReference.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonReference.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonReference.Location = new System.Drawing.Point(5, 498);
            this.buttonReference.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.buttonReference.Name = "buttonReference";
            this.buttonReference.Size = new System.Drawing.Size(400, 140);
            this.buttonReference.TabIndex = 17;
            this.buttonReference.Text = "Reference";
            this.buttonReference.UseVisualStyleBackColor = false;
            this.buttonReference.Click += new System.EventHandler(this.buttonReference_Click);
            // 
            // labelProductSelect
            // 
            this.labelProductSelect.AutoSize = true;
            this.labelProductSelect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.labelProductSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProductSelect.Location = new System.Drawing.Point(5, 924);
            this.labelProductSelect.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelProductSelect.Name = "labelProductSelect";
            this.labelProductSelect.Size = new System.Drawing.Size(225, 58);
            this.labelProductSelect.TabIndex = 31;
            this.labelProductSelect.Text = "Product: ";
            // 
            // labelUsePiezo
            // 
            this.labelUsePiezo.AutoSize = true;
            this.labelUsePiezo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.labelUsePiezo.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUsePiezo.Location = new System.Drawing.Point(455, 924);
            this.labelUsePiezo.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelUsePiezo.Name = "labelUsePiezo";
            this.labelUsePiezo.Size = new System.Drawing.Size(164, 58);
            this.labelUsePiezo.TabIndex = 30;
            this.labelUsePiezo.Text = "Piezo:";
            // 
            // comboBoxUsePiezo
            // 
            this.comboBoxUsePiezo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.comboBoxUsePiezo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxUsePiezo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxUsePiezo.ForeColor = System.Drawing.Color.Silver;
            this.comboBoxUsePiezo.FormattingEnabled = true;
            this.comboBoxUsePiezo.Items.AddRange(new object[] {
            "Yes",
            "No"});
            this.comboBoxUsePiezo.Location = new System.Drawing.Point(632, 927);
            this.comboBoxUsePiezo.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.comboBoxUsePiezo.Name = "comboBoxUsePiezo";
            this.comboBoxUsePiezo.Size = new System.Drawing.Size(164, 46);
            this.comboBoxUsePiezo.TabIndex = 2;
            this.comboBoxUsePiezo.SelectedIndexChanged += new System.EventHandler(this.comboBoxUsePiezo_SelectedIndexChanged);
            // 
            // richTextBoxErrorMsg
            // 
            this.richTextBoxErrorMsg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(65)))), ((int)(((byte)(94)))));
            this.richTextBoxErrorMsg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxErrorMsg.EnableAutoDragDrop = true;
            this.richTextBoxErrorMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxErrorMsg.ForeColor = System.Drawing.Color.LightGray;
            this.richTextBoxErrorMsg.Location = new System.Drawing.Point(5, 1122);
            this.richTextBoxErrorMsg.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.richTextBoxErrorMsg.Name = "richTextBoxErrorMsg";
            this.richTextBoxErrorMsg.Size = new System.Drawing.Size(798, 565);
            this.richTextBoxErrorMsg.TabIndex = 15;
            this.richTextBoxErrorMsg.Text = "";
            this.richTextBoxErrorMsg.WordWrap = false;
            this.richTextBoxErrorMsg.TextChanged += new System.EventHandler(this.richTextBoxErrorMsg_TextChanged);
            // 
            // buttonCancleRun
            // 
            this.buttonCancleRun.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.buttonCancleRun.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonCancleRun.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Indigo;
            this.buttonCancleRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancleRun.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCancleRun.Location = new System.Drawing.Point(402, 640);
            this.buttonCancleRun.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.buttonCancleRun.Name = "buttonCancleRun";
            this.buttonCancleRun.Size = new System.Drawing.Size(400, 140);
            this.buttonCancleRun.TabIndex = 21;
            this.buttonCancleRun.Text = "Cancle Run";
            this.buttonCancleRun.UseVisualStyleBackColor = false;
            this.buttonCancleRun.Click += new System.EventHandler(this.buttonCancleRun_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.buttonClose.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Indigo;
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonClose.Location = new System.Drawing.Point(5, 640);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(400, 140);
            this.buttonClose.TabIndex = 20;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // labelTimeValue
            // 
            this.labelTimeValue.AutoSize = true;
            this.labelTimeValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTimeValue.ForeColor = System.Drawing.Color.Silver;
            this.labelTimeValue.Location = new System.Drawing.Point(2072, 272);
            this.labelTimeValue.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelTimeValue.Name = "labelTimeValue";
            this.labelTimeValue.Size = new System.Drawing.Size(399, 113);
            this.labelTimeValue.TabIndex = 28;
            this.labelTimeValue.Text = "0min 0s";
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTime.Location = new System.Drawing.Point(2070, 195);
            this.labelTime.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(224, 76);
            this.labelTime.TabIndex = 27;
            this.labelTime.Text = "Time: ";
            // 
            // labelStatusValue
            // 
            this.labelStatusValue.AutoSize = true;
            this.labelStatusValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatusValue.ForeColor = System.Drawing.Color.Lime;
            this.labelStatusValue.Location = new System.Drawing.Point(2395, 908);
            this.labelStatusValue.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelStatusValue.Name = "labelStatusValue";
            this.labelStatusValue.Size = new System.Drawing.Size(200, 107);
            this.labelStatusValue.TabIndex = 25;
            this.labelStatusValue.Text = "Idle";
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatus.Location = new System.Drawing.Point(2062, 908);
            this.labelStatus.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(370, 107);
            this.labelStatus.TabIndex = 24;
            this.labelStatus.Text = "Status: ";
            // 
            // labelTargetValue
            // 
            this.labelTargetValue.AutoSize = true;
            this.labelTargetValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTargetValue.Location = new System.Drawing.Point(2070, 515);
            this.labelTargetValue.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelTargetValue.Name = "labelTargetValue";
            this.labelTargetValue.Size = new System.Drawing.Size(512, 113);
            this.labelTargetValue.TabIndex = 23;
            this.labelTargetValue.Text = "-50.000dB";
            // 
            // labelTarget
            // 
            this.labelTarget.AutoSize = true;
            this.labelTarget.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTarget.Location = new System.Drawing.Point(2070, 428);
            this.labelTarget.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelTarget.Name = "labelTarget";
            this.labelTarget.Size = new System.Drawing.Size(604, 76);
            this.labelTarget.TabIndex = 22;
            this.labelTarget.Text = "Target Loss (dB):  ";
            // 
            // labelIL
            // 
            this.labelIL.AutoSize = true;
            this.labelIL.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelIL.Location = new System.Drawing.Point(2070, 685);
            this.labelIL.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelIL.Name = "labelIL";
            this.labelIL.Size = new System.Drawing.Size(527, 76);
            this.labelIL.TabIndex = 16;
            this.labelIL.Text = "Current IL (dB): ";
            // 
            // cartesianChartZ
            // 
            this.cartesianChartZ.Location = new System.Drawing.Point(1620, 1038);
            this.cartesianChartZ.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.cartesianChartZ.Name = "cartesianChartZ";
            this.cartesianChartZ.Size = new System.Drawing.Size(918, 650);
            this.cartesianChartZ.TabIndex = 14;
            this.cartesianChartZ.Text = "cartesianChart2";
            // 
            // buttonDisconnectPM
            // 
            this.buttonDisconnectPM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(64)))), ((int)(((byte)(100)))));
            this.buttonDisconnectPM.FlatAppearance.BorderSize = 0;
            this.buttonDisconnectPM.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(64)))), ((int)(((byte)(150)))));
            this.buttonDisconnectPM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDisconnectPM.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDisconnectPM.Location = new System.Drawing.Point(2585, 25);
            this.buttonDisconnectPM.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.buttonDisconnectPM.Name = "buttonDisconnectPM";
            this.buttonDisconnectPM.Size = new System.Drawing.Size(288, 140);
            this.buttonDisconnectPM.TabIndex = 18;
            this.buttonDisconnectPM.Text = "Disconnect PM";
            this.buttonDisconnectPM.UseVisualStyleBackColor = false;
            this.buttonDisconnectPM.Click += new System.EventHandler(this.buttonDisconnectPM_Click);
            // 
            // cartesianChartXY
            // 
            this.cartesianChartXY.Location = new System.Drawing.Point(815, 1038);
            this.cartesianChartXY.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.cartesianChartXY.Name = "cartesianChartXY";
            this.cartesianChartXY.Size = new System.Drawing.Size(750, 650);
            this.cartesianChartXY.TabIndex = 13;
            this.cartesianChartXY.Text = "cartesianChart1";
            // 
            // label_camSelect
            // 
            this.label_camSelect.AutoSize = true;
            this.label_camSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_camSelect.Location = new System.Drawing.Point(812, 22);
            this.label_camSelect.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label_camSelect.Name = "label_camSelect";
            this.label_camSelect.Size = new System.Drawing.Size(403, 58);
            this.label_camSelect.TabIndex = 12;
            this.label_camSelect.Text = "Camera Source: ";
            // 
            // buttonConnectPM
            // 
            this.buttonConnectPM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(64)))), ((int)(((byte)(100)))));
            this.buttonConnectPM.FlatAppearance.BorderSize = 0;
            this.buttonConnectPM.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(64)))), ((int)(((byte)(150)))));
            this.buttonConnectPM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonConnectPM.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonConnectPM.Location = new System.Drawing.Point(2888, 25);
            this.buttonConnectPM.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.buttonConnectPM.Name = "buttonConnectPM";
            this.buttonConnectPM.Size = new System.Drawing.Size(288, 140);
            this.buttonConnectPM.TabIndex = 19;
            this.buttonConnectPM.Text = "Connect PM";
            this.buttonConnectPM.UseVisualStyleBackColor = false;
            this.buttonConnectPM.Click += new System.EventHandler(this.buttonConnectPM_Click);
            // 
            // comboBoxCamSelect
            // 
            this.comboBoxCamSelect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(64)))));
            this.comboBoxCamSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxCamSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxCamSelect.ForeColor = System.Drawing.Color.Silver;
            this.comboBoxCamSelect.FormattingEnabled = true;
            this.comboBoxCamSelect.Location = new System.Drawing.Point(1230, 25);
            this.comboBoxCamSelect.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.comboBoxCamSelect.Name = "comboBoxCamSelect";
            this.comboBoxCamSelect.Size = new System.Drawing.Size(482, 46);
            this.comboBoxCamSelect.TabIndex = 11;
            this.comboBoxCamSelect.SelectedIndexChanged += new System.EventHandler(this.comboBoxCamSelect_SelectedIndexChanged);
            // 
            // pictureBoxCam
            // 
            this.pictureBoxCam.Location = new System.Drawing.Point(805, 92);
            this.pictureBoxCam.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.pictureBoxCam.Name = "pictureBoxCam";
            this.pictureBoxCam.Size = new System.Drawing.Size(1250, 938);
            this.pictureBoxCam.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxCam.TabIndex = 10;
            this.pictureBoxCam.TabStop = false;
            // 
            // tabPageStageControl
            // 
            this.tabPageStageControl.BackColor = System.Drawing.Color.White;
            this.tabPageStageControl.Controls.Add(this.labelRy);
            this.tabPageStageControl.Controls.Add(this.labelRz);
            this.tabPageStageControl.Controls.Add(this.labelRx);
            this.tabPageStageControl.Controls.Add(this.labelZ);
            this.tabPageStageControl.Controls.Add(this.labelT);
            this.tabPageStageControl.Controls.Add(this.labelX);
            this.tabPageStageControl.Controls.Add(this.label1);
            this.tabPageStageControl.Controls.Add(this.label14);
            this.tabPageStageControl.Controls.Add(this.label12);
            this.tabPageStageControl.Controls.Add(this.comboBoxMotorSelectTop);
            this.tabPageStageControl.Controls.Add(this.buttonSetInitial);
            this.tabPageStageControl.Controls.Add(this.label11);
            this.tabPageStageControl.Controls.Add(this.numericUpDownPx);
            this.tabPageStageControl.Controls.Add(this.buttonSetPivot);
            this.tabPageStageControl.Controls.Add(this.numericUpDownPy);
            this.tabPageStageControl.Controls.Add(this.buttonClearErrorBC);
            this.tabPageStageControl.Controls.Add(this.numericUpDownPz);
            this.tabPageStageControl.Controls.Add(this.buttonSetPosition);
            this.tabPageStageControl.Controls.Add(this.label8);
            this.tabPageStageControl.Controls.Add(this.comboBoxMotorSelectMid);
            this.tabPageStageControl.Controls.Add(this.label10);
            this.tabPageStageControl.Controls.Add(this.comboBoxMotorSelectBot);
            this.tabPageStageControl.Controls.Add(this.cartesianChartMotorC);
            this.tabPageStageControl.Controls.Add(this.panel1);
            this.tabPageStageControl.Controls.Add(this.numericUpDownY);
            this.tabPageStageControl.Controls.Add(this.numericUpDownRz);
            this.tabPageStageControl.Controls.Add(this.label7);
            this.tabPageStageControl.Controls.Add(this.cartesianChartMotorB);
            this.tabPageStageControl.Controls.Add(this.numericUpDownRy);
            this.tabPageStageControl.Controls.Add(this.cartesianChartMotorA);
            this.tabPageStageControl.Controls.Add(this.label6);
            this.tabPageStageControl.Controls.Add(this.numericUpDownZ);
            this.tabPageStageControl.Controls.Add(this.numericUpDownRx);
            this.tabPageStageControl.Controls.Add(this.numericUpDownX);
            this.tabPageStageControl.Controls.Add(this.pictureBoxFigure);
            this.tabPageStageControl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tabPageStageControl.Location = new System.Drawing.Point(4, 34);
            this.tabPageStageControl.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.tabPageStageControl.Name = "tabPageStageControl";
            this.tabPageStageControl.Padding = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.tabPageStageControl.Size = new System.Drawing.Size(3217, 1762);
            this.tabPageStageControl.TabIndex = 1;
            this.tabPageStageControl.Text = "Stage Control";
            // 
            // labelRy
            // 
            this.labelRy.AutoSize = true;
            this.labelRy.BackColor = System.Drawing.Color.Transparent;
            this.labelRy.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRy.ForeColor = System.Drawing.Color.Black;
            this.labelRy.Location = new System.Drawing.Point(1375, 815);
            this.labelRy.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelRy.Name = "labelRy";
            this.labelRy.Size = new System.Drawing.Size(95, 63);
            this.labelRy.TabIndex = 50;
            this.labelRy.Text = "Ry";
            // 
            // labelRz
            // 
            this.labelRz.AutoSize = true;
            this.labelRz.BackColor = System.Drawing.Color.Transparent;
            this.labelRz.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRz.ForeColor = System.Drawing.Color.Black;
            this.labelRz.Location = new System.Drawing.Point(1858, 345);
            this.labelRz.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelRz.Name = "labelRz";
            this.labelRz.Size = new System.Drawing.Size(95, 63);
            this.labelRz.TabIndex = 49;
            this.labelRz.Text = "Rz";
            // 
            // labelRx
            // 
            this.labelRx.AutoSize = true;
            this.labelRx.BackColor = System.Drawing.Color.Transparent;
            this.labelRx.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRx.ForeColor = System.Drawing.Color.Black;
            this.labelRx.Location = new System.Drawing.Point(2152, 752);
            this.labelRx.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelRx.Name = "labelRx";
            this.labelRx.Size = new System.Drawing.Size(95, 63);
            this.labelRx.TabIndex = 48;
            this.labelRx.Text = "Rx";
            // 
            // labelZ
            // 
            this.labelZ.AutoSize = true;
            this.labelZ.BackColor = System.Drawing.Color.Transparent;
            this.labelZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelZ.ForeColor = System.Drawing.Color.Black;
            this.labelZ.Location = new System.Drawing.Point(1778, 265);
            this.labelZ.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelZ.Name = "labelZ";
            this.labelZ.Size = new System.Drawing.Size(61, 63);
            this.labelZ.TabIndex = 47;
            this.labelZ.Text = "Z";
            // 
            // labelT
            // 
            this.labelT.AutoSize = true;
            this.labelT.BackColor = System.Drawing.Color.Transparent;
            this.labelT.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelT.ForeColor = System.Drawing.Color.Black;
            this.labelT.Location = new System.Drawing.Point(1312, 738);
            this.labelT.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelT.Name = "labelT";
            this.labelT.Size = new System.Drawing.Size(64, 63);
            this.labelT.TabIndex = 46;
            this.labelT.Text = "Y";
            // 
            // labelX
            // 
            this.labelX.AutoSize = true;
            this.labelX.BackColor = System.Drawing.Color.Transparent;
            this.labelX.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX.ForeColor = System.Drawing.Color.Black;
            this.labelX.Location = new System.Drawing.Point(2150, 605);
            this.labelX.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(64, 63);
            this.labelX.TabIndex = 45;
            this.labelX.Text = "X";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(2558, 8);
            this.label1.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(510, 54);
            this.label1.TabIndex = 44;
            this.label1.Text = "Real && Target Position";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.Black;
            this.label14.Location = new System.Drawing.Point(2198, 180);
            this.label14.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(228, 46);
            this.label14.TabIndex = 43;
            this.label14.Text = "Unit: mm / °";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label12.Location = new System.Drawing.Point(2428, 78);
            this.label12.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(268, 46);
            this.label12.TabIndex = 42;
            this.label12.Text = "Motor Select: ";
            // 
            // comboBoxMotorSelectTop
            // 
            this.comboBoxMotorSelectTop.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxMotorSelectTop.FormattingEnabled = true;
            this.comboBoxMotorSelectTop.Items.AddRange(new object[] {
            "T1x",
            "T1y",
            "T2x",
            "T2y",
            "T3x",
            "T3y",
            "OFF"});
            this.comboBoxMotorSelectTop.Location = new System.Drawing.Point(2698, 70);
            this.comboBoxMotorSelectTop.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.comboBoxMotorSelectTop.Name = "comboBoxMotorSelectTop";
            this.comboBoxMotorSelectTop.Size = new System.Drawing.Size(124, 46);
            this.comboBoxMotorSelectTop.TabIndex = 41;
            this.comboBoxMotorSelectTop.SelectedIndexChanged += new System.EventHandler(this.comboBoxMotorSelectTop_SelectedIndexChanged);
            // 
            // buttonSetInitial
            // 
            this.buttonSetInitial.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.buttonSetInitial.FlatAppearance.BorderSize = 0;
            this.buttonSetInitial.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.buttonSetInitial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSetInitial.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSetInitial.ForeColor = System.Drawing.Color.Black;
            this.buttonSetInitial.Location = new System.Drawing.Point(1155, 212);
            this.buttonSetInitial.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.buttonSetInitial.Name = "buttonSetInitial";
            this.buttonSetInitial.Size = new System.Drawing.Size(308, 95);
            this.buttonSetInitial.TabIndex = 34;
            this.buttonSetInitial.Text = "Set Initial";
            this.buttonSetInitial.UseVisualStyleBackColor = false;
            this.buttonSetInitial.Click += new System.EventHandler(this.buttonSetInitial_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label11.Location = new System.Drawing.Point(2428, 615);
            this.label11.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(268, 46);
            this.label11.TabIndex = 40;
            this.label11.Text = "Motor Select: ";
            // 
            // numericUpDownPx
            // 
            this.numericUpDownPx.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.numericUpDownPx.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numericUpDownPx.DecimalPlaces = 1;
            this.numericUpDownPx.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownPx.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownPx.Location = new System.Drawing.Point(1155, 78);
            this.numericUpDownPx.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
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
            this.numericUpDownPx.Size = new System.Drawing.Size(155, 64);
            this.numericUpDownPx.TabIndex = 31;
            // 
            // buttonSetPivot
            // 
            this.buttonSetPivot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.buttonSetPivot.FlatAppearance.BorderSize = 0;
            this.buttonSetPivot.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Violet;
            this.buttonSetPivot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSetPivot.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSetPivot.ForeColor = System.Drawing.Color.Black;
            this.buttonSetPivot.Location = new System.Drawing.Point(1690, 2);
            this.buttonSetPivot.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.buttonSetPivot.Name = "buttonSetPivot";
            this.buttonSetPivot.Size = new System.Drawing.Size(248, 145);
            this.buttonSetPivot.TabIndex = 22;
            this.buttonSetPivot.Text = "Set Pivot Point";
            this.buttonSetPivot.UseVisualStyleBackColor = false;
            this.buttonSetPivot.Click += new System.EventHandler(this.buttonSetPivot_Click);
            // 
            // numericUpDownPy
            // 
            this.numericUpDownPy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.numericUpDownPy.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numericUpDownPy.DecimalPlaces = 1;
            this.numericUpDownPy.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownPy.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownPy.Location = new System.Drawing.Point(1338, 78);
            this.numericUpDownPy.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
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
            this.numericUpDownPy.Size = new System.Drawing.Size(155, 64);
            this.numericUpDownPy.TabIndex = 32;
            // 
            // buttonClearErrorBC
            // 
            this.buttonClearErrorBC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.buttonClearErrorBC.FlatAppearance.BorderSize = 0;
            this.buttonClearErrorBC.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Violet;
            this.buttonClearErrorBC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClearErrorBC.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonClearErrorBC.ForeColor = System.Drawing.Color.Black;
            this.buttonClearErrorBC.Location = new System.Drawing.Point(1952, 2);
            this.buttonClearErrorBC.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.buttonClearErrorBC.Name = "buttonClearErrorBC";
            this.buttonClearErrorBC.Size = new System.Drawing.Size(248, 145);
            this.buttonClearErrorBC.TabIndex = 37;
            this.buttonClearErrorBC.Text = "Clear Error";
            this.buttonClearErrorBC.UseVisualStyleBackColor = false;
            this.buttonClearErrorBC.Click += new System.EventHandler(this.buttonClearErrorBC_Click);
            // 
            // numericUpDownPz
            // 
            this.numericUpDownPz.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.numericUpDownPz.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numericUpDownPz.DecimalPlaces = 1;
            this.numericUpDownPz.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownPz.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownPz.Location = new System.Drawing.Point(1520, 78);
            this.numericUpDownPz.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.numericUpDownPz.Name = "numericUpDownPz";
            this.numericUpDownPz.Size = new System.Drawing.Size(155, 64);
            this.numericUpDownPz.TabIndex = 33;
            // 
            // buttonSetPosition
            // 
            this.buttonSetPosition.AutoSize = true;
            this.buttonSetPosition.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.buttonSetPosition.FlatAppearance.BorderSize = 0;
            this.buttonSetPosition.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.buttonSetPosition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSetPosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSetPosition.ForeColor = System.Drawing.Color.Black;
            this.buttonSetPosition.Location = new System.Drawing.Point(1155, 375);
            this.buttonSetPosition.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.buttonSetPosition.Name = "buttonSetPosition";
            this.buttonSetPosition.Size = new System.Drawing.Size(308, 182);
            this.buttonSetPosition.TabIndex = 13;
            this.buttonSetPosition.Text = "Go";
            this.buttonSetPosition.UseVisualStyleBackColor = false;
            this.buttonSetPosition.Click += new System.EventHandler(this.buttonSetPosition_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(1142, 10);
            this.label8.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(92, 63);
            this.label8.TabIndex = 16;
            this.label8.Text = "Px";
            // 
            // comboBoxMotorSelectMid
            // 
            this.comboBoxMotorSelectMid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxMotorSelectMid.FormattingEnabled = true;
            this.comboBoxMotorSelectMid.Items.AddRange(new object[] {
            "T1x",
            "T1y",
            "T2x",
            "T2y",
            "T3x",
            "T3y",
            "OFF"});
            this.comboBoxMotorSelectMid.Location = new System.Drawing.Point(2698, 610);
            this.comboBoxMotorSelectMid.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.comboBoxMotorSelectMid.Name = "comboBoxMotorSelectMid";
            this.comboBoxMotorSelectMid.Size = new System.Drawing.Size(124, 46);
            this.comboBoxMotorSelectMid.TabIndex = 39;
            this.comboBoxMotorSelectMid.SelectedIndexChanged += new System.EventHandler(this.comboBoxMotorSelectMid_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label10.Location = new System.Drawing.Point(2428, 1158);
            this.label10.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(268, 46);
            this.label10.TabIndex = 38;
            this.label10.Text = "Motor Select: ";
            // 
            // comboBoxMotorSelectBot
            // 
            this.comboBoxMotorSelectBot.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxMotorSelectBot.FormattingEnabled = true;
            this.comboBoxMotorSelectBot.Items.AddRange(new object[] {
            "T1x",
            "T1y",
            "T2x",
            "T2y",
            "T3x",
            "T3y",
            "OFF"});
            this.comboBoxMotorSelectBot.Location = new System.Drawing.Point(2698, 1148);
            this.comboBoxMotorSelectBot.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.comboBoxMotorSelectBot.Name = "comboBoxMotorSelectBot";
            this.comboBoxMotorSelectBot.Size = new System.Drawing.Size(124, 46);
            this.comboBoxMotorSelectBot.TabIndex = 27;
            this.comboBoxMotorSelectBot.SelectedIndexChanged += new System.EventHandler(this.comboBoxMotorSelectBot_SelectedIndexChanged);
            // 
            // cartesianChartMotorC
            // 
            this.cartesianChartMotorC.Location = new System.Drawing.Point(2428, 1212);
            this.cartesianChartMotorC.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.cartesianChartMotorC.Name = "cartesianChartMotorC";
            this.cartesianChartMotorC.Size = new System.Drawing.Size(758, 465);
            this.cartesianChartMotorC.TabIndex = 26;
            this.cartesianChartMotorC.Text = "cartesianChart4";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(35)))), ((int)(((byte)(64)))));
            this.panel1.Controls.Add(this.labelILValueBC);
            this.panel1.Controls.Add(this.labelILBC);
            this.panel1.Controls.Add(this.labelPiezoPositionBC);
            this.panel1.Controls.Add(this.labelPiezoPositionValueBC);
            this.panel1.Controls.Add(this.buttonPiezoReset);
            this.panel1.Controls.Add(this.comboBoxPiezoStep);
            this.panel1.Controls.Add(this.buttonPiezoSearch);
            this.panel1.Controls.Add(this.labelPiezoStep);
            this.panel1.Controls.Add(this.pictureBoxCamBC);
            this.panel1.Controls.Add(this.labelStatusValueBC);
            this.panel1.Controls.Add(this.labelStatusBC);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.panel1.Location = new System.Drawing.Point(-2, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1130, 1698);
            this.panel1.TabIndex = 25;
            // 
            // labelILValueBC
            // 
            this.labelILValueBC.AutoSize = true;
            this.labelILValueBC.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelILValueBC.ForeColor = System.Drawing.Color.Yellow;
            this.labelILValueBC.Location = new System.Drawing.Point(820, 1555);
            this.labelILValueBC.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelILValueBC.Name = "labelILValueBC";
            this.labelILValueBC.Size = new System.Drawing.Size(266, 76);
            this.labelILValueBC.TabIndex = 50;
            this.labelILValueBC.Text = "-50.000";
            // 
            // labelILBC
            // 
            this.labelILBC.AutoSize = true;
            this.labelILBC.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelILBC.ForeColor = System.Drawing.Color.Silver;
            this.labelILBC.Location = new System.Drawing.Point(545, 1555);
            this.labelILBC.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelILBC.Name = "labelILBC";
            this.labelILBC.Size = new System.Drawing.Size(279, 76);
            this.labelILBC.TabIndex = 49;
            this.labelILBC.Text = "IL (dB): ";
            // 
            // labelPiezoPositionBC
            // 
            this.labelPiezoPositionBC.AutoSize = true;
            this.labelPiezoPositionBC.BackColor = System.Drawing.Color.Transparent;
            this.labelPiezoPositionBC.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPiezoPositionBC.ForeColor = System.Drawing.Color.Silver;
            this.labelPiezoPositionBC.Location = new System.Drawing.Point(32, 1482);
            this.labelPiezoPositionBC.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelPiezoPositionBC.Name = "labelPiezoPositionBC";
            this.labelPiezoPositionBC.Size = new System.Drawing.Size(446, 39);
            this.labelPiezoPositionBC.TabIndex = 48;
            this.labelPiezoPositionBC.Text = "Piezo DAC Value (0 - 4096):";
            // 
            // labelPiezoPositionValueBC
            // 
            this.labelPiezoPositionValueBC.AutoSize = true;
            this.labelPiezoPositionValueBC.BackColor = System.Drawing.Color.Transparent;
            this.labelPiezoPositionValueBC.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPiezoPositionValueBC.ForeColor = System.Drawing.Color.Silver;
            this.labelPiezoPositionValueBC.Location = new System.Drawing.Point(512, 1482);
            this.labelPiezoPositionValueBC.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelPiezoPositionValueBC.Name = "labelPiezoPositionValueBC";
            this.labelPiezoPositionValueBC.Size = new System.Drawing.Size(281, 39);
            this.labelPiezoPositionValueBC.TabIndex = 47;
            this.labelPiezoPositionValueBC.Text = "2048, 2048, 2048";
            // 
            // buttonPiezoReset
            // 
            this.buttonPiezoReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.buttonPiezoReset.FlatAppearance.BorderSize = 0;
            this.buttonPiezoReset.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Indigo;
            this.buttonPiezoReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPiezoReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPiezoReset.ForeColor = System.Drawing.Color.Silver;
            this.buttonPiezoReset.Location = new System.Drawing.Point(10, 1385);
            this.buttonPiezoReset.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.buttonPiezoReset.Name = "buttonPiezoReset";
            this.buttonPiezoReset.Size = new System.Drawing.Size(340, 90);
            this.buttonPiezoReset.TabIndex = 46;
            this.buttonPiezoReset.Text = "Piezo Reset";
            this.buttonPiezoReset.UseVisualStyleBackColor = false;
            this.buttonPiezoReset.Click += new System.EventHandler(this.buttonPiezoReset_Click);
            // 
            // comboBoxPiezoStep
            // 
            this.comboBoxPiezoStep.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.comboBoxPiezoStep.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxPiezoStep.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxPiezoStep.ForeColor = System.Drawing.Color.Silver;
            this.comboBoxPiezoStep.FormattingEnabled = true;
            this.comboBoxPiezoStep.Items.AddRange(new object[] {
            "2",
            "4",
            "6",
            "8",
            "10",
            "12"});
            this.comboBoxPiezoStep.Location = new System.Drawing.Point(990, 1400);
            this.comboBoxPiezoStep.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.comboBoxPiezoStep.Name = "comboBoxPiezoStep";
            this.comboBoxPiezoStep.Size = new System.Drawing.Size(106, 46);
            this.comboBoxPiezoStep.TabIndex = 44;
            this.comboBoxPiezoStep.SelectedIndexChanged += new System.EventHandler(this.comboBoxPiezoStep_SelectedIndexChanged);
            // 
            // buttonPiezoSearch
            // 
            this.buttonPiezoSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.buttonPiezoSearch.FlatAppearance.BorderSize = 0;
            this.buttonPiezoSearch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Indigo;
            this.buttonPiezoSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPiezoSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPiezoSearch.ForeColor = System.Drawing.Color.Silver;
            this.buttonPiezoSearch.Location = new System.Drawing.Point(345, 1385);
            this.buttonPiezoSearch.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.buttonPiezoSearch.Name = "buttonPiezoSearch";
            this.buttonPiezoSearch.Size = new System.Drawing.Size(340, 90);
            this.buttonPiezoSearch.TabIndex = 45;
            this.buttonPiezoSearch.Text = "Piezo Search";
            this.buttonPiezoSearch.UseVisualStyleBackColor = false;
            this.buttonPiezoSearch.Click += new System.EventHandler(this.buttonPiezoSearch_Click);
            // 
            // labelPiezoStep
            // 
            this.labelPiezoStep.AutoSize = true;
            this.labelPiezoStep.BackColor = System.Drawing.Color.Transparent;
            this.labelPiezoStep.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPiezoStep.ForeColor = System.Drawing.Color.Silver;
            this.labelPiezoStep.Location = new System.Drawing.Point(700, 1400);
            this.labelPiezoStep.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelPiezoStep.Name = "labelPiezoStep";
            this.labelPiezoStep.Size = new System.Drawing.Size(273, 54);
            this.labelPiezoStep.TabIndex = 44;
            this.labelPiezoStep.Text = "Piezo Step: ";
            // 
            // pictureBoxCamBC
            // 
            this.pictureBoxCamBC.Location = new System.Drawing.Point(2, 500);
            this.pictureBoxCamBC.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxCamBC.Name = "pictureBoxCamBC";
            this.pictureBoxCamBC.Size = new System.Drawing.Size(1125, 842);
            this.pictureBoxCamBC.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxCamBC.TabIndex = 38;
            this.pictureBoxCamBC.TabStop = false;
            // 
            // labelStatusValueBC
            // 
            this.labelStatusValueBC.AutoSize = true;
            this.labelStatusValueBC.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatusValueBC.ForeColor = System.Drawing.Color.Lime;
            this.labelStatusValueBC.Location = new System.Drawing.Point(265, 1555);
            this.labelStatusValueBC.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelStatusValueBC.Name = "labelStatusValueBC";
            this.labelStatusValueBC.Size = new System.Drawing.Size(186, 76);
            this.labelStatusValueBC.TabIndex = 36;
            this.labelStatusValueBC.Text = "IDLE";
            // 
            // labelStatusBC
            // 
            this.labelStatusBC.AutoSize = true;
            this.labelStatusBC.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatusBC.ForeColor = System.Drawing.Color.Silver;
            this.labelStatusBC.Location = new System.Drawing.Point(22, 1555);
            this.labelStatusBC.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelStatusBC.Name = "labelStatusBC";
            this.labelStatusBC.Size = new System.Drawing.Size(269, 76);
            this.labelStatusBC.TabIndex = 35;
            this.labelStatusBC.Text = "Status: ";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 40F, System.Drawing.FontStyle.Bold);
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label9.Location = new System.Drawing.Point(92, 338);
            this.label9.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(930, 153);
            this.label9.TabIndex = 30;
            this.label9.Text = "Beetle Control";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(180, 50);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(738, 275);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 29;
            this.pictureBox1.TabStop = false;
            // 
            // numericUpDownY
            // 
            this.numericUpDownY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.numericUpDownY.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numericUpDownY.DecimalPlaces = 4;
            this.numericUpDownY.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownY.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numericUpDownY.Location = new System.Drawing.Point(1388, 738);
            this.numericUpDownY.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
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
            this.numericUpDownY.Size = new System.Drawing.Size(250, 64);
            this.numericUpDownY.TabIndex = 24;
            // 
            // numericUpDownRz
            // 
            this.numericUpDownRz.BackColor = System.Drawing.Color.Cyan;
            this.numericUpDownRz.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numericUpDownRz.DecimalPlaces = 1;
            this.numericUpDownRz.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownRz.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownRz.Location = new System.Drawing.Point(1968, 345);
            this.numericUpDownRz.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
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
            this.numericUpDownRz.Size = new System.Drawing.Size(152, 64);
            this.numericUpDownRz.TabIndex = 28;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(1325, 10);
            this.label7.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(92, 63);
            this.label7.TabIndex = 17;
            this.label7.Text = "Py";
            // 
            // cartesianChartMotorB
            // 
            this.cartesianChartMotorB.Location = new System.Drawing.Point(2428, 675);
            this.cartesianChartMotorB.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.cartesianChartMotorB.Name = "cartesianChartMotorB";
            this.cartesianChartMotorB.Size = new System.Drawing.Size(758, 465);
            this.cartesianChartMotorB.TabIndex = 24;
            this.cartesianChartMotorB.Text = "cartesianChart4";
            // 
            // numericUpDownRy
            // 
            this.numericUpDownRy.BackColor = System.Drawing.Color.Cyan;
            this.numericUpDownRy.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numericUpDownRy.DecimalPlaces = 1;
            this.numericUpDownRy.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownRy.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownRy.Location = new System.Drawing.Point(1482, 818);
            this.numericUpDownRy.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
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
            this.numericUpDownRy.Size = new System.Drawing.Size(155, 64);
            this.numericUpDownRy.TabIndex = 27;
            // 
            // cartesianChartMotorA
            // 
            this.cartesianChartMotorA.Location = new System.Drawing.Point(2428, 132);
            this.cartesianChartMotorA.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.cartesianChartMotorA.Name = "cartesianChartMotorA";
            this.cartesianChartMotorA.Size = new System.Drawing.Size(758, 465);
            this.cartesianChartMotorA.TabIndex = 23;
            this.cartesianChartMotorA.Text = "cartesianChart3";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(1510, 10);
            this.label6.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(92, 63);
            this.label6.TabIndex = 18;
            this.label6.Text = "Pz";
            // 
            // numericUpDownZ
            // 
            this.numericUpDownZ.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDownZ.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.numericUpDownZ.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numericUpDownZ.DecimalPlaces = 4;
            this.numericUpDownZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownZ.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numericUpDownZ.Location = new System.Drawing.Point(1850, 262);
            this.numericUpDownZ.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
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
            this.numericUpDownZ.Size = new System.Drawing.Size(270, 64);
            this.numericUpDownZ.TabIndex = 25;
            this.numericUpDownZ.Value = new decimal(new int[] {
            140,
            0,
            0,
            0});
            // 
            // numericUpDownRx
            // 
            this.numericUpDownRx.BackColor = System.Drawing.Color.Cyan;
            this.numericUpDownRx.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numericUpDownRx.DecimalPlaces = 1;
            this.numericUpDownRx.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownRx.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownRx.Location = new System.Drawing.Point(2260, 755);
            this.numericUpDownRx.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
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
            this.numericUpDownRx.Size = new System.Drawing.Size(152, 64);
            this.numericUpDownRx.TabIndex = 26;
            // 
            // numericUpDownX
            // 
            this.numericUpDownX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.numericUpDownX.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numericUpDownX.DecimalPlaces = 4;
            this.numericUpDownX.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownX.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numericUpDownX.Location = new System.Drawing.Point(2162, 675);
            this.numericUpDownX.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
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
            this.numericUpDownX.Size = new System.Drawing.Size(250, 64);
            this.numericUpDownX.TabIndex = 23;
            // 
            // pictureBoxFigure
            // 
            this.pictureBoxFigure.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxFigure.Image = global::Beetle.Properties.Resources.Beetle_3D;
            this.pictureBoxFigure.Location = new System.Drawing.Point(1138, 180);
            this.pictureBoxFigure.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxFigure.Name = "pictureBoxFigure";
            this.pictureBoxFigure.Size = new System.Drawing.Size(1282, 1518);
            this.pictureBoxFigure.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxFigure.TabIndex = 0;
            this.pictureBoxFigure.TabStop = false;
            // 
            // refreshTimer
            // 
            this.refreshTimer.Enabled = true;
            this.refreshTimer.Interval = 250;
            this.refreshTimer.Tick += new System.EventHandler(this.RefreshTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(240F, 240F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(3210, 1778);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Beetle";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageAlignCuring.ResumeLayout(false);
            this.tabPageAlignCuring.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCam)).EndInit();
            this.tabPageStageControl.ResumeLayout(false);
            this.tabPageStageControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPz)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCamBC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRz)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRx)).EndInit();
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
        private LiveCharts.WinForms.CartesianChart cartesianChartMotorB;
        private LiveCharts.WinForms.CartesianChart cartesianChartMotorA;
        private System.Windows.Forms.Button buttonSetPivot;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
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
        private System.Windows.Forms.Button buttonChartOnOff;
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
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox comboBoxMotorSelectTop;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox comboBoxMotorSelectMid;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox comboBoxMotorSelectBot;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label labelBeetlePosition;
        private System.Windows.Forms.Label labelPiezoPositionValue;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label labelPiezoPosition;
        private System.Windows.Forms.Label labelPositionAngles;
        private System.Windows.Forms.Label labelPositionXYZ;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.PictureBox pictureBoxCamBC;
        private System.Windows.Forms.ComboBox comboBoxPiezoStep;
        private System.Windows.Forms.Button buttonPiezoSearch;
        private System.Windows.Forms.Label labelPiezoStep;
        private System.Windows.Forms.Button buttonPiezoReset;
        private System.Windows.Forms.Label labelPiezoPositionBC;
        private System.Windows.Forms.Label labelPiezoPositionValueBC;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelILValueBC;
        private System.Windows.Forms.Label labelILBC;
        private System.Windows.Forms.ComboBox comboBoxPMChl;
        private System.Windows.Forms.Label labelPMChl;
        private System.Windows.Forms.Label labelRy;
        private System.Windows.Forms.Label labelRz;
        private System.Windows.Forms.Label labelRx;
        private System.Windows.Forms.Label labelZ;
        private System.Windows.Forms.Label labelT;
        private System.Windows.Forms.Label labelX;
        private System.Windows.Forms.ComboBox comboBoxFixtureNum;
        private System.Windows.Forms.Label labelBeetleFixture;
    }
}

