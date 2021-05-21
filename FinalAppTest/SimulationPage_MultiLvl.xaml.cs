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
    public partial class SimulationPage_MultiLvl : Page
    {
        public Ordonnancement.Ordonnancement prog;
        private int nbNiveaux;
        public SimulationPage_MultiLvl(Ordonnancement.Ordonnancement prog)
        {
            InitializeComponent();
            this.prog = prog;
            nbNiveaux = ((MultiNiveau)prog).nbNiveau;
            StackPanel[] ListesPretsViews = { ListProcessusView0,ListProcessusView1,ListProcessusView2,ListProcessusView3};
            ((MultiNiveau)prog).InitVisualisation(ListesPretsViews);
        }

        private void ResultFinalBtn_Click(object sender, RoutedEventArgs e)
        {
            List<ProcessusNiveau> P = new List<ProcessusNiveau>();
            foreach (ProcessusNiveau Pro in prog.listeProcessus)
            {
                //P.Add(new ProcessusNiveau(Pro));
            }
            //ResultatFinal resultatFinal = new ResultatFinal(P);
            //resultatFinal.Show();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (nbNiveaux == 1)
            {
                Border1.Visibility = Visibility.Visible;
            }
            else if (nbNiveaux == 2)
            {
                Border1.Visibility = Visibility.Visible;
                Border2.Visibility = Visibility.Visible;
            }
            else if (nbNiveaux == 3)
            {
                Border1.Visibility = Visibility.Visible;
                Border2.Visibility = Visibility.Visible;
                Border3.Visibility = Visibility.Visible;
            }
            else if (nbNiveaux == 4)
            {
                Border1.Visibility = Visibility.Visible;
                Border2.Visibility = Visibility.Visible;
                Border3.Visibility = Visibility.Visible;
                Border4.Visibility = Visibility.Visible;
            }
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            _ = prog.Executer(ListProcessusView0, Processeur, TempsView, ListeBloqueView,deroulement,GanttChart);
        }
    }
}
