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

        public PCA_TabRow(TextBox id, TextBox tempsArriv, TextBox duree, StackPanel Table, TextBlock Ajouter)
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
            PCA_Tab.modifier = true;
            PCA_Tab.proModifier = this;
            Ajouter.Text = "Modifier";
        }

        private void suprimer_Button_Click(object sender, RoutedEventArgs e)
        {
            if (PCA_Tab.modifier)
            {
                PCA_Tab.modifier = false;
                Ajouter.Text = "Ajouter";
                PCA_Tab.FixIndice();
            }
            PCA_Tab.prog.listeProcessus.RemoveAll(p => p.id.ToString().Equals(this.idTest.Text) && p.tempsArriv.ToString().Equals(this.tempsArrTest.Text));
            Table.Children.Remove(this);
            if (Table.Children.Count == 0)
            {
                PCA_Tab.ThisPage.IdTextBox.Text = 0.ToString();
                PCA_Tab.indice = 0;
            }
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
    }
}
