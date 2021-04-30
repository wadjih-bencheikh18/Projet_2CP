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
        public int Executer()  // executer la liste des processus et retourner le temps total pour le faire
        {
            SortListeProcessus();
            int indice = 0, temps = 0, q = 0;
            while (indice < listeProcessus.Count || listeExecution.Count != 0)  // il y reste un processus à executer
            {
                if (indice < listeProcessus.Count && listeExecution.Count == 0)  // la liste d'execution est vide pour le moment
                {
                    if (temps < listeProcessus[indice].tempsArriv)  // si aucun processus est arrivant, donc horloge++
                    {
                        AfficheLigne(temps);
                        temps++;
                    }
                    else indice = AjouterTous(temps, indice);  // sinon on lui ajoute à la liste d'execution
                }
                else  // la liste d'execution n'est pas vide pour le moment
                {
                    AfficheLigne(temps, listeExecution[0].id);
                    temps++;  // horloge++
                    q++;  // quantum++
                    listeExecution[0].tempsRestant--;
                    indice = AjouterTous(temps, indice);  // ajouter si il y a un processus qui a arrivé
                    
                    if (listeExecution[0].tempsRestant == 0)  // on a terminé ce processus
                    {
                        listeExecution[0].tempsFin = temps;
                        listeExecution[0].tempsService = temps - listeExecution[0].tempsArriv;
                        listeExecution[0].tempsAtt = listeExecution[0].tempsService - listeExecution[0].duree;
                        // maitenant on le supprime de la liste d'execution
                        listeExecution.RemoveAt(0);
                        q = 0;  // un nouveau quantum va commencer
                    }
                    else if (q == quantum)  // on a terminé ce quantum, donc il faut passer au suivant processus, donc on defile et enfile à la fin le processus courant
                    {
                        listeExecution[0].tempsFin = temps;  // sauvegarder quand on a arrété l'execution de ce processus
                        q = 0;  // nouveau quantum
                        listeExecution.Add(listeExecution[0]);  // enfiler à la fin
                        listeExecution.RemoveAt(0);  // defiler 
                    }
                }
            }
            return temps;
        }

        // des algos pour utiliser dans MultiNiveaux
        public void InitRoundRobin(List<Processus> listeProcessus, List<Processus> listeExecution)
        {
            this.listeProcessus = listeProcessus;
            this.listeExecution = listeExecution;
        }
        public int Executer(int tempsDebut, int tempsFin, int[] indices, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral)
        // executer l'algo pendant un intervalle de temps où indices[0] est l'indice de listeProcessus où on doit reprendre et indices[1] est le quantum du temps du dernier execution
        {
            SortListeProcessus();
            while (indices[0] < listeProcessus.Count || listeExecution.Count != 0)  // il y reste un processus à executer
            {
                if (indices[0] < listeProcessus.Count && listeExecution.Count == 0)  // la liste d'execution est vide pour le moment
                {
                    tempsDebut++;  // si aucun processus est arrivant, donc horloge++
                    indices[0] = AjouterTous(tempsDebut, indices[0], niveaux, listeGeneral, indiceNiveau);  // sinon on lui ajoute à la liste d'execution
                }
                else  // la liste d'execution n'est pas vide pour le moment
                {
                    AfficheLigne(tempsDebut, listeExecution[0].id);
                    tempsDebut++;  // horloge++
                    indices[1]++;  // quantum++
                    listeExecution[0].tempsRestant--;
                    indices[0] = AjouterTous(tempsDebut, indices[0], niveaux, listeGeneral, indiceNiveau);  // ajouter si il y a un processus qui a arrivé
                    
                    if (listeExecution[0].tempsRestant == 0)  // on a terminé ce processus
                    {
                        listeExecution[0].tempsFin = tempsDebut;
                        listeExecution[0].tempsService = tempsDebut - listeExecution[0].tempsArriv;
                        listeExecution[0].tempsAtt = listeExecution[0].tempsService - listeExecution[0].duree;
                        // maitenant on le supprime de la liste d'execution
                        listeExecution.RemoveAt(0);
                        indices[1] = 0;  // un nouveau quantum va commencer
                    }
                    else if (indices[1] == quantum)  // on a terminé ce quantum, donc il faut passer au suivant processus, donc on defile et enfile le processus courant
                    {
                        listeExecution[0].tempsFin = tempsDebut;  // sauvegarder quand on a arrété l'execution de ce processus
                        indices[1] = 0;  // nouveau quantum
                        listeExecution.Add(listeExecution[0]);  // enfiler à la fin
                        listeExecution.RemoveAt(0);  // defiler 
                    }
                }
                if (tempsDebut == tempsFin)  // attent le fin de l'intervalle du temps
                {
                    return tempsFin;
                }
            }
            return tempsDebut;
        }


    }
}