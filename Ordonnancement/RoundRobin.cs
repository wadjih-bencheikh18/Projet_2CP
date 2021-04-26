using System;
using System.Collections.Generic;

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
            SortListeProcessus();
            int indice = 0, temps = 0, i = 0, q = 0;
            while (indice < listeProcessus.Count || listeExecution.Count != 0)  // il y reste un processus à executer
            {
                if (indice < listeProcessus.Count && listeExecution.Count == 0)  // la liste d'execution est vide pour le moment
                {
                    if (temps < listeProcessus[indice].tempsArriv) temps++;  // si aucun processus est arrivant, donc horloge++
                    else indice = AjouterTous(temps, indice);  // sinon on lui ajoute à la liste d'execution
                }
                else  // la liste d'execution n'est pas vide pour le moment
                {
                    if (q == 0)  // si l'indice du quantum == 0 donc c'est un nouveau processus qu'on va commencer à executer
                    {
                        listeExecution[i].tempsAtt += temps - listeExecution[i].tempsFin;  // mis a jour son temps d'attente
                    }
                    temps++;  // horloge++
                    q++;  // quantum++
                    listeExecution[i].tempsRestant--;
                    if (listeExecution[i].tempsRestant == 0)  // on a terminé ce processus
                    {
                        listeExecution[i].tempsAtt -= listeExecution[i].tempsArriv;
                        listeExecution[i].tempsFin = temps;
                        // maitenant on le supprime de la liste d'execution
                        int j = listeProcessus.FindIndex(p => ((p.id == listeExecution[i].id) && (p.prio == listeExecution[i].prio)));
                        listeProcessus[j] = listeExecution[i];
                        listeExecution.RemoveAt(i);
                        q = 0;  // un nouveau quantum va commencer
                    }
                    else if (q == quantum)  // on a terminé ce quantum, donc il faut passer au suivant processus
                    {
                        listeExecution[i].tempsFin = temps;  // sauvegarder quand on a arrété l'execution de ce processus
                        q = 0;  // nouveau quantum
                        i++;  // passer au suivant processus dans la liste d'execution
                    }
                    indice = AjouterTous(temps, indice);  // ajouter si il y a un processus qui a arrivé
                    if (i >= listeExecution.Count) i = 0;
                }
            }
            return temps;
        }

        public void InitRoundRobin(List<Processus> listeProcessus, List<Processus> listeExecution)
        {
            this.listeProcessus = listeProcessus;
            this.listeExecution = listeExecution;
        }
        public int Executer(int tempsDebut, int tempsFin, int[] indices, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral)
        // executer l'algo pendant un intervalle de temps où indices[0] est l'indice de listeProcessus où on doit reprendre et indices[1] est l'indice de listeExecution où on doit reprendre et indices[2] est le quantum du temps du dernier execution
        {
            SortListeProcessus();
            MisAJourTempsAtt(tempsDebut);
            while (indices[0] < listeProcessus.Count || listeExecution.Count != 0)  // il y reste un processus à executer
            {
                if (indices[0] < listeProcessus.Count && listeExecution.Count == 0)  // la liste d'execution est vide pour le moment
                {
                    tempsDebut++;  // si aucun processus est arrivant, donc horloge++
                    indices[0] = AjouterTous(tempsDebut, indices[0],niveaux,listeGeneral, indiceNiveau);  // sinon on lui ajoute à la liste d'execution
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
                    Console.WriteLine(tempsDebut + "-" + listeExecution[0].id);
                    if (listeExecution[indices[1]].tempsRestant == 0)  // on a terminé ce processus
                    {
                        listeExecution[indices[1]].tempsAtt -= listeExecution[indices[1]].tempsArriv;
                        listeExecution[indices[1]].tempsFin = tempsDebut;
                        // maitenant on le supprime de la liste d'execution
                        listeExecution.RemoveAt(indices[1]);
                        indices[2] = 0;  // un nouveau quantum va commencer
                    }
                    else if (indices[2] == quantum)  // on a terminé ce quantum, donc il faut passer au suivant processus
                    {
                        listeExecution[indices[1]].tempsFin = tempsDebut;  // sauvegarder quand on a arrété l'execution de ce processus
                        indices[2] = 0;  // nouveau quantum
                        indices[1]++;  // passer au suivant processus dans la liste d'execution
                    }
                    indices[0] = AjouterTous(tempsDebut, indices[0], niveaux, listeGeneral, indiceNiveau);  // ajouter si il y a un processus qui a arrivé
                    if (indices[1] >= listeExecution.Count) indices[1] = 0;
                }
                if (tempsDebut == tempsFin)  // attent le fin de l'intervalle du temps
                {
                    //MisAJourTempsAtt(-tempsFin);
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
        public int AjouterTous(int temps, int indice, Niveau[] niveaux, List<ProcessusNiveau> listeGeneral,int indiceNiveau)  // collecter tous les processus a partit de "listeProcessus" (liste ordonnée) où leur temps d'arrivé est <= le temps réel d'execution, et les ajouter à la liste d'execution 
        {
            for (; indice < listeProcessus.Count; indice++)
            {
                if (listeProcessus[indice].tempsArriv > temps) break;
                else
                {
                    //listeExecution.Add(listeProcessus[indice]);
                    if (listeGeneral[indice].niveau == indiceNiveau) listeExecution.Add(listeGeneral[indice]);
                    else niveaux[listeGeneral[indice].niveau].listeExecution.Add(listeGeneral[indice]);
                }
            }
            return indice;
        }
    }
}