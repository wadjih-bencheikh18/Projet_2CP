﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordonnancement
{
    class PSP : Ordonnancement
    {
        public int Executer()  // executer la liste des processus 
        {
            listeProcessus.Sort(delegate (Processus x, Processus y) { return x.tempsArriv.CompareTo(y.tempsArriv); }); //tri par ordre d'arrivé
            int Horloge = listeProcessus[0].tempsArriv;  // horloge
            int i = 0;
            while (listeProcessus[i].tempsArriv <= Horloge)
            {
                listeExecution.Add(listeProcessus[i]);
                i++;
            }
            while (listeExecution.Count != 0 && i < listeProcessus.Count)
            {
                listeExecution.Sort(delegate (Processus x, Processus y) { return x.prio.CompareTo(y.prio); }); //tri par Priorité
                listeExecution[0].tempsRestant --;
                if (listeExecution[0].tempsRestant == 0)
                {
                    listeExecution[0].tempsFin = Horloge;
                    listeExecution[0].tempsService = Horloge - listeExecution[0].tempsArriv;
                    listeExecution[0].tempsAtt = listeExecution[0].tempsService - listeExecution[0].duree;
                    int j = listeProcessus.FindIndex(p => ((p.id == listeExecution[i].id) && (p.prio == listeExecution[i].prio)));
                    listeProcessus[j] = listeExecution[0];
                    listeProcessus[j].tempsFin = listeExecution[0].tempsFin; // temps de fin d'execution
                    listeProcessus[j].tempsService = listeExecution[0].tempsService;
                    listeProcessus[j].tempsAtt = listeExecution[0].tempsAtt;
                    listeExecution.RemoveAt(0);
                }
                Horloge ++;
                while (listeProcessus[i].tempsArriv <= Horloge)
                {
                    listeExecution.Add(listeProcessus[i]);
                    i++;
                }
            }
            return Horloge;
        }

    }
}