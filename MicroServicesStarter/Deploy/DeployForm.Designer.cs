namespace MicroServicesStarter.Deploy
{
    partial class DeployForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.projectsFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.currentVersionLabel = new System.Windows.Forms.Label();
            this.majorRadioButton = new System.Windows.Forms.RadioButton();
            this.minorRadioButton = new System.Windows.Forms.RadioButton();
            this.releaseRadioButton = new System.Windows.Forms.RadioButton();
            this.debugRadioButton = new System.Windows.Forms.RadioButton();
            this.updatingToLabel = new System.Windows.Forms.Label();
            this.releaseCommentsTextBox = new System.Windows.Forms.TextBox();
            this.goButton = new System.Windows.Forms.Button();
            this.doDeployBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.reportToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "Deploy";
            // 
            // projectsFlowLayoutPanel
            // 
            this.projectsFlowLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.projectsFlowLayoutPanel.AutoScroll = true;
            this.projectsFlowLayoutPanel.Location = new System.Drawing.Point(12, 42);
            this.projectsFlowLayoutPanel.Name = "projectsFlowLayoutPanel";
            this.projectsFlowLayoutPanel.Size = new System.Drawing.Size(510, 285);
            this.projectsFlowLayoutPanel.TabIndex = 1;
            // 
            // currentVersionLabel
            // 
            this.currentVersionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.currentVersionLabel.AutoSize = true;
            this.currentVersionLabel.Location = new System.Drawing.Point(528, 42);
            this.currentVersionLabel.Name = "currentVersionLabel";
            this.currentVersionLabel.Size = new System.Drawing.Size(131, 13);
            this.currentVersionLabel.TabIndex = 2;
            this.currentVersionLabel.Text = "Current Version: Unknown";
            // 
            // majorRadioButton
            // 
            this.majorRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.majorRadioButton.AutoSize = true;
            this.majorRadioButton.Location = new System.Drawing.Point(531, 68);
            this.majorRadioButton.Name = "majorRadioButton";
            this.majorRadioButton.Size = new System.Drawing.Size(51, 17);
            this.majorRadioButton.TabIndex = 3;
            this.majorRadioButton.Text = "Major";
            this.majorRadioButton.UseVisualStyleBackColor = true;
            this.majorRadioButton.CheckedChanged += new System.EventHandler(this.majorRadioButton_CheckedChanged);
            // 
            // minorRadioButton
            // 
            this.minorRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.minorRadioButton.AutoSize = true;
            this.minorRadioButton.Location = new System.Drawing.Point(588, 68);
            this.minorRadioButton.Name = "minorRadioButton";
            this.minorRadioButton.Size = new System.Drawing.Size(51, 17);
            this.minorRadioButton.TabIndex = 4;
            this.minorRadioButton.Text = "Minor";
            this.minorRadioButton.UseVisualStyleBackColor = true;
            this.minorRadioButton.CheckedChanged += new System.EventHandler(this.minorRadioButton_CheckedChanged);
            // 
            // releaseRadioButton
            // 
            this.releaseRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.releaseRadioButton.AutoSize = true;
            this.releaseRadioButton.Location = new System.Drawing.Point(645, 68);
            this.releaseRadioButton.Name = "releaseRadioButton";
            this.releaseRadioButton.Size = new System.Drawing.Size(64, 17);
            this.releaseRadioButton.TabIndex = 5;
            this.releaseRadioButton.Text = "Release";
            this.releaseRadioButton.UseVisualStyleBackColor = true;
            this.releaseRadioButton.CheckedChanged += new System.EventHandler(this.releaseRadioButton_CheckedChanged);
            // 
            // debugRadioButton
            // 
            this.debugRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.debugRadioButton.AutoSize = true;
            this.debugRadioButton.Checked = true;
            this.debugRadioButton.Location = new System.Drawing.Point(715, 68);
            this.debugRadioButton.Name = "debugRadioButton";
            this.debugRadioButton.Size = new System.Drawing.Size(57, 17);
            this.debugRadioButton.TabIndex = 6;
            this.debugRadioButton.TabStop = true;
            this.debugRadioButton.Text = "Debug";
            this.debugRadioButton.UseVisualStyleBackColor = true;
            this.debugRadioButton.CheckedChanged += new System.EventHandler(this.debugRadioButton_CheckedChanged);
            // 
            // updatingToLabel
            // 
            this.updatingToLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.updatingToLabel.AutoSize = true;
            this.updatingToLabel.Location = new System.Drawing.Point(688, 42);
            this.updatingToLabel.Name = "updatingToLabel";
            this.updatingToLabel.Size = new System.Drawing.Size(84, 13);
            this.updatingToLabel.TabIndex = 7;
            this.updatingToLabel.Text = "To: Not decided";
            // 
            // releaseCommentsTextBox
            // 
            this.releaseCommentsTextBox.AcceptsReturn = true;
            this.releaseCommentsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.releaseCommentsTextBox.Location = new System.Drawing.Point(528, 91);
            this.releaseCommentsTextBox.Multiline = true;
            this.releaseCommentsTextBox.Name = "releaseCommentsTextBox";
            this.releaseCommentsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.releaseCommentsTextBox.Size = new System.Drawing.Size(352, 236);
            this.releaseCommentsTextBox.TabIndex = 8;
            this.releaseCommentsTextBox.WordWrap = false;
            // 
            // goButton
            // 
            this.goButton.Location = new System.Drawing.Point(796, 37);
            this.goButton.Name = "goButton";
            this.goButton.Size = new System.Drawing.Size(75, 23);
            this.goButton.TabIndex = 9;
            this.goButton.Text = "Go";
            this.goButton.UseVisualStyleBackColor = true;
            this.goButton.Click += new System.EventHandler(this.goButton_Click);
            // 
            // doDeployBackgroundWorker
            // 
            this.doDeployBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.doDeployBackgroundWorker_DoWork);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reportToolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 330);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(892, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // reportToolStripStatusLabel
            // 
            this.reportToolStripStatusLabel.Name = "reportToolStripStatusLabel";
            this.reportToolStripStatusLabel.Size = new System.Drawing.Size(164, 17);
            this.reportToolStripStatusLabel.Text = "Application is ready to deploy";
            // 
            // DeployForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 352);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.goButton);
            this.Controls.Add(this.releaseCommentsTextBox);
            this.Controls.Add(this.updatingToLabel);
            this.Controls.Add(this.debugRadioButton);
            this.Controls.Add(this.releaseRadioButton);
            this.Controls.Add(this.minorRadioButton);
            this.Controls.Add(this.majorRadioButton);
            this.Controls.Add(this.currentVersionLabel);
            this.Controls.Add(this.projectsFlowLayoutPanel);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DeployForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Deploy Services";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.DeployForm_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel projectsFlowLayoutPanel;
        private System.Windows.Forms.Label currentVersionLabel;
        private System.Windows.Forms.RadioButton majorRadioButton;
        private System.Windows.Forms.RadioButton minorRadioButton;
        private System.Windows.Forms.RadioButton releaseRadioButton;
        private System.Windows.Forms.RadioButton debugRadioButton;
        private System.Windows.Forms.Label updatingToLabel;
        private System.Windows.Forms.TextBox releaseCommentsTextBox;
        private System.Windows.Forms.Button goButton;
        private System.ComponentModel.BackgroundWorker doDeployBackgroundWorker;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel reportToolStripStatusLabel;
    }
}