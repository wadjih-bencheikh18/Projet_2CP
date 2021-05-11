using Ordonnancement;
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

namespace Simulation_Test
{
    /// <summary>
    /// Interaction logic for SimulationPage.xaml
    /// </summary>
    public partial class SimulationPage : Page
    {
        int id;
        RoundRobin prgm;
        public SimulationPage(RoundRobin prgm)
        {
            InitializeComponent();
            this.prgm = prgm;
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {

            _ = prgm.Executer(ListProcessusView, Processeur, TempsView);
        }

        private void ResultFinalBtn_Click(object sender, RoutedEventArgs e)
        {
            List<Processus> P = new List<Processus>();
            foreach (Processus Pro in prgm.listeProcessus)
            {
                P.Add(new Processus(Pro));
            }
            ResultatFinal resultatFinal = new ResultatFinal(P);
            resultatFinal.Show();
        }
    }
}
