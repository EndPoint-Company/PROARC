using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

using PROARC.src.Models;

namespace PROARC.src.Control
{
    public static class UsuarioControl
    {
        public static Usuario? GetUsuario(int id)
        {
            // Pega usuario da database pelo id unico

            return null;
        }

        public static Usuario? GetUsuario(SecureString acessKey)
        {
            // Pega usuario da database pela chave de acesso unica

            return null;
        }

        public static LinkedList<Usuario>? GetAllUsuario()
        {
            // Pega todos os usuarios da database

            return null;
        }

        public static void RemoveUsuario(int id)
        {
            Usuario? toBeRemoved = GetUsuario(id);

            if (toBeRemoved != null)
            {
                throw new Exception("Usuario não encontrado no banco de dados.");
            }

            // Remove o usuario da database por ID
        }

        public static void AddUsuario(Usuario usuario)
        {
            // Adiciona o usuario na database
        }

        public static void ChangeNameUsuario()
        {
            // Muda o nome do usuario na database
        }
    }
}
