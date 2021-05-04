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
        int id;
        PAPS prgm = new PAPS();
        public MainWindow()
        {
            InitializeComponent();
            idTB.Text = id.ToString();
            id++;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool Valid = true;
            int  duree, tempsArriv;
            var bc = new BrushConverter();

            if (!Int32.TryParse(dureeTB.Text, out duree))
            {
                dureeTB.BorderBrush = (Brush)bc.ConvertFrom("#FFF52C2C");
                dureeTB.Background = (Brush)bc.ConvertFrom("#FFEEBEBE");
                Valid = false;
            }
            if (!Int32.TryParse(tempsArrivTB.Text, out tempsArriv))
            {
                tempsArrivTB.Background = (Brush)bc.ConvertFrom("#FFEEBEBE");
                tempsArrivTB.BorderBrush = (Brush)bc.ConvertFrom("#FFF52C2C") ;

                Valid = false;
            }
            if (Valid)
            {
                idTB.Text = id.ToString();
                
                Processus pro = new Processus(id-1, tempsArriv, duree);
                idTB.Background = Brushes.White;
                dureeTB.Background = Brushes.White;
                tempsArrivTB.Background = Brushes.White;
                InitTab.Items.Add(pro);
                prgm.Push(pro);
                id++;
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
