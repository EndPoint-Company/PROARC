using Microsoft.VisualStudio.TestTools.UnitTesting;
using PROARC.src.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PROARC.src.Models.Arquivos;


namespace PROARC.src.Control.Tests
{
    [TestClass()]
    public class ReclamacaoControlTests
    {
        [TestMethod()]
        public async Task DeleteAsyncTest()
        {
            await ReclamacaoControl.DeleteAsync("REC-5058");
        }

        [TestMethod()]
        public async Task CountAsyncTest()
        {
            Console.WriteLine(await ReclamacaoControl.CountAsync());
        }

        [TestMethod()]
        public async Task GetAsyncTest()
        {
            Reclamacao reclamacao = await ReclamacaoControl.GetAsync("teste");
            Console.WriteLine(reclamacao);
        }       
        
        [TestMethod()]
        public async Task GetNRowsTest()
        {
            LinkedList<Reclamacao> abacate = await ReclamacaoControl.GetNRows(10, 0);
            foreach (Reclamacao reclamacao in abacate)
            {
                Console.WriteLine(reclamacao+"\n");
            }
        }
    }
}