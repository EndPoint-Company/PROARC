using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PROARC.src.Models;

namespace PROARC.src.ViewModels
{
    public class DashboardViewModel
    {
        // Propriedade para armazenar os dados do gráfico de reclamações
        public List<ComplaintData> ComplaintData { get; private set; }

        // Propriedade para armazenar os dados das empresas mais reclamadas
        public List<CompanyComplaintData> TopCompanies { get; private set; }

        // Propriedade para armazenar os dados dos motivos mais recorrentes
        public List<ReasonData> TopReasons { get; private set; }

        public DashboardViewModel()
        {
            ComplaintData = new List<ComplaintData>();
            TopCompanies = new List<CompanyComplaintData>();
            TopReasons = new List<ReasonData>();
        }

        // Método para carregar os dados de reclamações dos últimos 12 meses
        public async Task LoadComplaintDataAsync()
        {
            // Simulação de uma chamada ao banco de dados
            await Task.Delay(1000); // Simula um atraso de rede/banco de dados

            // Dados de exemplo (substitua por dados reais do banco de dados)
            var currentDate = DateTime.Now;
            var random = new Random();

            ComplaintData = Enumerable.Range(0, 12)
                .Select(i => new ComplaintData
                {
                    Date = currentDate.AddMonths(-i), // Armazena a data completa
                    Complaints = random.Next(10, 100) // Número aleatório de reclamações
                })
                .OrderBy(d => d.Date) // Ordena pela data completa
                .ToList();
        }

        // Método para carregar as 5 empresas mais reclamadas
        public async Task LoadTopCompaniesAsync()
        {
            // Simulação de uma chamada ao banco de dados
            await Task.Delay(1000); // Simula um atraso de rede/banco de dados

            // Dados de exemplo (substitua por dados reais do banco de dados)
            TopCompanies = new List<CompanyComplaintData>
        {
            new CompanyComplaintData { CompanyName = "Empresa A", Complaints = 120 },
            new CompanyComplaintData { CompanyName = "Empresa B", Complaints = 95 },
            new CompanyComplaintData { CompanyName = "Empresa C", Complaints = 80 },
            new CompanyComplaintData { CompanyName = "Empresa D", Complaints = 70 },
            new CompanyComplaintData { CompanyName = "Empresa E", Complaints = 60 }
        };
        }

        // Método para carregar os 5 motivos mais recorrentes
        public async Task LoadTopReasonsAsync()
        {
            // Simulação de uma chamada ao banco de dados
            await Task.Delay(1000); // Simula um atraso de rede/banco de dados

            // Dados de exemplo (substitua por dados reais do banco de dados)
            TopReasons = new List<ReasonData>
        {
            new ReasonData { Reason = "Problemas com Entrega", Count = 150 },
            new ReasonData { Reason = "Produto Danificado", Count = 120 },
            new ReasonData { Reason = "Atendimento ao Cliente", Count = 100 },
            new ReasonData { Reason = "Cobrança Incorreta", Count = 90 },
            new ReasonData { Reason = "Produto Não Recebido", Count = 80 }
        };
        }
    }

    // Classe para representar os dados de motivos de reclamações
    public class ReasonData
    {
        public string Reason { get; set; } // Motivo da reclamação
        public int Count { get; set; } // Quantidade de reclamações
    }

    // Classe para representar os dados de reclamações por empresa
    public class CompanyComplaintData
    {
        public string CompanyName { get; set; } // Nome da empresa
        public int Complaints { get; set; } // Quantidade de reclamações
    }

    // Classe para representar os dados de reclamações
    public class ComplaintData
    {
        public DateTime Date { get; set; } // Armazena a data completa
        public int Complaints { get; set; } // Quantidade de reclamações

        // Propriedade para exibir o mês e o ano formatados
        public string MonthYear => Date.ToString("MMM yyyy");
    }
}