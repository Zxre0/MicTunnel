namespace MicClient.ClientUI
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
            txtServerIP = new TextBox();
            txtPort = new TextBox();
            comboDevices = new ComboBox();
            btnStart = new Button();
            btnStop = new Button();
            lblStatus = new Label();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            SuspendLayout();
            // 
            // txtServerIP
            // 
            txtServerIP.Location = new Point(96, 117);
            txtServerIP.Name = "txtServerIP";
            txtServerIP.Size = new Size(98, 23);
            txtServerIP.TabIndex = 0;
            txtServerIP.Text = "Server IP";
            txtServerIP.TextChanged += txtServerIP_TextChanged;
            // 
            // txtPort
            // 
            txtPort.Location = new Point(237, 117);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(75, 23);
            txtPort.TabIndex = 1;
            txtPort.Text = "9000";
            txtPort.TextChanged += TxtPort_TextChanged;
            // 
            // comboDevices
            // 
            comboDevices.FormattingEnabled = true;
            comboDevices.Location = new Point(96, 88);
            comboDevices.Name = "comboDevices";
            comboDevices.Size = new Size(216, 23);
            comboDevices.TabIndex = 2;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(96, 146);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(75, 23);
            btnStart.TabIndex = 3;
            btnStart.Text = "Start";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(237, 146);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(75, 23);
            btnStop.TabIndex = 4;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(96, 70);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(26, 15);
            lblStatus.TabIndex = 5;
            lblStatus.Text = "Idle";
            lblStatus.Click += label1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 91);
            label1.Name = "label1";
            label1.Size = new Size(78, 15);
            label1.TabIndex = 6;
            label1.Text = "Microphone: ";
            label1.Click += label1_Click_1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 254);
            label2.Name = "label2";
            label2.Size = new Size(350, 15);
            label2.TabIndex = 7;
            label2.Text = "UX design is hard if you dont like this just use the console version";
            label2.Click += label2_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(35, 120);
            label3.Name = "label3";
            label3.Size = new Size(55, 15);
            label3.TabIndex = 8;
            label3.Text = "Server IP:";
            label3.Click += label3_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(199, 120);
            label4.Name = "label4";
            label4.Size = new Size(32, 15);
            label4.TabIndex = 9;
            label4.Text = "Port:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(384, 269);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(lblStatus);
            Controls.Add(btnStop);
            Controls.Add(btnStart);
            Controls.Add(comboDevices);
            Controls.Add(txtPort);
            Controls.Add(txtServerIP);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "MicTunnel Client";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtServerIP;
        private TextBox txtPort;
        private ComboBox comboDevices;
        private Button btnStart;
        private Button btnStop;
        private Label lblStatus;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
    }
}
