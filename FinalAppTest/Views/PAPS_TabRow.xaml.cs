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
    /// Interaction logic for PAPS_TabRow.xaml
    /// </summary>
    public partial class PAPS_TabRow : UserControl
    {
        public PAPS_TabRow()
        {
            InitializeComponent();
        }
        private TextBox id;
        private TextBox tempsArriv;
        private TextBox duree;
        private Button Ajouter;
        private Grid Table;
        public PAPS_TabRow(TextBox id,TextBox tempsArriv,TextBox duree,Grid Table, Button Ajouter)
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
            PAPS_Tab.modifier = true;
            PAPS_Tab.proModifier = this;
            Ajouter.Content = "Modifer";
        }

        private void suprimer_Button_Click(object sender, RoutedEventArgs e)
        {
            Table.Children.Remove(this);
        }
    }
}
