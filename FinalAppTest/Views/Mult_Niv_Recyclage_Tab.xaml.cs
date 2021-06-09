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

namespace FinalAppTest.Views
{
    /// <summary>
    /// Interaction logic for Mult_Niv_Tab.xaml
    /// </summary>
    public partial class Mult_Niv_Recyclage_Tab : UserControl
    {
        public static MultiNiveauRecyclage prog;

        public Mult_Niv_Recyclage_Tab()
        {
            indicepro = 0;
            indiceniv = 0;
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
        public static Mult_Niv_Recyclage_Tab ThisPage;

        private void RandomButton_Click(object sender, RoutedEventArgs e)  // générer aléatoirement des processus
        {
            var bc = new BrushConverter();
            if (!Int32.TryParse(NbProcessusTextBox.Text, out int NbProcessus) && NbProcessus <= 0)
            {
                RectRand.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
            }
            else
            {
                ProcessusGrid.Children.RemoveRange(0, ProcessusGrid.Children.Count);
                RectRand.Fill = (Brush)bc.ConvertFrom("#FFFFFF");
                Random r = new Random();
                for (int i = 0; i < NbProcessus; i++)
                {
                    AffichageProcessus pro = new AffichageProcessus
                    {
                        id = i,
                        tempsArriv = r.Next(20),
                        duree = r.Next(1, 20),
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
            if (!Int32.TryParse(NivTextBox.Text, out int niv) || niv >= indiceniv || niv < 0 )  // get temps d'arrivé
            {
                valide = false;
                RectProNiv.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
            }

            if (!Int32.TryParse(PrioTextBox.Text, out int prio) || prio < 0 )  // get temps d'arrivé
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
                TempsArrivTextBox.Text = "0";
                DureeTextBox.Text = "1";
                PrioTextBox.Text = "0";
                NivTextBox.Text = "0";
                if (!modifier)
                {
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
                    pro.InsererProcML(ProcessusGrid, IdTextBox, TempsArrivTextBox, DureeTextBox,PrioTextBox,NivTextBox, ajouterTB);
                    ListPro.Add(new ProcessusNiveau(pro.id, pro.tempsArriv, pro.duree, pro.prio, pro.niveau));
                    indicepro++;
                }
                else
                {
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
            if (int.Parse(nivId.Text)>3)
            {
                return;
            }
            bool valide = true;
            string type = algoSelect.Text;
            int niv, algo, q=0;
            var bc = new BrushConverter();
            
            niv = int.Parse(nivId.Text);
            algo = algoSelect.SelectedIndex;

            if ((algo==7 || algo==8) && (!Int32.TryParse(nivQuantum.Text, out q) || q<= 0))  // get durée
            {
                RectQuantum.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                valide = false;
            }
            string quan = q.ToString();
            if (algo != 7 && algo!=8) quan = "/";
            if (valide && !modifier)  // si tous est correcte
            {
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
                if (algo == 7 || algo==8) niveaux[indiceniv] = new Niveau(algo, q);
                else niveaux[indiceniv] = new Niveau(algo);
                indiceniv++;
                randNiv.Text = indiceniv.ToString();
            }
            else if (valide && modifier)
            {
                AffichageProcessus pro = new AffichageProcessus
                {
                    id = niv,
                    Background = type,
                    quantum = quan,
                };
                Mult_Niv_TabRow item = (Mult_Niv_TabRow)NiveauGrid.Children[NiveauGrid.Children.IndexOf(proModifier)];
                item.DataContext = pro;
                if (algo == 7|| algo ==8) niveaux[niv] = new Niveau(algo, q);
                else niveaux[NiveauGrid.Children.IndexOf(proModifier)] = new Niveau(algo);
                NiveauGrid.Children[NiveauGrid.Children.IndexOf(proModifier)] = item;
                modifier = false;
                Mult_Niv_Tab.modifier = false;
                nivId.Text = indiceniv.ToString();
                ajouterNV.Text = "Ajouter";
                if (indiceniv == 4) ajouterButton.Visibility = Visibility.Hidden;
            }

        }

        private void algoSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded) return;
            if (algoSelect.SelectedIndex == 7 || algoSelect.SelectedIndex == 8)
            {
                RectQuantum.Fill = (Brush)new BrushConverter().ConvertFrom("#FFE9F2FE");
                if (algoSelect.SelectedIndex == 8) OptionText.Text = "Temps de MAJ";
                else OptionText.Text = "Quantum";
                nivQuantum.Text = "5";
                nivQuantum.IsReadOnly = false;
                nivQuantum.Cursor = NbProcessusTextBox.Cursor;
            }
            else
            {
                RectQuantum.Fill = (Brush)new BrushConverter().ConvertFrom("#FFFFFF");
                OptionText.Text = "Option";
                nivQuantum.Text = "/";
                nivQuantum.IsReadOnly = true;
                nivQuantum.Cursor = nivId.Cursor;
            }
        }

        private void proTitle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            proTitle.FontSize = 50;
            nivTitle.FontSize = 30;
            nivGen.Height = 0;
            proGen.Height = 120;
            proGrid.Visibility = Visibility.Visible;
            nivGrid.Visibility = Visibility.Hidden;
            SimulationButton.Height = 120;
        }

        private void nivTitle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            proTitle.FontSize = 30;
            nivTitle.FontSize = 50;
            nivGen.Height = 120;
            proGen.Height = 0;
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
            var bc = new BrushConverter();
            string[] algos = { "PAPS", "PCA", "PLA", "PCTR", "PAR", "PSR", "Slack-Time",  "RR", "PARD" };
            Random random = new Random();
            int niv,algo= random.Next(0, 9), q = 0;
            string type=algos[algo];
            niv = indiceniv;
            if (algo == 7|| algo ==8) 
            {
                q= random.Next(1, 6);
                niveaux[indiceniv] = new Niveau(algo, q);
            }
            else niveaux[indiceniv] = new Niveau(algo);
            indiceniv++;
            randNiv.Text = indiceniv.ToString();
            nivId.Text = indiceniv.ToString();
            string quan = q.ToString();
            if (algo != 7 && algo !=8) quan = "/";
            AffichageProcessus pro = new AffichageProcessus
            {
                id = niv,
                Background = type,
                quantum = quan,
            };
            pro.InsererNivML(NiveauGrid, nivId, algoSelect, nivQuantum, ajouterNV);
        }

        private void DelNiv(object sender, MouseButtonEventArgs e)
        {
            if (indiceniv < 1)
            {
                return;
            }
            indiceniv--;
            for (int i = 0; i < ListPro.Count; i++)
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
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (ListPro.Count == 0 || indiceniv == 0)
            {
                var bc = new BrushConverter();
                //StartButton.BorderBrush = (Brush)bc.ConvertFrom("#FFF52C2C");
                StartButton.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
            }
            else
            {
                prog = new MultiNiveauRecyclage(indiceniv, niveaux);
                foreach (ProcessusNiveau pro in ListPro)
                {
                    prog.Push(pro);
                }
                MainWindow.main.Content = new SimulationPage_MultiLvl(prog, 1);
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

        private void NbProcessusTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Text = "";
        }

        private void NbProcessusTextBoxx_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded) return;
            if (algoSelect.SelectedIndex == 7 || algoSelect.SelectedIndex == 8)
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
                plusButton.Visibility = Visibility.Hidden;
                ajouterButton.Visibility = Visibility.Hidden;
            }
            else
            {
                minusButton.Visibility = Visibility.Visible;
                plusButton.Visibility = Visibility.Visible;
                ajouterButton.Visibility = Visibility.Visible;
            }
        }
    }
}
