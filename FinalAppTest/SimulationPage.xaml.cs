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
using FinalAppTest.ViewModels;
using FinalAppTest.Views;

namespace FinalAppTest
{
    /// <summary>
    /// Interaction logic for SimulationPage.xaml
    /// </summary>
    public partial class SimulationPage : Page
    {
        public int previous_algo_num;
        public Ordonnancement.Ordonnancement prog;
        public static SimulationPage save;
        public static bool activated = false;
        public static bool paused = false;
        public static bool stop = false;
        public static double Speed = 2;

        public SimulationPage(Ordonnancement.Ordonnancement prog)
        {
            InitializeComponent();
            Ordonnancement.Ordonnancement.ScrollGantt = ScrollGantt;
            Ordonnancement.Ordonnancement.ScrollDeroulement = ScrollDeroulement;
            Ordonnancement.Ordonnancement.GanttChart = GanttChart;
            this.prog = prog;
            save = this;
            activated = false;
            paused = false;
        }

        public SimulationPage(Ordonnancement.Ordonnancement prog, int i)
        {
            InitializeComponent();
            Ordonnancement.Ordonnancement.ScrollGantt = ScrollGantt;
            Ordonnancement.Ordonnancement.ScrollDeroulement = ScrollDeroulement;
            Ordonnancement.Ordonnancement.GanttChart = GanttChart;
            this.prog = prog;
            save = this;
            activated = false;
            paused = false;
            this.previous_algo_num = i;
            if (previous_algo_num == 0) SimAlgo.Text = "Simulation PAPS";
            else if (previous_algo_num == 1) SimAlgo.Text = "Simulation Round Robin";
            else if (previous_algo_num == 2) SimAlgo.Text = "Simulation PSR";
            else if (previous_algo_num == 3) SimAlgo.Text = "Simulation PAR";
            else if (previous_algo_num == 4) SimAlgo.Text = "Simulation PARD";
            else if (previous_algo_num == 5) SimAlgo.Text = "Simulation PCA";
            else if (previous_algo_num == 6) SimAlgo.Text = "Simulation PCTR";
            else if (previous_algo_num == 7) SimAlgo.Text = "Simulation PLA";
            else if (previous_algo_num == 8) SimAlgo.Text = "Simulation Slack Time";
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!activated) prog.Executer(ListePretsView, Processeur, TempsView, ListeBloqueView, deroulement, GanttChart);
            else if (paused) paused = false;
            activated = true;
        }

        private void ResultFinalBtn_Click(object sender, RoutedEventArgs e)
        {
            Ordonnancement.Ordonnancement proc = prog;
            proc.listeProcessus.Sort(delegate (Processus x, Processus y) { return x.id.CompareTo(y.id); });
            MainWindow.main.Content = new ResultFinal_Tab(proc.listeProcessus);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            prog.listePrets.Clear();
            prog.listeProcessus.Clear();
            prog.listebloque.Clear();
            MainWindow.main.Content = new InitPage();
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            paused = true;
        }

        private void VitesseSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
           Speed = VitesseSlider.Value / 2;
        }

        private void VitesseSlider_Loaded(object sender, RoutedEventArgs e)
        {
            VitesseSlider.Value = 3;
        }

        private async void Repeat_Click(object sender, MouseButtonEventArgs e)
        {
            stop = true;
            prog.listebloque.Clear();
            prog.listePrets.Clear();
            for (int i = 0; i < prog.listeProcessus.Count(); i++)
            {
                prog.listeProcessus[i].tempsRestant = prog.listeProcessus[i].duree;
                prog.listeProcessus[i].etat = 3;
                prog.listeProcessus[i].indiceInterruptions[1] = 0;
                prog.listeProcessus[i].indiceInterruptions[0] = prog.listeProcessus[i].indiceInterruptions[1];
                for (int j = 0; j < prog.listeProcessus[i].listeInterruptions.Count(); j++)
                {
                    prog.listeProcessus[i].listeInterruptions[j].tempsRestant = prog.listeProcessus[i].listeInterruptions[j].duree;
                }
            }
            ListePretsView.Children.Clear();
            ListeBloqueView.Children.Clear();
            Processeur.Children.Clear();
            deroulement.Children.Clear();
            GanttChart.Children.Clear();
            TempsView.Text = "0";
            await Task.Delay(2000);
            stop = false;

            _ = prog.Executer(ListePretsView, Processeur, TempsView, ListeBloqueView, deroulement, GanttChart);
        }

        private void Home_Click(object sender, MouseButtonEventArgs e)
        {
            prog.listePrets.Clear();
            prog.listeProcessus.Clear();
            prog.listebloque.Clear();
            MainWindow.main.Content = new WelcomePage();
        }

        private void Return_Click(object sender, MouseButtonEventArgs e)
        {
            prog.listePrets.Clear();
            prog.listeProcessus.Clear();
            prog.listebloque.Clear();
            if (previous_algo_num == 0) 
            { 
                MainWindow.main.Content = new InitPage { DataContext = new PAPS_ViewModel() };
                ((InitPage)MainWindow.main.Content).paps.Fill = (Brush)new BrushConverter().ConvertFrom("#F16022");
            }
            else if (previous_algo_num == 1)
            {
                MainWindow.main.Content = new InitPage { DataContext = new RR_ViewModel() };
            }
            else if (previous_algo_num == 2) {
                MainWindow.main.Content = new InitPage { DataContext = new PSR_ViewModel() };
                ((InitPage)MainWindow.main.Content).psr.Fill = (Brush)new BrushConverter().ConvertFrom("#F16022");
            }
            else if (previous_algo_num == 3) {
                MainWindow.main.Content = new InitPage { DataContext = new PAR_ViewModel() };
                ((InitPage)MainWindow.main.Content).par.Fill = (Brush)new BrushConverter().ConvertFrom("#F16022");
            }
            else if (previous_algo_num == 4) {
                MainWindow.main.Content = new InitPage { DataContext = new PARD_ViewModel() };
                ((InitPage)MainWindow.main.Content).pard.Fill = (Brush)new BrushConverter().ConvertFrom("#F16022");
            }
            else if (previous_algo_num == 5) {
                MainWindow.main.Content = new InitPage { DataContext = new PCA_ViewModel() };
                ((InitPage)MainWindow.main.Content).pca.Fill = (Brush)new BrushConverter().ConvertFrom("#F16022");
            }
            else if (previous_algo_num == 6) {
                MainWindow.main.Content = new InitPage { DataContext = new PCTR_ViewModel() };
                ((InitPage)MainWindow.main.Content).pctr.Fill = (Brush)new BrushConverter().ConvertFrom("#F16022");
            }
            else if (previous_algo_num == 7) {
                MainWindow.main.Content = new InitPage { DataContext = new PLA_ViewModel() };
                ((InitPage)MainWindow.main.Content).pla.Fill = (Brush)new BrushConverter().ConvertFrom("#F16022");
            }
            else if (previous_algo_num == 8) {
                MainWindow.main.Content = new InitPage { DataContext = new SlackTime_ViewModel() };
                ((InitPage)MainWindow.main.Content).slacktime.Fill = (Brush)new BrushConverter().ConvertFrom("#F16022");
            }
        }
    }
}
