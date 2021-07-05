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
using System.Windows.Media.Effects;

namespace FinalAppTest.Views
{
    /// <summary>
    /// Interaction logic for Mult_Niv_Tab.xaml
    /// </summary>
    public partial class Mult_Niv_Tab : UserControl
    {
        public static MultiNiveau prog;
        public static int NbHint = 0;
        public static bool NextHintCondition = true;
        public Mult_Niv_Tab()
        {
            indiceniv = 0;
            indicepro = 0;
            InitializeComponent();
            IdTextBox.Text = indicepro.ToString();
            randNiv.Text = indiceniv.ToString();
            nivId.Text = indiceniv.ToString();
            ThisPage = this;
        }

        public static List<ProcessusNiveau> ListPro = new List<ProcessusNiveau>();
        public static Niveau[] niveaux = new Niveau[4];
        public static bool modifier = false;
        public static UserControl proModifier;
        public static int indiceniv = 0;
        public static int indicepro = 0;
        public static Mult_Niv_Tab ThisPage;

        public static void FixIndice()
        {
            ThisPage.IdTextBox.Text = indicepro.ToString();
        }

        private void RandomButton_Click(object sender, RoutedEventArgs e)  // générer aléatoirement des processus
        {
            ListPro.Clear();
            var bc = new BrushConverter();
            if (!Int32.TryParse(NbProcessusTextBox.Text, out int NbProcessus) || NbProcessus <= 0)
            {
                RectRand.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
            }
            else
            {
                if (NbHint == 13) HintSuivant();
                ProcessusGrid.Children.RemoveRange(0, ProcessusGrid.Children.Count);
                RectRand.Fill = (Brush)bc.ConvertFrom("#FFFFFF");
                Random r = new Random();
                for (int i = 0; i < NbProcessus; i++)
                {
                    AffichageProcessus pro = new AffichageProcessus
                    {
                        id = i,
                        tempsArriv = r.Next(20),
                        duree = r.Next(1, 10),
                        prio = r.Next(0, 6),
                        niveau = r.Next(0, indiceniv)
                    };
                    Multi_Niv_TabRow_Proc processus = pro.InsererProcML(ProcessusGrid, IdTextBox, TempsArrivTextBox, DureeTextBox, PrioTextBox, NivTextBox, ajouterTB);
                    ProcessusNiveau proc = new ProcessusNiveau(pro.id, pro.tempsArriv, pro.duree, pro.prio, pro.niveau);
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

                    ListPro.Add(proc);
                }
                IdTextBox.Text = NbProcessus.ToString();
                indicepro = NbProcessus;
            }
        }

