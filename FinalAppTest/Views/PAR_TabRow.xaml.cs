﻿using System.Windows;
using System.Windows.Controls;

namespace FinalAppTest.Views
{
    /// <summary>
    /// Interaction logic for PAR_TabRow.xaml
    /// </summary>
    public partial class PAR_TabRow : UserControl
    {
        public PAR_TabRow()
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

        public PAR_TabRow(TextBox id, TextBox tempsArriv, TextBox duree,TextBox prio, StackPanel Table, TextBlock Ajouter)
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
            PAR_Tab.modifier = true;
            PAR_Tab.proModifier = this;
            Ajouter.Text = "Modifier";
        }

        private void suprimer_Button_Click(object sender, RoutedEventArgs e)
        {
            if (PAR_Tab.modifier)
            {
                PAR_Tab.modifier = false;
                Ajouter.Text = "Ajouter";
                PAR_Tab.FixIndice();
            }
            PAR_Tab.prog.listeProcessus.RemoveAll(p => p.id.ToString().Equals(this.idTest.Text) && p.tempsArriv.ToString().Equals(this.tempsArrTest.Text));
            Table.Children.Remove(this);
        }

        private void TreeViewParent_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewParent.IsSelected = false;
        }
    }
}
