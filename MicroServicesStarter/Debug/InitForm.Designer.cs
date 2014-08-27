namespace MicroServicesStarter.Debug
{
    partial class InitForm
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
            this.setupLocalEnvironmentBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.reportLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.integrationTestBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.debugCheckerBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // setupLocalEnvironmentBackgroundWorker
            // 
            this.setupLocalEnvironmentBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.setupLocalEnvironmentBackgroundWorker_DoWork);
            // 
            // reportLabel
            // 
            this.reportLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reportLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reportLabel.Location = new System.Drawing.Point(12, 39);
            this.reportLabel.Name = "reportLabel";
            this.reportLabel.Size = new System.Drawing.Size(488, 125);
            this.reportLabel.TabIndex = 1;
            this.reportLabel.Text = "....";
            this.reportLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 30);
            this.label1.TabIndex = 2;
            this.label1.Text = "Init";
            // 
            // integrationTestBackgroundWorker
            // 
            this.integrationTestBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.integrationTestBackgroundWorker_DoWork);
            // 
            // debugCheckerBackgroundWorker
            // 
            this.debugCheckerBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.debugCheckerBackgroundWorker_DoWork);
            // 
            // InitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 173);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.reportLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "InitForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Micro-services";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Starter_FormClosing);
            this.Load += new System.EventHandler(this.Starter_Load);
            this.Shown += new System.EventHandler(this.Starter_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker setupLocalEnvironmentBackgroundWorker;
        private System.Windows.Forms.Label reportLabel;
        private System.Windows.Forms.Label label1;
        private System.ComponentModel.BackgroundWorker integrationTestBackgroundWorker;
        private System.ComponentModel.BackgroundWorker debugCheckerBackgroundWorker;
    }
}

