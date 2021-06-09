﻿using Ordonnancement;
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

namespace FinalAppTest
{
    /// <summary>
    /// Interaction logic for ResultFinal_Tab.xaml
    /// </summary>
    public partial class ResultFinal_Tab : Page
    {
        List<Processus> P = new List<Processus>();
        List<ProcessusNiveau> PN = new List<ProcessusNiveau>();
        public int h ;
        public ResultFinal_Tab(List<Processus> P)
        {
            this.P = P;
            this.h = 0;
            InitializeComponent();
        }
        public ResultFinal_Tab(List<ProcessusNiveau> PN)
        {
            this.PN = PN;
            this.h = 1;
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int tempsAttMoy = 0;
            int tempsServiceMoy = 0;
            int tempsReponseMoy = 0;
            int tempsFinMoy = 0;
            if (h == 0)
            {
                for (int i = 0; i < P.Count; i++)
                {
                    P[i].Affichage(Grid1, i);
                    tempsAttMoy += P[i].tempsAtt;
                    tempsServiceMoy += P[i].tempsService;
                    tempsReponseMoy += P[i].tempsReponse;
                    tempsFinMoy += P[i].tempsFin;
                }
                TempsAttMoyTB.Text = Math.Round((double)tempsAttMoy / P.Count, 3).ToString();
                TempsSerMoyTB.Text = Math.Round((double)tempsServiceMoy / P.Count, 3).ToString();
                TempsRepMoyTB.Text = Math.Round((double)tempsReponseMoy / P.Count, 3).ToString();
                TempsFinMoyTB.Text = Math.Round((double)tempsFinMoy / P.Count, 3).ToString();
            }
            else if (h == 1)
            {
                for (int i = 0; i < PN.Count; i++)
                {
                    PN[i].Affichage(Grid1, i);
                    tempsAttMoy += PN[i].tempsAtt;
                    tempsServiceMoy += PN[i].tempsService;
                    tempsReponseMoy += PN[i].tempsReponse;
                    tempsFinMoy += PN[i].tempsFin;
                }
                TempsAttMoyTB.Text = Math.Round((double)tempsAttMoy / PN.Count, 3).ToString();
                TempsSerMoyTB.Text = Math.Round((double)tempsServiceMoy / PN.Count, 3).ToString();
                TempsRepMoyTB.Text = Math.Round((double)tempsReponseMoy / PN.Count, 3).ToString();
                TempsFinMoyTB.Text = Math.Round((double)tempsFinMoy / PN.Count, 3).ToString();
            }
        }

        private void Terminer_Click(object sender, MouseButtonEventArgs e)
        {
            MainWindow.main.Content = SimulationPage.save;
        }

        private void TerminerButton_MouseEnter(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            TerminerButton.Fill = (Brush)bc.ConvertFrom("#FF575757");
        }

        private void TerminerButton_MouseLeave(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            TerminerButton.Fill = (Brush)bc.ConvertFrom("#5829AA");
        }

        private void TextBlock_Initialized(object sender, EventArgs e)
        {

        }
    }
}
