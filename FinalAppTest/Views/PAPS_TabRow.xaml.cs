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
            TreeViewItem header = new TreeViewItem
            {
                Header = processusHeader
            };
            TreeViewParent.Items.Add(header);
        }

        public TextBox id;
        public TextBox tempsArriv;
        public TextBox duree;
        public TextBlock Ajouter;
        public StackPanel Table;
        public TreeViewItem parent;

        public PAPS_TabRow(TextBox id, TextBox tempsArriv, TextBox duree, StackPanel Table, TextBlock Ajouter)
        {
            InitializeComponent();
            this.id = id;
            this.tempsArriv = tempsArriv;
            this.duree = duree;
            this.Table = Table;
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
            PAPS_Tab.modifier = true;
            PAPS_Tab.proModifier = this;
            Ajouter.Text = "Modifier";
            if (PAPS_Tab.NbHint == 9) PAPS_Tab.HintSuivant();
        }

        private void suprimer_Button_Click(object sender, RoutedEventArgs e)
        {
            PAPS_Tab.prog.listeProcessus.RemoveAll(p => p.id.ToString().Equals(this.idTest.Text) && p.tempsArriv.ToString().Equals(this.tempsArrTest.Text));
            Table.Children.Remove(this);
            if (Table.Children.Count == 0)
            {
                PAPS_Tab.ThisPage.IdTextBox.Text = 0.ToString();
                PAPS_Tab.indice = 0;
            }
            if(PAPS_Tab.NbHint==8) PAPS_Tab.HintSuivant();
        }

        private void TreeViewParent_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewParent.IsSelected = false;
        }

        private void GridScale(object sender, RoutedEventArgs e)
        {
            processusHeader.Width = mainGrid.ActualWidth-5;
        }

        private void Afficher_Interrup(object sender, MouseEventArgs e)
        { 
            TreeViewParent.IsExpanded = (!TreeViewParent.IsExpanded);
        }

    }
}

