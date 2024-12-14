using Microsoft.VisualStudio.TestTools.UnitTesting;
using PROARC.src.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments.AppointmentsProvider;
using PROARC.src.Models;
using PROARC.src.Control.Database;

namespace PROARC.src.Control.Tests
{
    [TestClass()]
    public class UsuarioControlTests
    {
        [TestMethod("teste de metodo getUsuario")]
        public void GetUsuarioTest()
        {
            try
            {
               Console.WriteLine (UsuarioControl.GetUsuario(6));
            }

            catch (Exception e)
            {
                throw new Exception("Erro ao buscar usuário com ID {id}: {e.Message}");
            }
        }

        [TestMethod("teste de metodo removerUsuario")]
        public void RemoverUsuario()
        {
            try
            {
               // UsuarioControl.RemoveUsuario(3);
            }

            catch (Exception e)
            {
                throw new Exception("Erro ao buscar usuário com ID {id}: {e.Message}");
            }
        }

        [TestMethod("teste de metodo AdicionarUsuario")]
        public void AdicionarUsuario()
        {
            try
            {
                //Usuario user = new Usuario("pedro", 2);
               // UsuarioControl.AddUsuario(user);
            }

            catch (Exception e)
            {
                throw new Exception("Erro ao adicionar usuario {e.Message}");
            }
        }

        [TestMethod("teste de metodo getAllUsuario")]
        public void GetAllUsuarioTest()
        {
            try
            {
                LinkedList<Usuario>? usuarios = UsuarioControl.GetAllUsuario();

                if (usuarios != null && usuarios.Count > 0)
                {
                    Console.WriteLine("Lista de Usuários:");
                    foreach (Usuario usuario in usuarios)
                    {
                        Console.WriteLine($"Nome: {usuario.Nome}, Nível de Permissão: {usuario.NivelDePermissao}");
                    }
                }
                else
                {
                    Console.WriteLine("Nenhum usuário encontrado ou houve um erro na consulta.");
                }
            }

            catch (Exception e)
            {
                throw new Exception("Erro ao buscar usuário com ID {id}: {e.Message}");
            }
        }

        [TestMethod()]
        public void AtualizarUsuarioTest1()
        {
            try
            {              
                UsuarioControl.AtualizarUsuario(12, "JONATHAN");
            }

            catch (Exception e)
            {
                throw new Exception("Erro ao buscar usuário com ID {id}: {e.Message}");
            }
        }

        [TestMethod()]
        public void AtualizarUsuarioTest2()
        {
            try
            {
                UsuarioControl.AtualizarUsuario(11, 3);
            }

            catch (Exception e)
            {
                throw new Exception("Erro ao buscar usuário com ID {id}: {e.Message}");
            }
        }

        [TestMethod()]
        public void AtualizarUsuarioTest3()
        {
            try
            {
                UsuarioControl.AtualizarUsuario(6, "carlos vitor",3);
            }

            catch (Exception e)
            {
                throw new Exception("Erro ao buscar usuário com ID {id}: {e.Message}");
            }
        }


    }
}