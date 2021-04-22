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
            while ((i < listeProcessus.Count)&&(listeProcessus[i].tempsArriv == Horloge))
            {
                listeExecution.Add(listeProcessus[i]);
                i++;
            }
            while (listeExecution.Count != 0)
            {
                listeExecution.Sort(delegate (Processus x, Processus y) { return y.prio.CompareTo(x.prio); }); //tri par Priorité
                listeExecution[0].tempsRestant --;
                Horloge++;
                if (listeExecution[0].tempsRestant == 0)
                {
                    int j = listeProcessus.FindIndex(p => ((p.id == listeExecution[0].id) && (p.prio == listeExecution[0].prio)));
                    listeProcessus[j].tempsFin = Horloge; // temps de fin d'execution
                    listeProcessus[j].tempsService = Horloge - listeExecution[0].tempsArriv;
                    listeProcessus[j].tempsAtt = listeExecution[0].tempsService - listeExecution[0].duree;
                    listeExecution.RemoveAt(0);
                }
                
                while ((i < listeProcessus.Count)&&(listeProcessus[i].tempsArriv == Horloge))
                {
                    listeExecution.Add(listeProcessus[i]);
                    i++;
                }
            }
            return Horloge;
        }

    }
}
