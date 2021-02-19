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
            this.ButtonReset = new System.Windows.Forms.Button();
            this.ButtonAlignment = new System.Windows.Forms.Button();
            this.ButtonPreCuring = new System.Windows.Forms.Button();
            this.ButtonCuring = new System.Windows.Forms.Button();
            this.ButtonClearError = new System.Windows.Forms.Button();
            this.ButtonCalibration = new System.Windows.Forms.Button();
            this.IL = new System.Windows.Forms.Label();
            this.ControlBoxDetection = new System.Windows.Forms.Button();
            this.Test = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ButtonReset
            // 
            this.ButtonReset.Location = new System.Drawing.Point(113, 75);
            this.ButtonReset.Name = "ButtonReset";
            this.ButtonReset.Size = new System.Drawing.Size(123, 40);
            this.ButtonReset.TabIndex = 0;
            this.ButtonReset.Text = "Reset";
            this.ButtonReset.UseVisualStyleBackColor = true;
            this.ButtonReset.Click += new System.EventHandler(this.ButtonReset_Click);
            // 
            // ButtonAlignment
            // 
            this.ButtonAlignment.Location = new System.Drawing.Point(113, 141);
            this.ButtonAlignment.Name = "ButtonAlignment";
            this.ButtonAlignment.Size = new System.Drawing.Size(123, 43);
            this.ButtonAlignment.TabIndex = 1;
            this.ButtonAlignment.Text = "Alignment";
            this.ButtonAlignment.UseVisualStyleBackColor = true;
            this.ButtonAlignment.Click += new System.EventHandler(this.ButtonAlignment_Click);
            // 
            // ButtonPreCuring
            // 
            this.ButtonPreCuring.Location = new System.Drawing.Point(113, 210);
            this.ButtonPreCuring.Name = "ButtonPreCuring";
            this.ButtonPreCuring.Size = new System.Drawing.Size(123, 42);
            this.ButtonPreCuring.TabIndex = 2;
            this.ButtonPreCuring.Text = "PreCuring";
            this.ButtonPreCuring.UseVisualStyleBackColor = true;
            this.ButtonPreCuring.Click += new System.EventHandler(this.ButtonPreCuring_Click);
            // 
            // ButtonCuring
            // 
            this.ButtonCuring.Location = new System.Drawing.Point(113, 275);
            this.ButtonCuring.Name = "ButtonCuring";
            this.ButtonCuring.Size = new System.Drawing.Size(123, 44);
            this.ButtonCuring.TabIndex = 3;
            this.ButtonCuring.Text = "Curing";
            this.ButtonCuring.UseVisualStyleBackColor = true;
            this.ButtonCuring.Click += new System.EventHandler(this.ButtonCuring_Click);
            // 
            // ButtonClearError
            // 
            this.ButtonClearError.Location = new System.Drawing.Point(507, 75);
            this.ButtonClearError.Name = "ButtonClearError";
            this.ButtonClearError.Size = new System.Drawing.Size(96, 30);
            this.ButtonClearError.TabIndex = 4;
            this.ButtonClearError.Text = "Clear Errors";
            this.ButtonClearError.UseVisualStyleBackColor = true;
            this.ButtonClearError.Click += new System.EventHandler(this.ButtonClearError_Click);
            // 
            // ButtonCalibration
            // 
            this.ButtonCalibration.Location = new System.Drawing.Point(507, 123);
            this.ButtonCalibration.Name = "ButtonCalibration";
            this.ButtonCalibration.Size = new System.Drawing.Size(96, 28);
            this.ButtonCalibration.TabIndex = 5;
            this.ButtonCalibration.Text = "Calibration";
            this.ButtonCalibration.UseVisualStyleBackColor = true;
            this.ButtonCalibration.Click += new System.EventHandler(this.ButtonCalibration_Click);
            // 
            // IL
            // 
            this.IL.AutoSize = true;
            this.IL.Location = new System.Drawing.Point(381, 225);
            this.IL.Name = "IL";
            this.IL.Size = new System.Drawing.Size(35, 13);
            this.IL.TabIndex = 7;
            this.IL.Text = "-50dB";
            this.IL.Click += new System.EventHandler(this.IL_Click);
            // 
            // ControlBoxDetection
            // 
            this.ControlBoxDetection.Location = new System.Drawing.Point(507, 188);
            this.ControlBoxDetection.Name = "ControlBoxDetection";
            this.ControlBoxDetection.Size = new System.Drawing.Size(96, 34);
            this.ControlBoxDetection.TabIndex = 8;
            this.ControlBoxDetection.Text = "Control Box Detection";
            this.ControlBoxDetection.UseVisualStyleBackColor = true;
            this.ControlBoxDetection.Click += new System.EventHandler(this.ControlBoxDetection_Click);
            // 
            // Test
            // 
            this.Test.Location = new System.Drawing.Point(507, 275);
            this.Test.Name = "Test";
            this.Test.Size = new System.Drawing.Size(96, 23);
            this.Test.TabIndex = 9;
            this.Test.Text = "Test";
            this.Test.UseVisualStyleBackColor = true;
            this.Test.Click += new System.EventHandler(this.Test_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 463);
            this.Controls.Add(this.Test);
            this.Controls.Add(this.ControlBoxDetection);
            this.Controls.Add(this.IL);
            this.Controls.Add(this.ButtonCalibration);
            this.Controls.Add(this.ButtonClearError);
            this.Controls.Add(this.ButtonCuring);
            this.Controls.Add(this.ButtonPreCuring);
            this.Controls.Add(this.ButtonAlignment);
            this.Controls.Add(this.ButtonReset);
            this.Name = "MainForm";
            this.Text = "Beetle";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonReset;
        private System.Windows.Forms.Button ButtonAlignment;
        private System.Windows.Forms.Button ButtonPreCuring;
        private System.Windows.Forms.Button ButtonCuring;
        private System.Windows.Forms.Button ButtonClearError;
        private System.Windows.Forms.Button ButtonCalibration;
        private System.Windows.Forms.Label IL;
        private System.Windows.Forms.Button ControlBoxDetection;
        private System.Windows.Forms.Button Test;
    }
}

