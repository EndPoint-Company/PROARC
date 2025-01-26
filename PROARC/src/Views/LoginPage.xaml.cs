using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;
using Azure;
using Microsoft.UI.Xaml.Input;

namespace PROARC.src.Views
{
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
            this.RequestedTheme = Microsoft.UI.Xaml.ElementTheme.Light;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorPanel.Visibility = Visibility.Collapsed;
            carregando.Visibility = Visibility.Visible;
            carregando.IsActive = true;
            LoginButton.IsEnabled = false;

            bool senhaValida = await LoginConnect(CaixaSenha.Password);

            carregando.Visibility = Visibility.Collapsed;
            carregando.IsActive = false;

            if (senhaValida)
            {
                Frame.Navigate(typeof(HomeNavigationPage));
            }
            else
            {
                ErrorPanel.Visibility = Visibility.Visible;
            }

            LoginButton.IsEnabled = true;
        }


        private async Task<bool> LoginConnect(string password)
        {
            if(password == "")
            {
                return false;
            }

            var ipEndPoint = new IPEndPoint(IPAddress.Parse("34.95.187.203"), 9999);

            using TcpClient client = new();
            await client.ConnectAsync(ipEndPoint);

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

        private void PasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                LoginButton_Click(sender, e);
            }
        }
    }
}
