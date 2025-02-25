using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PROARC.src.Control.Utils;
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
            try
            {
                // Chama o método GetReclamadosPorMesAsync para obter os dados do banco de dados
                var reclamacoesPorMes = await EstatisticasControl.GetReclamadosPorMesAsync();

                // Log dos dados recebidos
                Debug.WriteLine("Dados recebidos do GetReclamadosPorMesAsync: " + string.Join(", ", reclamacoesPorMes));

                // Cria uma lista para armazenar os dados formatados
                ComplaintData = new List<ComplaintData>();

                // Obtém o ano atual
                int anoAtual = DateTime.Now.Year;

                // Processa os dados retornados
                foreach (var item in reclamacoesPorMes)
                {
                    // Divide a string no formato "Mês X: Y"
                    var parts = item.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2 && int.TryParse(parts[0].Replace("Mês", "").Trim(), out int mes) && int.TryParse(parts[1].Trim(), out int quantidade))
                    {
                        // Cria uma data para o mês e ano atual
                        var data = new DateTime(anoAtual, mes, 1);

                        // Adiciona os dados à lista
                        ComplaintData.Add(new ComplaintData
                        {
                            Date = data, // Armazena a data completa
                            Complaints = quantidade // Quantidade de reclamações
                        });
                    }
                    else
                    {
                        // Log de entradas mal formatadas
                        Debug.WriteLine("Entrada mal formatada: " + item);
                    }
                }

                // Garante que os dados estejam ordenados por mês
                ComplaintData = ComplaintData.OrderBy(d => d.Date).ToList();

                // Log da lista final de reclamações
                Debug.WriteLine("Lista final de reclamações: " + string.Join(", ", ComplaintData.Select(d => $"{d.Date:MM/yyyy}: {d.Complaints}")));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao carregar dados de reclamações: {ex.Message}");
                // Define uma lista vazia em caso de erro
                ComplaintData = new List<ComplaintData>();
            }
        }

        // Método para carregar as 5 empresas mais reclamadas
        public async Task LoadTopCompaniesAsync()
        {
            try
            {
                // Chama o método GetAsync(5) do controlador EstatisticasControl para obter as 5 empresas mais reclamadas
                var empresasReclamadas = await EstatisticasControl.GetAsync(5);

                // Log dos dados recebidos
                Debug.WriteLine("Dados recebidos do GetAsync: " + string.Join(", ", empresasReclamadas));

                // Processa os dados retornados e preenche a lista TopCompanies
                TopCompanies = empresasReclamadas.Select(empresa =>
                {
                    // Log da string sendo processada
                    Debug.WriteLine("Processando: " + empresa);

                    // Divide a string no formato "Empresa: Quantidade"
                    var parts = empresa.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2 && int.TryParse(parts[1].Trim(), out int quantidadeReclamacoes))
                    {
                        return new CompanyComplaintData
                        {
                            CompanyName = parts[0].Trim(), // Nome da empresa
                            Complaints = quantidadeReclamacoes // Quantidade de reclamações
                        };
                    }
                    else
                    {
                        // Log de entradas mal formatadas
                        Debug.WriteLine("Entrada mal formatada: " + empresa);
                        return null; // Ignora entradas mal formatadas
                    }
                }).Where(c => c != null).ToList(); // Filtra valores nulos e converte para lista

                // Log da lista final de empresas
                Debug.WriteLine("Lista final de empresas: " + string.Join(", ", TopCompanies.Select(c => c.CompanyName)));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao carregar empresas mais reclamadas: {ex.Message}");
                // Define uma lista vazia em caso de erro
                TopCompanies = new List<CompanyComplaintData>();
            }
        }

        // Método para carregar os 5 motivos mais recorrentes
        public async Task LoadTopReasonsAsync()
        {
            // Chama o método GetMotivosNumAsync() do controlador EstatisticasControl
            var motivos = await EstatisticasControl.GetMotivosNumAsync();

            // Processa os dados retornados e preenche a lista TopReasons
            TopReasons = motivos.Select(motivo =>
            {
                var parts = motivo.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2 && int.TryParse(parts[1].Trim(), out int count))
                {
                    return new ReasonData { Reason = parts[0].Trim(), Count = count };
                }
                return null;
            }).Where(r => r != null).Take(5).ToList();
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