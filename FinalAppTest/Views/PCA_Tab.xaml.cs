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
    /// Interaction logic for PCA_Tab.xaml
    /// </summary>
    public partial class PCA_Tab : UserControl
    {
        public PCA_Tab()
        {
            InitializeComponent();
        }

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
                // ProcessusGrid.Children.RemoveRange(4, ProcessusGrid.Children.Count);
                NbProcessusTextBox.Background = (Brush)bc.ConvertFrom("#FFFFFFFF");
                Random r = new Random();
                for (int i = 0; i < NbProcessus; i++)
                {
                    AffichageProcessus pro = new AffichageProcessus();
                    pro.id = ProcessusGrid.Children.Count;
                    pro.tempsArriv = r.Next(20);
                    pro.duree = r.Next(1, 20);
                    pro.Inserer(ProcessusGrid, ProcessusGrid.Children.Count);
                }
            }
            IdTextBox.Text = (ProcessusGrid.Children.Count).ToString();
            NbProcessusTextBox.Text = "0";
        }

        private void AddProcessusButton_Click(object sender, RoutedEventArgs e)  // ajouter un processus
        {
            bool valide = true;
            int id, tempsArrive, duree;
            var bc = new BrushConverter();
            Int32.TryParse(IdTextBox.Text, out id);  // get id
            if (!Int32.TryParse(TempsArrivTextBox.Text, out tempsArrive) && tempsArrive <= 0)  // get temps d'arrivé
            {
                TempsArrivTextBox.BorderBrush = (Brush)bc.ConvertFrom("#FFF52C2C");
                TempsArrivTextBox.Background = (Brush)bc.ConvertFrom("#FFEEBEBE");
                valide = false;
            }
            if (!Int32.TryParse(DureeTextBox.Text, out duree) && duree <= 0)  // get durée
            {
                DureeTextBox.BorderBrush = (Brush)bc.ConvertFrom("#FFF52C2C");
                DureeTextBox.Background = (Brush)bc.ConvertFrom("#FFEEBEBE");
                valide = false;
            }
            if (valide)  // si tous est correcte
            {
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
                pro.Inserer(ProcessusGrid, id);
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
