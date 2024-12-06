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
        public static Usuario? getUsuario(int id)
        {
            // Pega usuario da database pelo id unico

            return null;
        }

        public static Usuario? getUsuario(SecureString acessKey)
        {
            // Pega usuario da database pela chave de acesso unica

            return null;
        }

        public static LinkedList<Usuario>? getAllUsuario()
        {
            // Pega todos os usuarios da database

            return null;
        }

        public static void removeUsuario(int id)
        {
            Usuario? toBeRemoved = getUsuario(id);

            if (toBeRemoved != null)
            {
                throw new Exception("Usuario não encontrado no banco de dados.");
            }

            // Remove o usuario da database por ID
        }

        public static void addUsuario(Usuario usuario)
        {
            // Adiciona o usuario na database
        }

        public static void changeNameUsuario()
        {
            // Muda o nome do usuario na database
        }
    }
}
