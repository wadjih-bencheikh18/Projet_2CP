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
using System.Xml.Linq;

namespace TaskSimulListePrets
{
    /// <summary>
    /// imported from our project (class processus)
    /// </summary>
    class Processus
    {
        //donnés
        public int id;//{ get; } //ID du processus
        public int tempsArriv;// { get; } //temps d'arrivé
        public int duree { get; } //temps d'execution du processus (burst time)
        public int prio { get; } //priorite du processus
        //à remplir
        public int etat; // 0:bloqué  1:prêt  2:active
        public int tempsFin;
        public int tempsAtt;
        public int tempsService;
        public int tempsReponse;
        public int tempsRestant;
        public Processus(int id, int tempsArriv, int duree, int prio)  //constructeur pour l'algorithme de priorité
        {
            this.id = id;
            this.tempsArriv = tempsArriv;
            this.duree = duree;
            this.prio = prio;
            tempsRestant = duree;
        }
        public Processus(int id, int tempsArriv, int duree) //constructeur pour les autres algorithmes
        {
            this.id = id;
            this.tempsArriv = tempsArriv;
            this.duree = duree;
            prio = 0;
            tempsRestant = duree;
        }
        public virtual void Affichage() //affiche les caracteristiques d'un processus
        {
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.Write("ID : " + id);
            Console.Write("\ttemps d'arrivé : " + tempsArriv);
            Console.Write("\tduree : " + duree);
            Console.Write("\tpriorité : " + prio);
            Console.Write("\ttemps d'attente : " + tempsAtt);
            Console.Write("\ttemps de fin :  " + tempsFin);
            Console.Write("\ttemps de service  : " + tempsService);
            Console.Write("\ttemps de reponse : " + tempsReponse);
        }
    }

    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Processus Pro;
            int Nb=5;
            Random r1,r2,r3;
            List<Processus> P = new List<Processus>();
            for (int i = 0; i < Nb; i++)
            {
                r1 = new Random();
                r2 = new Random();
                r3 = new Random();
                Pro = new Processus(i, r1.Next(0, 15), r2.Next(1, 15), r3.Next(1, 5));
                //MessageBox.Show($"The ID n°{i+1} is : {Pro.tempsArriv}");
                P.Add(Pro);
            }

            foreach (var process in P)
            {
                var item = new TreeViewItem();
                item.Header = process.id;
                ListView.Items.Add(item);
            }
        }

    }
}
