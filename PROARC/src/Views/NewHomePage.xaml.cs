using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.Web.WebView2.Core;
using PROARC.src.Control;
using PROARC.src.Models;
using PROARC.src.Models.Arquivos;
using PROARC.src.ViewModels;

namespace PROARC.src.Views
{
    public sealed partial class NewHomePage : Page
    {
        private DashboardViewModel _viewModel;

        public NewHomePage()
        {
            this.InitializeComponent();

            // Inicializa o ViewModel
            _viewModel = new DashboardViewModel();

            // Carrega os dados do dashboard
            this.DataContext = this;
            LoadDashboardData();
            _ = CarregarProcessos();
        }




        // Lista Reclamações

        public ObservableCollection<ReclamacaoViewModel> Processos { get; set; } = new();
        private bool _isLoading;
        private int _limit = 15;
        private int _offset = 0;
        private int _paginaAtual = 1;
        private int _totalProcessos = 0;

        public int PaginaAtual
        {
            get => _paginaAtual;
            set
            {
                if (_paginaAtual != value)
                {
                    _paginaAtual = value;
                    OnPropertyChanged(nameof(PaginaAtual));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            DispatcherQueue.TryEnqueue(() =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            });
        }

        private async void Processo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Status")
            {
                if (sender is ReclamacaoViewModel processoViewModel)
                {
                    var modeloOriginal = processoViewModel.ObterModeloOriginal();


                    if (modeloOriginal.Situacao == processoViewModel.Status)
                    {
                        Debug.WriteLine(" Nenhuma alteração detectada, ignorando atualização.");
                        return;
                    }

                    modeloOriginal.Situacao = processoViewModel.Status; // Atualiza o modelo original aqui

                    if (modeloOriginal is ReclamacaoEnel reclamacaoEnel)
                    {
                        await ReclamacaoControl.UpdateAsync(processoViewModel.Titulo, reclamacaoEnel);
                    }
                    else if (modeloOriginal is ReclamacaoGeral reclamacaoGeral)
                    {
                        await ReclamacaoControl.UpdateAsync(processoViewModel.Titulo, reclamacaoGeral);
                    }
                }
            }
        }

        private async Task CarregarProcessos()
        {
            if (_isLoading) return;

            carregando.Visibility = Visibility.Visible;
            carregando.IsActive = true;
            _isLoading = true;

            try
            {
                if (_totalProcessos == 0)
                {
                    _totalProcessos = await ReclamacaoControl.CountAsync();
                }

                var processos = await ReclamacaoControl.GetNRows(_limit, _offset);
                DispatcherQueue.TryEnqueue(() => Processos.Clear());

                if (processos != null && processos.Any())
                {
                    foreach (var processo in processos)
                    {
                        if (processo != null)
                        {
                            var processoViewModel = new ReclamacaoViewModel(processo);
                            processoViewModel.PropertyChanged += Processo_PropertyChanged; // Escutar mudanças

                            DispatcherQueue.TryEnqueue(() => Processos.Add(processoViewModel));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Erro ao carregar processos: {ex.Message}");
            }
            finally
            {
                carregando.IsActive = false;
                carregando.Visibility = Visibility.Collapsed;
                _isLoading = false;
                AtualizarEstadoDosBotoes();
            }
        }

        private void AtualizarEstadoDosBotoes()
        {
            BotaoPaginaAnterior.IsEnabled = _paginaAtual > 1;
            BotaoProximaPagina.IsEnabled = _offset + _limit < _totalProcessos;
        }

        private async void ProximaPagina_Click(object sender, RoutedEventArgs e)
        {
            if (_isLoading || _offset + _limit >= _totalProcessos) return;

            _paginaAtual++;
            _offset = (_paginaAtual - 1) * _limit;
            OnPropertyChanged(nameof(PaginaAtual));
            await CarregarProcessos();
        }

        private async void PaginaAnterior_Click(object sender, RoutedEventArgs e)
        {
            if (_paginaAtual > 1)
            {
                _paginaAtual--;
                _offset = (_paginaAtual - 1) * _limit;
                OnPropertyChanged(nameof(PaginaAtual));
                await CarregarProcessos();
            }
        }

        private void Processo_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                var menuFlyout = new MenuFlyout();

                var visualizarItem = new MenuFlyoutItem { Text = "Visualizar Processo" };
                var editarItem = new MenuFlyoutItem { Text = "Editar Processo" };
                //editarItem.Click += (s, args) => EditarProcesso(processo);

                var excluirItem = new MenuFlyoutItem
                {
                    Text = "Excluir Processo",
                    Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Red)
                };

                menuFlyout.Items.Add(visualizarItem);
                menuFlyout.Items.Add(editarItem);
                menuFlyout.Items.Add(new MenuFlyoutSeparator());
                menuFlyout.Items.Add(excluirItem);

                menuFlyout.ShowAt(element, e.GetPosition(element));
            }
        }












