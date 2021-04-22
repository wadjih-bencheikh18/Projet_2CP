using System;
using System.Collections.Generic;
using System.Linq;

namespace Ordonnancement
{
    class PSP : Ordonnancement
    {
        public Queue<Processus> prioFifo = new Queue<Processus>();
        public int Horloge;
        public void Faire()
        {
            int i=0;
            Processus element;
            Processus q;
            listeProcessus.Sort(delegate (Processus x, Processus y) { return x.tempsArriv.CompareTo(y.tempsArriv); }); //tri par ordre d'arrivé
            Horloge = 0;
            element = listeProcessus[0];
            while(element.tempsArriv <= Horloge)
            {
                prioFifo.Enqueue(element);
                i++;
                element = listeProcessus[i];
            }
            prioFifo = new Queue<Processus>(prioFifo.OrderBy(q.prio)
                );
            foreach (var element2 in prioFifo)
            {
                element2.prio = prioFifo.Dequeue;
            }

            Affichage();
        }

        public override void CalculFin() //temps de fin d'execution FIN
        {
            listeProcessus[0].tempsFin = listeProcessus[0].duree + listeProcessus[0].tempsArriv;
            for (int i = 1; i < nb; i++)
            {
                if (listeProcessus[i].tempsArriv < listeProcessus[i - 1].tempsFin)
                {
                    listeProcessus[i].tempsFin = listeProcessus[i - 1].tempsFin + listeProcessus[i].duree;
                }
                else
                {
                    listeProcessus[i].tempsFin = listeProcessus[i].duree + listeProcessus[i].tempsArriv;
                }
            }
        }

        public override void CalculAtt() //Waiting Time WT (pour FCFS waiting time = response time)
        {
            listeProcessus[0].tempsAtt = 0;
            for (int i = 1; i < nb; i++)
            {
                if (listeProcessus[i].tempsArriv < listeProcessus[i - 1].tempsFin)
                {
                    listeProcessus[i].tempsAtt = listeProcessus[i - 1].tempsFin - listeProcessus[i].tempsArriv;
                }
                else
                {
                    listeProcessus[i].tempsAtt = 0;
                }
            }
        }

        public override void CalculService() //Turn Around Time TAT
        {
            for (int i = 0; i < nb; i++)
            {
                listeProcessus[i].tempsService = listeProcessus[i].tempsAtt + listeProcessus[i].duree;
            }
        }
    }
}