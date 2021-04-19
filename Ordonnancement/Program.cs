using System;
using System.Collections.Generic;

namespace Ordonnancement
{
    class Processus
    {
        public int ID { get; set; } //ID du processus
        public int ARRIV { get; set; } //temps d'arrivé
        public int BT { get; set; } //le temps qu'il faut pour executer le processus
        public int PRIO { get; set; } //priorite du processus

        public Processus(int ID, int ARRIV, int BT, int PRIO)
        {
            this.ID = ID;
            this.ARRIV = ARRIV;
            this.BT = BT;
            this.PRIO = PRIO;
        }

        public Processus(int ID, int ARRIV, int BT) //pour PAPS (FCFS)
        {
            this.ID = ID;
            this.ARRIV = ARRIV;
            this.BT = BT;
            this.PRIO = 0;
        }
        public void affichage()
        {
            Console.WriteLine("Le processus");
            Console.WriteLine("ID : " + ID);
            Console.WriteLine("ARRIV : " + ARRIV);
            Console.WriteLine("BT : " + BT);
            Console.WriteLine("PRIO : " + PRIO);
        }
    }
    class Schedule
    {
        public List<Processus> P = new List<Processus>();
        private int nb = 0; //le nombre de processus dans la liste P
        private int[] FIN = new int[30];
        private int[] WT = new int[30];
        private int[] TAT = new int[30];


        public void PAPS()
        {
            P.Sort(delegate (Processus x, Processus y) { return x.ARRIV.CompareTo(y.ARRIV); }); //tri par ordre d'arrivé
            Affichage();
        }
        public void Priorité()
        {
            P.Sort(
                delegate (Processus x, Processus y) 
                {
                    if (x.PRIO.CompareTo(y.PRIO) == 0) return x.ARRIV.CompareTo(y.ARRIV);
                    else return y.PRIO.CompareTo(x.PRIO); 
                }); //tri par priorité
            Affichage();
        }

        public void push(Processus pro) //ajout d'un processus à la liste P
        {
            P.Add(pro);
            nb++;
        }

        public void Affichage() //A MODIFIER
        {
            TempsFinExecution();
            TempsDAttente();
            TempsDeSejour();
            for (int i=0;i<nb;i++)
            {
                P[i].affichage();
                Console.WriteLine("FIN : " + FIN[i]);
                Console.WriteLine("WT : " + WT[i]);
                Console.WriteLine("TAT : " + TAT[i]);
            }
            /*Console.WriteLine("Les processus");
            Console.WriteLine("ID : " + P[1].ID);
            Console.WriteLine("ARRIV : " + P[1].ARRIV);
            Console.WriteLine("BT : " + P[1].BT);
            Console.WriteLine("FIN : " + FIN[0]);
            Console.WriteLine("WT : " + WT[0]);
            Console.WriteLine("TAT : " + TAT[0]);*/
        }

        private void TempsFinExecution() //temps de fin d'execution FIN
        {
            FIN[0] = P[0].BT + P[0].ARRIV;
            for (int i = 1; i < nb; i++)
            {
                if (P[i].ARRIV < FIN[i - 1])
                {
                    FIN[i] = FIN[i - 1] + P[i].BT;
                }
                else
                {
                    FIN[i] = P[i].BT + P[i].ARRIV;
                }
            }
        }

        private void TempsDAttente() //Waiting Time WT (pour FCFS waiting time = response time)
        {
            WT[0] = 0;
            for (int i = 1; i < nb; i++)
            {
                if (P[i].ARRIV < FIN[i - 1])
                {
                    WT[i] = FIN[i - 1] - P[i].ARRIV;
                }
                else
                {
                    WT[i] = 0;
                }
            }
        }

        private void TempsDeSejour() //Turn Around Time TAT
        {
            for (int i = 0; i < nb; i++)
            {
                TAT[i] = WT[i] + P[i].BT;
            }
        }
    }

    class Programme
    {
        static void Main(string[] args)
        {
            Processus proa = new Processus(1, 4, 3);
            Processus prob = new Processus(5, 2, 3);
            Processus proc = new Processus(5, 5, 3,0);
            Processus prod = new Processus(5, 1, 3,1);
            Schedule prgm = new Schedule();
            prgm.push(proa);
            prgm.push(prob);
            prgm.push(proc);
            prgm.push(prod);
            prgm.Priorité();
        }
    }
}