        private async void LoadDashboardData()
        {
            // Carrega os dados de reclamações
            await _viewModel.LoadComplaintDataAsync();

            // Gera o HTML com os dados de reclamações
            string lineChartHtml = GenerateLineChartHtml(_viewModel.ComplaintData);

            // Salva o conteúdo HTML em um arquivo temporário
            string lineChartFilePath = Path.Combine(Path.GetTempPath(), "line_chart.html");
            File.WriteAllText(lineChartFilePath, lineChartHtml);

            // Navega para o arquivo HTML no WebViewChart
            await WebViewChart.EnsureCoreWebView2Async();
            WebViewChart.Source = new Uri(lineChartFilePath);

            // Carrega os dados das empresas mais reclamadas
            await _viewModel.LoadTopCompaniesAsync();

            // Gera o HTML com os dados das empresas mais reclamadas
            string barChartHtml = GenerateBarChartHtml(_viewModel.TopCompanies);

            // Salva o conteúdo HTML em um arquivo temporário
            string barChartFilePath = Path.Combine(Path.GetTempPath(), "bar_chart.html");
            File.WriteAllText(barChartFilePath, barChartHtml);

            // Navega para o arquivo HTML no WebViewChart2
            await WebViewChart2.EnsureCoreWebView2Async();
            WebViewChart2.Source = new Uri(barChartFilePath);

            // Carrega os dados dos motivos mais recorrentes
            await _viewModel.LoadTopReasonsAsync();

            // Gera o HTML com os dados dos motivos mais recorrentes
            string polarChartHtml = GeneratePolarChartHtml(_viewModel.TopReasons);

            // Salva o conteúdo HTML em um arquivo temporário
            string polarChartFilePath = Path.Combine(Path.GetTempPath(), "polar_chart.html");
            File.WriteAllText(polarChartFilePath, polarChartHtml);

            // Navega para o arquivo HTML no WebViewChart3
            await WebViewChart3.EnsureCoreWebView2Async();
            WebViewChart3.Source = new Uri(polarChartFilePath);
        }

