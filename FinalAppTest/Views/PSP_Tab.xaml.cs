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
    /// Interaction logic for PSP_Tab.xaml
    /// </summary>
    public partial class PSP_Tab : UserControl
    {
        public PSP_Tab()
        {
            InitializeComponent();
            IdTextBox.Text = indice.ToString();
        }

        public static PSP prog = new PSP();
        public static bool modifier = false;
        public static PSP_TabRow proModifier;
        private int indice = 0;

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
                    PSP_TabRow processus = pro.InsererPSP(ProcessusGrid, IdTextBox, TempsArrivTextBox, DureeTextBox, PrioTextBox, ajouterTB);  // inserer son ligne dans le tableau des processus
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
                    pro.InsererPSP(ProcessusGrid, IdTextBox, TempsArrivTextBox, DureeTextBox, PrioTextBox, ajouterTB);
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
                    PSP_TabRow item = (PSP_TabRow)ProcessusGrid.Children[ProcessusGrid.Children.IndexOf(proModifier)];
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
    }
}
