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
        public Processus(int id, int tempsArriv, int duree, int prio)
        {
            this.id = id;
            this.tempsArriv = tempsArriv;
            this.duree = duree;
            this.prio = prio;
        }

        public Processus(int id, int tempsArriv, int duree) //pour PAPS (FCFS)
        {
            this.id = id;
            this.tempsArriv = tempsArriv;
            this.duree = duree;
            this.prio = 0;
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
        public List<Processus> listeProcessus = new List<Processus>();
        public int nb; //le nombre de processus dans la liste P
        public void Push(Processus pro) //ajout d'un processus à la liste P
        {
            listeProcessus.Add(pro);
            nb++;
        }
        public void Affichage()
        {
            CalculFin();
            CalculAtt();
            CalculService();
            for (int i = 0; i < nb; i++) listeProcessus[i].Affichage();
        }
        public abstract void CalculFin();
        public abstract void CalculAtt();
        public abstract void CalculService();

    }

    class Programme
    {
        static void Main(string[] args)
        {
            Processus Pro;
            PAPS prgm = new();
            Random r1, r2, r3, r4;
            for (int i = 0; i < 20; i++)
            {
                r1 = new Random();
                r2 = new Random();
                r3 = new Random();
                r4 = new Random();
                Pro = new(r1.Next(0, 100), r2.Next(0, 100), r3.Next(1, 100), r4.Next(0, 100));
                prgm.Push(Pro);
            }
            prgm.Faire();
        }
    }
}