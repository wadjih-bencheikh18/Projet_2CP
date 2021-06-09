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
    /// Interaction logic for SlackTime_TabRow.xaml
    /// </summary>
    public partial class SlackTime_TabRow : UserControl
    {
        public SlackTime_TabRow()
        {
            InitializeComponent();
            TreeViewItem header = new TreeViewItem();
            header.Header = processusHeader;
            TreeViewParent.Items.Add(header);
        }

        private TextBox id;
        private TextBox tempsArriv;
        private TextBox duree;
        private TextBox deadline;
        private TextBlock Ajouter;
        private StackPanel Table;

        public TreeViewItem parent;

        public SlackTime_TabRow(TextBox id, TextBox tempsArriv, TextBox duree,TextBox deadline, StackPanel Table, TextBlock Ajouter)
        {
            InitializeComponent();
            this.id = id;
            this.tempsArriv = tempsArriv;
            this.duree = duree;
            this.Table = Table;
            this.deadline = deadline;
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
            deadline.Text = deadlineTest.Text;
            SlackTime_Tab.modifier = true;
            SlackTime_Tab.proModifier = this;
            Ajouter.Text = "Modifier";
        }

        private void suprimer_Button_Click(object sender, RoutedEventArgs e)
        {
            if (SlackTime_Tab.modifier)
            {
                SlackTime_Tab.modifier = false;
                Ajouter.Text = "Ajouter";
                SlackTime_Tab.FixIndice();
            }
            SlackTime_Tab.prog.listeProcessus.RemoveAll(p => p.id.ToString().Equals(this.idTest.Text) && p.tempsArriv.ToString().Equals(this.tempsArrTest.Text));
            Table.Children.Remove(this);
            if (Table.Children.Count == 0)
            {
                SlackTime_Tab.ThisPage.IdTextBox.Text = 0.ToString();
                SlackTime_Tab.indice = 0;
            }
        }

        private void TreeViewParent_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewParent.IsSelected = false;
        }
    }
}