        private void AddProcessusButton_Click(object sender, RoutedEventArgs e)  // ajouter un processus
        {
            bool valide = true;
            int id;
            var bc = new BrushConverter();
            if (!Int32.TryParse(TempsArrivTextBox.Text, out int tempsArrive) || tempsArrive < 0)  // get temps d'arrivé
            {
                valide = false;
                RectProTar.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
            }
            if (!Int32.TryParse(NivTextBox.Text, out int niv) || niv >= indiceniv || niv < 0)  // get temps d'arrivé
            {
                valide = false;
                RectProNiv.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
            }

            if (!Int32.TryParse(PrioTextBox.Text, out int prio) || prio < 0)  // get temps d'arrivé
            {
                valide = false;
                RectProPrio.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
            }

            if (!Int32.TryParse(DureeTextBox.Text, out int duree) || duree <= 0)  // get durée
            {
                valide = false;
                RectProDuree.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
            }
            if (valide)  // si tous est correcte
            {
                RectProTar.Fill = (Brush)bc.ConvertFrom("#FFEFF3F9");
                RectProDuree.Fill = (Brush)bc.ConvertFrom("#FFEFF3F9");
                RectProPrio.Fill = (Brush)bc.ConvertFrom("#FFEFF3F9");
                RectProNiv.Fill = (Brush)bc.ConvertFrom("#FFEFF3F9");
                TempsArrivTextBox.Text = "0";
                DureeTextBox.Text = "1";
                PrioTextBox.Text = "0";
                NivTextBox.Text = "0";
                algoSelect.SelectedIndex = 0;
                if (!modifier)
                {
                    if (NbHint == 16) HintSuivant();
                    id = indicepro;
                    IdTextBox.Text = (id + 1).ToString();
                    AffichageProcessus pro = new AffichageProcessus
                    {
                        id = id,
                        tempsArriv = tempsArrive,
                        duree = duree,
                        prio = prio,
                        niveau = niv,
                    };
                    pro.InsererProcML(ProcessusGrid, IdTextBox, TempsArrivTextBox, DureeTextBox, PrioTextBox, NivTextBox, ajouterTB);
                    ListPro.Add(new ProcessusNiveau(pro.id, pro.tempsArriv, pro.duree, pro.prio, pro.niveau));
                    indicepro++;
                }
                else
                {
                    if (NbHint == 20) HintSuivant();
                    id = int.Parse(IdTextBox.Text);
                    AffichageProcessus pro = new AffichageProcessus
                    {
                        id = id,
                        tempsArriv = tempsArrive,
                        duree = duree,
                        prio = prio,
                        niveau = niv
                    };
                    Multi_Niv_TabRow_Proc item = (Multi_Niv_TabRow_Proc)ProcessusGrid.Children[ProcessusGrid.Children.IndexOf(proModifier)];
                    item.DataContext = pro;
                    ListPro[id] = new ProcessusNiveau(id, tempsArrive, duree, prio, niv);
                    ProcessusGrid.Children[ProcessusGrid.Children.IndexOf(proModifier)] = item;
                    modifier = false;
                    IdTextBox.Text = indicepro.ToString();
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

        private void AddNiv(object sender, MouseButtonEventArgs e)
        {
            if (int.Parse(nivId.Text) > 3)
            {
                return;
            }
            bool valide = true;
            string type = algoSelect.Text;
            int niv, algo, q = 0;
            var bc = new BrushConverter();

            niv = int.Parse(nivId.Text);
            algo = algoSelect.SelectedIndex;

            if ((algo == 7 || algo == 6) && (!Int32.TryParse(nivQuantum.Text, out q) || q <= 0))  // get durée
            {
                RectQuantum.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                valide = false;
            }
            string quan = q.ToString();
            if (algo != 7 && algo != 6) quan = "/";
            if (valide && !modifier)  // si tous est correcte
            {
                if (NbHint == 6 ) HintSuivant();
                niv = indiceniv;
                nivId.Text = (niv + 1).ToString();
                NbProcessusTextBox.Background = (Brush)bc.ConvertFrom("#00000000");
                AffichageProcessus pro = new AffichageProcessus
                {
                    id = niv,
                    Background = type,
                    quantum = quan
                };
                pro.InsererNivML(NiveauGrid, nivId, algoSelect, nivQuantum, ajouterNV);
                if (algo == 7 || algo == 6) niveaux[indiceniv] = new Niveau(algo, q);
                else niveaux[indiceniv] = new Niveau(algo);
                indiceniv++;
                randNiv.Text = indiceniv.ToString();
            }
            else if (valide && modifier)
            {
                if (NbHint == 9 ) HintSuivant();
                AffichageProcessus pro = new AffichageProcessus
                {
                    id = niv,
                    Background = type,
                    quantum = quan,
                };
                Mult_Niv_TabRow item = (Mult_Niv_TabRow)NiveauGrid.Children[NiveauGrid.Children.IndexOf(proModifier)];
                item.DataContext = pro;
                if (algo == 7 || algo == 6) niveaux[niv] = new Niveau(algo, q);
                else niveaux[NiveauGrid.Children.IndexOf(proModifier)] = new Niveau(algo);
                NiveauGrid.Children[NiveauGrid.Children.IndexOf(proModifier)] = item;
                modifier = false;
                nivId.Text = indiceniv.ToString();
                ajouterNV.Text = "Ajouter";
                if (indiceniv == 4) ajouterButton.Visibility = Visibility.Hidden;
            }
        }

        private void algoSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded) return;
            if (algoSelect.SelectedIndex == 7 || algoSelect.SelectedIndex == 6)
            {
                RectQuantum.Fill = (Brush)new BrushConverter().ConvertFrom("#FFEFF3F9");
                if (algoSelect.SelectedIndex == 7) OptionText.Text = "Temps de MAJ";
                else OptionText.Text = "Quantum";
                nivQuantum.Text = "5";
                nivQuantum.IsReadOnly = false;
                nivQuantum.Cursor = Cursors.IBeam;
            }
            else
            {
                RectQuantum.Fill = (Brush)new BrushConverter().ConvertFrom("#FFFFFF");
                OptionText.Text = "Option";
                nivQuantum.Text = "/";
                nivQuantum.IsReadOnly = true;
                nivQuantum.Cursor = Cursors.Arrow;
            }
        }

        private void proTitle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (indiceniv == 0)
            {
                nivIndexRectangle.Fill = (Brush)new BrushConverter().ConvertFrom("#FFEEBEBE");
                return;
            }
            proTitle.FontSize = 50;
            nivTitle.FontSize = 30;
            nivAff.Height = 0;
            nivAffText.Visibility = Visibility.Hidden;
            proAff.Height = 120;
            proAffText.Visibility = Visibility.Visible;
            proGrid.Visibility = Visibility.Visible;
            nivGrid.Visibility = Visibility.Hidden;
            SimulationButton.Height = 120;
            if (NbHint == 10) HintSuivant();
        }

