using Microsoft.VisualStudio.TestTools.UnitTesting;
using PROARC.src.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments.AppointmentsProvider;
using PROARC.src.Models;
using PROARC.src.Models.Tipos;

namespace PROARC.src.Control.Tests
{
    [TestClass()]
    public class ReclamadoControlTests
    {
        [TestMethod()]
        public async Task GetReclamadoByIdAsyncTest()
        {
           // Console.WriteLine(await ReclamadoControl.GetReclamadoByIdAsync(3));
        }

        [TestMethod()]
        public async Task GetAllReclamadosAsyncTest()
        {
            List<Reclamado> abacate = await ReclamadoControl.GetAllAsync();

            foreach (Reclamado abacates in abacate)
            {
                Console.WriteLine(abacates.ToString());
            }
        }

        [TestMethod()]
        public async Task AddReclamadoAsyncTest()
        {
            //certo
           //await ReclamadoControl.AddReclamadoAsync(new Reclamado("marco pinto", 123, "guaraci", "centro", "marcosvbitor@gmail.com", "ipubi", "pe", null, null));
        }

        [TestMethod()]
        public async Task UpdateReclamadoByIdAsyncTest()
        {
            //await ReclamadoControl.UpdateReclamadoByIdAsync(5, new Reclamado("marco pinto segundo", 1234, "guaracis", "centros", "marcosvbitor@gmaisl.com", "ipubis", "pe", null, null));
        }

        [TestMethod()]
        public async Task RemoveReclamadoByIdAsyncTest()
        {
            //certo
            //await ReclamadoControl.RemoveReclamadoByIdAsync(10);
        }

        [TestMethod()]
        public async Task CountReclamadosAsyncTest()
        {
            //certo
           //Console.WriteLine( await ReclamadoControl.CountReclamadosAsync());
        }
    }
}