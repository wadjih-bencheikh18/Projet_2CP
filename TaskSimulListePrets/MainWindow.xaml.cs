using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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
    class ProcessusString
    {
        public string id { get; set; }
        public string tempsRestant { get; set; }
        public ProcessusString(int id, int tempsRestant)
        {
            this.id = id.ToString();
            this.tempsRestant = tempsRestant.ToString();
        }
    }
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random random = new Random();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Processus Pro;
            int Nb = 7;
            int duree, tempsArriv;
            Random r1, r2, r3;
            List<Processus> P = new List<Processus>();
            for (int i = 0; i < Nb; i++)
            {
                duree = random.Next(1, 15);
                tempsArriv = random.Next(0, 15);
                Pro = new Processus(i, tempsArriv, duree);
                P.Add(Pro);
            }
            ProcessusString Proc;
            foreach (var process in P)
            {
                var item = new TreeViewItem();
                Proc = new ProcessusString (process.id,process.tempsRestant);
                item.Header = Proc;
                ListView.Children.Add(item);
            }
        }

    }
}
