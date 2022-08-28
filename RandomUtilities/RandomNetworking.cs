using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.Http;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace RandomUtilities
{
    public class RandomNetworking
    {
        public static TcpClient ConnectToServer(string ip, int port)
        {
            IPAddress actualIp = IPAddress.Parse(ip);

            return ConnectToServer(actualIp, port);
        }

        public static TcpClient ConnectToServer(IPAddress ip, int port)
        {
            TcpClient client = new();
            client.Connect(ip, port);

            return client;
        }

        public static string GetPublicIP()
        {
            return new HttpClient().GetStringAsync("https://api.ipify.org").GetAwaiter().GetResult();
        }

        public static string GetLocalIP()
        {
            return Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).First().ToString();
        }

        public static void SendToStream(NetworkStream stream, string msg)
        {
            byte[] pack = Encoding.UTF8.GetBytes(msg);

            stream.Write(pack);
        }

        public static string ReadFromStream(NetworkStream stream, int length = 1024)
        {
            byte[] buffer = new byte[length];
            int bufferLen = stream.Read(buffer);

            return Encoding.UTF8.GetString(buffer.AsSpan(0, bufferLen));
        }
        
        public static TcpClient ListenForConnection(int port)
        {
            IPAddress local = IPAddress.Parse("127.0.0.1");
            TcpListener listener = new(local, port);
            listener.Start();

            return listener.AcceptTcpClient();
        }
    }

    public class VerificationSender
    {
        public static CancellationTokenSource cts = new CancellationTokenSource();

        CancellationToken token;
        NetworkStream stream;

        public void Init(object data)
        {
            stream = (NetworkStream)data;
            token = cts.Token;

            StartSending();
        }

        public static Thread StartThread(NetworkStream stream)
        {
            Thread verifier = new Thread(new ParameterizedThreadStart(new VerificationSender().Init));
            verifier.Start(stream);

            return verifier;
        }

        void StartSending()
        {
            while (true)
            {
                Thread.Sleep(5000);

                if (token.IsCancellationRequested)
                    return;

                RandomNetworking.SendToStream(stream, "alive");
            }
        }
    }

    public class AliveCheck
    {
        CancellationTokenSource tokenSrc;
        TcpClient tcpClient;

        public static Thread StartThread(CancellationTokenSource cancelSrc, TcpClient client)
        {
            Thread check = new(new ParameterizedThreadStart(new AliveCheck().Init));
            check.Start(new object[] { cancelSrc, client });

            return check;
        }

        public void Init(object data)
        {
            tokenSrc = (CancellationTokenSource)(data as object[])[0];
            tcpClient = (TcpClient)(data as object[])[1];

            Detect();
        }

        void Detect()
        {
            while (true)
            {
                Stopwatch sw = Stopwatch.StartNew();

                while (!tcpClient.GetStream().DataAvailable)
                {
                    if (sw.Elapsed.TotalSeconds > 10)
                    {
                        tcpClient.Close();
                        tokenSrc.Cancel();

                        Console.WriteLine("\nConnection closed! Press [ENTER] to exit...");
                        return;
                    }
                }

                var buffer = new byte[4096];
                while (tcpClient.GetStream().DataAvailable)
                {
                    tcpClient.GetStream().Read(buffer, 0, buffer.Length);
                }

            }
        }
    }
}
