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
        public async Task DeleteAsyncTest()
        {
            await ReclamacaoControl.DeleteAsync("REC-6717");
        }

        [TestMethod()]
        public async Task CountAsyncTest()
        {
            Console.WriteLine(await ReclamacaoControl.CountAsync());
        }

        [TestMethod()]
        public async Task GetAsyncTest()
        {
            Console.WriteLine(await ReclamacaoControl.GetAsync("titulo234"));
        }
    }
}