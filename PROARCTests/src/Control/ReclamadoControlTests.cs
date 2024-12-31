using Microsoft.VisualStudio.TestTools.UnitTesting;
using PROARC.src.Control;
using PROARC.src.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Control.Tests
{
    [TestClass()]
    public class ReclamadoControlTests
    {
        [TestMethod()]
        public void GetReclamadoTest()
        {
            try
            {
                Console.WriteLine(ReclamadoControl.GetReclamado(1));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        [TestMethod()]
        public void GetAllReclamadosTest()
        {

            try
            {
                LinkedList<Reclamado>? reclamados = ReclamadoControl.GetAllReclamados();

                if (reclamados != null && reclamados.Count > 0)
                {
                    Console.WriteLine("Lista de Reclamados:");
                    foreach (Reclamado reclamado in reclamados)
                    {
                        Console.WriteLine(reclamado + "\n");


                    }
                }
                else
                {
                    Console.WriteLine("Nenhum Reclamado encontrado ou houve um erro na consulta.");
                }

            }

            catch (Exception e)
            {
                throw new Exception("Erro ao buscar Reclamado com ID {id}: {e.Message}");
            }
        }

        [TestMethod()]
        public void AddReclamadoTest()
        {
            try
            {
                // Reclamado reclamado = new Reclamado("Jeferson", 148, "guaraci", "centro", "mv@gmail.com", "ipubi", "pe", "123123", "70934922403");
                // ReclamadoControl.AddReclamado(reclamado);

            }
            catch (Exception e)
            {
                throw new Exception("Erro ao adicionar Reclamado: {e.Message}");
            }

        }

        [TestMethod()]
        public void AtualizarReclamadoTest()
        {
            //Reclamado reclamado = new Reclamado("jeferson", 148, "guaraci", "centro", "mv@gmail.com", "ipubi", "pe", "123123", "70934922403");
            //ReclamadoControl.AtualizarReclamado(6, reclamado);
        }

        [TestMethod()]
        public void GetReclamadoIdTest()
        {
            ReclamadoControl.GetReclamadoId("12345678900", "João da Silva");
        }

        [TestMethod()]
        public void AddReclamadoTest1()
        {
            try
            {
                Reclamado reclamado = new Reclamado("Jeferson", 148, "guaraci", "centro", "mv@gmail.com", "ipubi", "pe", "123123", "70934922403");
                ReclamadoControl.AddReclamado(reclamado);

            }
            catch (Exception e)
            {
                throw new Exception("Erro ao adicionar Reclamado: {e.Message}");
            }
        }
    }
}