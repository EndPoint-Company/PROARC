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
    public class ReclamanteControlTests
    {
        [TestMethod()]
        public async Task GetReclamanteByIdAsyncTest()
        {
            //Console.WriteLine(await ReclamanteControl.GetAsync(1));
        }

        [TestMethod()]
        public async Task GetReclamanteByCpfAsyncTest()
        {
            //Console.WriteLine(await ReclamanteControl.GetReclamanteByCpfAsync("55566677788"));
        }

        [TestMethod()]
        public async Task GetReclamanteByRgAsyncTest()
        {
            //Console.WriteLine(await ReclamanteControl.GetReclamanteByRgAsync("123456789"));
        }

        [TestMethod()]
        public async Task GetAllReclamantesAsyncTest()
        {
            List<Reclamante> reclamantes = await ReclamanteControl.GetAllAsync();
             foreach (var reclamante in reclamantes)
            {
                Console.WriteLine(reclamante + "\n");
            }
        }

        [TestMethod()]
        public async Task AddReclamanteAsyncTest()
        {
            await ReclamanteControl.InsertAsync(new Reclamante ("cabisbaixo", "70934922403", null, "12312312319", "marcoscabs@gmail.com"));
        }

        [TestMethod()]
        public async Task UpdateReclamanteByIdAsyncTest()
        {
            await ReclamanteControl.UpdateAsync(4,new Reclamante("jonathan cabecao", "70934922403", null, "12312312319", "marcoscabs@gmail.com"));
        }

        [TestMethod()]
        public async Task RemoveReclamanteByIdAsyncTest()
        {
           await ReclamanteControl.DeleteAsync(4);
        }

        [TestMethod()]
        public async Task CountReclamantesAsyncTest()
        {
           Console.WriteLine(await ReclamanteControl.CountAsync());
        }
    }
}