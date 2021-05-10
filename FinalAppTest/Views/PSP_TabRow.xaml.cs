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
    /// Interaction logic for PSP_TabRow.xaml
    /// </summary>
    public partial class PSP_TabRow : UserControl
    {
        public PSP_TabRow()
        {
            InitializeComponent();
        }
        private TextBox id;
        private TextBox tempsArriv;
        private TextBox duree;
        private TextBox prio;
        private TextBlock Ajouter;
        private StackPanel Table;
        public PSP_TabRow(TextBox id, TextBox tempsArriv, TextBox duree,TextBox prio, StackPanel Table, TextBlock Ajouter)
        {
            InitializeComponent();
            this.id = id;
            this.tempsArriv = tempsArriv;
            this.duree = duree;
            this.Table = Table;
            this.prio = prio;
            this.Ajouter = Ajouter;
        }
        private void modifier_Button_Click(object sender, RoutedEventArgs e)
        {
            id.Text = idTest.Text;
            tempsArriv.Text = tempsArrTest.Text;
            duree.Text = dureeTest.Text;
            prio.Text = prioTest.Text;
            PSP_Tab.modifier = true;
            PSP_Tab.proModifier = this;
            Ajouter.Text = "Modifer";
        }

        private void suprimer_Button_Click(object sender, RoutedEventArgs e)
        {
            Table.Children.Remove(this);
        }
    }
}
