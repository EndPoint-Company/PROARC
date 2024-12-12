using Microsoft.VisualStudio.TestTools.UnitTesting;
using PROARC.src.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Models.Tests
{
    [TestClass()]
    public class UsuarioTests
    {
        [TestMethod("Promover: 2 -> 3")]
        public void PromoverTest()
        {
            Usuario usuario = new("Judiscrei", 2);
            usuario.Promover();
        }

        [TestMethod("Promover: 4 -> 5")]
        public void PromoverTest1()
        {
            Usuario usuario = new("Judiscrei", 4);
            try { usuario.Promover(); }
            catch (Exception ex) 
            { 
                Assert.AreEqual(usuario.NivelDePermissao, 4); 
            }
        }
    }
}