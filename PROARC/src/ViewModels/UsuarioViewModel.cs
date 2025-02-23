using PROARC.src.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PROARC.src.ViewModels
{
    public class UsuarioViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Usuario _usuario;

        public string Nome { get; }
        public string Cargo { get; }

        private static LinkedList<UsuarioViewModel> _usuarios = new();

        public UsuarioViewModel(Usuario usuario)
        {
            _usuario = usuario;
            Nome = usuario.Nome;
            Cargo = usuario.Cargo;
        }

        public static async Task<IEnumerable<UsuarioViewModel>> CarregarUsuariosAsync()
        {
            var usuarios = await PROARC.src.Control.UsuarioControl.GetAll();
            _usuarios.Clear();

            if (usuarios != null)
            {
                foreach (var usuario in usuarios)
                {
                    if (usuario != null)
                    {
                        _usuarios.AddLast(new UsuarioViewModel(usuario));
                    }
                }
            }
            return _usuarios;
        }
    }
}
