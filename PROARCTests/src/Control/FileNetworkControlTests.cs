﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public async Task SendFileTest()
        {
            await FileNetworkControl.SendFile(@"C:\Users\Mykae\Downloads\counting_stars.mp3", "G001-2025");
        }

        [TestMethod()]
        public async Task ReceiveFileTest()
        {
            await FileNetworkControl.ReceiveFile("G001-2025", "counting_stars.mp3");
        }
    }
}