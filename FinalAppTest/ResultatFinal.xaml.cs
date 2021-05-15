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

namespace FinalAppTest
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
            for (int i = 0; i < P.Count; i++)
            {
                P[i].Affichage(Grid1, i);
            }
        }

        private void Terminer_Click(object sender, MouseButtonEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        private void TerminerButton_MouseEnter(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            TerminerButton.Fill = (Brush)bc.ConvertFrom("#FF575757");
        }

        private void TerminerButton_MouseLeave(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            TerminerButton.Fill = (Brush)bc.ConvertFrom("#5829AA");
        }
    }
}
