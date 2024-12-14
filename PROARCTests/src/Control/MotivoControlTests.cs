﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using PROARC.src.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments.AppointmentsProvider;
using PROARC.src.Models;
using PROARC.src.Control.Database;
using PROARC.src.Models.Tipos;

namespace PROARC.src.Control.Tests
{
    [TestClass()]
    public class MotivoControlTests
    {
        [TestMethod("teste de metodo getMotivo")]
        public void GetMotivoTest()
        {
            try
            {
                Console.WriteLine(MotivoControl.GetMotivo("juros abusivos"));
            }

            catch (Exception e)
            {
                throw new Exception("Erro ao buscar usuário com ID {id}: {e.Message}");
            }
        }

        [TestMethod("teste de metodo getMotivo por id")]
        public void GetMotivoTest1()
        {
            try
            {
                Console.WriteLine(MotivoControl.GetMotivo(1));
            }

            catch (Exception e)
            {
                throw new Exception("Erro ao buscar usuário com ID {id}: {e.Message}");
            }
        }

        [TestMethod("teste de metodo addmotivo")]
        public void AddMotivoTest()
        {
            try
            {
                 Motivo motivo = new Motivo("juros abusivos", "exemplo exemplo exemplo");
                 MotivoControl.AddMotivo(motivo);
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao adicionar usuario {e.Message}");
            }
        }

        [TestMethod("teste do metodo removerMotivo")]
        public void RemoverMotivoTest()
        {
            try
            {
               MotivoControl.RemoverMotivo("juros catastroficos");
            }

            catch (Exception e)
            {
                throw new Exception("Erro ao buscar Motivo com {nome}: {e.Message}");
            }
        }

        [TestMethod()]
        public void AtualizarMotivoTest()
        {
            try
            {
               //MotivoControl.AtualizarMotivo("Motivo Exemplo", "juros catastroficos", "deu certo");
            }

            catch (Exception e)
            {
                throw new Exception("Erro ao buscar Motivo com nome {nome}: {e.Message}");
            }
        }
    }
}