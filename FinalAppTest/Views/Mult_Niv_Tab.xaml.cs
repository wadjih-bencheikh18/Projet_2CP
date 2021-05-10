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
        public Mult_Niv_Tab()
        {
            InitializeComponent();
            IdTextBox.Text = indice.ToString();
            indice++;
        }

        public static bool modifier = false;
        public static PAPS_TabRow proModifier;
        private int indice = 0;
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
                    pro.Inserer(ProcessusGrid, IdTextBox, TempsArrivTextBox, DureeTextBox, ajouterTB);
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
                id = indice;
                TempsArrivTextBox.Text = "0";
                DureeTextBox.Text = "1";
                IdTextBox.Text = (id + 1).ToString();
                NbProcessusTextBox.Background = (Brush)bc.ConvertFrom("#00000000");
                AffichageProcessus pro = new AffichageProcessus
                {
                    id = id,
                    tempsArriv = tempsArrive,
                    duree = duree
                };
                pro.Inserer(ProcessusGrid, IdTextBox, TempsArrivTextBox, DureeTextBox, ajouterTB);
                indice++;
            }
            else if (valide && modifier)
            {
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
                modifier = false;
                IdTextBox.Text = indice.ToString();
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
    }
}
