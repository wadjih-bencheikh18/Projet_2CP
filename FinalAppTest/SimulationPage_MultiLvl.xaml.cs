using FinalAppTest.ViewModels;
using Ordonnancement;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace FinalAppTest
{
    /// <summary>
    /// Interaction logic for SimulationPage.xaml
    /// </summary>
    public partial class SimulationPage_MultiLvl : Page
    {
        public Ordonnancement.Ordonnancement prog;
        public static bool paused = false;
        public static bool activated = false;

        public static SimulationPage_MultiLvl save;
        public static bool stop = false;
        private int nbNiveaux = 0;
        public static double Speed = 2;
        public int previous_algo_num;
        public SimulationPage_MultiLvl(Ordonnancement.Ordonnancement prog, int i)
        {
            InitializeComponent();
            this.prog = prog;
            activated = false;
            paused = false;
            save = this;
            Ordonnancement.Ordonnancement.ScrollGantt = ScrollGantt;
            Ordonnancement.Ordonnancement.ScrollDeroulement = ScrollDeroulement;
            Ordonnancement.Ordonnancement.GanttChart = GanttChart;
            StackPanel[] ListesPretsViews = { ListProcessusView0, ListProcessusView1, ListProcessusView2, ListProcessusView3 };
            previous_algo_num = i;
            if (i == 0)
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

        private void ResultFinalBtn_Click(object sender, RoutedEventArgs e)
        {
            if (previous_algo_num == 0) MainWindow.main.Content = new ResultFinal_Tab((List<ProcessusNiveau>)((MultiNiveau)prog).listeProcessus);
            else if (previous_algo_num == 1) MainWindow.main.Content = new ResultFinal_Tab((List<ProcessusNiveau>)((MultiNiveauRecyclage)prog).listeProcessus);
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
            if (!activated) prog.Executer(ListProcessusView0, Processeur, TempsView, ListeBloqueView, deroulement, GanttChart);
            else if (paused) paused = false;
            activated = true;
        }

        private void VitesseSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Speed = VitesseSlider.Value / 2;
        }

        private void VitesseSlider_Loaded(object sender, RoutedEventArgs e)
        {
            VitesseSlider.Value = 3;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {

            MainWindow.main.Content = new InitPage();
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            paused = true;
        }

        private async void Repeat_Click(object sender, MouseButtonEventArgs e)
        {
            stop = true;

            await Task.Delay(2000);
            prog.listebloque.Clear();
            if (previous_algo_num == 0)
            {
                for (int i = 0; i < ((MultiNiveau)prog).nbNiveau; i++)
                {
                    ((MultiNiveau)prog).niveaux[i].listePrets.Clear();
                    ((MultiNiveau)prog).niveaux[i].listeProcessus.Clear();
                    ((MultiNiveau)prog).niveaux[i].listebloque.Clear();
                }
            }
            else if (previous_algo_num == 1)
            {
                for (int i = 0; i < ((MultiNiveauRecyclage)prog).nbNiveau; i++)
                {
                    ((MultiNiveauRecyclage)prog).niveaux[i].listePrets.Clear();
                    ((MultiNiveauRecyclage)prog).niveaux[i].listeProcessus.Clear();
                    ((MultiNiveauRecyclage)prog).niveaux[i].listebloque.Clear();
                }
            }
            
            

            if (previous_algo_num == 0)
            {
                for (int i = 0; i < ((MultiNiveau)prog).listeProcessus.Count(); i++)
                {
                    ((MultiNiveau)prog).listeProcessus[i].tempsRestant = ((MultiNiveau)prog).listeProcessus[i].duree;
                    ((MultiNiveau)prog).listeProcessus[i].etat = 3;
                    ((MultiNiveau)prog).listeProcessus[i].indiceInterruptions[1] = 0;
                    ((MultiNiveau)prog).listeProcessus[i].indiceInterruptions[0] = ((MultiNiveau)prog).listeProcessus[i].indiceInterruptions[1];
                    for (int j = 0; j < ((MultiNiveau)prog).listeProcessus[i].listeInterruptions.Count(); j++)
                    {
                        ((MultiNiveau)prog).listeProcessus[i].listeInterruptions[j].tempsRestant = ((MultiNiveau)prog).listeProcessus[i].listeInterruptions[j].duree;
                    }
                }
            }
            else if (previous_algo_num == 1)
            {
                for (int i = 0; i < ((MultiNiveauRecyclage)prog).listeProcessus.Count(); i++)
                {
                    ((MultiNiveauRecyclage)prog).listeProcessus[i].tempsRestant = ((MultiNiveauRecyclage)prog).listeProcessus[i].duree;
                    ((MultiNiveauRecyclage)prog).listeProcessus[i].indiceInterruptions[1] = 0;
                    ((MultiNiveauRecyclage)prog).listeProcessus[i].indiceInterruptions[0] = ((MultiNiveauRecyclage)prog).listeProcessus[i].indiceInterruptions[1];
                    ((MultiNiveauRecyclage)prog).listeProcessus[i].etat = 3;

                    for (int j = 0; j < ((MultiNiveauRecyclage)prog).listeProcessus[i].listeInterruptions.Count(); j++)
                    {
                        ((MultiNiveauRecyclage)prog).listeProcessus[i].listeInterruptions[j].tempsRestant = ((MultiNiveauRecyclage)prog).listeProcessus[i].listeInterruptions[j].duree;
                    }
                }
            }
            ListProcessusView0.Children.Clear();
            ListProcessusView1.Children.Clear();
            ListProcessusView2.Children.Clear();
            ListProcessusView3.Children.Clear();
            ListeBloqueView.Children.Clear();
            Processeur.Children.Clear();
            deroulement.Children.Clear();
            GanttChart.Children.Clear();
            TempsView.Text = "0";
            stop = false;

            prog.Executer(ListProcessusView0, Processeur, TempsView, ListeBloqueView, deroulement, GanttChart);
        }

        private void Home_Click(object sender, MouseButtonEventArgs e)
        {

            prog.listeProcessus.Clear();
            prog.listebloque.Clear();
            if (previous_algo_num == 0)
            {
                ((MultiNiveau)prog).nbNiveau = 0;
                ((MultiNiveau)prog).niveaux = null;
            }
            else if (previous_algo_num == 1)
            {
                ((MultiNiveauRecyclage)prog).nbNiveau = 0;
                ((MultiNiveauRecyclage)prog).niveaux = null;
            }
            MainWindow.main.Content = new WelcomePage();
        }

        private void Return_Click(object sender, MouseButtonEventArgs e)
        {
            prog.listeProcessus.Clear();
            prog.listebloque.Clear();

            if (previous_algo_num == 0)
            {
                ((MultiNiveau)prog).nbNiveau = 0;
                ((MultiNiveau)prog).niveaux = null;
                MainWindow.main.Content = new InitPage { DataContext = new MultiNiveauViewModel() };
            }
            else if (previous_algo_num == 1)
            {
                ((MultiNiveauRecyclage)prog).nbNiveau = 0;
                ((MultiNiveauRecyclage)prog).niveaux = null;
                MainWindow.main.Content = new InitPage { DataContext = new MultiNiveauRecyclageViewModel() };
            }
        }
    }
}
