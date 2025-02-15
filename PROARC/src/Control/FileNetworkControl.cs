using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace PROARC.src.Control
{
    public static class FileNetworkControl
    {
        public static void SendFile(string filePathClient)
        {
            const string serverIp = "192.168.18.169";
            const int serverPort = 6666;

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

            System.Threading.Thread WorkerThread = new System.Threading.Thread(() =>
            {
                try
                {
                    // Conecta ao servidor
                    System.Net.Sockets.TcpClient TcpClient = new System.Net.Sockets.TcpClient(serverIp, serverPort);
                    System.Net.Sockets.NetworkStream NetworkStream = TcpClient.GetStream();

                    // Envia o comando "FTR" e o caminho do arquivo
                    NetworkStream.Write(Encoding.UTF8.GetBytes("FTR"), 0, "FTR".Length);

                    NetworkStream.Write(Encoding.UTF8.GetBytes(filePathServer), 0, Encoding.UTF8.GetBytes(filePathServer).Length);

                    Console.WriteLine("[+] Comando e caminho enviados ao servidor.");

                    // Configurações para receber o arquivo
                    int BlockSize = 1024;
                    Byte[] DataByte = new Byte[BlockSize];
                    int DataRead;

                    // Garante que o diretório do caminho fornecido exista
                    string directoryPath = Path.GetDirectoryName(filePathServer);
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    Console.WriteLine($"[+] Recebendo arquivo e salvando em: {filePathServer}");

                    // Abre o arquivo para escrita no caminho fornecido
                    using (System.IO.Stream FileStream = System.IO.File.OpenWrite(@"C:\Users\marco\OneDrive\Área de Trabalho\receivers\abacate4.txt"))
                    {
                        while ((DataRead = NetworkStream.Read(DataByte, 0, BlockSize)) > 0)
                        {
                            FileStream.Write(DataByte, 0, DataRead);
                        }
                    }

                    Console.WriteLine("[+] Arquivo recebido com sucesso!");

                    // Fecha a conexão
                    NetworkStream.Close();
                    TcpClient.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[-] Erro ao receber o arquivo: {ex.Message}");
                }
            });

            // Inicia a thread
            WorkerThread.Start();
        }

    }
}