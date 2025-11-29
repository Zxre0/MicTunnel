using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using NAudio.Wave;

namespace MicClient.ClientUI
{
    public partial class Form1 : Form
    {
        private WaveInEvent? waveIn;
        private UdpClient? udp;
        private IPEndPoint? serverEndpoint;
        private bool streaming;

        public Form1()
        {
            InitializeComponent();
            LoadDevices();
            InitDefaultValues();
        }

        private void InitDefaultValues()
        {
            txtServerIP.Text = "IP Goes Here"; // change to your server IP if you want
            txtPort.Text = "9000";
            lblStatus.Text = "Idle";
            btnStop.Enabled = false;
        }

        private void LoadDevices()
        {
            comboDevices.Items.Clear();

            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                var caps = WaveIn.GetCapabilities(i);
                comboDevices.Items.Add(caps.ProductName);
            }

            if (comboDevices.Items.Count > 0)
                comboDevices.SelectedIndex = 0;
            else
                lblStatus.Text = "No input devices found.";
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (streaming)
                return;

            try
            {
                string ipText = txtServerIP.Text.Trim();
                string portText = txtPort.Text.Trim();

                if (!IPAddress.TryParse(ipText, out var ip))
                {
                    lblStatus.Text = "Invalid IP address.";
                    return;
                }

                if (!int.TryParse(portText, out var port) || port <= 0 || port > 65535)
                {
                    lblStatus.Text = "Invalid port.";
                    return;
                }

                if (comboDevices.SelectedIndex < 0)
                {
                    lblStatus.Text = "Select a microphone device.";
                    return;
                }

                serverEndpoint = new IPEndPoint(ip, port);
                udp = new UdpClient();

                waveIn = new WaveInEvent
                {
                    DeviceNumber = comboDevices.SelectedIndex,
                    WaveFormat = new WaveFormat(48000, 16, 1),
                    BufferMilliseconds = 20
                };

                waveIn.DataAvailable += WaveIn_DataAvailable;
                waveIn.RecordingStopped += WaveIn_RecordingStopped;

                waveIn.StartRecording();
                streaming = true;

                btnStart.Enabled = false;
                btnStop.Enabled = true;
                lblStatus.Text = $"Streaming to {ip}:{port}...";
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error starting: " + ex.Message;
                CleanupAudio();
            }
        }

        private void WaveIn_DataAvailable(object? sender, WaveInEventArgs e)
        {
            try
            {
                if (!streaming || udp == null || serverEndpoint == null)
                    return;

                udp.Send(e.Buffer, e.BytesRecorded, serverEndpoint);
            }
            catch
            {
                // ignore send errors for now
            }
        }

        private void WaveIn_RecordingStopped(object? sender, StoppedEventArgs e)
        {
            CleanupAudio();
            lblStatus.Text = "Streaming stopped.";
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (!streaming)
                return;

            streaming = false;

            try
            {
                waveIn?.StopRecording();
            }
            catch
            {
                CleanupAudio();
            }

            btnStart.Enabled = true;
            btnStop.Enabled = false;
            lblStatus.Text = "Stopped.";
        }

        private void CleanupAudio()
        {
            try
            {
                if (waveIn != null)
                {
                    waveIn.DataAvailable -= WaveIn_DataAvailable;
                    waveIn.RecordingStopped -= WaveIn_RecordingStopped;
                    waveIn.Dispose();
                    waveIn = null;
                }
            }
            catch { }

            try
            {
                udp?.Dispose();
                udp = null;
            }
            catch { }

            streaming = false;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            streaming = false;

            try
            {
                waveIn?.StopRecording();
            }
            catch { }

            CleanupAudio();
            base.OnFormClosing(e);
        }

        // These three are ONLY here to satisfy events the Designer might have hooked.
        // If you later remove those events in the Designer, you can delete these methods.
        private void TxtPort_TextChanged(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label1_Click_1(object sender, EventArgs e) { }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txtServerIP_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
