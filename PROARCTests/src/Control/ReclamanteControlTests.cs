using Microsoft.VisualStudio.TestTools.UnitTesting;
using PROARC.src.Control;
using PROARC.src.Models;
using PROARC.src.Models.Tipos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Control.Tests
{
    [TestClass()]
    public class ReclamanteControlTests
    {
        [TestMethod()]
        public void GetReclamanteIdTest()
        {
            //Console.WriteLine(ReclamanteControl.GetReclamanteId("10"));
        }

        [TestMethod()]
        public void GetReclamanteTest()
        {
            // Console.WriteLine(ReclamanteControl.GetReclamante(1));
        }

        [TestMethod()]
        public void AddReclamanteTest()
        {
            // Reclamante reclamante = new Reclamante("João", "123456789", "987654321");
            // ReclamanteControl.AddReclamante(reclamante);

        }

        [TestMethod()]
        public void AtualizarReclamanteTest()
        {
            // ReclamanteControl.AtualizarReclamante("10", "João arcondicionado", "11", "11");
        }

        [TestMethod()]
        public void RemoverReclamanteTest()
        {
            //ReclamanteControl.RemoverReclamante("11");
        }

        [TestMethod()]
        public void GetAllReclamanteTest()
        {

            try
            {
                LinkedList<Reclamante>? reclamantes = ReclamanteControl.GetAllReclamante();

                if (reclamantes != null && reclamantes.Count > 0)
                {
                    Console.WriteLine("Lista de Reclamantes:");
                    foreach (Reclamante reclamante in reclamantes)
                    {
                        Console.WriteLine($"Nome: {reclamante.Nome}, RG: {reclamante.Rg}, CPF: {reclamante.Cpf}");
                    }
                }
                else
                {
                    Console.WriteLine("Nenhum reclamante encontrado ou houve um erro na consulta.");
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao buscar reclamantes: {e.Message}");
            }
          
        }
    }
}