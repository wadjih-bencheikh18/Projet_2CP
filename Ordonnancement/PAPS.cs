using System;
using System.Collections.Generic;
using System.Linq;

namespace Ordonnancement
{
    class PSP : Ordonnancement
    {
        public List<Processus> listPrio = new List<Processus>();
        public int Horloge = 0;
        public void Faire()
        {
            int i=0;
            Processus element;
            listeProcessus.Sort(delegate (Processus x, Processus y) { return x.tempsArriv.CompareTo(y.tempsArriv); }); //tri par ordre d'arrivé
            element = listeProcessus[0];
            while(element.tempsArriv <= Horloge)
            {
                listPrio.Add(element);
                i++;
                element = listeProcessus[i];
            }
            listPrio.Sort(delegate (Processus x, Processus y) { return x.prio.CompareTo(y.prio); }); //tri par Priorité


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