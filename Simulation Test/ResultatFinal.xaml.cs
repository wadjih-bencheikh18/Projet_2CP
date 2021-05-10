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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
namespace Simulation_Test
{
    /// <summary>
    /// Interaction logic for ResultatFinal.xaml
    /// </summary>
    public partial class ResultatFinal : Window
    {
        List<Processus> P = new List<Processus>();
        public ResultatFinal(List<Processus> P)
        {
            this.P = P;
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            for(int i=0;i<P.Count;i++)
            {
                P[i].Affichage(Grid1,i);
            }
            
        }
    }
}
