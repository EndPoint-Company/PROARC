using Microsoft.VisualStudio.TestTools.UnitTesting;
using PROARC.src.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PROARC.src.Models.Arquivos;
using PROARC.src.Models.Tipos;
using PROARC.src.Models;

namespace PROARC.src.Control.Tests
{
    [TestClass()]
    public class ProcessoAdministrativoControlTests
    {
        [TestMethod()]
        public async Task GetAllTest()
        {
            List<ProcessoAdministrativo>? lista = await ProcessoAdministrativoControl.GetAllAsync();
            for (int i = 0; i < lista.Count; i++)
            {
                ProcessoAdministrativo? listas = lista[i];
                Console.WriteLine(listas);
            }
        }

        [TestMethod()]
        public async Task GetTest()
        {
            await ProcessoAdministrativoControl.GetAsync(1);
        }

        [TestMethod()]
        public async Task InsertAsyncTest()
        {
            Assert.Fail();

        }

        [TestMethod()]
        public async Task GetReclamadoFromRelacaoTest()
        {
           Console.WriteLine(await ProcessoAdministrativoControl.GetReclamadoFromRelacao(34));
        }
    }
}