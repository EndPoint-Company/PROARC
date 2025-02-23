using Microsoft.VisualStudio.TestTools.UnitTesting;
using PROARC.src.Control.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Control.Utils.Tests
{
    [TestClass()]
    public class EstatisticasControlTests
    {
        [TestMethod()]
        public async Task GetAsyncTest()
        {
            List<string> lista = await EstatisticasControl.GetAsync(5);
            foreach (string item in lista)
            {
                Console.WriteLine(item);
            }
        }

        [TestMethod()]
        public async Task GetMAsyncTest()
        {
            List<string> lista = await EstatisticasControl.GetMotivosNumAsync();
            foreach (string item in lista)
            {
                Console.WriteLine(item);
            }
        }

        [TestMethod()]
        public async Task GetReclamadosPorMesAsyncTest()
        {
            List<string> lista = await EstatisticasControl.GetReclamadosPorMesAsync();
            foreach (string item in lista)
            {
                Console.WriteLine(item);
            }
        }
    }
}