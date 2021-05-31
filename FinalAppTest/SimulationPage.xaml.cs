﻿using System;
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
        public static SimulationPage save;
        public static bool activated = false;
        public static bool paused = false;
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
    }
}
