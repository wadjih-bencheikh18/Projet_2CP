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
        public void Affichage()
        {
            Console.WriteLine(" ");
            Console.Write("ID : " + id);
            Console.Write("\t\tARRIV : " + tempsArriv);
            Console.Write("\tBT : " + duree);
            Console.Write("\tPRIO : " + prio);
            Console.Write("\tFIN : " + tempsFin);
            Console.Write("\tWT : " + tempsAtt);
            Console.WriteLine("\tTAT : " + tempsService);
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
        public void Affichage()
        {
            for (int i = 0; i < listeProcessus.Count; i++) listeProcessus[i].Affichage();
        }
        public void SortListeProcessus()
        {
            listeProcessus.Sort(delegate (Processus x, Processus y) { return x.tempsArriv.CompareTo(y.tempsArriv); }); //tri par ordre d'arrivé
        }
    }
}