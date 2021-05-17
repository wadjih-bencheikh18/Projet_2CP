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
    public partial class Mult_Niv_Tab : UserControl
    {
        public static MultiNiveau prog;
        public Mult_Niv_Tab()
        {
            InitializeComponent();
            IdTextBox.Text = indicepro.ToString();
            randNiv.Text = indiceniv.ToString();
            nivId.Text = indiceniv.ToString();
        }
        public Niveau[] niveaux = new Niveau[4];
        public static bool modifier = false;
        public static UserControl proModifier;
        private int indiceniv = 0; 
        private int indicepro = 0;
        private void RandomButton_Click(object sender, RoutedEventArgs e)  // générer aléatoirement des processus
        {
            Niveau[] niveaux = { new Niveau(3,2), new Niveau(3,4), new Niveau(0), new Niveau(2) };
            prog = new MultiNiveau(4, niveaux);
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
                    pro.niveau = r.Next(0, 4);
                    pro.Inserer(ProcessusGrid, IdTextBox, TempsArrivTextBox, DureeTextBox, ajouterTB);
                    prog.Push(new ProcessusNiveau(pro.id, pro.tempsArriv, pro.duree, pro.prio, pro.niveau));
                }
                IdTextBox.Text = NbProcessus.ToString();
                indicepro = NbProcessus;
            }
        }

        private void AddProcessusButton_Click(object sender, RoutedEventArgs e)  // ajouter un processus
        {
            bool valide = true;
            int id, tempsArrive, duree=0,prio=0,niveau=0;
            var bc = new BrushConverter();
            if (!Int32.TryParse(TempsArrivTextBox.Text, out tempsArrive) || tempsArrive < 0)  // get temps d'arrivé
            {
                TempsArrivTextBox.BorderBrush = (Brush)bc.ConvertFrom("#FFF52C2C");
                TempsArrivTextBox.Background = (Brush)bc.ConvertFrom("#FFEEBEBE");
                valide = false;
            }
            if (!Int32.TryParse(DureeTextBox.Text, out duree) || duree <= 0)  // get durée
            {
                DureeTextBox.BorderBrush = (Brush)bc.ConvertFrom("#FFF52C2C");
                DureeTextBox.Background = (Brush)bc.ConvertFrom("#FFEEBEBE");
                valide = false;
            }
            
            if (valide && !modifier)  // si tous est correcte
            {
                id = indiceniv;
                TempsArrivTextBox.Text = "0";
                DureeTextBox.Text = "1";
                IdTextBox.Text = (id + 1).ToString();
                NbProcessusTextBox.Background = (Brush)bc.ConvertFrom("#00000000");
                AffichageProcessus pro = new AffichageProcessus
                {
                    id = id,
                    tempsArriv = tempsArrive,
                    duree = duree,
                    prio = prio,
                    niveau = niveau,
                };
                pro.Inserer(ProcessusGrid, IdTextBox, TempsArrivTextBox, DureeTextBox, PrioTextBox, NivTextBox, ajouterTB);
                indicepro++;
            }
            else if (valide && modifier)
            {
                AffichageProcessus pro = new AffichageProcessus
                {
                    id = int.Parse(IdTextBox.Text),
                    tempsArriv = tempsArrive,
                    duree = duree,
                    prio = prio,
                    niveau = niveau,
                };
                Multi_Niv_TabRow_Proc item = (Multi_Niv_TabRow_Proc)ProcessusGrid.Children[ProcessusGrid.Children.IndexOf(proModifier)];
                item.DataContext = pro;
                ProcessusGrid.Children[ProcessusGrid.Children.IndexOf(proModifier)] = item;
                modifier = false;
                IdTextBox.Text = indicepro.ToString();
                ajouterTB.Text = "Ajouter";
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
            if (indiceniv > 3)
            {
                return;
            }
            bool valide = true;
            string type = algoSelect.Text;
            int niv, algo, quantum=0;
            var bc = new BrushConverter();
            
            niv = int.Parse(nivId.Text);
            algo = algoSelect.SelectedIndex;

            if (algo==3 && (!Int32.TryParse(nivQuantum.Text, out quantum) || quantum <= 0))  // get durée
            {
                RectQuantum.Fill = (Brush)bc.ConvertFrom("#FFEEBEBE");
                valide = false;
            }
            if (valide && !modifier)  // si tous est correcte
            {
                niv = indiceniv;
                nivId.Text = (niv + 1).ToString();
                NbProcessusTextBox.Background = (Brush)bc.ConvertFrom("#00000000");
                AffichageProcessus pro = new AffichageProcessus
                {
                    id = niv,
                    Background = type,
                    duree = quantum
                };
                pro.Inserer(NiveauGrid, nivId, algoSelect, nivQuantum, ajouterNV);
                if (algo == 3) niveaux[indiceniv] = new Niveau(algo, quantum);
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
                    duree = quantum,
                };
                Mult_Niv_TabRow item = (Mult_Niv_TabRow)NiveauGrid.Children[NiveauGrid.Children.IndexOf(proModifier)];
                item.DataContext = pro;
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
            string[] algos = { "PAPS", "PCA", "PSP", "Round-Robin" };
            Random random = new Random();
            int niv,algo= random.Next(0, 4), quantum = 0;
            string type=algos[algo];
            niv = indiceniv;
            if (algo == 3) 
            {
                quantum = random.Next(1, 6);
                niveaux[indiceniv] = new Niveau(algo, quantum);
            }
            else niveaux[indiceniv] = new Niveau(algo);
            indiceniv++;
            randNiv.Text = indiceniv.ToString();
            nivId.Text = indiceniv.ToString();
            AffichageProcessus pro = new AffichageProcessus
            {
                id = niv,
                Background = type,
                duree = quantum,
            };
            pro.Inserer(NiveauGrid, nivId, algoSelect, nivQuantum, ajouterNV);
        }

        private void DelNiv(object sender, MouseButtonEventArgs e)
        {
            if (indiceniv < 1)
            {
                return;
            }
            indiceniv--;
            randNiv.Text = indiceniv.ToString();
            NiveauGrid.Children.RemoveAt(NiveauGrid.Children.Count - 1);
        }
    }
}
