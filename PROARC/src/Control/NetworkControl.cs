using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PROARC.src.Control
{
    public class NetworkControl
    {
        public static IPEndPoint GetAddr()
        {
            JObject data = JObject.Parse(File.ReadAllText(@"Assets/config.json"));
            string? ipAddr = (string?)data.GetValue("ip_addr");
            int? portAddr = (int?)data.GetValue("port_addr");

            Console.WriteLine(ipAddr + ":" + portAddr);

            if (ipAddr == null)
                throw new ArgumentNullException(nameof(ipAddr), "Tá faltando um IP aí, meu fi");
            if (portAddr == null)
                throw new ArgumentNullException(nameof(portAddr), "Tá faltando uma porta aí, meu fi");

            return new IPEndPoint(IPAddress.Parse(ipAddr), portAddr.Value);
        }

        public static async Task<string> SendRequestAsync(object request)
        {
            using TcpClient client = new();
            if (!client.ConnectAsync(GetAddr()).Wait(2000))
                throw new SocketException((int)SocketError.NotConnected);

            await client.Client.SendAsync(Encoding.UTF8.GetBytes("DB"));

            await Task.Delay(300);

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
