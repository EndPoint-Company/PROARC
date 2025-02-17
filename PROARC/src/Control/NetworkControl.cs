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
        private static readonly string ServerIp = "34.39.145.11";

        private static readonly int ServerPort = 9999;

        public static async Task<string> SendRequestAsync(object request)
        {
            using TcpClient client = new();
            if (!client.ConnectAsync(ServerIp, ServerPort).Wait(2000))
                throw new SocketException((int)SocketError.NotConnected);

            await client.Client.SendAsync(Encoding.UTF8.GetBytes("DB"));

            using NetworkStream stream = client.GetStream();
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
