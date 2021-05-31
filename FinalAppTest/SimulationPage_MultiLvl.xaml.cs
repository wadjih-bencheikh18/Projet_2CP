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
        public static double Speed = 2;
        public SimulationPage_MultiLvl(Ordonnancement.Ordonnancement prog,int i)
        {
            InitializeComponent();
            this.prog = prog;
            Ordonnancement.Ordonnancement.ScrollGantt = ScrollGantt;
            Ordonnancement.Ordonnancement.ScrollDeroulement = ScrollDeroulement;
            Ordonnancement.Ordonnancement.GanttChart = GanttChart;
            StackPanel[] ListesPretsViews = { ListProcessusView0, ListProcessusView1, ListProcessusView2, ListProcessusView3 };
            if (i==0)
            {
                nbNiveaux = ((MultiNiveau)prog).nbNiveau;
                ((MultiNiveau)prog).InitVisualisation(ListesPretsViews);
            }
            else
            {
                nbNiveaux = ((MultiNiveauRecyclage)prog).nbNiveau;
                ((MultiNiveauRecyclage)prog).InitVisualisation(ListesPretsViews);
            }
            
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            prog.listePrets.Clear();
            prog.listeProcessus.Clear();
            prog.listebloque.Clear();
            MainWindow.main.Content = new InitPage();
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
            prog.Executer(ListProcessusView0, Processeur, TempsView, ListeBloqueView,deroulement,GanttChart);
        }

        private void VitesseSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Speed = VitesseSlider.Value / 2;
        }

        private void VitesseSlider_Loaded(object sender, RoutedEventArgs e)
        {
            VitesseSlider.Value = 3;
        }
        private void ResultFinalBtn_Click(object sender, RoutedEventArgs e)
        {
            Ordonnancement.Ordonnancement proc = prog;
            proc.listeProcessus.Sort(delegate (Processus x, Processus y) { return x.id.CompareTo(y.id); });
            MainWindow.main.Content = new ResultFinal_Tab(proc.listeProcessus);
        }
    }
}
