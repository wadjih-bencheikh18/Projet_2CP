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
            
            prog.Executer(ListePretsView, Processeur, TempsView, ListeBloqueView, deroulement, GanttChart);
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
            if (previous_algo_num == 0) MainWindow.main.Content = new InitPage { DataContext = new PAPS_ViewModel() };
            else if (previous_algo_num == 1) MainWindow.main.Content = new InitPage { DataContext = new RR_ViewModel() };
            else if (previous_algo_num == 2) MainWindow.main.Content = new InitPage { DataContext = new PSR_ViewModel() };
            else if (previous_algo_num == 3) MainWindow.main.Content = new InitPage { DataContext = new PAR_ViewModel() };
            else if (previous_algo_num == 4) MainWindow.main.Content = new InitPage { DataContext = new PARD_ViewModel() };
            else if (previous_algo_num == 5) MainWindow.main.Content = new InitPage { DataContext = new PCA_ViewModel() };
            else if (previous_algo_num == 6) MainWindow.main.Content = new InitPage { DataContext = new PCTR_ViewModel() };
            else if (previous_algo_num == 7) MainWindow.main.Content = new InitPage { DataContext = new PLA_ViewModel() };
            else if (previous_algo_num == 8) MainWindow.main.Content = new InitPage { DataContext = new SlackTime_ViewModel() };
        }

        private void Border_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < prog.listeProcessus.Count; i++)
            {
                TextBlock item = new TextBlock();
                RowDefinition rowdef = new RowDefinition { Height = new GridLength(60) };
                IdGantt.VerticalAlignment = VerticalAlignment.Bottom;
                IdGantt.RowDefinitions.Insert(i, rowdef);
                item.Text = $"ID = { prog.listeProcessus[i].id }";
                item.FontSize = 14;
                item.Foreground = Brushes.Black;
                item.Margin = new Thickness(0, 5, 0, 5);
                item.VerticalAlignment = VerticalAlignment.Center;
                item.FontFamily = new FontFamily("Lexend");
                item.FontWeight = FontWeights.Medium;
                Grid.SetRow(item, i);
                IdGantt.Children.Add(item);
            }
            TextBlock item0 = new TextBlock();
            RowDefinition rowdef0 = new RowDefinition { Height = new GridLength(60) };
            IdGantt.VerticalAlignment = VerticalAlignment.Bottom;
            IdGantt.RowDefinitions.Insert(prog.listeProcessus.Count, rowdef0);
            item0.Text = "";
            Grid.SetRow(item0, prog.listeProcessus.Count);
            IdGantt.Children.Add(item0);
        }
    }
}
