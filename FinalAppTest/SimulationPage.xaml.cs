using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ordonnancement;
namespace FinalAppTest
{
    /// <summary>
    /// Interaction logic for SimulationPage.xaml
    /// </summary>
    public partial class SimulationPage : Page
    {
        public Ordonnancement.Ordonnancement prog;

        public SimulationPage(Ordonnancement.Ordonnancement prog)
        {
            InitializeComponent();
            this.prog = prog;
        }
        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            prog.Executer(ListePretsView, Processeur, TempsView,ListeBloqueView,deroulement);
        }

        private void ResultFinalBtn_Click(object sender, RoutedEventArgs e)
        {
            ResultatFinal resultatFinal = new ResultatFinal(prog.listeProcessus);
            resultatFinal.Show();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.main.Content = new InitPage();
        }
    }
}
