﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordonnancement
{
    class PSP : Ordonnancement
    {
        public void Executer(int tempsDebut, int tempsFin)  // executer la liste des processus 
        {
            listeProcessus.Sort(delegate (Processus x, Processus y) { return x.tempsArriv.CompareTo(y.tempsArriv); }); //tri par ordre d'arrivé
            int temps = tempsDebut;  // horloge
            int indice = 0;
            while ((listeExecution.Count != 0 || indice < listeProcessus.Count) && temps <= tempsFin)
            {
                indice = AjouterTous(temps, indice); 
                temps++;
                if (listeExecution.Count != 0)
                {
                    listeExecution.Sort(delegate (Processus x, Processus y) { return y.prio.CompareTo(x.prio); });
                    listeExecution[0].tempsRestant--;
                    for (int i = 1; i < listeExecution.Count; i++) listeExecution[i].tempsAtt++; // a chaque fois on incremente le temps d'attente jusqu'a le processus sera suprimé
                    if (listeExecution[0].tempsRestant == 0) // si le temps restant de l'execution = 0
                    {
                        listeExecution[0].tempsFin = temps; // temps de fin d'execution
                        listeExecution[0].tempsService = temps - listeExecution[0].tempsArriv; // temps de service d'execution
                        listeExecution.RemoveAt(0); //suprimer le processus lequel sa duree est ecoulé
                    }
                }
            }
        }
        public int AjouterTous(int temps, int indice)  // collecter tous les processus a partit de "listeProcessus" (liste ordonnée) où leur temps d'arrivé est <= le temps réel d'execution, et les ajouter à la liste d'execution 
        {
            for (; indice < listeProcessus.Count; indice++)
            {
                if (listeProcessus[indice].tempsArriv > temps) break;
                else listeExecution.Add(listeProcessus[indice]);
            }
            return indice;
        }
    }
}
