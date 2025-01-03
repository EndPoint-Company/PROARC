using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PROARC.src.Models;
using WinRT;
using PROARC.src.Models.Tipos;
using PROARC.src.Models.Arquivos;
using PROARC.src.Control;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PROARC.src.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegistrarProcesso03Page : Page
    {
        private Reclamante reclamante;
        private Reclamado reclamado;
        private Dictionary<string, object> dicionarioObjetos = new();

        public RegistrarProcesso03Page()
        {
            InitializeComponent();
            CarregarMotivos();
            MotivoSection.Translation = new System.Numerics.Vector3(1, 1, 20);
            AudienciaSection.Translation = new System.Numerics.Vector3(1, 1, 20);
            StatusSection.Translation = new System.Numerics.Vector3(1, 1, 20);
        }

        private void CarregarMotivos()
        {
            // Simula busca no banco de dados
            List<string> motivos = new List<string> { "JUROS ABUSIVOS", "COBRAN�A INDEVIDA", "E AFINS" };
            cbMotivo.ItemsSource = motivos;
        }

        private void ContinuarButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime dataAudiencia = calendario.Date.Value.DateTime;
            Motivo motivo = new(cbMotivo.SelectedItem.ToString());
            ProcessoAdministrativo processo = new(@"D:/ProarcFiles/Teste1", "0003o2024", short.Parse(DateTime.Now.Year.ToString()), motivo, reclamado, reclamante, dataAudiencia);
            ProcessoAdministrativoControl.RegistrarProcessoAdministrativo(processo);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            dicionarioObjetos = (Dictionary<string, object>)e.Parameter;
            reclamante = (Reclamante)dicionarioObjetos["Reclamante"];
            reclamado = (Reclamado)dicionarioObjetos["Reclamado"];
        }
    }
}
