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
            Console.WriteLine((await MotivoControl.GetMotivoAsync("Curiosidade")).Nome);
        }

        [TestMethod()]
        public async Task GetMotivoAsyncTest1()
        {
            Console.WriteLine((await MotivoControl.GetMotivoAsync(20)).Nome);
        }

        [TestMethod()]
        public async Task GetIdMotivoAsyncTest()
        {
            int? abacaxi = await MotivoControl.GetIdMotivoAsync("Apetite");
            Console.WriteLine(abacaxi);
        }

        [TestMethod()]
        public async Task GetAllMotivosAsyncTest()
        {
            List<Motivo> motivo = await MotivoControl.GetAllMotivosAsync();

            foreach (Motivo motivos in motivo)
            {
                Console.WriteLine($"Nome = {motivos.Nome}, Descrição = {motivos.Descricao}");
            }
        }
    
        [TestMethod()]
        public async Task AddMotivoAsyncTest()
        {
            await MotivoControl.AddMotivoAsync(new Motivo("juros splashsticos", "marcos gostoso"));
        }

        [TestMethod()]
        public async Task RemoveMotivoAsyncTest()
        {
            await MotivoControl.UpdateMotivoAsync("Estudo", "juros capistropolicos", "redefinido para análise futura");
        }

        [TestMethod()]
        public async Task UpdateMotivoAsyncTest()
        {
            await MotivoControl.RemoveMotivoAsync("juros capistropolicos");
        }
    }
}