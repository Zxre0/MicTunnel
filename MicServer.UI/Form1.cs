using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System.Net.NetworkInformation;

namespace MicServer.UI
{
    public partial class Form1 : Form
    {
        private const int SampleRate = 48000;
        private const int Channels = 1;
        private const int BitsPerSample = 16;

        private BufferedWaveProvider? _provider;
        private WasapiOut? _wasapiOut;
        private UdpClient? _udp;
        private CancellationTokenSource? _cts;
        private Task? _receiveTask;
        private bool _running;

        public Form1()
        {
            InitializeComponent();
            LoadOutputDevices();
            InitDefaults();
        }

        private void InitDefaults()
        {
            txtPort.Text = "9000";
            lblStatus.Text = "Idle";
            btnStop.Enabled = false;

            string localIp = GetLocalLanIp();
            lblLocalIp.Text = $"Local IP: {localIp}";
        }

        private string GetLocalLanIp()
        {
            try
            {
                string? ip192 = null;
                string? ip10 = null;
                string? ip172 = null;
                string? firstIpv4 = null;

                foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.OperationalStatus != OperationalStatus.Up)
                        continue;

                    // Skip loopback and tunnel adapters
                    if (nic.NetworkInterfaceType == NetworkInterfaceType.Loopback ||
                        nic.NetworkInterfaceType == NetworkInterfaceType.Tunnel)
                        continue;

                    // Skip obvious virtual stuff by name (you can tweak this list)
                    var name = nic.Name.ToLowerInvariant();
                    var desc = nic.Description.ToLowerInvariant();
                    if (name.Contains("virtual") || desc.Contains("virtual") ||
                        name.Contains("vmware") || desc.Contains("vmware") ||
                        name.Contains("hyper-v") || desc.Contains("hyper-v") ||
                        name.Contains("vbox") || desc.Contains("vbox") ||
                        name.Contains("bluetooth"))
                        continue;

                    var props = nic.GetIPProperties();
                    foreach (var addr in props.UnicastAddresses)
                    {
                        if (addr.Address.AddressFamily != AddressFamily.InterNetwork)
                            continue;

                        string ipStr = addr.Address.ToString();

                        // Remember the very first IPv4 we see for fallback
                        firstIpv4 ??= ipStr;

                        if (ipStr.StartsWith("192.168."))
                            ip192 ??= ipStr;
                        else if (ipStr.StartsWith("10."))
                            ip10 ??= ipStr;
                        else if (ipStr.StartsWith("172."))
                        {
                            // Only treat 172.16.0.0–172.31.255.255 as private
                            var parts = ipStr.Split('.');
                            if (parts.Length >= 2 &&
                                int.TryParse(parts[1], out int secondOctet) &&
                                secondOctet >= 16 && secondOctet <= 31)
                            {
                                ip172 ??= ipStr;
                            }
                        }
                    }
                }

                if (ip192 != null) return ip192;
                if (ip10 != null) return ip10;
                if (ip172 != null) return ip172;
                if (firstIpv4 != null) return firstIpv4;

                return "Unknown";
            }
            catch
            {
                return "Unknown";
            }
        }


        private void LoadOutputDevices()
        {
            comboOutputDevices.Items.Clear();

            var enumerator = new MMDeviceEnumerator();
            var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);

            foreach (var d in devices)
            {
                // store device object, show its name
                comboOutputDevices.Items.Add(d);
            }

            comboOutputDevices.DisplayMember = "FriendlyName";

            if (comboOutputDevices.Items.Count > 0)
                comboOutputDevices.SelectedIndex = 0;
            else
                lblStatus.Text = "No output devices found.";
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            if (_running)
                return;

            try
            {
                if (comboOutputDevices.SelectedItem is not MMDevice device)
                {
                    lblStatus.Text = "Select an output device.";
                    return;
                }

                if (!int.TryParse(txtPort.Text.Trim(), out int port) ||
                    port <= 0 || port > 65535)
                {
                    lblStatus.Text = "Invalid port.";
                    return;
                }

                var format = new WaveFormat(SampleRate, BitsPerSample, Channels);

                _provider = new BufferedWaveProvider(format)
                {
                    DiscardOnBufferOverflow = true
                };

                _wasapiOut = new WasapiOut(device, AudioClientShareMode.Shared, true, 20);
                _wasapiOut.Init(_provider);
                _wasapiOut.Play();

                _udp = new UdpClient(port);
                _udp.Client.ReceiveBufferSize = 1024 * 1024;

                _cts = new CancellationTokenSource();
                var token = _cts.Token;

                _running = true;

                btnStart.Enabled = false;
                btnStop.Enabled = true;
                lblStatus.Text = $"Listening on UDP {port} → {device.FriendlyName}";

                _receiveTask = Task.Run(async () =>
                {
                    while (!token.IsCancellationRequested)
                    {
                        try
                        {
                            var result = await _udp.ReceiveAsync(token);
                            _provider.AddSamples(result.Buffer, 0, result.Buffer.Length);
                        }
                        catch (OperationCanceledException)
                        {
                            break; // stopping
                        }
                        catch
                        {
                            // ignore transient errors
                        }
                    }
                }, token);

                await Task.Yield();
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error starting: " + ex.Message;
                StopServer();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopServer();
        }

        private void StopServer()
        {
            if (!_running)
                return;

            _running = false;

            try
            {
                _cts?.Cancel();
            }
            catch { }

            try
            {
                _receiveTask?.Wait(200);
            }
            catch { }

            try
            {
                _udp?.Close();
                _udp?.Dispose();
            }
            catch { }

            try
            {
                _wasapiOut?.Stop();
                _wasapiOut?.Dispose();
            }
            catch { }

            _udp = null;
            _wasapiOut = null;
            _provider = null;
            _cts = null;
            _receiveTask = null;

            btnStart.Enabled = true;
            btnStop.Enabled = false;
            lblStatus.Text = "Stopped.";
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            StopServer();
            base.OnFormClosing(e);
        }

        private void lblLocalIp_Click(object sender, EventArgs e)
        {

        }
    }
}
