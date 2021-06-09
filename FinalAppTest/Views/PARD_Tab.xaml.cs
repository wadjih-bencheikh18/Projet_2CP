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

namespace FinalAppTest.Views
{
    /// <summary>
    /// Interaction logic for PARD_Tab.xaml
    /// </summary>
    public partial class PARD_Tab : UserControl
    {
        public PARD_Tab()
        {
            InitializeComponent();
            IdTextBox.Text = indice.ToString();
            ThisPage = this;
        }
        public static PARD_Tab ThisPage;
        public static PARD prog = new PARD(5);
        public static bool modifier = false;
        public static PARD_TabRow proModifier;
        public static int indice = 0;
        public static void FixIndice()
        {
            ThisPage.IdTextBox.Text = indice.ToString();
        }
        private void RandomButton_Click(object sender, RoutedEventArgs e)  // générer aléatoirement des processus
        {
            int NbProcessus;
            var bc = new BrushConverter();
            if (!Int32.TryParse(NbProcessusTextBox.Text, out NbProcessus) && NbProcessus <= 0)
            {
                RectRand.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
            }
            else
            {
                prog.listeProcessus.Clear();  // vider la liste pour l'ecraser
                NbProcessusTextBox.Text = "0";
                ProcessusGrid.Children.RemoveRange(0, ProcessusGrid.Children.Count);
                RectRand.Fill = (Brush)bc.ConvertFrom("#FFFFFFFF");
                Random r = new Random();
                for (int i = 0; i < NbProcessus; i++)
                {
                    AffichageProcessus pro = new AffichageProcessus  // générer un processus
                    {
                        id = i,
                        tempsArriv = r.Next(20),
                        duree = r.Next(1, 5),
                        prio = r.Next(1, 20)
                    };
                    PARD_TabRow processus = pro.InsererPARD(ProcessusGrid, IdTextBox, TempsArrivTextBox, DureeTextBox, PrioTextBox, ajouterTB);  // inserer son ligne dans le tableau des processus
                    Processus proc = new Processus(pro.id, pro.tempsArriv, pro.duree, pro.prio);
                    processus.parent.Items.RemoveAt(processus.parent.Items.Count - 1);  // remove the ajouter_row
                    for (int j = 0; ((bool)RandomizeInterrup.IsChecked) && pro.duree > 1 && j < r.Next(0, 3); j++)  // générer des interruptions
                    {
                        Interruption inter;
                        if (r.Next(0, 2) == 1) inter = new Interruption("Entrée/sortie", r.Next(1, 5), r.Next(1, pro.duree));
                        else inter = new Interruption("appel methode", r.Next(1, 5), r.Next(1, pro.duree));
                        proc.Push(inter);  // ajouter l'interruption au liste des interruptions du processus

                        Interruption_TabRow row = new Interruption_TabRow(processus)  // créer une ligne interruption
                        {
                            DataContext = inter
                        };
                        processus.parent.Items.Add(row);  // inserer sa ligne dans les éléments de sa TreeViewItem
                    }
                    processus.parent.Items.Add(new Interruption_Ajouter(processus));  // append ajouter_row
                    prog.Push(proc);  // added processus to the program
                }
                IdTextBox.Text = NbProcessus.ToString();
                indice = NbProcessus;
            }
        }

        private void AddProcessusButton_Click(object sender, RoutedEventArgs e)  // ajouter un processus
        {
            bool valide = true;
            int id, tempsArrive, duree,prio ;
            var bc = new BrushConverter();
            if (!Int32.TryParse(TempsArrivTextBox.Text, out tempsArrive) || tempsArrive < 0)  // get temps d'arrivé
            {
                valide = false;
                RectTar.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
            }
            if (!Int32.TryParse(DureeTextBox.Text, out duree) || duree <= 0)  // get durée
            {
                RectDuree.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                valide = false;
            }
            if (!Int32.TryParse(PrioTextBox.Text, out prio) || prio < 0)  // get priorité
            {
                RectPrio.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                valide = false;
            }
            if (valide)  // si tous est correcte
            {
                RectTar.Fill = (Brush)bc.ConvertFrom("#FFEFF3F9");
                RectDuree.Fill = (Brush)bc.ConvertFrom("#FFEFF3F9");
                RectPrio.Fill = (Brush)bc.ConvertFrom("#FFEFF3F9");
                if (!modifier)  // un nouveau processus
                {
                    id = indice;
                    TempsArrivTextBox.Text = "0";
                    DureeTextBox.Text = "0";
                    IdTextBox.Text = (id + 1).ToString();
                    PrioTextBox.Text = "0";
                    RectDuree.Fill = (Brush)bc.ConvertFrom("#FFEFF3F9");
                    RectTar.Fill = (Brush)bc.ConvertFrom("#FFEFF3F9");
                    RectPrio.Fill = (Brush)bc.ConvertFrom("#FFEFF3F9");
                    AffichageProcessus pro = new AffichageProcessus
                    {
                        id = id,
                        tempsArriv = tempsArrive,
                        duree = duree,
                        prio = prio
                    };
                    pro.InsererPARD(ProcessusGrid, IdTextBox, TempsArrivTextBox, DureeTextBox, PrioTextBox, ajouterTB);
                    prog.Push(new Processus(pro.id, pro.tempsArriv, pro.duree, pro.prio));  // added to the program
                    indice++;
                }
                else  // modifier un existant
                {
                    AffichageProcessus pro = new AffichageProcessus
                    {
                        id = int.Parse(IdTextBox.Text),
                        tempsArriv = tempsArrive,
                        duree = duree,
                        prio = prio,
                        Background = "#FFEFF3F9"
                    };
                    PARD_TabRow item = (PARD_TabRow)ProcessusGrid.Children[ProcessusGrid.Children.IndexOf(proModifier)];
                    item.DataContext = pro;
                    ProcessusGrid.Children[ProcessusGrid.Children.IndexOf(proModifier)] = item;
                    prog.listeProcessus[ProcessusGrid.Children.IndexOf(proModifier)] = new Processus(pro.id, pro.tempsArriv, pro.duree, pro.prio);  // modifier le processus correspondant
                    modifier = false;
                    IdTextBox.Text = indice.ToString();
                    ajouterTB.Text = "Ajouter";
                }
            }
        }

