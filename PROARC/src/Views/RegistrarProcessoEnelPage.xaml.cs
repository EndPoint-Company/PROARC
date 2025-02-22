using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage.Pickers;
using Windows.ApplicationModel.DataTransfer;
using PROARC.src.Models;
using PROARC.src.Control;
using PROARC.src.Converters;
using System.ComponentModel;
using Microsoft.UI.Xaml.Media;
using System.Drawing;
using System.Numerics;
using Windows.Storage;
using Microsoft.UI.Text;
using Microsoft.UI;
using PROARC.src.Models.Tipos;
using PROARC.src.Models.Arquivos;
using static PROARC.src.Control.MotivoControl;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Navigation;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Windows.UI.Popups;
using Microsoft.UI.Xaml.Controls.Primitives;
using Windows.Globalization;
using System.Text.RegularExpressions;
using System.Globalization;
using PROARC.src.Strategies;
using PROARC.src.Models.Arquivos.FabricaReclamacao;

namespace PROARC.src.Views
{
    public sealed partial class RegistrarProcessoEnelPage : Page, INotifyPropertyChanged
    {
        private string numeroProcesso;
        private string anoProcesso;
        private List<string> arquivosSelecionados = new();
        private static readonly IFabricaReclamacao fabricaReclamacao = new FabricaReclamacao();

        public string NumeroProcesso
        {
            get => numeroProcesso;
            set
            {
                if (int.TryParse(value, out int numero))
                {
                    value = numero.ToString("D3"); // Garante três dígitos
                }
                SetProperty(ref numeroProcesso, value);
            }
        }

        public string AnoProcesso
        {
            get => anoProcesso;
            set => SetProperty(ref anoProcesso, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public RegistrarProcessoEnelPage()
        {
            InitializeComponent();
            DataContext = this;
            CarregarMotivosAsync();
            ConfigureShadows();
        }

        protected void OnPropertyChanged(string propertyName)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
            }
        }
        private async Task CarregarMotivosAsync() 
         => cbMotivo.ItemsSource = await MotivoControl.GetAllAsync();

        private void ProcessoNovo_Click(object sender, RoutedEventArgs e)
        => ConfigurarEstadoProcesso(true);

        private void ProcessoAntigo_Click(object sender, RoutedEventArgs e)
            => ConfigurarEstadoProcesso(false);


        private void ConfigurarEstadoProcesso(bool isNovoProcesso)
        {
            radio_agResposta.IsChecked = isNovoProcesso;
            AtualizarEstilosBotoes(isNovoProcesso);
            AjustarCamposProcesso(isNovoProcesso);
        }

        private void AtualizarEstilosBotoes(bool isNovoProcesso)
        {
            btnProcessoNovo.Background = new SolidColorBrush(isNovoProcesso ? Microsoft.UI.Colors.DarkBlue : Microsoft.UI.Colors.White);
            btnProcessoNovo.Foreground = new SolidColorBrush(isNovoProcesso ? Microsoft.UI.Colors.White : Microsoft.UI.Colors.DarkBlue);

            btnProcessoAntigo.Background = new SolidColorBrush(isNovoProcesso ? Microsoft.UI.Colors.White : Microsoft.UI.Colors.DarkBlue);
            btnProcessoAntigo.Foreground = new SolidColorBrush(isNovoProcesso ? Microsoft.UI.Colors.DarkBlue : Microsoft.UI.Colors.White);
            btnProcessoAntigo.BorderBrush = new SolidColorBrush(Microsoft.UI.Colors.CornflowerBlue);
        }

        private async void AjustarCamposProcesso(bool isNovoProcesso)
        {
            inputNProcesso.IsReadOnly = isNovoProcesso;
            inputAnoProcesso.IsReadOnly = isNovoProcesso;
            MainStackPanel.Opacity = isNovoProcesso ? 0.4 : 1;

            if (isNovoProcesso)
            {
                await DefinirNovoProcesso();

            }
            else
            {
                NumeroProcesso = string.Empty;
                AnoProcesso = string.Empty;
            }
        }

