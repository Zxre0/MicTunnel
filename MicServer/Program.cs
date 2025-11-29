using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using NAudio.CoreAudioApi;
using NAudio.Wave;

class Program
{
    const int SampleRate = 48000;
    const int Channels = 1;
    const int BitsPerSample = 16;

    static async Task Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("Usage: NetworkMicServer <listen_port> [device_name_substring]");
            return;
        }

        int port = int.Parse(args[0]);
        string deviceMatch = args.Length >= 2 ? args[1] : "CABLE"; // match VB-Audio by default

        var format = new WaveFormat(SampleRate, BitsPerSample, Channels);

        // Choose output device (virtual cable) using MMDeviceEnumerator
        var enumerator = new MMDeviceEnumerator();
        var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);

        var targetDevice = devices
            .FirstOrDefault(d => d.FriendlyName.IndexOf(deviceMatch, StringComparison.OrdinalIgnoreCase) >= 0);

        if (targetDevice == null)
        {
            Console.WriteLine($"No output device found matching \"{deviceMatch}\".");
            Console.WriteLine("Available devices:");
            foreach (var d in devices)
                Console.WriteLine($" - {d.FriendlyName}");
            return;
        }

        Console.WriteLine($"Using output device: {targetDevice.FriendlyName}");

        var provider = new BufferedWaveProvider(format)
        {
            DiscardOnBufferOverflow = true
        };

        using var wasapiOut = new WasapiOut(targetDevice, AudioClientShareMode.Shared, true, 20);
        wasapiOut.Init(provider);
        wasapiOut.Play();

        using var udp = new UdpClient(port);
        udp.Client.ReceiveBufferSize = 1024 * 1024;

        Console.WriteLine($"Listening on UDP port {port}...");
        Console.WriteLine("Press Ctrl+C to exit.");

        while (true)
        {
            try
            {
                var result = await udp.ReceiveAsync();
                // Add raw PCM to playback buffer
                provider.AddSamples(result.Buffer, 0, result.Buffer.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Receive error: {ex.Message}");
            }
        }
    }
}