        private void AddProcessusButton_MouseEnter(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            AddProcessusButton.Fill = (Brush)bc.ConvertFrom("#FFE9FFF0");
        }

        private void AddProcessusButton_MouseLeave(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            AddProcessusButton.Fill = (Brush)bc.ConvertFrom("#FFCCFFDD");
        }

        private void RandomButton_MouseEnter(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            RandomButton.Fill = (Brush)bc.ConvertFrom("#FF575757");
        }
        private void RandomButton_MouseLeave(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            RandomButton.Fill = (Brush)bc.ConvertFrom("#FF000000");
        }

        private void TempsArrivTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            if (!int.TryParse(TempsArrivTextBox.Text, out int i) || i < 0) RectTar.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
            else RectTar.Fill = (Brush)bc.ConvertFrom("#FFEFF3F9");
        }

        private void DureeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            if (!int.TryParse(DureeTextBox.Text, out int i) || i <= 0) RectDuree.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
            else RectDuree.Fill = (Brush)bc.ConvertFrom("#FFEFF3F9");
        }

        private void NbProcessusTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            if (!int.TryParse(NbProcessusTextBox.Text, out int i) || i < 0) RectRand.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
            else RectRand.Fill = (Brush)bc.ConvertFrom("#FFFFFFFF");
        }

        private void PrioTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            if (!int.TryParse(PrioTextBox.Text, out int i) || i < 0) RectPrio.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
            else RectPrio.Fill = (Brush)bc.ConvertFrom("#FFEFF3F9");
        }
        private void Quantum_LostFocus(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            if (!int.TryParse(QuantumTxt.Text, out int i) || i <= 0) RectQuantum.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
            else RectQuantum.Fill = (Brush)bc.ConvertFrom("#FFFFFFFF");
        }

        private void QuantumTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            if (!int.TryParse(QuantumTxt.Text, out int i) || i <= 0)
            {
                prog.refrechTemps = -1;
                RectQuantum.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
            }
            else
            {
                prog.refrechTemps = i;
                RectQuantum.Fill = (Brush)bc.ConvertFrom("#FFFFFFFF");
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (prog.listeProcessus.Count == 0)
            {
                var bc = new BrushConverter();
                //StartButton.BorderBrush = (Brush)bc.ConvertFrom("#FFF52C2C");
                StartButton.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
            }
            else
            {
                MainWindow.main.Content = new SimulationPage(prog, 4);
            }
        }


        private void Home_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.main.Content = new WelcomePage();
        }

        private void Home_MouseEnter(object sender, MouseEventArgs e)
        {
            shadowHome.ShadowDepth = 2;
            shadowHome.BlurRadius = 7;
        }

        private void Home_MouseLeave(object sender, MouseEventArgs e)
        {
            shadowHome.ShadowDepth = 0;
            shadowHome.BlurRadius = 5;
        }

        private void Return_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.main.Content = new DashBoard();
        }

        private void Return_MouseEnter(object sender, MouseEventArgs e)
        {
            shadowReturn.ShadowDepth = 2;
            shadowReturn.BlurRadius = 7;
        }

        private void Return_MouseLeave(object sender, MouseEventArgs e)
        {
            shadowReturn.ShadowDepth = 0;
            shadowReturn.BlurRadius = 5;
        }

        private void Cours_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //MainWindow.main.Content = new WelcomePage();
        }

        private void Cours_MouseEnter(object sender, MouseEventArgs e)
        {
            shadowCours.ShadowDepth = 2;
            shadowCours.BlurRadius = 7;
        }

        private void Cours_MouseLeave(object sender, MouseEventArgs e)
        {
            shadowCours.ShadowDepth = 0;
            shadowCours.BlurRadius = 5;
        }

        private void Hint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { }/*
        {
            Hint Test;
            if (NbHint == 0)
            {
                hint.Margin = new Thickness(264, 120, 0, 0);
                Test = new Hint(
                                    "Generer les Processus",
                                    "Vous pouvez generer les procesus aleatoirement",
                                    this,
                                    hint
                                );
                NextHintCondition = true;
            }
            else if (NbHint == 1)
            {
                hint.Margin = new Thickness(264, 300, 0, 0);
                Test = new Hint(
                                    "Generer les Processus",
                                    "Entré un nombre aleatoire des processus a generer",
                                    this,
                                    hint
                                );
                NextHintCondition = false;
            }
            else
            {
                hint.Margin = new Thickness(264, 117, 0, 0);
                Test = new Hint(
                                    "Error",
                                    "Error 404",
                                    this,
                                    hint
                                );
            }
            Test.DataContext = Test;
            hint.Child = Test;
        }*/

        private void Hint_MouseEnter(object sender, MouseEventArgs e)
        {
            shadowHint.ShadowDepth = 2;
            shadowHint.BlurRadius = 7;
        }

        private void Hint_MouseLeave(object sender, MouseEventArgs e)
        {
            shadowHint.ShadowDepth = 0;
            shadowHint.BlurRadius = 5;
        }
    }
}
