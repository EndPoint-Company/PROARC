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
    public class ReclamacaoControlTests
    {
        [TestMethod()]
        public async Task GetNRowsTest()
        {
            await ReclamacaoControl.GetNRows(10, 0);
        }
    }
}