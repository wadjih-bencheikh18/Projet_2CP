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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ordonnancement;

namespace FinalAppTest.Views
{
    /// <summary>
    /// Interaction logic for RR_Tab.xaml
    /// </summary>
    public partial class RR_Tab : UserControl
    {
        public static int NbHint = 0;
        public static bool NextHintCondition = true;

        public RR_Tab()
        {
            indice = 0;
            InitializeComponent();
            IdTextBox.Text = indice.ToString();
            ThisPage = this;
        }

        public static RR prog = new RR(5);
        public static bool modifier = false;
        public static RR_TabRow proModifier;
        public static int indice = 0;
        public static RR_Tab ThisPage;
        public static void FixIndice()
        {
            ThisPage.IdTextBox.Text = indice.ToString();
        }

        private void RandomButton_Click(object sender, RoutedEventArgs e)  // générer aléatoirement des processus
        {
            var bc = new BrushConverter();
            if (!Int32.TryParse(NbProcessusTextBox.Text, out int NbProcessus) || NbProcessus <= 0)
            {
                RectRand.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
            }
            else
            {
                if (NbHint == 5) HintSuivant();
                prog.listeProcessus.Clear();  // vider la liste pour l'ecraser
                ProcessusGrid.Children.RemoveRange(0, ProcessusGrid.Children.Count);
                RectRand.Fill = (Brush)bc.ConvertFrom("#FFFFFFFF");
                Random r = new Random();
                for (int i = 0; i < NbProcessus; i++)
                {
                    AffichageProcessus pro = new AffichageProcessus  // générer un processus
                    {
                        id = i,
                        tempsArriv = r.Next(20),
                        duree = r.Next(1, 5)
                    };
                    RR_TabRow processus = pro.InsererRR(ProcessusGrid, IdTextBox, TempsArrivTextBox, DureeTextBox, ajouterTB);  // inserer son ligne dans le tableau des processus

                    Processus proc = new Processus(pro.id, pro.tempsArriv, pro.duree);
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
            int id;
            var bc = new BrushConverter();
            if (!Int32.TryParse(TempsArrivTextBox.Text, out int tempsArrive) || tempsArrive < 0)  // get temps d'arrivé
            {
                RectTar.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                valide = false;
            }
            if (!Int32.TryParse(DureeTextBox.Text, out int duree) || duree <= 0)  // get durée
            {
                RectDuree.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                valide = false;
            }
            if (valide)  // si tous est correcte
            {
                RectTar.Fill = (Brush)bc.ConvertFrom("#FFEFF3F9");
                RectDuree.Fill = (Brush)bc.ConvertFrom("#FFEFF3F9");
                TempsArrivTextBox.Text = "0";
                DureeTextBox.Text = "1";
                if (!modifier)  // un nouveau processus
                {
                    if (NbHint == 8) HintSuivant();
                    id = indice;
                    IdTextBox.Text = (id + 1).ToString();
                    RectDuree.Fill = (Brush)bc.ConvertFrom("#FFEFF3F9");
                    RectTar.Fill = (Brush)bc.ConvertFrom("#FFEFF3F9");
                    AffichageProcessus pro = new AffichageProcessus
                    {
                        id = id,
                        tempsArriv = tempsArrive,
                        duree = duree,
                    };
                    pro.InsererRR(ProcessusGrid, IdTextBox, TempsArrivTextBox, DureeTextBox, ajouterTB);
                    prog.Push(new Processus(pro.id, pro.tempsArriv, pro.duree));  // added to the program
                    indice++;
                }
                else  // modifier un existant
                {
                    if (NbHint == 12) HintSuivant();
                    AffichageProcessus pro = new AffichageProcessus
                    {
                        id = int.Parse(IdTextBox.Text),
                        tempsArriv = tempsArrive,
                        duree = duree,
                        Background = "#FFEFF3F9"
                    };
                    RR_TabRow item = (RR_TabRow)ProcessusGrid.Children[ProcessusGrid.Children.IndexOf(proModifier)];
                    item.DataContext = pro;
                    ProcessusGrid.Children[ProcessusGrid.Children.IndexOf(proModifier)] = item;
                    prog.listeProcessus[ProcessusGrid.Children.IndexOf(proModifier)] = new Processus(pro.id, pro.tempsArriv, pro.duree);  // modifier le processus correspondant
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

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Int32.TryParse(QuantumTxt.Text, out int q) || q <= 0)  // get quantum
            {
                RectQuantum.Fill = (Brush)new BrushConverter().ConvertFrom("#FFEEBEBE");
            }
            else if (prog.listeProcessus.Count != 0)
            {
                prog.quantum = q;
                MainWindow.main.Content = new SimulationPage(prog, 1);
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

        /// <summary>
        /// HINT PART
        /// </summary>
        
        public static void HintSuivant()
        {
            if (NbHint != 0)
            {
                NextHintCondition = true;
                NbHint++;
                ThisPage.Hint();
            }
        }

        public void ApplyEffect()
        {
            BlurEffect Effect = new BlurEffect();
            Effect.Radius = 8;
            buttons.Effect = Effect;
            Description.Effect = Effect;
            Random.Effect = Effect;
            Simuler.Effect = Effect;
            Tableau.Effect = Effect;
            InitPage.navigation.Effect = Effect;
            Grey.Visibility = Visibility.Visible;
            InitPage.grey.Visibility = Visibility.Visible;
            Panel.SetZIndex(buttons, 0);
            Panel.SetZIndex(Description, 0);
            Panel.SetZIndex(Random, 0);
            Panel.SetZIndex(Simuler, 0);
            Panel.SetZIndex(Tableau, 0);
        }

        public void FinHint()
        {
            NbHint = 0;
            buttons.Effect = null;
            Description.Effect = null;
            Random.Effect = null;
            Simuler.Effect = null;
            Tableau.Effect = null;
            InitPage.navigation.Effect = null;
            Grey.Visibility = Visibility.Hidden;
            InitPage.grey.Visibility = Visibility.Hidden;
            Panel.SetZIndex(buttons, 0);
            Panel.SetZIndex(Description, 0);
            Panel.SetZIndex(Random, 0);
            Panel.SetZIndex(Simuler, 0);
            Panel.SetZIndex(Tableau, 0);
            hint.Child = null;
        }

        private void Hint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) 
        {
            NbHint = 0;
            ApplyEffect();
            Description.Effect = null;
            Panel.SetZIndex(Description, 1);
            if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(300, 180, 0, 0);
            else hint.Margin = new Thickness(290, 165, 0, 0);
            Hint Test = new Hint(
                                "Simulation RR",
                                "Commençons la simulation de l'algorithme Round-Robin",
                                this,
                                hint
                            );
            NextHintCondition = true;
            Test.DataContext = Test;
            hint.Child = Test;
        }

        public void Hint()
        {
            Hint Test;
            if (NextHintCondition)
            {
                ApplyEffect();
                if (NbHint == 0)
                {
                    Description.Effect = null;
                    Panel.SetZIndex(Description, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(315, 175, 0, 0);
                    else hint.Margin = new Thickness(295, 145, 0, 0);
                    Test = new Hint(
                                        "Simulation RR",
                                        "Commençons la simulation de l'algorithme Round-Robin",
                                        this,
                                        hint
                                    );
                    NextHintCondition = true;
                }
                else if (NbHint == 1)
                {
                    Random.Effect = null;
                    Panel.SetZIndex(Random, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(400, 360, 0, 0);
                    else hint.Margin = new Thickness(370, 355, 0, 0);
                    Test = new Hint(
                                        "Générer les processus",
                                        "Vous pouvez générer les processus aléatoirement",
                                        this,
                                        hint
                                    );
                    NextHintCondition = true;
                }
                else if (NbHint == 2)
                {
                    Random.Effect = null;
                    Panel.SetZIndex(Random, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(10, 235, 0, 0);
                    else hint.Margin = new Thickness(25, 230, 0, 0);
                    Test = new Hint(
                                        "Générer les processus",
                                        "Entrez le nombre des processus à générer",
                                        this,
                                        hint
                                    );
                    NextHintCondition = true;
                }
                else if (NbHint == 3)
                {
                    Random.Effect = null;
                    Panel.SetZIndex(Random, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(215, 235, 0, 0);
                    else hint.Margin = new Thickness(345, 370, 0, 0);
                    Test = new Hint(
                                        "Générer les processus",
                                        "Entrez le quantum",
                                        this,
                                        hint
                                    );
                    NextHintCondition = true;
                }
                else if (NbHint == 4)
                {
                    Random.Effect = null;
                    Panel.SetZIndex(Random, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(50, 340, 0, 0);
                    else hint.Margin = new Thickness(25, 300, 0, 0);
                    Test = new Hint(
                                        "Générer les processus",
                                        "Vous pouvez générer des interruptions en cochant cette case",
                                        this,
                                        hint
                                    );
                    NextHintCondition = true;
                }
                else if (NbHint == 5)
                {
                    Random.Effect = null;
                    Panel.SetZIndex(Random, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(415, 465, 0, 0);
                    else hint.Margin = new Thickness(365, 395, 0, 0);
                    Test = new Hint(
                                        "Générer les processus",
                                        "Cliquez sur le button 'Générer' pour créer les processus",
                                        this,
                                        hint
                                    );
                    NextHintCondition = false;
                }
                else if (NbHint == 6)
                {
                    Tableau.Effect = null;
                    Panel.SetZIndex(Tableau, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(675, 345, 0, 0);
                    else hint.Margin = new Thickness(685, 325, 0, 0);
                    Test = new Hint(
                                        "Tableau des processus",
                                        "Voici le tableau des processus générés",
                                        this,
                                        hint
                                    );
                    NextHintCondition = true;
                }
                else if (NbHint == 7)
                {
                    Tableau.Effect = null;
                    Panel.SetZIndex(Tableau, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(670, 435, 0, 0);
                    else hint.Margin = new Thickness(585, 350, 0, 0);
                    Test = new Hint(
                                        "Ajouter un processus",
                                        "Entrer les paramètres du processus à insérer",
                                        this,
                                        hint
                                    );
                    NextHintCondition = true;
                }
                else if (NbHint == 8)
                {
                    Tableau.Effect = null;
                    Panel.SetZIndex(Tableau, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(1115, 460, 0, 0);
                    else hint.Margin = new Thickness(950, 360, 0, 0);
                    Test = new Hint(
                                        "Ajouter un processus",
                                        "Cliquez sur 'Ajouter' pour insérer ce processus",
                                        this,
                                        hint
                                    );
                    NextHintCondition = false;
                }
                else if (NbHint == 9)
                {
                    Tableau.Effect = null;
                    Panel.SetZIndex(Tableau, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(820, 248, 0, 0);
                    else hint.Margin = new Thickness(650, 255, 0, 0);
                    Test = new Hint(
                                        "Supprimer un processus",
                                        "Cliquez sur 'Supprimer' pour supprimer un processus",
                                        this,
                                        hint
                                    );
                    NextHintCondition = false;
                }
                else if (NbHint == 10)
                {
                    Tableau.Effect = null;
                    Panel.SetZIndex(Tableau, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(820, 248, 0, 0);
                    else hint.Margin = new Thickness(650, 255, 0, 0);
                    Test = new Hint(
                                        "Modifier un processus",
                                        "Cliquez sur 'Modifier' pour modifier les paramètres d'un processus",
                                        this,
                                        hint
                                    );
                    NextHintCondition = false;
                }
                else if (NbHint == 11)
                {
                    Tableau.Effect = null;
                    Panel.SetZIndex(Tableau, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(670, 435, 0, 0);
                    else hint.Margin = new Thickness(585, 350, 0, 0);
                    Test = new Hint(
                                        "Modifier un processus",
                                        "Changez les paramètres du processus",
                                        this,
                                        hint
                                    );
                    NextHintCondition = true;
                }
                else if (NbHint == 12)
                {
                    Tableau.Effect = null;
                    Panel.SetZIndex(Tableau, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(1115, 460, 0, 0);
                    else hint.Margin = new Thickness(950, 360, 0, 0);
                    Test = new Hint(
                                        "Modifier un processus",
                                        "Cliquez sur 'Modifier' pour confirmer votre modification",
                                        this,
                                        hint
                                    );
                    NextHintCondition = false;
                }
                else if (NbHint == 13)
                {
                    Tableau.Effect = null;
                    Panel.SetZIndex(Tableau, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(190, 240, 0, 0);
                    else hint.Margin = new Thickness(180, 305, 0, 0);
                    Test = new Hint(
                                        "Gérer les interruptions",
                                        "Cliquez sur un processus pour gérer ses interruptions",
                                        this,
                                        hint
                                    );
                    NextHintCondition = false;
                }
                else if (NbHint == 14)
                {
                    Tableau.Effect = null;
                    Panel.SetZIndex(Tableau, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(190, 240, 0, 0);
                    else hint.Margin = new Thickness(180, 305, 0, 0);
                    Test = new Hint(
                                        "Gérer les interruptions",
                                        "Changez les paramètres de l'interruption à insérer" +
                                        "\n\nNB: le temps d'arrivé de l'interruption doit être " +
                                        "inférieur à la durée du processus",
                                        this,
                                        hint
                                    );
                    NextHintCondition = true;
                }
                else if (NbHint == 15)
                {
                    Tableau.Effect = null;
                    Panel.SetZIndex(Tableau, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(190, 240, 0, 0);
                    else hint.Margin = new Thickness(180, 305, 0, 0);
                    Test = new Hint(
                                        "Gérer les interruptions",
                                        "Cliquez sur '+' pour insérer cette interruption",
                                        this,
                                        hint
                                    );
                    NextHintCondition = false;
                }
                else if (NbHint == 16)
                {
                    Tableau.Effect = null;
                    Panel.SetZIndex(Tableau, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(190, 240, 0, 0);
                    else hint.Margin = new Thickness(180, 305, 0, 0);
                    Test = new Hint(
                                        "Gérer les interruptions",
                                        "Cliquez sur 'x' pour supprimer une interruption",
                                        this,
                                        hint
                                    );
                    NextHintCondition = false;
                }
                else if (NbHint == 17)
                {
                    Simuler.Effect = null;
                    Panel.SetZIndex(Simuler, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(360, 570, 0, 0);
                    else hint.Margin = new Thickness(330, 480, 0, 0);
                    Test = new Hint(
                                        "Simulation",
                                        "Cliquez sur le button 'Simuler' pour commencer la simulation",
                                        this,
                                        hint
                                    );
                    Test.Fin();
                    NextHintCondition = true;
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
            }
        }

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

        private void NbProcessusTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Text = "";
        }

        private void QuantumTxt_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Text = "";
        }

        private void proChange(object sender, EventArgs e)
        {
            if (ProcessusGrid.Children.Count == 0)
            {
                StartHover.Fill = (Brush)(new BrushConverter()).ConvertFrom("#8AAA");
                StartHover.Cursor = Cursors.No;
            }
            else
            {
                StartHover.Fill = (Brush)(new BrushConverter()).ConvertFrom("#0000");
                StartHover.Cursor = Cursors.Hand;
            }
        }
    }
}