        private void nivTitle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            proTitle.FontSize = 30;
            nivTitle.FontSize = 50;
            nivAff.Height = 120;
            nivAffText.Visibility = Visibility.Visible;
            proAff.Height = 0;
            proAffText.Visibility = Visibility.Hidden;
            nivGrid.Visibility = Visibility.Visible;
            proGrid.Visibility = Visibility.Hidden;
            SimulationButton.Height = 0;
        }

        private void GenAddNiv(object sender, MouseButtonEventArgs e)
        {
            if(indiceniv>3)
            {
                return;
            }
            string[] algos = { "PAPS", "PCA", "PLA", "PCTR", "PAR", "PSR", "RR" ,"PARD"};
            Random random = new Random();
            int niv,algo= random.Next(0, 8), q = 0;
            string type=algos[algo];
            niv = indiceniv;
            if (algo == 7 || algo == 6) 
            {
                q= random.Next(1, 6);
                niveaux[indiceniv] = new Niveau(algo, q);
            }
            else niveaux[indiceniv] = new Niveau(algo);
            indiceniv++;
            randNiv.Text = indiceniv.ToString();
            nivId.Text = indiceniv.ToString();
            string quan = q.ToString();
            if (algo != 7 && algo != 6) quan = "/";
            AffichageProcessus pro = new AffichageProcessus
            {
                id = niv,
                Background = type,
                quantum = quan,
            };
            pro.InsererNivML(NiveauGrid, nivId, algoSelect, nivQuantum, ajouterNV);
            if (NbHint == 2) HintSuivant();
        }