        private string GenerateLineChartHtml(List<ComplaintData> complaintData)
        {
            // Extrai os meses e as quantidades de reclamações
            var months = complaintData.Select(d => d.MonthYear).ToList();
            var complaints = complaintData.Select(d => d.Complaints).ToList();

            // Gera o HTML e JavaScript para o gráfico de linhas
            string htmlContent = $@"
<html>
<head>
    <script src='https://cdn.jsdelivr.net/npm/chart.js'></script>
</head>
<body style='background-color: white;'>
    <canvas id='myChart'></canvas>
    <script>
        var ctx = document.getElementById('myChart').getContext('2d');

        var gradient = ctx.createLinearGradient(0, 0, 0, 400);
        gradient.addColorStop(0, 'rgba(75, 192, 192, 0.4)');
        gradient.addColorStop(1, 'rgba(75, 192, 192, 0)');

        var myChart = new Chart(ctx, {{
            type: 'line',
            data: {{
                labels: {JsonSerializer.Serialize(months)},
                datasets: [{{
                    data: {JsonSerializer.Serialize(complaints)},
                    borderColor: 'rgb(75, 192, 255)',
                    backgroundColor: gradient,
                    borderWidth: 2,
                    fill: true,
                    tension: 0.4,
                    pointRadius: 5,
                    pointBackgroundColor: 'rgb(75, 192, 255)'
                }}]
            }},
            options: {{
                responsive: true,
                maintainAspectRatio: false,
                interaction: {{
                    mode: 'nearest',
                    intersect: false
                }},
                scales: {{
                    x: {{
                        ticks: {{ color: 'black' }},
                        grid: {{ color: 'rgba(0, 0, 0, 0.1)' }} 
                    }},
                    y: {{
                        ticks: {{ color: 'black' }},
                        grid: {{ display: false }} // Remove as linhas do eixo Y
                    }}
                }},
                plugins: {{
                    legend: {{
                        display: false // Remove a legenda/título completamente
                    }}
                }}
            }}
        }});

        // Bloqueia o zoom com Ctrl + Scroll
        document.addEventListener('wheel', function(event) {{
            if (event.ctrlKey) {{
                event.preventDefault();
            }}
        }}, {{ passive: false }});

        // Bloqueia o zoom com Ctrl + Toque (pinch)
        document.addEventListener('touchstart', function(event) {{
            if (event.ctrlKey) {{
                event.preventDefault();
            }}
        }}, {{ passive: false }});
    </script>
</body>
</html>";

            return htmlContent;
        }



        private string GenerateBarChartHtml(List<CompanyComplaintData> topCompanies)
        {
            // Extrai os nomes das empresas e as quantidades de reclamações
            var companyNames = topCompanies.Select(c => c.CompanyName).ToList();
            var complaints = topCompanies.Select(c => c.Complaints).ToList();

            // Gera o HTML e JavaScript para o gráfico de barras horizontais invertidas (da direita para a esquerda)
            string htmlContent = $@"
<html>
<head>
    <script src='https://cdn.jsdelivr.net/npm/chart.js'></script>
</head>
<body style='background-color: white;'>
    <canvas id='myBarChart'></canvas>
    <script>
        var ctx = document.getElementById('myBarChart').getContext('2d');

        var myBarChart = new Chart(ctx, {{
            type: 'bar',
            data: {{
                labels: {JsonSerializer.Serialize(companyNames)},
                datasets: [{{
                    data: {JsonSerializer.Serialize(complaints)},
                    backgroundColor: 'rgba(75, 192, 192, 0.6)', // Cor azul
                    borderColor: 'rgba(75, 192, 192, 1)',
                    borderWidth: 1,
                    hoverBackgroundColor: 'rgba(75, 192, 192, 0.8)', // Cor ao passar o mouse
                    hoverBorderColor: 'rgba(75, 192, 192, 1)', // Cor da borda ao passar o mouse
                    hoverBorderWidth: 3, // Espessura da borda ao passar o mouse
                    barThickness: 40, // Aumenta a espessura das barras (em pixels)
                    categoryPercentage: 0.9,
                    barPercentage: 0.9
                }}]
            }},
            options: {{
                responsive: true,
                maintainAspectRatio: false,
                indexAxis: 'y', // Transforma o gráfico em barras horizontais
                scales: {{
                    x: {{
                        position: 'top', // Move os valores para a parte superior
                        ticks: {{
                            reverse: true // Faz com que os valores sejam lidos da direita para a esquerda
                        }},
                        grid: {{
                            color: 'rgba(0, 0, 0, 0.1)' // Grades mais suaves
                        }}
                    }},
                    y: {{
                        ticks: {{
                            display: false // Remove os nomes das empresas do eixo Y
                        }},
                        grid: {{
                            display: false // Remove as linhas de grade do eixo Y
                        }}
                    }}
                }},
                plugins: {{
                    legend: {{
                        display: false // Remove a legenda completamente
                    }}
                }},
                hover: {{
                    animationDuration: 200,
                    onHover: function(event, chartElement) {{
                        if (chartElement.length) {{
                            event.native.target.style.cursor = 'pointer';
                        }} else {{
                            event.native.target.style.cursor = 'default';
                        }}
                    }}
                }}
            }}
        }});

        // Bloqueia o zoom com Ctrl + Scroll
        document.addEventListener('wheel', function(event) {{
            if (event.ctrlKey) {{
                event.preventDefault();
            }}
        }}, {{ passive: false }});

        // Bloqueia o zoom com Ctrl + Toque (pinch)
        document.addEventListener('touchstart', function(event) {{
            if (event.ctrlKey) {{
                event.preventDefault();
            }}
        }}, {{ passive: false }});
    </script>
</body>
</html>";

            return htmlContent;
        }




