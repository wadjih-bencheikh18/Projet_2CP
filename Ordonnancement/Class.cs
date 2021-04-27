using System;
using System.Collections.Generic;

namespace Ordonnancement
{
    class Processus
    {
        public int id { get; } //ID du processus
        public int tempsArriv { get; } //temps d'arrivé
        public int duree { get; } //le temps qu'il faut pour executer le processus
        public int prio { get; } //priorite du processus
        // a remplir
        public int etat; // 0 disactivé  1 pret   2 en cours
        public int tempsFin;
        public int tempsAtt;
        public int tempsService;
        public int tempsRestant;
        public Processus(int id, int tempsArriv, int duree, int prio)
        {
            this.id = id;
            this.tempsArriv = tempsArriv;
            this.duree = duree;
            this.prio = prio;
            this.tempsRestant = duree;
        }
        public Processus(int id, int tempsArriv, int duree) //pour PAPS (FCFS)
        {
            this.id = id;
            this.tempsArriv = tempsArriv;
            this.duree = duree;
            this.prio = 0;
            this.tempsRestant = duree;
        }
        public virtual void Affichage()
        {
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.Write("ID : " + id);
            Console.Write("\t\ttemps d'arrivé : " + tempsArriv);
            Console.Write("\tduree : " + duree);
            //Console.Write("\tPRIO : " + prio);
            Console.Write("\ttemps de fin :  " + tempsFin);
            Console.Write("\temps d'attente :   " + tempsAtt);
            Console.Write("\ttemps de service  : " + tempsService);
            Console.Write("\ttemps restant : " + tempsRestant);
        }
    }

    abstract class Ordonnancement
    {
        protected List<Processus> listeProcessus = new List<Processus>();  // liste des processus fournis par l'utilisateur
        protected List<Processus> listeExecution = new List<Processus>();  // liste d'execution par le precesseur
        public void Push(Processus pro) //ajout d'un processus à la liste P
        {
            listeProcessus.Add(pro);
        }
        public virtual void Affichage() //affichage de listeProcessus
        {
            for (int i = 0; i < listeProcessus.Count; i++) listeProcessus[i].Affichage();
        }
        public virtual void SortListeProcessus() //tri des processus par ordre d'arrivé
        {
            listeProcessus.Sort(delegate (Processus x, Processus y) { return x.tempsArriv.CompareTo(y.tempsArriv); }); //tri par ordre d'arrivé
        }
    }
}