using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordonnancement
{
    class RoundRobin : Ordonnancement
    {
        private int quantum { get; }

        public RoundRobin(int q)
        {
            quantum = q;
        }

        public int AjouterTous(int temps, int indice)  // collecter tous les processus a partit de "listeProcessus" (liste ordonnée) où leur temps d'arrivé est <= le temps réel d'execution, et les ajouter à la liste d'execution 
        {
            for (; indice < listeProcessus.Count; indice++)
            {
                if (listeProcessus[indice].tempsArriv <= temps) listeExecution.Add(listeProcessus[indice]);
                else break;
            }
            return indice;
        }
        
        public int Executer()  // executer la liste des processus et retourner le temps total pour le faire
        {
            int indice = 0;
            listeProcessus.Sort(delegate (Processus x, Processus y) { return x.tempsArriv.CompareTo(y.tempsArriv); }); //tri par ordre d'arrivé
            int debut = listeProcessus[0].tempsArriv;  // indiquer quand le processeur à commencer d'executer le premier processus
            int temps = listeProcessus[0].tempsArriv;  // horloge
            indice = AjouterTous(temps, indice);
            int i = 0;
            while ((indice < listeProcessus.Count) || (!listeExecution.TrueForAll(p => p.tempsRestant == 0)))
            {
                if ((indice < listeProcessus.Count) && (listeExecution.TrueForAll(p => p.tempsRestant == 0)))  // si le processeur a terminé mais il y a des processus arrivant
                {
                    debut = listeProcessus[0].tempsArriv;  // une nouvelle serie d'execution
                    temps += listeProcessus[0].tempsArriv;
                    indice = AjouterTous(temps, indice);
                }
                else if (listeExecution[i].tempsRestant == 0)  // le processus est fini, passer au suivant
                {
                    i++;
                    if (i >= listeExecution.Count) i = 0;
                }
                else  // le processus n'est pas terminé
                {
                    listeExecution[i].tempsAtt += temps - listeExecution[i].tempsFin;
                    if (listeExecution[i].tempsRestant > quantum)  // il ne sera pas terminé pendant ce quantum
                    {
                        listeExecution[i].etat = 1;  // prés
                        listeExecution[i].tempsRestant -= quantum;
                        temps += quantum;
                        listeExecution[i].tempsFin = temps;  // stocker la fin d'execution du processus pour qu'on puisse calculer le temps d'att le prochain quantum
                        i++;  // passer au processus suivant
                    }
                    else  // sera terminé pendant ce quantum
                    {
                        temps += listeExecution[i].tempsRestant;
                        listeExecution[i].tempsRestant = 0;
                        listeExecution[i].tempsAtt -= debut;  // enlever le debut d'execution
                        listeExecution[i].etat = 0;  // terminé
                        // on supprime ce processus à partir de la liste d'execution mais on le mis à jour dans la liste des processus
                        int j = listeProcessus.FindIndex(p => ((p.id == listeExecution[i].id) && (p.prio == listeExecution[i].prio)));
                        listeProcessus[j] = listeExecution[i];
                        listeProcessus[j].tempsFin = temps;  // temps de fin d'execution
                        listeExecution.RemoveAt(i);
                    }
                    indice = AjouterTous(temps, indice);
                    if (i >= listeExecution.Count) i = 0;
                }
            }
            return temps;
        }
    }
}
