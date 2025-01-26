using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PROARC.src.Control
{
    public class NetworkControl
    {
        private static readonly string ServerIp = "192.168.0.14";
        private static readonly int ServerPort = 6666;

        public static async Task<string> SendRequestAsync(object request)
        {
            using TcpClient client = new();
            await client.ConnectAsync(ServerIp, ServerPort);

            await client.Client.SendAsync(Encoding.UTF8.GetBytes("DB"));    

            NetworkStream stream = client.GetStream();
            string? jsonRequest = JsonSerializer.Serialize(request);

            byte[] requestBytes = Encoding.UTF8.GetBytes(jsonRequest);
            await stream.WriteAsync(requestBytes, 0, requestBytes.Length);

            string returnString = String.Empty;

            while (true)
            {
                byte[] responseBuffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(responseBuffer, 0, responseBuffer.Length);

                if (bytesRead == 0) break;

                returnString += Encoding.UTF8.GetString(responseBuffer, 0, bytesRead);
            }

            return returnString;
        }
    }
}
