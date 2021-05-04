using Ordonnancement;
using System;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Threading;
using System.Windows.Threading;

namespace Simulation_Test
{

    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PAPS prgm = new PAPS();
        public MainWindow()
        {
            //InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool Valid = true;
            int id, duree, tempsArriv;
            if (!Int32.TryParse(idTB.Text, out id))
            {
                idTB.Background = Brushes.Red;
                Valid = false;
            }
            if (!Int32.TryParse(dureeTB.Text, out duree))
            {
                dureeTB.Background = Brushes.Red;
                Valid = false;
            }
            if (!Int32.TryParse(tempsArrivTB.Text, out tempsArriv))
            {
                tempsArrivTB.Background = Brushes.Red;
                Valid = false;
            }
            if (Valid)
            {
                Processus pro = new Processus(id, tempsArriv, duree);
                idTB.Background = Brushes.White;
                dureeTB.Background = Brushes.White;
                tempsArrivTB.Background = Brushes.White;
                InitTab.Items.Add(pro);
                prgm.Push(pro);
            }
           
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            prgm.Executer(ListProcessusView, Processeur,TempsView);
        }

        private void ResultFinalBtn_Click(object sender, RoutedEventArgs e)
        {
            List<Processus> P = new List<Processus>();
            foreach(Processus Pro in prgm.listeProcessus)
            {
                P.Add(new Processus(Pro));
            }
            ResultatFinal resultatFinal = new ResultatFinal(P);
            resultatFinal.Show();
        }
    }
}
