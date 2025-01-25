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
            //Console.WriteLine(await ReclamanteControl.GetReclamanteByIdAsync(1));
        }

        [TestMethod()]
        public async Task GetReclamanteByCpfAsyncTest()
        {
            //Console.WriteLine(await ReclamanteControl.GetReclamanteByCpfAsync("12345678901"));
        }

        [TestMethod()]
        public async Task GetReclamanteByRgAsyncTest()
        {
            //Console.WriteLine(await ReclamanteControl.GetReclamanteByRgAsync("10366083"));
        }

        [TestMethod()]
        public async Task GetAllReclamantesAsyncTest()
        {
            //List<Reclamante> reclamantes = await ReclamanteControl.GetAllReclamantesAsync();
            // foreach (var reclamante in reclamantes)
            //{
            //    Console.WriteLine(reclamante);
            //}
        }

        [TestMethod()]
        public async Task AddReclamanteAsyncTest()
        {
           // await ReclamanteControl.AddReclamanteAsync(new Reclamante ("cabisbaixo", "70934922403", "10366083"));
        }

        [TestMethod()]
        public async Task UpdateReclamanteByIdAsyncTest()
        {
            //await ReclamanteControl.UpdateReclamanteByIdAsync(3,new Reclamante("jeferson", null, "1029312"));
        }

        [TestMethod()]
        public async Task RemoveReclamanteByIdAsyncTest()
        {
           // await ReclamanteControl.RemoveReclamanteByIdAsync(10);
        }

        [TestMethod()]
        public async Task CountReclamantesAsyncTest()
        {
           // Console.WriteLine(await ReclamanteControl.CountReclamantesAsync());
        }
    }
}