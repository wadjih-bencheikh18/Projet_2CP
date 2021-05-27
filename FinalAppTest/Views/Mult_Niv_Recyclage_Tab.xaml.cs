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
            InitializeComponent();
            IdTextBox.Text = indicepro.ToString();
            randNiv.Text = indiceniv.ToString();
            nivId.Text = indiceniv.ToString();
        }
        public static List<ProcessusNiveau> ListPro = new List<ProcessusNiveau>();
        public static Niveau[] niveaux = new Niveau[4];
        public static bool modifier = false;
        public static UserControl proModifier;
        public static int indiceniv = 0; 
        private int indicepro = 0;
        private void RandomButton_Click(object sender, RoutedEventArgs e)  // générer aléatoirement des processus
        {
            
            int NbProcessus;
            var bc = new BrushConverter();
            if (!Int32.TryParse(NbProcessusTextBox.Text, out NbProcessus) && NbProcessus <= 0)
            {
                NbProcessusTextBox.BorderBrush = (Brush)bc.ConvertFrom("#FFF52C2C");
                NbProcessusTextBox.Background = (Brush)bc.ConvertFrom("#FFEEBEBE");
            }
            else
            {
                NbProcessusTextBox.Text = "";
                ProcessusGrid.Children.RemoveRange(0, ProcessusGrid.Children.Count);
                NbProcessusTextBox.Background = (Brush)bc.ConvertFrom("#00000000");
                Random r = new Random();
                for (int i = 0; i < NbProcessus; i++)
                {
                    AffichageProcessus pro = new AffichageProcessus();
                    pro.id = i;
                    pro.tempsArriv = r.Next(20);
                    pro.duree = r.Next(1, 20);
                    pro.prio = r.Next(0, 6);
                    pro.niveau = r.Next(0, indiceniv);
                    pro.InsererProcML(ProcessusGrid, IdTextBox, TempsArrivTextBox, DureeTextBox, PrioTextBox, NivTextBox, ajouterTB);
                    ListPro.Add(new ProcessusNiveau(pro.id, pro.tempsArriv, pro.duree, pro.prio, pro.niveau));
                }
                IdTextBox.Text = NbProcessus.ToString();
                indicepro = NbProcessus;
            }
        }

        private void AddProcessusButton_Click(object sender, RoutedEventArgs e)  // ajouter un processus
        {
            bool valide = true;
            int id, tempsArrive, duree,niv,prio;
            var bc = new BrushConverter();
            if (!Int32.TryParse(TempsArrivTextBox.Text, out tempsArrive) || tempsArrive < 0)  // get temps d'arrivé
            {
                TempsArrivTextBox.BorderBrush = (Brush)bc.ConvertFrom("#FFF52C2C");
                TempsArrivTextBox.Background = (Brush)bc.ConvertFrom("#FFEEBEBE");
                valide = false;
            }
            if (!Int32.TryParse(NivTextBox.Text, out niv) || niv > indiceniv || niv < 0 )  // get temps d'arrivé
            {
                NivTextBox.Background = (Brush)bc.ConvertFrom("#FFEEBEBE");
                valide = false;
            }

            if (!Int32.TryParse(PrioTextBox.Text, out prio) || prio < 0 )  // get temps d'arrivé
            {
                PrioTextBox.BorderBrush = (Brush)bc.ConvertFrom("#FFF52C2C");
                PrioTextBox.Background = (Brush)bc.ConvertFrom("#FFEEBEBE");
                valide = false;
            }

            if (!Int32.TryParse(DureeTextBox.Text, out duree) || duree <= 0)  // get durée
            {
                DureeTextBox.BorderBrush = (Brush)bc.ConvertFrom("#FFF52C2C");
                DureeTextBox.Background = (Brush)bc.ConvertFrom("#FFEEBEBE");
                valide = false;
            }
            if (valide)  // si tous est correcte
            {

                if (!modifier)
                {
                    id = indicepro;
                    TempsArrivTextBox.Text = "0";
                    DureeTextBox.Text = "1";
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
                    AffichageProcessus pro = new AffichageProcessus
                    {
                        id = int.Parse(IdTextBox.Text),
                        tempsArriv = tempsArrive,
                        duree = duree,
                        prio = prio,
                        niveau = niv,
                    };
                    Multi_Niv_TabRow_Proc item = (Multi_Niv_TabRow_Proc)ProcessusGrid.Children[ProcessusGrid.Children.IndexOf(proModifier)];
                    item.DataContext = pro;
                    ListPro[ProcessusGrid.Children.IndexOf(proModifier)] = new ProcessusNiveau(int.Parse(IdTextBox.Text), tempsArrive, duree, prio, niv);
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

            if (algo==7 && (!Int32.TryParse(nivQuantum.Text, out q) || q<= 0))  // get durée
            {
                RectQuantum.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                valide = false;
            }
            string quan = q.ToString();
            if (algo != 7) quan = "/";
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
                if (algo == 7) niveaux[indiceniv] = new Niveau(algo, q);
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
                if (algo == 7) niveaux[NiveauGrid.Children.IndexOf(proModifier)] = new Niveau(algo, q);
                else niveaux[NiveauGrid.Children.IndexOf(proModifier)] = new Niveau(algo);
                NiveauGrid.Children[NiveauGrid.Children.IndexOf(proModifier)] = item;
                modifier = false;
                nivId.Text = indiceniv.ToString();
                ajouterNV.Text = "Ajouter";
            }

        }

        private void algoSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void proTitle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            proTitle.FontSize = 50;
            nivTitle.FontSize = 30;
            nivGen.Height = 0;
            proGen.Height = 120;
            proGrid.Visibility = Visibility.Visible;
            nivGrid.Visibility = Visibility.Hidden;

        }

        private void nivTitle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            proTitle.FontSize = 30;
            nivTitle.FontSize = 50;
            nivGen.Height = 120;
            proGen.Height = 0;
            nivGrid.Visibility = Visibility.Visible;
            proGrid.Visibility = Visibility.Hidden;
        }

        private void GenAddNiv(object sender, MouseButtonEventArgs e)
        {
            if(indiceniv>3)
            {
                return;
            }
            var bc = new BrushConverter();
            string[] algos = { "PAPS", "PCA", "PLA", "PCTR", "PSP", "PRIO", "PSPDynamique",  "Round-Robin" };
            Random random = new Random();
            int niv,algo= random.Next(0, 8), q = 0;
            string type=algos[algo];
            niv = indiceniv;
            if (algo == 7) 
            {
                q= random.Next(1, 6);
                niveaux[indiceniv] = new Niveau(algo, q);
            }
            else niveaux[indiceniv] = new Niveau(algo);
            indiceniv++;
            randNiv.Text = indiceniv.ToString();
            nivId.Text = indiceniv.ToString();
            string quan = q.ToString();
            if (algo != 7) quan = "/";
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
            randNiv.Text = indiceniv.ToString();
            nivId.Text= indiceniv.ToString();
            NiveauGrid.Children.RemoveAt(indiceniv);
        }
    }
}
