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

namespace FinalAppTest.Views
{
    /// <summary>
    /// Interaction logic for PCA_TabRow.xaml
    /// </summary>
    public partial class PCA_TabRow : UserControl
    {
        public PCA_TabRow()
        {
            InitializeComponent();
        }

        private TextBox id;
        private TextBox tempsArriv;
        private TextBox duree;
        private TextBlock Ajouter;
        private StackPanel Table;

        public PCA_TabRow(TextBox id, TextBox tempsArriv, TextBox duree, StackPanel Table, TextBlock Ajouter)
        {
            InitializeComponent();
            this.id = id;
            this.tempsArriv = tempsArriv;
            this.duree = duree;
            this.Table = Table;
            this.Ajouter = Ajouter;
        }
        private void modifier_Button_Click(object sender, RoutedEventArgs e)
        {
            id.Text = idTest.Text;
            tempsArriv.Text = tempsArrTest.Text;
            duree.Text = dureeTest.Text;
            PCA_Tab.modifier = true;
            PCA_Tab.proModifier = this;
            Ajouter.Text = "Modifier";
        }

        private void suprimer_Button_Click(object sender, RoutedEventArgs e)
        {
            PCA_Tab.prog.listeProcessus.RemoveAll(p => p.id.ToString().Equals(this.id.Text) && p.tempsArriv.ToString().Equals(this.tempsArriv.Text));
            Table.Children.Remove(this);
        }
    }
}
