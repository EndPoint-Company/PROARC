using Microsoft.VisualStudio.TestTools.UnitTesting;
using PROARC.src.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Control.Tests
{
    [TestClass()]
    public class FileNetworkControlTests
    {
        [TestMethod()]
        public void Local_CriarFolderProcessoAdministrativoTest()
        {
            FileNetworkControl.Local_SetDefaultFolder(@"D:/ProarcFiles/Teste1");
            FileNetworkControl.Local_CriarFolderProcessoAdministrativo(new("","0001ooo2024",2024));
        }
    }
}