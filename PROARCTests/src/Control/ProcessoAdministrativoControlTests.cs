using Microsoft.VisualStudio.TestTools.UnitTesting;
using PROARC.src.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Control.Tests
{
    [TestClass()]
    public class ProcessoAdministrativoControlTests
    {
        [TestMethod()]
        public async Task GetAllTest()
        {
            await ProcessoAdministrativoControl.GetAll();
        }

        [TestMethod()]
        public async Task GetTest()
        {
            await ProcessoAdministrativoControl.Get(1);
        }

        [TestMethod()]
        public async Task InsertTest()
        {
           // await ProcessoAdministrativoControl.Insert(new("caminho", "titulo", 2025, new("Cobrança indevida"), new("EmpresaX", 1, "rua", "bairro", "email@exemplo.com", "cidad", "CE", "07272636000131"), new("Zezin", "11122233345", "1234SP")));
        }
    }
}