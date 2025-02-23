using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using PROARC.src.Models;
using System.Text.Json;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using static PROARC.src.Control.NetworkControl;
using PROARC.src.Models.Arquivos;
using PROARC.src.Models.Tipos;

namespace PROARC.src.Control
{
    public static class UsuarioControl
    {
       public static async Task<bool> LoginConnect(string password)
       {
            if (password == "")
            {
                return false;
            }

            using TcpClient client = new();
            if (!client.ConnectAsync(NetworkControl.GetAddr()).Wait(2000))
                throw new SocketException((int)SocketError.NotConnected);

            NetworkStream stream = client.GetStream();

            client.Client.Send(Encoding.UTF8.GetBytes("AUTH"));

            byte[] buffer = new byte[4096];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine(response);
            System.Diagnostics.Debug.WriteLine(response);

            var tableData = JsonSerializer.Deserialize<List<object[]>>(response);

            List<String> list = new List<string>();

            foreach (var item in tableData)
            {
                list.Add(item[0].ToString());
            }

            foreach (var salt in list)
            {
                Console.WriteLine(salt);
                byte[] passwordHash = SHA256.HashData(Encoding.UTF8.GetBytes(password + salt));

                client.Client.Send(passwordHash);

                byte[] buffera = new byte[4096];
                int bytesReada = await stream.ReadAsync(buffera, 0, buffera.Length);
                string responsee = Encoding.UTF8.GetString(buffera, 0, bytesReada);

                System.Diagnostics.Debug.WriteLine(responsee);
                Console.WriteLine(responsee);

                if (responsee == "OK")
                {
                    client.Client.Send(Encoding.UTF8.GetBytes("BYE"));

                    return true;
                }
            }

            client.Client.Send(Encoding.UTF8.GetBytes("BYE"));

            return false;
       }

        public static async Task<LinkedList<Usuario?>?> GetAll()
        {
            var action = new { action = "get_all_usuarios" };
            string res = await SendRequestAsync(action);

            Console.WriteLine(res);

            LinkedList<Usuario?>? usuarios = new LinkedList<Usuario?>();
            JToken jsonToken = JToken.Parse(res);

            if (jsonToken.Type == JTokenType.String)
            {
                jsonToken = JToken.Parse(jsonToken.Value<string>());
            }

            JArray reclamacoesArray = (JArray)jsonToken["usuarios"];

            if (reclamacoesArray == null) return null;

            Console.WriteLine(reclamacoesArray.ToString());

            foreach (JArray usr in reclamacoesArray)
            {
                Usuario usuario = new Usuario();
                usuario.Nome = (string?)usr[0];
                usuario.Cargo = (string?)usr[1];

                Console.WriteLine(usuario.ToString());

                usuarios.AddLast(usuario);
            }

            return usuarios;
        }
    }
}
