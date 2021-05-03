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

namespace Video2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public class Processus
        {
            public int id{ get; set; }
            public int duree { get; set; }
            public int tempsArriv { get; set; }
            public int priorite { get; set; }
            
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool Valid=true;
            int id, duree, prio, tempsArriv;
            if (!Int32.TryParse(idTB.Text, out id))
            {
                idTB.Background = Brushes.Red;
                Valid=false;
            }
            if (!Int32.TryParse(dureeTB.Text, out duree))
            {
                dureeTB.Background = Brushes.Red;
                Valid = false;
            }
            if (!Int32.TryParse(tempsArrivTB.Text, out tempsArriv))
            {
                tempsArrivTB.Background = Brushes.Red;
                Valid = false;
            }
            if (!Int32.TryParse(prioriteTB.Text, out prio))
            {
                prioriteTB.Background = Brushes.Red;
                Valid = false;
            }
            if (Valid)
            {
                Processus pro = new Processus();
                pro.id = int.Parse(idTB.Text);
                idTB.Background = Brushes.White;
                pro.duree = int.Parse(dureeTB.Text);
                dureeTB.Background = Brushes.White;
                pro.tempsArriv = int.Parse(tempsArrivTB.Text);
                tempsArrivTB.Background = Brushes.White;
                pro.priorite = int.Parse(prioriteTB.Text);
                prioriteTB.Background = Brushes.White;
                InitTab.Items.Add(pro);
            }
        }
    }
}
