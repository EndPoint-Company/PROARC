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
using PROARC.src.Control;

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

            bool senhaValida = false;

            try
            {
                senhaValida = await LoginConnect(CaixaSenha.Password);
            }
            catch (SocketException ex)
            {
                ErrorText.Text = $"Erro ao conectar com o servidor.";
            }

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
            return await UsuarioControl.LoginConnect(password);
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