        private string GeneratePolarChartHtml(List<ReasonData> topReasons)
        {
            // Extrai os motivos e as quantidades de reclamações
            var reasons = topReasons.Select(r => r.Reason).ToList();
            var counts = topReasons.Select(r => r.Count).ToList();

            // Gera o HTML e JavaScript para o gráfico de pizza polar
            string htmlContent = $@"
<html>
<head>
    <script src='https://cdn.jsdelivr.net/npm/chart.js'></script>
</head>
<body style='background-color: white;'>
    <canvas id='myPolarChart'></canvas>
    <script>
        var ctx = document.getElementById('myPolarChart').getContext('2d');

        var myPolarChart = new Chart(ctx, {{
            type: 'polarArea',
            data: {{
                labels: {JsonSerializer.Serialize(reasons)},
                datasets: [{{
                    label: 'Reclamações',
                    data: {JsonSerializer.Serialize(counts)},
                    backgroundColor: [
                        'rgba(70, 130, 180, 0.6)',  // Steel Blue
                        'rgba(30, 144, 255, 0.6)',  // Dodger Blue
                        'rgba(0, 191, 255, 0.6)',   // Deep Sky Blue
                        'rgba(65, 105, 225, 0.6)',  // Royal Blue
                        'rgba(100, 149, 237, 0.6)'  // Cornflower Blue
                    ],
                    borderColor: [
                        'rgba(70, 130, 180, 1)',
                        'rgba(30, 144, 255, 1)',
                        'rgba(0, 191, 255, 1)',
                        'rgba(65, 105, 225, 1)',
                        'rgba(100, 149, 237, 1)'
                    ],
                    borderWidth: 1
                }}]
            }},
            options: {{
                responsive: true,
                maintainAspectRatio: false,
                plugins: {{
                    legend: {{
                        display: false // Remove a legenda
                    }},
                    // Desabilita o zoom
                    zoom: {{
                        enabled: false
                    }}
                }},
                interaction: {{
                    mode: 'nearest',
                    intersect: false
                }}
            }}
        }});
        
        // Bloqueia o zoom com Ctrl + Scroll
        document.addEventListener('wheel', function(event) {{
            if (event.ctrlKey) {{
                event.preventDefault();
            }}
        }}, {{ passive: false }});

        // Bloqueia o zoom com Ctrl + Toque (pinch)
        document.addEventListener('touchstart', function(event) {{
            if (event.ctrlKey) {{
                event.preventDefault();
            }}
        }}, {{ passive: false }});
    </script>
</body>
</html>";

            return htmlContent;
        }


        private void CadastrarReclamacaoButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RegistrarProcesso01Page), true);
        }

        private void ListarReclamacaoButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ProcessosListaPage));
        }



        private void WebViewChart_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Navegação concluída com sucesso!");
        }
    }
}