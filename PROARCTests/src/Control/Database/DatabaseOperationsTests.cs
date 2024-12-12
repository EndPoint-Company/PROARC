using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PROARC.src.Control.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Control.Database.Tests
{
    [TestClass()]
    public class DatabaseOperationsTests
    {
        [TestMethod()]
        public void QuerySqlCommandTest()
        {
            DatabaseOperations.QuerySqlCommand("SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'");
        }

        [TestMethod()]
        public void CreateProgramDatabaseTest()
        {
            DatabaseOperations.CreateProgramDatabase();
        }
    }
}