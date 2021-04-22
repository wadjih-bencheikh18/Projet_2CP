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

        public void AjouterTous(List<Processus> total, int temps)  // collecter tous les processus a partit de "total" (liste ordonnée) où leur temps d'arrivé est <= le temps réel d'execution
        {
            int k = 0;
            for (; k < total.Count; k++)
            {
                if (total[k].tempsArriv <= temps) Push(total[k]);
                else break;
            }
            total.RemoveRange(0, k);
        }

        private static bool Termine(Processus p)
        {
            return p.tempsRestant == 0;
        }
        
        public int Executer(List<Processus> total)  // executer la liste des processus "total" et retourner le temps total pour le faire
        {
            total.Sort(delegate (Processus x, Processus y) { return x.tempsArriv.CompareTo(y.tempsArriv); }); //tri par ordre d'arrivé
            int debut = total[0].tempsArriv;  // indiquer quand le processeur à commencer d'executer le premier processus
            int temps = total[0].tempsArriv;  // horloge
            AjouterTous(total, temps);
            int i = 0;
            while ((total.Count != 0) || (!listeProcessus.TrueForAll(Termine)))
            {
                if ((total.Count != 0) && (listeProcessus.TrueForAll(Termine)))  // si le processeur a terminé mais il y a des processus arrivant
                {
                    debut = total[0].tempsArriv;  // une nouvelle serie d'execution
                    temps += total[0].tempsArriv;
                    AjouterTous(total, temps);
                }
                else if (listeProcessus[i].tempsRestant == 0)  // le processus est fini, passer au suivant
                {
                    i++;
                    if (i >= listeProcessus.Count) i = 0;
                }
                else  // le processus n'est pas terminé
                {
                    listeProcessus[i].tempsAtt += temps - listeProcessus[i].tempsFin;
                    if (listeProcessus[i].tempsRestant > quantum)  // il ne sera pas terminé pendant ce quantum
                    {
                        listeProcessus[i].etat = 1;  // prés
                        listeProcessus[i].tempsRestant -= quantum;
                        temps += quantum;
                    }
                    else  // sera terminé pendant ce quantum
                    {
                        temps += listeProcessus[i].tempsRestant;
                        listeProcessus[i].tempsRestant = 0;
                        listeProcessus[i].tempsAtt -= debut;  // enlever le debut d'execution
                        listeProcessus[i].etat = 0;  // terminé
                        
                    }
                    listeProcessus[i].tempsFin = temps;  // stocker la fin d'execution du processus pour qu'on puisse calculer le temps d'att le prochain quantum
                    AjouterTous(total, temps);
                    i++;  // passer au processus suivant
                    if (i >= listeProcessus.Count) i = 0;
                }
            }
            return temps;
        }
    }
}
