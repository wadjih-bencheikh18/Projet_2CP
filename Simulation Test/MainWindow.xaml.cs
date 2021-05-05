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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AlgoComboBox_SelectionChanged(this.AlgoComboBox, null);
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

        private void AlgoComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.AlgoComboBox.SelectedItem.ToString().EndsWith("Round Robin"))
            {
                this.ParamEntryGrid.Visibility = Visibility.Visible;
                this.ParamTextBlock.Text = "Quantum:";
            }
            else if (this.AlgoComboBox.SelectedItem.ToString().EndsWith("Multi Niveaux"))
            {
                this.ParamEntryGrid.Visibility = Visibility.Visible;
                this.ParamTextBlock.Text = "Nombre des Niveaux:";
            }
            else if (this.ParamEntryGrid != null)
            {
                this.ParamEntryGrid.Visibility = Visibility.Hidden;
            }
        }

        private void RandomizeCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (this.NbProcessusGrid != null)
            {
                if (this.NbProcessusGrid.Visibility == Visibility.Visible) this.NbProcessusGrid.Visibility = Visibility.Hidden;
                else this.NbProcessusGrid.Visibility = Visibility.Visible;
            }
            if (this.InputFieldsGrid != null)
            {
                if (this.InputFieldsGrid.Visibility == Visibility.Visible) this.InputFieldsGrid.Visibility = Visibility.Hidden;
                else this.InputFieldsGrid.Visibility = Visibility.Visible;
            }
        }

        /*private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            bool random = (bool)this.RandomizeCheckBox.IsChecked;
            int NbProcessus;
            bool correct = int.TryParse(this.NbProcessusTextBox.Text, out NbProcessus);
            if (correct && NbProcessus > 0)
            {
                this.MainStackPanel.Children.RemoveRange(0, this.MainStackPanel.Children.Count - 1);  // vider la fenétre
            }
            else
            {
                if (correct)
                {
                    MessageBox.Show("Nombre des processus incorrect");
                }
                else
                {
                    MessageBox.Show("Input invalid");
                }
                this.NbProcessusTextBox.Text = "";
                return;
            }/*
            Random r = new Random();
            for (int i = 0; i < NbProcessus; i++)
            {
                Processus pro = new Processus();
                pro.id = (i + 1).ToString();
                pro.duree = (random ? r.Next(1, 10) : 1).ToString();
                pro.tempsArriv = (random ? r.Next(1, 10) : 0).ToString();
                pro.Priorite = (random ? r.Next(1, 10) : 1).ToString();
                this.InitTab.Items.Add(pro);
            }
        }*/
    }
}