        private void DelNiv(object sender, MouseButtonEventArgs e)
        {
            if (indiceniv < 1)
            {
                return;
            }
            indiceniv--;
            for(int i=0;i<ListPro.Count;i++)
            {
                if (ListPro[i].niveau == indiceniv)
                {
                    ListPro.RemoveAt(i);
                    ProcessusGrid.Children.RemoveAt(i);
                    i--;
                }
            }
            randNiv.Text = indiceniv.ToString();
            nivId.Text= indiceniv.ToString();
            NiveauGrid.Children.RemoveAt(indiceniv);
            if (NbHint == 3) HintSuivant();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (ListPro.Count != 0 && indiceniv != 0)
            {
                prog = new MultiNiveau(indiceniv, niveaux);
                foreach (ProcessusNiveau pro in ListPro)
                {
                    prog.Push(pro);
                }
                MainWindow.main.Content = new SimulationPage_MultiLvl(prog, 0);
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
            nivGen.Effect = Effect;
            Description.Effect = Effect;
            nivGrid.Effect = Effect;
            Change.Effect = Effect;
            SimulationButton.Effect = Effect;
            proGen.Effect = Effect;
            proGrid.Effect = Effect;
            InitPage.navigation.Effect = Effect;
            Grey.Visibility = Visibility.Visible;
            InitPage.grey.Visibility = Visibility.Visible;
            Panel.SetZIndex(buttons, 0);
            Panel.SetZIndex(nivGen, 0);
            Panel.SetZIndex(Description, 0);
            Panel.SetZIndex(nivGrid, 0);
            Panel.SetZIndex(Change, 0);
            Panel.SetZIndex(SimulationButton, 0);
            Panel.SetZIndex(proGen, 0);
            Panel.SetZIndex(proGrid, 0);
        }
        public void FinHint()
        {
            NbHint = 0;
            buttons.Effect = null;
            nivGen.Effect = null;
            Description.Effect = null;
            nivGrid.Effect = null;
            Change.Effect = null;
            SimulationButton.Effect = null;
            proGrid.Effect = null;
            proGen.Effect = null;
            InitPage.navigation.Effect = null;
            Grey.Visibility = Visibility.Hidden;
            InitPage.grey.Visibility = Visibility.Hidden;
            Panel.SetZIndex(buttons, 0);
            Panel.SetZIndex(nivGen, 0);
            Panel.SetZIndex(Description, 0);
            Panel.SetZIndex(nivGrid, 0);
            Panel.SetZIndex(Change, 0);
            Panel.SetZIndex(SimulationButton, 0);
            Panel.SetZIndex(proGen, 0);
            Panel.SetZIndex(proGrid, 0);
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
                                "Simulation Multi Niveaux",
                                "Commençons la simulation de l'algorithme Multi Niveaux",
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
                    else hint.Margin = new Thickness(290, 165, 0, 0);
                    Test = new Hint(
                                        "Simulation Multi Niveaux",
                                        "Commençons la simulation de l'Multi Niveaux",
                                        this,
                                        hint
                                    );

                    NextHintCondition = true;
                }
                else if (NbHint == 1)
                {
                    nivGen.Effect = null;
                    Panel.SetZIndex(nivGen, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(435, 390, 0, 0);
                    else hint.Margin = new Thickness(290, 165, 0, 0);
                    Test = new Hint(
                                        "Générer les niveaux",
                                        "Vous pouvez générer les niveaux manuellement en cliquant sur +/-",
                                        this,
                                        hint
                                    );
                    NextHintCondition = true;
                }
                else if (NbHint == 2)
                {
                    nivGen.Effect = null;
                    Panel.SetZIndex(nivGen, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(340, 330, 0, 0);
                    else hint.Margin = new Thickness(350, 300, 0, 0); 
                    Test = new Hint(
                                        "Ajouter un niveau",
                                        "Cliquez pour ajouter un niveau",
                                        this,
                                        hint
                                    );
                    NextHintCondition = false;
                }
                else if (NbHint == 3)
                {
                    nivGen.Effect = null;
                    Panel.SetZIndex(nivGen, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(30, 310, 0, 0);
                    else hint.Margin = new Thickness(20, 520, 0, 0);
                    Test = new Hint(
                                        "Suprimer un niveau",
                                        "Cliquez pour suprimer un niveau",
                                        this,
                                        hint
                                    );
                    NextHintCondition = false;
                }
                
                else if (NbHint == 4)
                {
                    nivGrid.Effect = null;
                    Panel.SetZIndex(nivGrid, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(190, 240, 0, 0);
                    else hint.Margin = new Thickness(550, 300, 0, 0);
                    Test = new Hint(
                                        "Tableau des niveaux",
                                        "Voici le tableau des niveaux générés",
                                        this,
                                        hint
                                    );
                    NextHintCondition = true;
                }
                
                else if (NbHint == 5)
                {
                    nivGrid.Effect = null;
                    Panel.SetZIndex(nivGrid, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(670, 435, 0, 0);
                    else hint.Margin = new Thickness(600, 345, 0, 0);
                    Test = new Hint(
                                        "Ajouter un niveau",
                                        "Entrer les paramètres du niveau à insérer",
                                        this,
                                        hint
                                    );
                    NextHintCondition = true;
                }
                else if (NbHint == 6)
                {
                    nivGrid.Effect = null;
                    Panel.SetZIndex(nivGrid, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(1115, 460, 0, 0);
                    else hint.Margin = new Thickness(600, 345, 0, 0);
                    Test = new Hint(
                                        "Ajouter un niveau",
                                        "Cliquez sur 'Ajouter' pour insérer ce niveau",
                                        this,
                                        hint
                                    );
                    NextHintCondition = false;
                }
                else if (NbHint == 7)
                {
                    nivGrid.Effect = null;
                    Panel.SetZIndex(nivGrid, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(820, 248, 0, 0);
                    else hint.Margin = new Thickness(650, 255, 0, 0);
                    Test = new Hint(
                                        "Modifier un niveau",
                                        "Cliquez sur 'Modifier' pour modifier les paramètres d'un processus",
                                        this,
                                        hint
                                    );
                    NextHintCondition = false;
                }
                else if (NbHint == 8)
                {
                    nivGrid.Effect = null;
                    Panel.SetZIndex(nivGrid, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(670, 435, 0, 0);
                    else hint.Margin = new Thickness(585, 345, 0, 0);
                    Test = new Hint(
                                        "Modifier un niveau",
                                        "Changez les paramètres du niveau",
                                        this,
                                        hint
                                    );
                    NextHintCondition = true;
                }
                else if (NbHint == 9)
                {
                    nivGrid.Effect = null;
                    Panel.SetZIndex(nivGrid, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(1115, 460, 0, 0);
                    else hint.Margin = new Thickness(585, 345, 0, 0);
                    Test = new Hint(
                                        "Modifier un niveau",
                                        "Cliquez sur 'Modifier' pour confirmer votre modification",
                                        this,
                                        hint
                                    );
                    NextHintCondition = false;
                }
                else if (NbHint == 10)
                {
                    Change.Effect = null;
                    Panel.SetZIndex(Change, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(905, 105, 0, 0);
                    else hint.Margin = new Thickness(905, 105, 0, 0);
                    Test = new Hint(
                                        "Gestion des Processus",
                                        "Cliquez sur 'Processus' pour faire les modifications sur les processus",
                                        this,
                                        hint
                                    );
                    NextHintCondition = false;
                }
                else if (NbHint == 11)
                {
                    proGen.Effect = null;
                    Panel.SetZIndex(proGen, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(35, 270, 0, 0);
                    else hint.Margin = new Thickness(25, 230, 0, 0);
                    Test = new Hint(
                                        "Générer les processus",
                                        "Entrez le nombre des processus à générer",
                                        this,
                                        hint
                                    );
                    NextHintCondition = true;
                }
                else if (NbHint == 12)
                {
                    proGen.Effect = null;
                    Panel.SetZIndex(proGen, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(250, 260, 0, 0);
                    else hint.Margin = new Thickness(250, 215, 0, 0);
                    Test = new Hint(
                                        "Générer les processus",
                                        "Vous pouvez générer des interruptions en cochant cette case",
                                        this,
                                        hint
                                    );
                    NextHintCondition = true;
                }
                else if (NbHint == 13)
                {
                    proGen.Effect = null;
                    Panel.SetZIndex(proGen, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(420, 390, 0, 0);
                    else hint.Margin = new Thickness(365, 395, 0, 0);
                    Test = new Hint(
                                        "Générer les processus",
                                        "Cliquez sur le button 'Générer' pour créer les processus",
                                        this,
                                        hint
                                    );
                    NextHintCondition = false;
                }
                else if (NbHint == 14)
                {
                    proGrid.Effect = null;
                    Panel.SetZIndex(proGrid, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(675, 345, 0, 0);
                    else hint.Margin = new Thickness(655, 300, 0, 0);
                    Test = new Hint(
                                        "Tableau des processus",
                                        "Voici le tableau des processus générés",
                                        this,
                                        hint
                                    );
                    NextHintCondition = true;
                }
                else if (NbHint == 15)
                {
                    proGrid.Effect = null;
                    Panel.SetZIndex(proGrid, 1);
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
                else if (NbHint == 16)
                {
                    proGrid.Effect = null;
                    Panel.SetZIndex(proGrid, 1);
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
                else if (NbHint == 17)
                {
                    proGrid.Effect = null;
                    Panel.SetZIndex(proGrid, 1);
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
                else if (NbHint == 18)
                {
                    proGrid.Effect = null;
                    Panel.SetZIndex(proGrid, 1);
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
                else if (NbHint == 19)
                {
                    proGrid.Effect = null;
                    Panel.SetZIndex(proGrid, 1);
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
                else if (NbHint == 20)
                {
                    proGrid.Effect = null;
                    Panel.SetZIndex(proGrid, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(670, 435, 0, 0);
                    else hint.Margin = new Thickness(1115, 460, 0, 0);
                    Test = new Hint(
                                        "Modifier un processus",
                                        "Cliquez sur 'Modifier' pour confirmer votre modification",
                                        this,
                                        hint
                                    );
                    NextHintCondition = false;
                }
                else if (NbHint == 21)
                {
                    proGrid.Effect = null;
                    Panel.SetZIndex(proGrid, 1);
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
                else if (NbHint == 22)
                {
                    proGrid.Effect = null;
                    Panel.SetZIndex(proGrid, 1);
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
                else if (NbHint == 23)
                {
                    proGrid.Effect = null;
                    Panel.SetZIndex(proGrid, 1);
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
                else if (NbHint == 24)
                {
                    proGrid.Effect = null;
                    Panel.SetZIndex(proGrid, 1);
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
                else if (NbHint == 25)
                {
                    SimulationButton.Effect = null;
                    Panel.SetZIndex(SimulationButton, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(360, 505, 0, 0);
                    else hint.Margin = new Thickness(300, 480, 0, 0);
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

        private void NbProcessusTextBoxx_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded) return;
            if (algoSelect.SelectedIndex == 7 || algoSelect.SelectedIndex == 6)
            {
                ((TextBox)sender).Text = "";
            }
        }

        private void randNiv_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (randNiv.Text == "0")
            {
                minusButton.Visibility = Visibility.Hidden;
            }
            else if (randNiv.Text == "4")
            {
                ajouterButton.Visibility = Visibility.Hidden;
                plusButton.Visibility = Visibility.Hidden;
            }
            else
            {
                nivIndexRectangle.Fill = (Brush)new BrushConverter().ConvertFrom("#FFE9F2FE");
                minusButton.Visibility = Visibility.Visible;
                ajouterButton.Visibility = Visibility.Visible;
                plusButton.Visibility = Visibility.Visible;
            }
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
