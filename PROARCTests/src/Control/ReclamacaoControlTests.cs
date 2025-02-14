using Microsoft.VisualStudio.TestTools.UnitTesting;
using PROARC.src.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PROARC.src.Models.Arquivos;
using PROARC.src.Models;
using PROARC.src.Models.Tipos;


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
            Reclamacao reclamacao = await ReclamacaoControl.GetAsync("titus");
            Console.WriteLine(reclamacao);
        }

        [TestMethod()]
        public async Task GetNRowsTest()
        {
            LinkedList<Reclamacao> abacate = await ReclamacaoControl.GetNRows(10, 0);
            foreach (Reclamacao reclamacao in abacate)
            {
                Console.WriteLine(reclamacao + "\n");
            }
        }

        [TestMethod()]
        public async Task InsertAsyncTest()
        {
            LinkedList<Reclamado> reclamados = new();
            LinkedList<Reclamado> reclamaenel = new();

            Reclamado reclamado1 = new Reclamado("João Silva", "12345678900", null, 123, "Rua das Flores", "Centro", "São Paulo", "SP", "0100000", "98765432111", "joao@email.com");
            Reclamado reclamado2 = new Reclamado("Mercado dois irmaos", null, "12345678000199", 456, "Avenida Paulista", "Bela Vista", "São Paulo", "SP", "01310000", "11912345678", "mercado@empresa.com");
            reclamaenel.AddFirst(reclamado2);
            reclamados.AddFirst(reclamado1);
            reclamados.AddFirst(reclamado2);
            ReclamacaoEnel recenel = new ReclamacaoEnel(new Motivo("Atraso na entrega"), new Reclamante("lorax", "12345678950", null), null, reclamaenel, "testenel9", "situacao", "caminhodir258", DateOnly.FromDateTime(DateTime.Now), "marquin", null, null, null, null);
            ReclamacaoGeral recgeral = new ReclamacaoGeral(new Motivo("Atraso na entrega"), new Reclamante("Marcos Vitor", "12345678900", null), null, reclamados, "titus", "Aberto", "C:\\Users\\marco\\Documents\\Rec", DateOnly.FromDateTime(DateTime.Now), "Marcos Vitor", DateTime.Now, "advogada");
            await ReclamacaoControl.InsertAsync(recenel);
        }

        [TestMethod()]
        public async Task CountGAsyncTest()
        {
            Console.WriteLine(await ReclamacaoControl.CountGAsync());
        }

        [TestMethod()]
        public async Task CountEAsyncTest()
        {
            Console.WriteLine(await ReclamacaoControl.CountEAsync());
        }

        [TestMethod()]
        public async Task UpdateSituacaoAsyncTest()
        {
            await ReclamacaoControl.UpdateSituacaoAsync("titus", "Fechado");
        }

        [TestMethod()]
        public async Task UpdateAsyncTest()
        {
            LinkedList<Reclamado> reclamadoenel = new();
            Reclamado reclamado1 = new Reclamado("Enel", "12345606200", null, 123, "Rua das Flores", "Centro", "São Paulo", "SP", "0100000", "98235432111", "Enel@email.com");
            reclamadoenel.AddFirst(reclamado1);

            ReclamacaoEnel recenel = new ReclamacaoEnel(new Motivo("Cobrança indevida"), new Reclamante("João Santos", "11122233344", null), null, reclamadoenel, "teste21a9", "Ruim", "caminho219", DateOnly.FromDateTime(DateTime.Now), "marquin", null, null, null, null);

            await ReclamacaoControl.UpdateAsync("teste21a9", recenel);
        }

        [TestMethod()]
        public async Task UpdatetAsyncTest()
        {
            LinkedList<Reclamado> reclamados = new();
            Reclamado reclamado1 = new Reclamado("João Silva", "12345678900", null, 123, "Rua das Flores", "Centro", "São Paulo", "SP", "0100000", "98765432111", "joao@email.com");
            Reclamado reclamado2 = new Reclamado("Mercado dois irmaos", null, "12345678000199", 456, "Avenida Paulista", "Bela Vista", "São Paulo", "SP", "01310000", "11912345678", "mercado@empresa.com");
            reclamados.AddFirst(reclamado1);
            reclamados.AddFirst(reclamado2);
            ReclamacaoGeral recgeral = new ReclamacaoGeral(new Motivo("Atraso na entrega"), new Reclamante("lorax", "12345678950", null), null, reclamados, "testeok", "Aberto", "C:\\Users\\marco\\Documents\\Rec12313", DateOnly.FromDateTime(DateTime.Now), "Marcos Vitor", DateTime.Now, "advogada");
            await ReclamacaoControl.UpdateAsync("testeok", recgeral);
        }
    }
}