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
            List<ProcessoAdministrativo> lista = await ProcessoAdministrativoControl.GetAllAsync();
            foreach (ProcessoAdministrativo listas in lista)
            {
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
    }
}