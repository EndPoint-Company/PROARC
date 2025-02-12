using Microsoft.VisualStudio.TestTools.UnitTesting;
using PROARC.src.Control;
using PROARC.src.Models.Arquivos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PROARC.src.Control.NetworkControl;
using PROARC.src.Models.Arquivos;

namespace PROARC.src.Models.Arquivos.Tests
{
    [TestClass()]
    public class ReclamacaoTests
    {
        [TestMethod()]
        public async Task ReclamacaoTest()
        {
            
            LinkedList<Reclamado> reclamados = new ();
            reclamados.AddLast(new Reclamado("uefice","12312312310",null,1234, "guaraci", "centro", "ipubas", "ce","12312312", "12312323410", "ufc@gmail.com"));
            reclamados.AddLast(new Reclamado("siri cascudo", null, "12312312311123", 12342, "guaraciaba", "centro", "ipubas", "ce", "12312312", "30129381208", "ufc@gmail.com"));
            ReclamacaoEnel reclamacao = new ReclamacaoEnel(new("Atraso na entrega"), new("marcos vitor", "12345678910",null), null,reclamados, "titulo2382", "situacao", "caminhoDir594", DateOnly.FromDateTime(DateTime.Now), "criador", "atendente", "12345634911", "jose@gmail.com", "observacao");
            var request = new { action = "insert_reclamacao", reclamacao };
            string response = await SendRequestAsync(request);
            Console.WriteLine(response);
            
        }
    }
}