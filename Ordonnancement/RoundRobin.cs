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
        public RoundRobin(int q,List<Processus> listeProcessus, List<Processus> listeExecution)
        {
            this.listeProcessus = listeProcessus;
            this.listeExecution = listeExecution;
            quantum = q;
        }
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

        private static bool Termine(Processus p)  // savoir si un processus a terminé ou pas encore
        {
            return p.tempsRestant == 0;
        }
        
        public int Executer()  // executer la liste des processus et retourner le temps total pour le faire
        {
            int indice = 0;
            listeProcessus.Sort(delegate (Processus x, Processus y) { return x.tempsArriv.CompareTo(y.tempsArriv); }); //tri par ordre d'arrivé
            int debut = listeProcessus[0].tempsArriv;  // indiquer quand le processeur à commencer d'executer le premier processus
            int temps = listeProcessus[0].tempsArriv;  // horloge
            indice = AjouterTous(temps, indice);
            int i = 0;
            while ((indice < listeProcessus.Count) || (!listeExecution.TrueForAll(Termine)))
            {
                if ((indice < listeProcessus.Count) && (listeExecution.TrueForAll(Termine)))  // si le processeur a terminé mais il y a des processus arrivant
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
                    }
                    else  // sera terminé pendant ce quantum
                    {
                        temps += listeExecution[i].tempsRestant;
                        listeExecution[i].tempsRestant = 0;
                        listeExecution[i].tempsAtt -= debut;  // enlever le debut d'execution
                        listeExecution[i].etat = 0;  // terminé
                    }
                    listeExecution[i].tempsFin = temps;  // stocker la fin d'execution du processus pour qu'on puisse calculer le temps d'att le prochain quantum
                    indice = AjouterTous(temps, indice);
                    i++;  // passer au processus suivant
                    if (i >= listeExecution.Count) i = 0;
                }
            }
            return temps;
        }
    }
}
