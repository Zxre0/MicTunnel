using System;
using System.Net;
using System.Net.Sockets;
using NAudio.Wave;

class Program
{
    // Audio settings
    const int SampleRate = 48000;
    const int Channels = 1;
    const int BitsPerSample = 16;

    static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: NetworkMicClient <server_ip> <port>");
            return;
        }

        string serverIp = args[0];
        int port = int.Parse(args[1]);

        var serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIp), port);
        using var udp = new UdpClient();

        // Setup audio capture from default microphone
        var waveFormat = new WaveFormat(SampleRate, BitsPerSample, Channels);
        using var waveIn = new WaveInEvent
        {
            WaveFormat = waveFormat,
            BufferMilliseconds = 20 // ~20 ms frames
        };

        waveIn.DataAvailable += (s, e) =>
        {
            try
            {
                // Send raw PCM frame over UDP
                udp.Send(e.Buffer, e.BytesRecorded, serverEndPoint);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Send error: {ex.Message}");
            }
        };

        waveIn.RecordingStopped += (s, e) =>
        {
            Console.WriteLine("Recording stopped.");
        };

        Console.WriteLine($"Streaming mic to {serverIp}:{port} ...");
        waveIn.StartRecording();

        Console.WriteLine("Press Enter to stop.");
        Console.ReadLine();

        waveIn.StopRecording();
    }
}
