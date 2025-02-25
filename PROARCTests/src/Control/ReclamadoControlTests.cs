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
            Console.WriteLine(await ReclamadoControl.GetAsync(1));
        }

        [TestMethod()]
        public async Task GetAllReclamadosAsyncTest()
        {
            List<Reclamado> abacate = await ReclamadoControl.GetAllAsync();

            foreach (Reclamado abacates in abacate)
            {
                Console.WriteLine(abacates.ToString() + "\n");
            }
        }

        [TestMethod()]
        public async Task InsertAsyncTest()
        {
            //certo
            await ReclamadoControl.InsertAsync(new Reclamado("balde de lixo", "12312312318", null, 12342, "guaraci", "centro", "ipubas", "ce", "12312312", "12312323410", "cabecinha@gmail.com"));
        }

        [TestMethod()]
        public async Task UpdateReclamadoByIdAsyncTest()
        {   
            //certo
            //await ReclamadoControl.UpdateAsync(3, new Reclamado("balde de lixo", "12312312318", null, 12342, "guaraci", "centro", "ipubas", "ce", "12312312", "12312323410", "cabecinha@gmail.com"));
        }

        [TestMethod()]
        public async Task RemoveReclamadoByIdAsyncTest()
        {
            //certo
            await ReclamadoControl.DeleteAsync(3);
        }

        [TestMethod()]
        public async Task CountReclamadosAsyncTest()
        {
            //certo
           Console.WriteLine( await ReclamadoControl.CountAsync());
        }
    }
}