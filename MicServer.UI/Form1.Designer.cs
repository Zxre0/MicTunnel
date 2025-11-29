namespace MicServer.UI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            comboOutputDevices = new ComboBox();
            txtPort = new TextBox();
            btnStart = new Button();
            btnStop = new Button();
            lblStatus = new Label();
            lblLocalIp = new Label();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            SuspendLayout();
            // 
            // comboOutputDevices
            // 
            comboOutputDevices.FormattingEnabled = true;
            comboOutputDevices.Location = new Point(102, 83);
            comboOutputDevices.Name = "comboOutputDevices";
            comboOutputDevices.Size = new Size(200, 23);
            comboOutputDevices.TabIndex = 0;
            // 
            // txtPort
            // 
            txtPort.Location = new Point(154, 112);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(100, 23);
            txtPort.TabIndex = 1;
            txtPort.Text = "9000";
            // 
            // btnStart
            // 
            btnStart.Location = new Point(102, 141);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(75, 23);
            btnStart.TabIndex = 2;
            btnStart.Text = "Start";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(227, 141);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(75, 23);
            btnStop.TabIndex = 3;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(102, 65);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(26, 15);
            lblStatus.TabIndex = 4;
            lblStatus.Text = "Idle";
            // 
            // lblLocalIp
            // 
            lblLocalIp.AutoSize = true;
            lblLocalIp.Location = new Point(12, 9);
            lblLocalIp.Name = "lblLocalIp";
            lblLocalIp.Size = new Size(124, 15);
            lblLocalIp.TabIndex = 5;
            lblLocalIp.Text = "Local IP : (detecting...)";
            lblLocalIp.Click += lblLocalIp_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(116, 115);
            label1.Name = "label1";
            label1.Size = new Size(32, 15);
            label1.TabIndex = 6;
            label1.Text = "Port:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 86);
            label2.Name = "label2";
            label2.Size = new Size(86, 15);
            label2.TabIndex = 7;
            label2.Text = "Output Device:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 33);
            label3.Name = "label3";
            label3.Size = new Size(269, 15);
            label3.TabIndex = 8;
            label3.Text = "this is the IP that is going to go into the client app";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(407, 270);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(lblLocalIp);
            Controls.Add(lblStatus);
            Controls.Add(btnStop);
            Controls.Add(btnStart);
            Controls.Add(txtPort);
            Controls.Add(comboOutputDevices);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "MicTunnel Server";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox comboOutputDevices;
        private TextBox txtPort;
        private Button btnStart;
        private Button btnStop;
        private Label lblStatus;
        private Label lblLocalIp;
        private Label label1;
        private Label label2;
        private Label label3;
    }
}
