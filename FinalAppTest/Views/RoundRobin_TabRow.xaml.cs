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
    /// Interaction logic for RoundRobin_TabRow.xaml
    /// </summary>
    public partial class RoundRobin_TabRow : UserControl
    {
        public RoundRobin_TabRow()
        {
            InitializeComponent();
            TreeViewItem header = new TreeViewItem();
            header.Header = processusHeader;
            TreeViewParent.Items.Add(header);
        }

        private TextBox id;
        private TextBox tempsArriv;
        private TextBox duree;
        private TextBlock Ajouter;
        private StackPanel Table;

        public TreeViewItem parent;

        public RoundRobin_TabRow(TextBox id, TextBox tempsArriv, TextBox duree, StackPanel Table, TextBlock Ajouter)
        {
            InitializeComponent();
            this.id = id;
            this.tempsArriv = tempsArriv;
            this.duree = duree;
            this.Table = Table;
            this.Ajouter = Ajouter;

            parent = TreeViewParent;

            parent.Items.Add(new Interruption_Ajouter(this));
        }

        private void modifier_Button_Click(object sender, RoutedEventArgs e)
        {
            id.Text = idTest.Text;
            tempsArriv.Text = tempsArrTest.Text;
            duree.Text = dureeTest.Text;
            RoundRobin_Tab.modifier = true;
            RoundRobin_Tab.proModifier = this;
            Ajouter.Text = "Modifier";
        }

        private void suprimer_Button_Click(object sender, RoutedEventArgs e)
        {
            RoundRobin_Tab.prog.listeProcessus.RemoveAll(p => p.id.ToString().Equals(this.idTest.Text) && p.tempsArriv.ToString().Equals(this.tempsArrTest.Text));
            Table.Children.Remove(this);
        }

        private void TreeViewParent_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewParent.IsSelected = false;
        }
    }
}
