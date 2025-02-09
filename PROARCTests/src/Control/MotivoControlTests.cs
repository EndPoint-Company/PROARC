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
    public class MotivoControlTests
    {
        [TestMethod()]
        public async Task GetMotivoAsyncTest()
        {
            Console.WriteLine(await MotivoControl.GetAsync("Atraso na entrega"));
        }

        [TestMethod()]
        public async Task GetMotivoAsyncTest1()
        {
            Console.WriteLine((await MotivoControl.GetAsync(2)).Nome);
        }

        [TestMethod()]
        public async Task GetIdMotivoAsyncTest()
        {
            int? abacaxi = await MotivoControl.GetIdAsync("Cobrança indevida");
            Console.WriteLine(abacaxi);
        }

        [TestMethod()]
        public async Task GetAllMotivosAsyncTest()
        {
            List<Motivo> motivo = await MotivoControl.GetAllAsync();

            foreach (Motivo motivos in motivo)
            {
                Console.WriteLine(motivos);
            }
        }

        [TestMethod()]
        public async Task AddMotivoAsyncTest()
        {
            await MotivoControl.InsertAsync(new Motivo("juros abusivos"));
        }

        [TestMethod()]
        public async Task UpdateMotivoAsyncTest()
        {
            await MotivoControl.UpdateAsync("juros bananasticos", "Atraso na entrega");
        }

        [TestMethod()]
        public async Task RemoveMotivoAsyncTest()
        {
            await MotivoControl.DeleteAsync("juros abusivos");
        }

        [TestMethod()]
        public async Task CountMotivosTest()
        {
            Console.WriteLine(await MotivoControl.CountAsync());
        }
    }
}