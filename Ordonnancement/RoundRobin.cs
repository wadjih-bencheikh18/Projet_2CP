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
            while (indice < listeProcessus.Count || listePrets.Count != 0)  // il reste un processus prêts
            {
                if (indice < listeProcessus.Count && listePrets.Count == 0)  // la liste des processus prêts est vide pour le moment
                {
                    if (temps < listeProcessus[indice].tempsArriv)  // si aucun processus est arrivant, donc horloge++
                    {
                        AfficheLigne(temps);
                        temps++;
                    }
                    else indice = AjouterTous(temps, indice);  // sinon on lui ajoute à la liste des processus prêts
                }
                else  // la liste des processus prêts n'est pas vide pour le moment
                {
                    AfficheLigne(temps, listePrets[0].id);
                    temps++;  // horloge++
                    q++;  // quantum++
                    listePrets[0].tempsRestant--;
                    indice = AjouterTous(temps, indice);  // ajouter si il y a un processus qui est arrivé
                    
                    if (listePrets[0].tempsRestant == 0)  // on a terminé ce processus
                    {
                        listePrets[0].tempsFin = temps;
                        listePrets[0].tempsService = temps - listePrets[0].tempsArriv;
                        listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree;
                        // maitenant on le supprime de la liste des processus prêts
                        listePrets.RemoveAt(0);
                        q = 0;  // un nouveau quantum va commencer
                    }
                    else if (q == quantum)  // on a terminé ce quantum, donc il faut passer au processus suivant, donc on defile et enfile à la fin le processus courant
                    {
                        listePrets[0].tempsFin = temps;  // sauvegarder quand on a arrété l'execution de ce processus
                        q = 0;  // nouveau quantum
                        listePrets.Add(listePrets[0]);  // enfiler à la fin
                        listePrets.RemoveAt(0);  // defiler 
                    }
                }
            }
            return temps;
        }

        // à utiliser dans MultiNiveaux
        public void InitRoundRobin(List<Processus> listeProcessus, List<Processus> listeExecution)
        {
            this.listeProcessus = listeProcessus;
            this.listePrets = listeExecution;
        }
        public int Executer(int tempsDebut, int tempsFin, int[] indices, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral)
        // executer l'algo pendant un intervalle de temps où indices[0] est l'indice de listeProcessus où on doit reprendre et indices[1] est le quantum du temps du dernier execution
        {
            while (indices[0] < listeProcessus.Count || listePrets.Count != 0)  // il y reste un processus à executer
            {
                if (indices[0] < listeProcessus.Count && listePrets.Count == 0)  // la liste des processus prêts est vide pour le moment
                {
                    tempsDebut++;  // si aucun processus est arrivant, donc horloge++
                    indices[0] = AjouterTous(tempsDebut, indices[0], niveaux, listeGeneral, indiceNiveau);  // sinon on lui ajoute à la liste des processus prêts
                }
                else  // la liste des processus prêts n'est pas vide pour le moment
                {
                    AfficheLigne(tempsDebut, listePrets[0].id);
                    tempsDebut++;  // horloge++
                    indices[1]++;  // quantum++
                    listePrets[0].tempsRestant--;
                    indices[0] = AjouterTous(tempsDebut, indices[0], niveaux, listeGeneral, indiceNiveau);  // ajouter si il y a un processus qui a arrivé
                    
                    if (listePrets[0].tempsRestant == 0)  // on a terminé ce processus
                    {
                        listePrets[0].tempsFin = tempsDebut;
                        listePrets[0].tempsService = tempsDebut - listePrets[0].tempsArriv;
                        listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree;
                        // maitenant on le supprime de la liste des processus prêts
                        listePrets.RemoveAt(0);
                        indices[1] = 0;  // un nouveau quantum va commencer
                    }
                    else if (indices[1] == quantum)  // on a terminé ce quantum, donc il faut passer au processus suivant, donc on defile et enfile le processus courant
                    {
                        listePrets[0].tempsFin = tempsDebut;  // sauvegarder quand on a arrété l'execution de ce processus
                        indices[1] = 0;  // nouveau quantum
                        listePrets.Add(listePrets[0]);  // enfiler à la fin
                        listePrets.RemoveAt(0);  // defiler 
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