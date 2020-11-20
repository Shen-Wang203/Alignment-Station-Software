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
            this.ButtonReset = new System.Windows.Forms.Button();
            this.ButtonAlignment = new System.Windows.Forms.Button();
            this.ButtonBackAlign = new System.Windows.Forms.Button();
            this.ButtonCuring = new System.Windows.Forms.Button();
            this.ButtonClearError = new System.Windows.Forms.Button();
            this.ButtonCalibration = new System.Windows.Forms.Button();
            this.IL = new System.Windows.Forms.Label();
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
            // ButtonBackAlign
            // 
            this.ButtonBackAlign.Location = new System.Drawing.Point(113, 210);
            this.ButtonBackAlign.Name = "ButtonBackAlign";
            this.ButtonBackAlign.Size = new System.Drawing.Size(123, 42);
            this.ButtonBackAlign.TabIndex = 2;
            this.ButtonBackAlign.Text = "Back Align";
            this.ButtonBackAlign.UseVisualStyleBackColor = true;
            this.ButtonBackAlign.Click += new System.EventHandler(this.ButtonBackAlign_Click);
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
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.IL);
            this.Controls.Add(this.ButtonCalibration);
            this.Controls.Add(this.ButtonClearError);
            this.Controls.Add(this.ButtonCuring);
            this.Controls.Add(this.ButtonBackAlign);
            this.Controls.Add(this.ButtonAlignment);
            this.Controls.Add(this.ButtonReset);
            this.Name = "MainForm";
            this.Text = "Beetle";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonReset;
        private System.Windows.Forms.Button ButtonAlignment;
        private System.Windows.Forms.Button ButtonBackAlign;
        private System.Windows.Forms.Button ButtonCuring;
        private System.Windows.Forms.Button ButtonClearError;
        private System.Windows.Forms.Button ButtonCalibration;
        private System.Windows.Forms.Label IL;
    }
}
