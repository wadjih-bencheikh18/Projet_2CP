using System;
using System.Collections.Generic;

namespace Ordonnancement
{
    class Processus
    {
        public int ID { get; } //ID du processus
        public int ARRIV { get; } //temps d'arrivé
        public int BT { get; } //le temps qu'il faut pour executer le processus
        public int PRIO { get; } //priorite du processus

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
        public void Affichage()
        {
            Console.WriteLine(" ");
            Console.Write("ID : " + ID);
            Console.Write("\t\tARRIV : " + ARRIV);
            Console.Write("\tBT : " + BT);
            Console.Write("\tPRIO : " + PRIO);
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
                        if (x.PRIO.CompareTo(y.PRIO) == 0) return x.ARRIV.CompareTo(y.ARRIV); //si priorité egale
                        else return x.PRIO.CompareTo(y.PRIO); //tri par priorité
                    }
                );
            Affichage();
        }

        public void Push(Processus pro) //ajout d'un processus à la liste P
        {
            P.Add(pro);
            nb++;
        }

        public void Affichage()
        {
            TempsFinExecution();
            TempsDAttente();
            TempsDeSejour();
            for (int i = 0; i < nb; i++)
            {
                P[i].Affichage();
                Console.Write("\tFIN : " + FIN[i]);
                Console.Write("\tWT : " + WT[i]);
                Console.WriteLine("\tTAT : " + TAT[i]);
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
            Processus Pro;
            Schedule prgm = new();
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
            prgm.Priorité();
        }
    }
}