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
    }
}