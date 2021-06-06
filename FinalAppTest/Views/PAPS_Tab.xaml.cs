using Ordonnancement;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace FinalAppTest.Views
{
    /// <summary>
    /// Interaction logic for PAPS_Tab.xaml
    /// </summary>

    public partial class PAPS_Tab : UserControl
    {
        public static int NbHint = 0;
        public static bool NextHintCondition = true;
        public static PAPS_Tab ThisPage;

        public PAPS_Tab()
        {
            InitializeComponent();
            IdTextBox.Text = indice.ToString();
            ThisPage = this;
        }

        public static PAPS prog = new PAPS();
        public static bool modifier = false;
        public static PAPS_TabRow proModifier;
        private int indice = 0;

        private void RandomButton_Click(object sender, RoutedEventArgs e)  // générer aléatoirement des processus
        {
            var bc = new BrushConverter();
            if (!Int32.TryParse(NbProcessusTextBox.Text, out int NbProcessus) || NbProcessus <= 0)
            {
                RectRand.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
            }
            else
            {
                if(NbHint==4) HintSuivant();
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
                        duree = r.Next(1, 5)
                    };
                    PAPS_TabRow processus = pro.InsererPAPS(ProcessusGrid, IdTextBox, TempsArrivTextBox, DureeTextBox, ajouterTB);  // inserer son ligne dans le tableau des processus

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
            int id, tempsArrive, duree;
            var bc = new BrushConverter();
            if (!Int32.TryParse(TempsArrivTextBox.Text, out tempsArrive) || tempsArrive < 0)  // get temps d'arrivé
            {
                valide = false;
                RectTar.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
            }
            if (!Int32.TryParse(DureeTextBox.Text, out duree) || duree <= 0)  // get durée
            {
                valide = false;
                RectDuree.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
            }
            if (valide)  // si tous est correcte
            {
                
                RectTar.Fill = (Brush)bc.ConvertFrom("#FFEFF3F9");
                RectDuree.Fill = (Brush)bc.ConvertFrom("#FFEFF3F9");
                if (!modifier)  // un nouveau processus
                {
                    if (NbHint == 7) HintSuivant();
                    id = indice;
                    TempsArrivTextBox.Text = "0";
                    DureeTextBox.Text = "1";
                    IdTextBox.Text = (id + 1).ToString();
                    NbProcessusTextBox.Background = (Brush)bc.ConvertFrom("#00000000");
                    AffichageProcessus pro = new AffichageProcessus
                    {
                        id = id,
                        tempsArriv = tempsArrive,
                        duree = duree,
                    };
                    pro.InsererPAPS(ProcessusGrid, IdTextBox, TempsArrivTextBox, DureeTextBox, ajouterTB);
                    prog.Push(new Processus(pro.id, pro.tempsArriv, pro.duree));  // added to the program
                    indice++;
                }
                else  // modifier un existant
                {
                    if (NbHint == 11) HintSuivant();
                    AffichageProcessus pro = new AffichageProcessus
                    {
                        id = int.Parse(IdTextBox.Text),
                        tempsArriv = tempsArrive,
                        duree = duree,
                        Background = "#FFEFF3F9"
                    };
                    PAPS_TabRow item = (PAPS_TabRow)ProcessusGrid.Children[ProcessusGrid.Children.IndexOf(proModifier)];
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
                if (NbHint == 0)
                    MainWindow.main.Content = new SimulationPage(prog, 0);
                else
                    FinHint();
               
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
            if(NbHint!=0)
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
            Grey.Visibility = Visibility.Visible;
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
            Grey.Visibility = Visibility.Hidden;
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
            hint.Margin = new Thickness(264, 120, 0, 0);
            Hint Test = new Hint(
                                "Simlation PAPS",
                                "On va comancer une simulation PAPS",
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
                if (NbHint == 1)
                {
                    Random.Effect = null;
                    Panel.SetZIndex(Random, 1);
                    hint.Margin = new Thickness(348, 276, 0, 0);
                    Test = new Hint(
                                        "Generer les Processus",
                                        "Vous pouvez Generer les processus aleatoirement",
                                        this,
                                        hint
                                    );
                    NextHintCondition = true;
                }
                else if (NbHint == 2)
                {
                    Random.Effect = null;
                    Panel.SetZIndex(Random, 1);
                    hint.Margin = new Thickness(196, 324, 0, 0);
                    Test = new Hint(
                                        "Generer les Processus",
                                        "Enter un nombre aleatoire des processus a generer",
                                        this,
                                        hint
                                    );
                    NextHintCondition = true;
                }
                else if (NbHint == 3)
                {
                    Random.Effect = null;
                    Panel.SetZIndex(Random, 1);
                    hint.Margin = new Thickness(364, 298, 0, 0);
                    Test = new Hint(
                                        "Generer les Processus",
                                        "Vous pouvez generer des interruption en cliquant ici",
                                        this,
                                        hint
                                    );
                    NextHintCondition = true;
                }
                else if (NbHint == 4)
                {
                    Random.Effect = null;
                    Panel.SetZIndex(Random, 1);
                    hint.Margin = new Thickness(360, 352, 0, 0);
                    Test = new Hint(
                                        "Generer les Processus",
                                        "Clique sur le button Générer pour creer les procesus",
                                        this,
                                        hint
                                    );
                    NextHintCondition = false;
                }
                else if (NbHint == 5)
                {
                    Tableau.Effect = null;
                    Panel.SetZIndex(Tableau, 1);
                    hint.Margin = new Thickness(676, 312, 0, 0);
                    Test = new Hint(
                                        "Tableau des procesus",
                                        "Voici le tableau des procesus vos pouvez changer les procesus comme vos voulez",
                                        this,
                                        hint
                                    );
                    NextHintCondition = true;
                }
                else if (NbHint == 6)
                {
                    Tableau.Effect = null;
                    Panel.SetZIndex(Tableau, 1);
                    hint.Margin = new Thickness(936, 490, 0, 0);
                    Test = new Hint(
                                        "Ajouter un processus",
                                        "Changer les parametre de processus a inserer",
                                        this,
                                        hint
                                    );
                    NextHintCondition = true;
                }
                else if (NbHint == 7)
                {
                    Tableau.Effect = null;
                    Panel.SetZIndex(Tableau, 1);
                    hint.Margin = new Thickness(778, 482, 0, 0);
                    Test = new Hint(
                                        "Ajouter un processus",
                                        "Clique sur Ajouter pour inserer le processus",
                                        this,
                                        hint
                                    );
                    NextHintCondition = false;
                }
                else if (NbHint == 8)
                {
                    Tableau.Effect = null;
                    Panel.SetZIndex(Tableau, 1);
                    hint.Margin = new Thickness(670, 248, 0, 0);
                    Test = new Hint(
                                        "Supprimer un processus",
                                        "Clique sur Supprimer pour supprimer un processus",
                                        this,
                                        hint
                                    );
                    NextHintCondition = false;
                }
                else if (NbHint == 9)
                {
                    Tableau.Effect = null;
                    Panel.SetZIndex(Tableau, 1);
                    hint.Margin = new Thickness(670, 248, 0, 0);
                    Test = new Hint(
                                        "Modifier un processus",
                                        "Clique sur Modifier pour commncer la modification sur ce processus",
                                        this,
                                        hint
                                    );
                    NextHintCondition = false;
                }
                else if (NbHint == 10)
                {
                    Tableau.Effect = null;
                    Panel.SetZIndex(Tableau, 1);
                    hint.Margin = new Thickness(936, 490, 0, 0);
                    Test = new Hint(
                                        "Modifier un processus",
                                        "Changer les parametre de processus",
                                        this,
                                        hint
                                    );
                    NextHintCondition = true;
                }
                else if (NbHint == 11)
                {
                    Tableau.Effect = null;
                    Panel.SetZIndex(Tableau, 1);
                    hint.Margin = new Thickness(778, 482, 0, 0);
                    Test = new Hint(
                                        "Modifier un processus",
                                        "Clique sur Modifier pour Confirmer votre modification",
                                        this,
                                        hint
                                    );
                    NextHintCondition = false;
                }
                else if (NbHint == 12)
                {
                    Tableau.Effect = null;
                    Panel.SetZIndex(Tableau, 1);
                    hint.Margin = new Thickness(198, 210, 0, 0);
                    Test = new Hint(
                                        "Gerer les interuptions",
                                        "Click sur le petit triangle pour commancer a gerer les interruptions.",
                                        this,
                                        hint
                                    );
                    NextHintCondition = false;
                }
                else if (NbHint == 13)
                {
                    Tableau.Effect = null;
                    Panel.SetZIndex(Tableau, 1);
                    hint.Margin = new Thickness(230, 272, 0, 0);
                    Test = new Hint(
                                        "Gerer les interuptions",
                                        "Changer les parametre de l'interruption a inserer.",
                                        this,
                                        hint
                                    );
                    NextHintCondition = true;
                }
                else if (NbHint == 14)
                {
                    Tableau.Effect = null;
                    Panel.SetZIndex(Tableau, 1);
                    hint.Margin = new Thickness(686, 312, 0, 0);
                    Test = new Hint(
                                        "Gerer les interuptions",
                                        "Clique sur Ajouter pour inserer cette interruption.",
                                        this,
                                        hint
                                    );
                    NextHintCondition = false;
                }
                else if (NbHint == 15)
                {
                    Tableau.Effect = null;
                    Panel.SetZIndex(Tableau, 1);
                    hint.Margin = new Thickness(686, 312, 0, 0);
                    Test = new Hint(
                                        "Gerer les interuptions",
                                        "Clique sur Supprimer pour supprimer une interruption.",
                                        this,
                                        hint
                                    );
                    NextHintCondition = false;
                }
                else if (NbHint == 16)
                {
                    Simuler.Effect = null;
                    Panel.SetZIndex(Simuler, 1);
                    hint.Margin = new Thickness(370, 328, 0, 0);
                    Test = new Hint(
                                        "Commancer la simulation",
                                        "Clique sur le button Simuler pour commnacer la simulation.",
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
    }
}
