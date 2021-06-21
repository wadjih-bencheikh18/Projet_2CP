using FinalAppTest.Views;
using Ordonnancement;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FinalAppTest.Comparaison
{
    /// <summary>
    /// Logique d'interaction pour Compar_Saisie.xaml
    /// </summary>
    public partial class Compar_Saisie : Page
    {
        public static int NbHint = 0;
        public static bool NextHintCondition = true;

        public Compar_Saisie()
        {
            indice = 0;
            InitializeComponent();
            IdTextBox.Text = indice.ToString();
            algos = new TextBlock[3] { algo1, algo2, algo3 };
            ThisPage = this;
        }
        public static bool modifier = false;
        public static Comp_TabRow proModifier;
        public static int indice = 0;
        private List<int> comp = new List<int>();
        private TextBlock[] algos;
        private int quantum = 5;
        private int tempsMAJ = 5;
        public static List<Processus> listeProc = new List<Processus>();
        public static Compar_Saisie ThisPage;

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
                listeProc.Clear();  // vider la liste pour l'ecraser
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
                    Comp_TabRow processus = pro.InsererComp(ProcessusGrid, IdTextBox, TempsArrivTextBox, DureeTextBox, PrioTextBox, ajouterTB);  // inserer son ligne dans le tableau des processus
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
                    listeProc.Add(proc);  // added processus to the program
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
                valide = false;
                RectTar.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
            }
            if (!Int32.TryParse(DureeTextBox.Text, out int duree) || duree <= 0)  // get durée
            {
                RectDuree.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                valide = false;
            }
            if (!Int32.TryParse(PrioTextBox.Text, out int prio) || prio < 0)  // get priorité
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
                    pro.InsererComp(ProcessusGrid, IdTextBox, TempsArrivTextBox, DureeTextBox, PrioTextBox, ajouterTB);
                    listeProc.Add(new Processus(pro.id, pro.tempsArriv, pro.duree, pro.prio));  // added to the program
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
                    Comp_TabRow item = (Comp_TabRow)ProcessusGrid.Children[ProcessusGrid.Children.IndexOf(proModifier)];
                    item.DataContext = pro;
                    ProcessusGrid.Children[ProcessusGrid.Children.IndexOf(proModifier)] = item;
                    listeProc[ProcessusGrid.Children.IndexOf(proModifier)] = new Processus(pro.id, pro.tempsArriv, pro.duree, pro.prio);  // modifier le processus correspondant
                    modifier = false;
                    IdTextBox.Text = indice.ToString();
                    ajouterTB.Text = "Ajouter";
                }
            }
        }

        private void Comparer_Click(object sender, MouseEventArgs e)
        {
            if (comp.Count > 1 && NbHint == 0)
                MainWindow.main.Content = new Comparaison_Page(comp, quantum, tempsMAJ);
            else FinHint();
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

        private void ReturnButton_Click(object sender, MouseButtonEventArgs e)
        {
            MainWindow.main.Content = new WelcomePage();
        }

        private void PapsBtn_Click(object sender, MouseButtonEventArgs e)
        {
            var bc = new BrushConverter();

            if (comp.IndexOf(0) < 0 && comp.Count < 3)
            {
                comp.Add(0);
                algos[comp.Count - 1].Text = "- PAPS";
                PapsBtn.Fill = (Brush)bc.ConvertFrom("#FD5825");
            }
            else if (comp.IndexOf(0) >= 0)
            {
                if (comp.IndexOf(0) == comp.Count - 1) algos[comp.IndexOf(0)].Text = "- None";
                else
                {
                    for (int i = comp.IndexOf(0); i < comp.Count - 1; i++)
                    {
                        algos[i].Text = algos[i + 1].Text;
                    }
                    algos[comp.Count - 1].Text = "- None";
                }
                comp.Remove(0);
                PapsBtn.Fill = (Brush)bc.ConvertFrom("#FF000000");
            }
            if (comp.Count >= 2 && NbHint == 1)
            {
                NextHintCondition = true;
            }
        }

        private void PcaBtn_Click(object sender, MouseButtonEventArgs e)
        {
            var bc = new BrushConverter();

            if (comp.IndexOf(1) < 0 && comp.Count < 3)
            {
                comp.Add(1);
                algos[comp.Count - 1].Text = "- PCA";
                PcaBtn.Fill = (Brush)bc.ConvertFrom("#FD5825");
            }
            else if (comp.IndexOf(1) >= 0)
            {
                if (comp.IndexOf(1) == comp.Count - 1) algos[comp.IndexOf(1)].Text = "- None";
                else
                {
                    for (int i = comp.IndexOf(1); i < comp.Count - 1; i++)
                    {
                        algos[i].Text = algos[i + 1].Text;
                    }
                    algos[comp.Count - 1].Text = "- None";
                }
                comp.Remove(1);
                PcaBtn.Fill = (Brush)bc.ConvertFrom("#FF000000");
            }
            if (comp.Count >= 2 && NbHint == 1)
            {
                NextHintCondition = true;
            }
        }

        private void ParBtn_Click(object sender, MouseButtonEventArgs e)
        {
            var bc = new BrushConverter();

            if (comp.IndexOf(4) < 0 && comp.Count < 3)
            {
                comp.Add(4);
                algos[comp.Count - 1].Text = "- PAR";
                ParBtn.Fill = (Brush)bc.ConvertFrom("#FD5825");
            }
            else if (comp.IndexOf(4) >= 0)
            {
                if (comp.IndexOf(4) == comp.Count - 1) algos[comp.IndexOf(4)].Text = "- None";
                else
                {
                    for (int i = comp.IndexOf(4); i < comp.Count - 1; i++)
                    {
                        algos[i].Text = algos[i + 1].Text;
                    }
                    algos[comp.Count - 1].Text = "- None";
                }
                comp.Remove(4);
                ParBtn.Fill = (Brush)bc.ConvertFrom("#FF000000");
            }
            if (comp.Count >= 2 && NbHint == 1)
            {
                NextHintCondition = true;
            }
        }

        private void SlackBtn_Click(object sender, MouseButtonEventArgs e)
        {
            var bc = new BrushConverter();

            if (comp.IndexOf(6) < 0 && comp.Count < 3)
            {
                comp.Add(6);
                algos[comp.Count - 1].Text = "- Slack Time";
                SlackBtn.Fill = (Brush)bc.ConvertFrom("#FD5825");
            }
            else if (comp.IndexOf(6) >= 0)
            {
                if (comp.IndexOf(6) == comp.Count - 1) algos[comp.IndexOf(6)].Text = "- None";
                else
                {
                    for (int i = comp.IndexOf(6); i < comp.Count - 1; i++)
                    {
                        algos[i].Text = algos[i + 1].Text;
                    }
                    algos[comp.Count - 1].Text = "- None";
                }
                comp.Remove(6);
                SlackBtn.Fill = (Brush)bc.ConvertFrom("#FF000000");
            }
            if (comp.Count >= 2 && NbHint == 1)
            {
                NextHintCondition = true;
            }
        }

        private void PlaBtn_Click(object sender, MouseButtonEventArgs e)
        {
            var bc = new BrushConverter();

            if (comp.IndexOf(2) < 0 && comp.Count < 3)
            {
                comp.Add(2);
                algos[comp.Count - 1].Text = "- PLA";
                PlaBtn.Fill = (Brush)bc.ConvertFrom("#FD5825");
            }
            else if (comp.IndexOf(2) >= 0)
            {
                if (comp.IndexOf(2) == comp.Count - 1) algos[comp.IndexOf(2)].Text = "- None";
                else
                {
                    for (int i = comp.IndexOf(2); i < comp.Count - 1; i++)
                    {
                        algos[i].Text = algos[i + 1].Text;
                    }
                    algos[comp.Count - 1].Text = "- None";
                }
                comp.Remove(2);
                PlaBtn.Fill = (Brush)bc.ConvertFrom("#FF000000");
            }
            if (comp.Count >= 2 && NbHint == 1)
            {
                NextHintCondition = true;
            }
        }

        private void PctrBtn_Click(object sender, MouseButtonEventArgs e)
        {
            var bc = new BrushConverter();

            if (comp.IndexOf(3) < 0 && comp.Count < 3)
            {
                comp.Add(3);
                algos[comp.Count - 1].Text = "- PCTR";
                PctrBtn.Fill = (Brush)bc.ConvertFrom("#FD5825");
            }
            else if (comp.IndexOf(3) >= 0)
            {
                if (comp.IndexOf(3) == comp.Count - 1) algos[comp.IndexOf(3)].Text = "- None";
                else
                {
                    for (int i = comp.IndexOf(3); i < comp.Count - 1; i++)
                    {
                        algos[i].Text = algos[i + 1].Text;
                    }
                    algos[comp.Count - 1].Text = "- None";
                }
                comp.Remove(3);
                PctrBtn.Fill = (Brush)bc.ConvertFrom("#FF000000");
            }
            if (comp.Count >= 2 && NbHint == 1)
            {
                NextHintCondition = true;
            }
        }

        private void PsrBtn_Click(object sender, MouseButtonEventArgs e)
        {
            var bc = new BrushConverter();

            if (comp.IndexOf(5) < 0 && comp.Count < 3)
            {
                comp.Add(5);
                algos[comp.Count - 1].Text = "- PSR";
                PsrBtn.Fill = (Brush)bc.ConvertFrom("#FD5825");
            }
            else if (comp.IndexOf(5) >= 0)
            {
                if (comp.IndexOf(5) == comp.Count - 1) algos[comp.IndexOf(5)].Text = "- None";
                else
                {
                    for (int i = comp.IndexOf(5); i < comp.Count - 1; i++)
                    {
                        algos[i].Text = algos[i + 1].Text;
                    }
                    algos[comp.Count - 1].Text = "- None";
                }
                comp.Remove(5);
                PsrBtn.Fill = (Brush)bc.ConvertFrom("#FF000000");
            }
            if (comp.Count >= 2 && NbHint == 1)
            {
                NextHintCondition = true;
            }
        }

        private void RRBtn_Click(object sender, MouseButtonEventArgs e)
        {
            var bc = new BrushConverter();

            if (comp.IndexOf(7) < 0 && comp.Count < 3)
            {
                comp.Add(7);
                algos[comp.Count - 1].Text = "- RR";
                RRBtn.Fill = (Brush)bc.ConvertFrom("#FD5825");
                RectQuantum.Fill = (Brush)bc.ConvertFrom("#FFFFFF");
                QuantumTxt.Cursor = Cursors.IBeam;
                QuantumTxt.IsReadOnly = false;
            }
            else if (comp.IndexOf(7) >= 0)
            {
                if (comp.IndexOf(7) == comp.Count - 1) algos[comp.IndexOf(7)].Text = "- None";
                else
                {
                    for (int i = comp.IndexOf(7); i < comp.Count - 1; i++)
                    {
                        algos[i].Text = algos[i + 1].Text;
                    }
                    algos[comp.Count - 1].Text = "- None";
                }
                comp.Remove(7);
                RRBtn.Fill = (Brush)bc.ConvertFrom("#FF000000");
                RectQuantum.Fill = (Brush)bc.ConvertFrom("#FFE9F2FE");
                QuantumTxt.Cursor = Cursors.Arrow;
                QuantumTxt.IsReadOnly = true;
            }
            if (comp.Count >= 2 && NbHint == 1)
            {
                NextHintCondition = true;
            }
        }
        private void PARDBtn_Click(object sender, MouseButtonEventArgs e)
        {
            var bc = new BrushConverter();

            if (comp.IndexOf(8) < 0 && comp.Count < 3)
            {
                comp.Add(8);
                algos[comp.Count - 1].Text = "- PARD";
                PARDBtn.Fill = (Brush)bc.ConvertFrom("#FD5825");
                RectTempsMAJ.Fill = (Brush)bc.ConvertFrom("#FFFFFF");
                TempsMAJtxt.Cursor = Cursors.IBeam;
                TempsMAJtxt.IsReadOnly = false;
            }
            else if (comp.IndexOf(8) >= 0)
            {
                if (comp.IndexOf(8) == comp.Count - 1) algos[comp.IndexOf(8)].Text = "- None";
                else
                {
                    for (int i = comp.IndexOf(8); i < comp.Count - 1; i++)
                    {
                        algos[i].Text = algos[i + 1].Text;
                    }
                    algos[comp.Count - 1].Text = "- None";
                }
                comp.Remove(8);
                PARDBtn.Fill = (Brush)bc.ConvertFrom("#FF000000");
                RectTempsMAJ.Fill = (Brush)bc.ConvertFrom("#FFE9F2FE");
                TempsMAJtxt.Cursor = Cursors.Arrow;
                TempsMAJtxt.IsReadOnly = true;
            }
            if (comp.Count >= 2 && NbHint == 1)
            {
                NextHintCondition = true;
            }
        }

        private void QuantumTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            if (!int.TryParse(QuantumTxt.Text, out int i) || i <= 0)
            {
                quantum = -1;
                RectQuantum.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
            }
            else
            {
                quantum = i;
                RectQuantum.Fill = (Brush)bc.ConvertFrom("#FFFFFFFF");
            }
        }

        private void TempsMAJtxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            if (!int.TryParse(TempsMAJtxt.Text, out int i) || i <= 0)
            {
                tempsMAJ = -1;
                RectTempsMAJ.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
            }
            else
            {
                tempsMAJ = i;
                RectTempsMAJ.Fill = (Brush)bc.ConvertFrom("#FFFFFFFF");
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
            if (QuantumTxt.IsReadOnly) return;
            ((TextBox)sender).Text = "";
        }

        private void NbProcessusTextBoxxx_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TempsMAJtxt.IsReadOnly) return;
            ((TextBox)sender).Text = "";
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
            AlgoSide.Effect = Effect;
            Description.Effect = Effect;
            Random.Effect = Effect;
            Tableau.Effect = Effect;
            Simuler.Effect = Effect;
            Grey.Visibility = Visibility.Visible;
            Panel.SetZIndex(buttons, 0);
            Panel.SetZIndex(AlgoSide, 0);
            Panel.SetZIndex(Description, 0);
            Panel.SetZIndex(Random, 0);
            Panel.SetZIndex(Tableau, 0);
            Panel.SetZIndex(Simuler, 0);
        }

        public void FinHint()
        {
            NbHint = 0;
            buttons.Effect = null;
            AlgoSide.Effect = null;
            Description.Effect = null;
            Random.Effect = null;
            Tableau.Effect = null;
            Simuler.Effect = null;
            Grey.Visibility = Visibility.Hidden;
            Panel.SetZIndex(buttons, 0);
            Panel.SetZIndex(AlgoSide, 0);
            Panel.SetZIndex(Description, 0);
            Panel.SetZIndex(Random, 0);
            Panel.SetZIndex(Tableau, 0);
            Panel.SetZIndex(Simuler, 0);
            hint.Child = null;
        }

        private void Hint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NbHint = 0;
            ApplyEffect();
            Description.Effect = null;
            Panel.SetZIndex(Description, 1);
            if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(300, 180, 0, 0);
            else hint.Margin = new Thickness(344, 172, 0, 0);
            Hint Test = new Hint(
                                "Comparaison des algorithmes",
                                "On va faire une comparaison entre les différents algorithmes",
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
                    else hint.Margin = new Thickness(344, 172, 0, 0);
                    Test = new Hint(
                                        "Comparaison des algorithmes",
                                        "On va faire une comparaison entre les différents algorithmes",
                                        this,
                                        hint
                                    );

                    NextHintCondition = true;
                }
                else if (NbHint == 1)
                {
                    AlgoSide.Effect = null;
                    Panel.SetZIndex(AlgoSide, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(315, 175, 0, 0);
                    else hint.Margin = new Thickness(198, 89, 0, 0);
                    Test = new Hint(
                                        "Choisir les algorithmes",
                                        "Sélectionnez 2 ou 3 algorithmes que vous voulez comparer entre eux",
                                        this,
                                        hint
                                    );

                    NextHintCondition = false;
                }
                else if (NbHint == 2)
                {
                    Random.Effect = null;
                    Panel.SetZIndex(Random, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(420, 390, 0, 0);
                    else hint.Margin = new Thickness(548, 274, 0, 0);
                    Test = new Hint(
                                        "Générer les processus + Présiser les paramétres",
                                        "Vous pouvez générer les processus aléatoirement et présiser les paramétres de quelques algorithmes ici",
                                        this,
                                        hint
                                    );
                    NextHintCondition = true;
                }
                else if (NbHint == 3)
                {
                    Random.Effect = null;
                    Panel.SetZIndex(Random, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(35, 240, 0, 0);
                    else hint.Margin = new Thickness(0, 274, 0, 0);
                    Test = new Hint(
                                        "Générer les processus",
                                        "Entrez le nombre des processus à générer",
                                        this,
                                        hint
                                    );
                    NextHintCondition = true;
                }
                else if (NbHint == 4)
                {
                    Random.Effect = null;
                    Panel.SetZIndex(Random, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(250, 230, 0, 0);
                    else hint.Margin = new Thickness(532, 158, 0, 0);
                    Test = new Hint(
                                        "Générer les processus",
                                        "Vous pouvez générer des interruptions pour les processus en cochant cette case",
                                        this,
                                        hint
                                    );
                    NextHintCondition = true;
                }
                else if (NbHint == 5)
                {
                    Random.Effect = null;
                    Panel.SetZIndex(Random, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(420, 390, 0, 0);
                    else hint.Margin = new Thickness(360, 384, 0, 0);
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
                    Random.Effect = null;
                    Panel.SetZIndex(Random, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(420, 390, 0, 0);
                    else hint.Margin = new Thickness(527, 407, 0, 0);
                    Test = new Hint(
                                        "Préciser les paramétres",
                                        "Si vous avez choisi RR ou PARD, vous pouvez modifier ses paramétres",
                                        this,
                                        hint
                                    );
                    NextHintCondition = true;
                }
                else if (NbHint == 7)
                {
                    Tableau.Effect = null;
                    Panel.SetZIndex(Tableau, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(675, 345, 0, 0);
                    else hint.Margin = new Thickness(366, 244, 0, 0);
                    Test = new Hint(
                                        "Tableau des processus",
                                        "Voici le tableau des processus générés, il a les mêmes fonctionalités que dans la page de simulation",
                                        this,
                                        hint
                                    );
                    NextHintCondition = true;
                }
                else if (NbHint == 8)
                {
                    Simuler.Effect = null;
                    Panel.SetZIndex(Simuler, 1);
                    if (MainWindow.PageWidth() > 1500) hint.Margin = new Thickness(360, 505, 0, 0);
                    else hint.Margin = new Thickness(505, 518, 0, -111);
                    Test = new Hint(
                                        "Comparaison",
                                        "Cliquez sur le button 'Comparer' pour voir les résultats de la comparaison",
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
    }
}
