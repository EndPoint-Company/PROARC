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
        public async static Task SendFile(string filePathClient, string titulo)
        {
            using TcpClient client = new();
            await client.ConnectAsync(NetworkControl.GetAddr());

            NetworkStream NetworkStream = client.GetStream();
            Stream FileStream = File.OpenRead(filePathClient);
            byte[] FileBuffer = new byte[FileStream.Length];

            await client.Client.SendAsync(Encoding.UTF8.GetBytes("FTS"));

            await Task.Delay(1000);

            await client.Client.SendAsync(Encoding.UTF8.GetBytes(titulo));

            await Task.Delay(1000);

            await client.Client.SendAsync(Encoding.UTF8.GetBytes(Path.GetFileName(filePathClient)));

            await FileStream.ReadAsync(FileBuffer, 0, (int)FileStream.Length);
            await NetworkStream.WriteAsync(FileBuffer, 0, FileBuffer.GetLength(0));
            NetworkStream.Close();
        }

        public async static Task ReceiveFile(string titulo, string arquivo)
        {
            try
            {
                using TcpClient client = new();
                await client.ConnectAsync(NetworkControl.GetAddr());

                using NetworkStream NetworkStream = client.GetStream();

                await client.Client.SendAsync(Encoding.UTF8.GetBytes("FTR"));

                await Task.Delay(1000);

                await client.Client.SendAsync(Encoding.UTF8.GetBytes(titulo));

                await Task.Delay(1000);

                await client.Client.SendAsync(Encoding.UTF8.GetBytes(arquivo));

                Console.WriteLine("[+] Comando e caminho enviados ao servidor.");

                int BlockSize = 1024;
                Byte[] DataByte = new Byte[BlockSize];
                int DataRead;

                string caminho = @"C:\Users\Mykae\Downloads\recv\uhuu.mp3";

                Console.WriteLine($"[+] Recebendo arquivo e salvando em: {caminho}");

                // Abre o arquivo para escrita no caminho fornecido
                using (Stream FileStream = File.OpenWrite(caminho))
                {
                    while ((DataRead = NetworkStream.Read(DataByte, 0, BlockSize)) > 0)
                    {
                        FileStream.Write(DataByte, 0, DataRead);
                    }
                }

                Console.WriteLine("[+] Arquivo recebido com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[-] Erro ao receber o arquivo: {ex.Message}");
            }
        }

    }
}