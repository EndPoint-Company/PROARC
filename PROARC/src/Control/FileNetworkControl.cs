using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using PROARC.src.Models.Tipos;

namespace PROARC.src.Control
{
    public static class FileNetworkControl
    {
        public static void SendFile(string filePathClient)
        {
            const string serverIp = "127.0.0.1";
            const int serverPort = 9999;

            System.Net.Sockets.TcpClient TcpClient = new System.Net.Sockets.TcpClient(serverIp, serverPort);
            System.Net.Sockets.NetworkStream NetworkStream = TcpClient.GetStream();
            System.IO.Stream FileStream = System.IO.File.OpenRead(filePathClient);
            byte[] FileBuffer = new byte[FileStream.Length];

            TcpClient.Client.Send(Encoding.UTF8.GetBytes("FTS"));

            FileStream.Read(FileBuffer, 0, (int)FileStream.Length);
            NetworkStream.Write(FileBuffer, 0, FileBuffer.GetLength(0));
            NetworkStream.Close();
        }

        public static void ReceiveFile(string filePathServer)
        {
            const string serverIp = "127.0.0.1";
            const int serverPort = 9999;

            System.Net.Sockets.TcpClient TcpClient = new System.Net.Sockets.TcpClient(serverIp, serverPort);
            TcpClient.Client.Send(Encoding.UTF8.GetBytes("FTR"));

            TcpClient.Client.Send(Encoding.UTF8.GetBytes(filePathServer));

            {
                System.Threading.Thread WorkerThread = new System.Threading.Thread(() =>
                {
                    System.Net.Sockets.TcpListener TcpListener = new System.Net.Sockets.TcpListener(IPAddress.Parse(serverIp), serverPort);
                    TcpListener.Start();
                    System.Net.Sockets.Socket HandlerSocket = TcpListener.AcceptSocket();
                    System.Net.Sockets.NetworkStream NetworkStream = new System.Net.Sockets.NetworkStream(HandlerSocket);
                    int BlockSize = 1024;
                    int DataRead = 0;
                    Byte[] DataByte = new Byte[BlockSize];

                    System.IO.Stream FileStream = System.IO.File.OpenWrite(filePathServer);

                    while (true)
                    {
                        DataRead = NetworkStream.Read(DataByte, 0, BlockSize);
                        FileStream.Write(DataByte, 0, DataRead);
                        if (DataRead == 0)
                        {
                            break;
                        }
                    }
                    FileStream.Close();
                });
                WorkerThread.Start();
            }
        }
    }
}
