using Microsoft.VisualStudio.TestTools.UnitTesting;
using PROARC.src.Models.Arquivos;
using PROARC.src.Models.Tipos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Models.Arquivos.Tests
{
    [TestClass()]
    public class ArquivoTests
    {
        [TestMethod()]
        public void ArquivoTest()
        {
            Arquivo arquivo = new(
                @"/home/server/proarc/processos/0001_2024/tdr/termo00012024.docx", 
                ArquivoTipo.TermoDeReclamação,
                20041);

            Assert.AreEqual(@"/home/server/proarc/processos/0001_2024/tdr/termo00012024.docx", arquivo.CaminhoDoArquivo);
        }

        [TestMethod()]
        public void EqualsTest()
        {
            Arquivo arquivo1 = new(
                @"/home/server/proarc/processos/0001_2024/tdr/termo00012024.docx",
                ArquivoTipo.TermoDeReclamação,
                20041);

            Arquivo arquivo2 = new(
                @"/home/server/proarc/processos/0001_2024/tdr/termo00012024.docx",
                ArquivoTipo.TermoDeReclamação,
                20041);

            Assert.AreEqual(arquivo1, arquivo2);
        }
    }
}