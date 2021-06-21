using FinalAppTest.Views;
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

namespace FinalAppTest.Comparaison
{
    /// <summary>
    /// Interaction logic for Comp_TabRow.xaml
    /// </summary>
    public partial class Comp_TabRow : UserControl
    {
        public Comp_TabRow()
        {
            InitializeComponent();
            TreeViewItem header = new TreeViewItem();
            header.Header = processusHeader;
            TreeViewParent.Items.Add(header);
        }

        private TextBox id;
        private TextBox tempsArriv;
        private TextBox duree;
        private TextBox prio;
        private TextBlock Ajouter;
        private StackPanel Table;

        public TreeViewItem parent;

        public Comp_TabRow(TextBox id, TextBox tempsArriv, TextBox duree, TextBox prio, StackPanel Table, TextBlock Ajouter)
        {
            InitializeComponent();
            this.id = id;
            this.tempsArriv = tempsArriv;
            this.duree = duree;
            this.Table = Table;
            this.prio = prio;
            this.Ajouter = Ajouter;

            parent = TreeViewParent;

            parent.Items.Add(new Interruption_TabHeader());
            parent.Items.Add(new Interruption_Ajouter(this));
        }
        private void modifier_Button_Click(object sender, RoutedEventArgs e)
        {
            id.Text = idTest.Text;
            tempsArriv.Text = tempsArrTest.Text;
            duree.Text = dureeTest.Text;
            prio.Text = prioTest.Text;
            Compar_Saisie.modifier = true;
            Compar_Saisie.proModifier = this;
            Ajouter.Text = "Modifier";
        }

        private void suprimer_Button_Click(object sender, RoutedEventArgs e)
        {
            Compar_Saisie.listeProc.RemoveAll(p => p.id.ToString().Equals(this.idTest.Text) && p.tempsArriv.ToString().Equals(this.tempsArrTest.Text));
            Table.Children.Remove(this);
        }

        private void TreeViewParent_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewParent.IsSelected = false;
        }

        private void GridScale(object sender, RoutedEventArgs e)
        {
            processusHeader.Width = mainGrid.ActualWidth - 5;
        }

        private void Afficher_Interrup(object sender, MouseEventArgs e)
        {
            TreeViewParent.IsExpanded = (!TreeViewParent.IsExpanded);
        }

        private void TreeViewParent_Expanded(object sender, RoutedEventArgs e)
        {
            // if (PAPS_Tab.NbHint == 12) PAPS_Tab.HintSuivant();
        }
    }
}
