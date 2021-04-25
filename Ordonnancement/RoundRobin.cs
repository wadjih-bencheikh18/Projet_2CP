using System.Collections.Generic;

namespace Ordonnancement
{
    class RoundRobin : Ordonnancement
    {
        private int quantum { get; }
        public RoundRobin(int q, List<Processus> listeProcessus, List<Processus> listeExecution)
        {
            this.listeProcessus = listeProcessus;
            this.listeExecution = listeExecution;
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

        public int Executer()  // executer la liste des processus et retourner le temps total pour le faire
        {
            int indice = 0;
            listeProcessus.Sort(delegate (Processus x, Processus y) { return x.tempsArriv.CompareTo(y.tempsArriv); }); //tri par ordre d'arrivé
            int temps = listeProcessus[0].tempsArriv;  // horloge
            indice = AjouterTous(temps, indice);
            int i = 0;
            while (indice < listeProcessus.Count || listeExecution.Count != 0)
            {
                if (indice < listeProcessus.Count && listeExecution.Count == 0)  // si le processeur a terminé mais il y a des processus arrivant
                {
                    temps = listeProcessus[indice].tempsArriv;  // passer au prochain processus arrivant
                    indice = AjouterTous(temps, indice);
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
                        listeExecution[i].tempsAtt -= listeExecution[i].tempsArriv;  // car il a commencer à attendre à partir où il a arrivé
                        listeExecution[i].tempsRestant = 0;
                        listeExecution[i].etat = 0;  // terminé
                        listeExecution[i].tempsFin = temps;  // temps de fin d'execution
                        // on supprime ce processus à partir de la liste d'execution mais on le mis à jour dans la liste des processus
                        int j = listeProcessus.FindIndex(p => ((p.id == listeExecution[i].id) && (p.prio == listeExecution[i].prio)));
                        listeProcessus[j] = listeExecution[i];
                        listeExecution.RemoveAt(i);
                    }
                    indice = AjouterTous(temps, indice);
                    if (i >= listeExecution.Count) i = 0;
                }
            }
            return temps;
        }

        public int Executer(int tempsDebut, int tempsFin, int[] indices)
        // executer l'algo pendant un intervalle de temps où indices[0] est l'indice de listeProcessus où on doit reprendre et indices[1] est l'indice de listeExecution où on doit reprendre et indices[2] est le quantum du temps du dernier execution
        {
            SortListeProcessus();
            MisAJourTempsAtt(tempsDebut);
            while (indices[0] < listeProcessus.Count || listeExecution.Count != 0)  // il y reste un processus à executer
            {
                if (indices[0] < listeProcessus.Count && listeExecution.Count == 0)  // la liste d'execution est vide pour le moment
                {
                    if (tempsDebut < listeProcessus[indices[0]].tempsArriv) tempsDebut++;  // si aucun processus est arrivant, donc horloge++
                    else indices[0] = AjouterTous(tempsDebut, indices[0]);  // sinon on lui ajoute à la liste d'execution
                }
                else  // la liste d'execution n'est pas vide pour le moment
                {
                    if (indices[2] == 0)  // si l'indice du quantum == 0 donc c'est un nouveau processus qu'on va commencer à executer
                    {
                        listeExecution[indices[1]].tempsAtt += tempsDebut - listeExecution[indices[1]].tempsFin;  // mis a jour son temps d'attente
                    }
                    tempsDebut++;  // horloge++
                    indices[2]++;  // quantum++
                    listeExecution[indices[1]].tempsRestant--;
                    if (listeExecution[indices[1]].tempsRestant == 0)  // on a terminé ce processus
                    {
                        listeExecution[indices[1]].tempsAtt -= listeExecution[indices[1]].tempsArriv;
                        listeExecution[indices[1]].tempsFin = tempsDebut;
                        // maitenant on le supprime de la liste d'execution
                        int j = listeProcessus.FindIndex(p => ((p.id == listeExecution[indices[1]].id) && (p.prio == listeExecution[indices[1]].prio)));
                        listeProcessus[j] = listeExecution[indices[1]];
                        listeExecution.RemoveAt(indices[1]);
                        indices[2] = 0;  // un nouveau quantum va commencer
                    }
                    else if (indices[2] == quantum)  // on a terminé ce quantum, donc il faut passer au suivant processus
                    {
                        listeExecution[indices[1]].tempsFin = tempsDebut;  // sauvegarder quand on a arrété l'execution de ce processus
                        indices[2] = 0;  // nouveau quantum
                        indices[1]++;  // passer au suivant processus dans la liste d'execution
                    }
                    indices[0] = AjouterTous(tempsDebut, indices[0]);  // ajouter si il y a un processus qui a arrivé
                    if (indices[1] >= listeExecution.Count) indices[1] = 0;
                }
                if (tempsDebut == tempsFin)  // attent le fin de l'intervalle du temps
                {
                    MisAJourTempsAtt(-tempsFin);
                    return tempsFin;
                }
            }
            return tempsDebut;
        }

        public void MisAJourTempsAtt(int deltaTemps)  // mis a jour le temps d'attente des processus non terminée avec la periode deltaTemps
        {
            foreach (Processus p in listeExecution)
            {
                if (p.tempsRestant == 0) continue;
                p.tempsAtt += deltaTemps;
            }
        }
    }
}