        private async Task DefinirNovoProcesso()
        {
            int count = await ReclamacaoControl.CountEAsync();
            NumeroProcesso = (count + 1).ToString();
            AnoProcesso = DateTime.Now.Year.ToString();
        }

        private void ProcuradorCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ProcuradorSection1.Visibility = Visibility.Visible;
        }

        private void ProcuradorCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ProcuradorSection1.Visibility = Visibility.Collapsed;
        }

        // POP-UPS
        private async Task<ContentDialogResult> MostrarContentDialogAsync(string title, object content, string primaryButtonText = null, string closeButtonText = "Ok")
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = content,
                PrimaryButtonText = primaryButtonText,
                CloseButtonText = closeButtonText,
                XamlRoot = this.Content.XamlRoot
            };

            return await dialog.ShowAsync();
        }

        private async Task ShowError(string mensagemErro)
        {
            await MostrarContentDialogAsync("Erro", mensagemErro);
        }

        private async Task ShowSuccess(string mensagemSucesso)
        {
            await MostrarContentDialogAsync("Sucesso", mensagemSucesso);
        }


        private async void OnNovoMotivoClick(object sender, RoutedEventArgs e)
        {
            var textBox = new TextBox
            {
                PlaceholderText = "Digite o motivo"
            };

            var result = await MostrarContentDialogAsync(
                "Adicionar Novo Motivo",
                textBox,
                "Salvar",
                "Cancelar");

            if (result == ContentDialogResult.Primary)
            {
                var motivoTexto = textBox.Text;

                if (!string.IsNullOrWhiteSpace(motivoTexto))
                {
                    var motivosExistentes = await MotivoControl.GetAllAsync();

                    if (motivosExistentes.Any(m => m.Nome.Equals(motivoTexto, StringComparison.OrdinalIgnoreCase)))
                    {
                        await ShowError("Este motivo já existe.");
                    }
                    else
                    {
                        var motivo = new Motivo(motivoTexto, null);

                        try
                        {
                            await MotivoControl.InsertAsync(motivo);
                            await CarregarMotivosAsync();

                            await ShowSuccess("Motivo salvo com sucesso!");
                        }
                        catch (Exception ex)
                        {
                            await ShowError($"Falha ao salvar motivo: {ex.Message}");
                        }
                    }
                }
                else
                {
                    await ShowError("O motivo não pode estar vazio.");
                }
            }
        }

        private async void ContinuarButton_Click(object sender, RoutedEventArgs e)
        {
            var resetProcessor = new FieldProcessor(new ResetFieldStrategy());
            resetProcessor.ProcessMultipleFields(GetFieldsToProcess());

            NumeroProcesso = inputNProcesso.Text.Trim();
            AnoProcesso = inputAnoProcesso.Text.Trim();

            bool isNovoProcesso = radio_agResposta.IsChecked == true;

            if (isNovoProcesso)
            {
                if (string.IsNullOrEmpty(NumeroProcesso))
                {
                    int count = await ReclamacaoControl.CountAsync();
                    NumeroProcesso = (count + 1).ToString();
                }

                if (string.IsNullOrEmpty(AnoProcesso))
                {
                    AnoProcesso = DateTime.Now.Year.ToString();
                }
            }

            if (!CamposPreenchidos())
            {
                await ShowError("Preencha todos os campos obrigatórios antes de continuar.");

                var highlightProcessor = new FieldProcessor(new HighlightFieldStrategy());
                highlightProcessor.ProcessMultipleFields(GetFieldsToProcess());
                return;
            }

            if (!ValidarCampos()) return;

            Motivo? motivoSelecionado = cbMotivo.SelectedItem != null ? new Motivo(cbMotivo.SelectedItem.ToString()) : null;

            string cpfLimpoReclamante = new string(inputCpfReclamante.Text.Where(char.IsDigit).ToArray());

            var reclamante = new Reclamante(
                inputNomeReclamante.Text,
                cpfLimpoReclamante,
                string.IsNullOrWhiteSpace(inputRgReclamante.Text) ? null : inputRgReclamante.Text,
                string.IsNullOrWhiteSpace(inputNumeroReclamante.Text) ? null : inputNumeroReclamante.Text,
                string.IsNullOrWhiteSpace(inputEmailReclamante.Text) ? null : inputEmailReclamante.Text
            );

            Procurador procurador = null;

            if (ProcuradorCheckBox.IsChecked == true)
            {
                string cpfLimpoProcurador = new string(inputCpfProcurador.Text.Where(char.IsDigit).ToArray());
                procurador = new Procurador(
                    inputNomeProcurador.Text,
                    cpfLimpoProcurador,
                    string.IsNullOrWhiteSpace(inputRgProcurador.Text) ? null : inputRgProcurador.Text,
                    string.IsNullOrWhiteSpace(inputNumeroProcurador.Text) ? null : inputNumeroProcurador.Text,
                    string.IsNullOrWhiteSpace(inputEmailProcurador.Text) ? null : inputEmailProcurador.Text
                );
            }

            string caminhoPasta = $"dir/folder{NumeroProcesso}";
            string nProcesso = NumeroProcesso;
            short anoProcesso = short.TryParse(AnoProcesso, out short parsedAno) ? parsedAno : (short)DateTime.Now.Year;
            string titulo = "E" + nProcesso + "/" + anoProcesso;

            string atendente = inputNomeAtendente.Text;
            string situacao = GetSelectedRadioButton();
            string observacao = inputObservacao.Text;

            Reclamado? enel = await ReclamadoControl.GetAsync(74);

            if (enel != null)
            {
                LinkedList<Reclamado> reclamados = new LinkedList<Reclamado>();
                reclamados.AddLast(enel);

                var reclamacaoEnel = fabricaReclamacao.CriarReclamacao(
                    EnumReclamacao.ReclamacaoEnel,
                    motivoSelecionado,
                    reclamante,
                    procurador,
                    reclamados,
                    titulo,
                    situacao,
                    caminhoPasta,
                    DateOnly.FromDateTime(DateTime.Now),
                    "Sistema",
                    atendente,
                    enel.Telefone,
                    enel.Email,
                    observacao
                );

                ButtonContinuar.IsEnabled = false;
                bool success = await ReclamacaoControl.InsertAsync((ReclamacaoEnel)reclamacaoEnel);
                ButtonContinuar.IsEnabled = true;

                if (success)
                {
                    await ShowSuccess("O processo foi cadastrado com sucesso!");
                    Frame.Navigate(typeof(RegistrarProcessoEnelPage), true);
                }
                else
                {
                    await ShowError("Falha ao cadastrar o processo. Tente novamente.");
                }
            }
        }

        private string FormatWithMask(string text, int[] positions, char[] separators)
        {
            if (text.Length == 0) return text;

            string formattedText = text;

            for (int i = 0; i < positions.Length; i++)
            {
                if (formattedText.Length > positions[i])
                    formattedText = formattedText.Insert(positions[i], separators[i].ToString());
            }

            return formattedText;
        }

        private bool ValidarCampos()
        {
            var erros = new List<string>();

            var validacoes = new (IValidacaoStrategy estrategia, string valor, string mensagemErro)[]
            {
                (new ValidacaoCPF(), inputCpfReclamante.Text, "CPF do Reclamante inválido."),
                (new ValidacaoEmail(), inputEmailReclamante.Text, "E-mail inválido."),
                (new ValidacaoTelefone(), inputNumeroReclamante.Text, "Telefone inválido.")
            };

            foreach (var (estrategia, valor, mensagemErro) in validacoes)
            {
                if (!string.IsNullOrWhiteSpace(valor))
                {
                    var validador = new Validador(estrategia);
                    if (!validador.Validar(valor))
                    {
                        erros.Add(mensagemErro);
                    }
                }
            }

            if (erros.Count > 0)
            {
                ShowError(string.Join("\n", erros));
                return false;
            }

            return true;
        }

        private void OnCpfTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is not TextBox textBox) return;
            string rawText = Regex.Replace(textBox.Text, @"\D", "");
            textBox.Text = FormatWithMask(rawText, new[] { 3, 7, 11 }, new[] { '.', '.', '-' });
            textBox.SelectionStart = textBox.Text.Length;
        }

        private IEnumerable<(FrameworkElement Field, TextBlock Label, TextBlock? Title)> GetFieldsToProcess()
        {
            yield return (inputNProcesso, TextBlockNProcesso, null);
            yield return (inputAnoProcesso, TextBlockAnoProcesso, null);
            yield return (inputNomeReclamante, TextBlockNomeReclamante, TextBlockReclamante);
            yield return (inputCpfReclamante, TextBlockCpfReclamante, TextBlockReclamante);

            yield return (cbMotivo, TextBlockMotivo, null);

            if (GetSelectedRadioButton() == "Nenhum status selecionado")
            {
                yield return (radio_agResposta, TextBlockTramitacao, TextBlockStatus);
                yield return (radio_agAguardandoPrazo, TextBlockTramitacao, TextBlockStatus);
                yield return (radio_atendido, TextBlockArquivado, TextBlockStatus);
                yield return (radio_naoAtendido, TextBlockArquivado, TextBlockStatus);
            }
        }

        private bool CamposPreenchidos() => new[] { inputNomeReclamante, inputCpfReclamante, inputNProcesso, inputAnoProcesso }
         .All(campo => !string.IsNullOrWhiteSpace(campo.Text))
         && cbMotivo.SelectedItem != null
         && GetSelectedRadioButton() != "Nenhum status selecionado";

        private string GetSelectedRadioButton()
        {
            if (radio_agAguardandoPrazo.IsChecked == true)
                return "Aguardando prazo";
            if (radio_agResposta.IsChecked == true)
                return "Aguardando resposta da empresa";
            if (radio_atendido.IsChecked == true)
                return "Atendido";
            if (radio_naoAtendido.IsChecked == true)
                return "Não Atendido";

            return "Nenhum status selecionado";
        }

        private async void PickFileButton_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };

            picker.FileTypeFilter.Add("*");

            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

            var files = await picker.PickMultipleFilesAsync();

            if (files != null && files.Any())
            {
                AdicionarArquivos(files.Select(file => file.Path));
            }
        }

        private void AdicionarArquivos(IEnumerable<string> arquivos)
        {
            foreach (var arquivo in arquivos)
            {
                var nomeArquivo = System.IO.Path.GetFileName(arquivo);

                if (!ListaArquivos.Children.OfType<TextBlock>().Any(tb => tb.Text == nomeArquivo))
                {
                    ListaArquivos.Children.Add(new TextBlock
                    {
                        Text = nomeArquivo,
                        FontSize = 14,
                        Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Green)
                    });
                }
            }

            AtualizarMensagemNenhumArquivo();
        }

        private void AtualizarMensagemNenhumArquivo()
        {
            MensagemNenhumArquivo.Visibility = ListaArquivos.Children.OfType<TextBlock>().Any()
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        private async void DragDropArea_Drop(object sender, Microsoft.UI.Xaml.DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var items = await e.DataView.GetStorageItemsAsync();
                var arquivos = items.OfType<StorageFile>().Select(file => file.Path);

                if (arquivos.Any())
                {
                    AdicionarArquivos(arquivos);
                }
            }
        }

        private void DragDropArea_DragOver(object sender, Microsoft.UI.Xaml.DragEventArgs e)
        {
            e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Copy;
        }

        private void ConfigureShadows()
        {
            MotivoSection.Translation = new Vector3(1, 1, 20);
            NProcessoSection.Translation = new Vector3(1, 1, 20);
            StatusSection.Translation = new Vector3(1, 1, 20);
            ReclamanteSection.Translation = new Vector3(1, 1, 20);
            ProcuradorSection2.Translation = new Vector3(1, 1, 20);
            AnexarArquivosSection.Translation = new Vector3(1, 1, 20);
            AtendenteSection.Translation = new Vector3(1, 1, 20);
            ObservacaoSection.Translation = new Vector3(1, 1, 20);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is bool isNovoProcesso)
            {
                ConfigurarEstadoProcesso(isNovoProcesso);
            }
        }
    }
}