using System.Collections.Generic;

namespace Ordonnancement
{
    class RoundRobin : Ordonnancement
    {
        private int quantum { get; }
        public RoundRobin(int q) // Constructeur 
        {
            quantum = q;
        }
        public int Executer()  // exécuter la liste des processus et retourner le temps total pour terminer l'exécution
        {
            SortListeProcessus(); //Tri de la liste des processus par temps d'arrivée
            int indice = 0, temps = 0, q = 0;
            while (indice < listeProcessus.Count || listeExecution.Count != 0)  //s'il existe des processus non executés
            {
                if (indice < listeProcessus.Count && listeExecution.Count == 0)  // Si il y a des processus dans listProcessus et la listExecution est vide
                {
                    if (temps < listeProcessus[indice].tempsArriv)  // si aucun processus n'est arrivé
                    {
                        AfficheLigne(temps); //affiche le temps actuel et le mot "repos", i.e le processeur n'execute aucun processus
                        temps++;
                    }
                    else indice = AjouterTous(temps, indice);  // sinon, on ajoute les processus arrivés à listExecution
                }
                else  // listExecution n'est pas vide 
                {
                    AfficheLigne(temps, listeExecution[0].id); //affiche le temps actuel et l'ID du processus entrain d'être executé
                    temps++;  
                    q++;  // on incrémente le quantum
                    listeExecution[0].tempsRestant--; //L'exécution courante du 1er processus de listeExecution => décrémenter tempsRestant
                    indice = AjouterTous(temps, indice);  // ajouter les processus arrivés à listExecution

                    if (listeExecution[0].tempsRestant == 0) //fin d'exécution du processus 
                    {
                        listeExecution[0].tempsFin = temps; // temps de fin d'execution = temps actuel
                        listeExecution[0].tempsService = temps - listeExecution[0].tempsArriv;// temps de service = temps de fin d'execution - temps d'arrivé
                        listeExecution[0].tempsAtt = listeExecution[0].tempsService - listeExecution[0].duree;  //temps d'attente = temps de service - durée d'execution
                        listeExecution.RemoveAt(0);//supprimer le premier processus executé
                        q = 0;  // un nouveau quantum va commencer
                    }
                    else if (q == quantum)  // on a terminé ce quantum => il faut passer au processus suivant => on defile, et à la fin, on enfile le processus courant
                    {
                        listeExecution[0].tempsFin = temps;  // On sauvegarde le tempsFin puisqu'on a interrompu l'exécution de ce processus
                        q = 0;  //Un nouveau quantum
                        listeExecution.Add(listeExecution[0]);  //Enfilement à la fin
                        listeExecution.RemoveAt(0);  // defiler 
                    }
                }
            }
            return temps;
        }

        // Des algorithmes nécessaires ^pour implémenter MultiNiveaux
        public void InitRoundRobin(List<Processus> listeProcessus, List<Processus> listeExecution)
        {
            this.listeProcessus = listeProcessus;
            this.listeExecution = listeExecution;
        }
        public int Executer(int tempsDebut, int tempsFin, int[] indices, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral)
        // executer l'algo pendant un intervalle de temps où :
        // indices[0] est l'indice de listeProcessus où on doit reprendre l'exécution
        // indices[1] est le quantum du temps de la derniére exécution
        {
            while (indices[0] < listeProcessus.Count || listeExecution.Count != 0)  //s'il existe des processus non executés
            {
                if (indices[0] < listeProcessus.Count && listeExecution.Count == 0)  // Si il y a des processus dans listProcessus et la listExecution est vide
                {
                    tempsDebut++;  // aucun processus n'est arrivé => on incrémente le temps
                    indices[0] = AjouterTous(tempsDebut, indices[0], niveaux, listeGeneral, indiceNiveau);  // On rempli listExecution
                }
                else  // listExecution n'est pas vide 
                {
                    AfficheLigne(tempsDebut, listeExecution[0].id); //affiche le temps actuel et l'ID du processus entrain d'être executé
                    tempsDebut++;
                    indices[1]++;  // quantum++
                    listeExecution[0].tempsRestant--; //L'exécution courante du 1er processus de listeExecution => décrémenter tempsRestant
                    indices[0] = AjouterTous(tempsDebut, indices[0], niveaux, listeGeneral, indiceNiveau);  // On rempli la listExecution
                    
                    if (listeExecution[0].tempsRestant == 0)  // fin d'exécution du processus 
                    {
                        listeExecution[0].tempsFin = tempsDebut;
                        listeExecution[0].tempsService = tempsDebut - listeExecution[0].tempsArriv;
                        listeExecution[0].tempsAtt = listeExecution[0].tempsService - listeExecution[0].duree;
                        listeExecution.RemoveAt(0); //supprimer le premier processus executé
                        indices[1] = 0;  // un nouveau quantum va commencer
                    }
                    else if (indices[1] == quantum)  // on a terminé ce quantum => il faut passer au processus suivant => on defile, et à la fin, on enfile le processus courant
                    {
                        listeExecution[0].tempsFin = tempsDebut;  // On sauvegarde le tempsFin puisqu'on a interrompu l'exécution de ce processus
                        indices[1] = 0;  // nouveau quantum
                        listeExecution.Add(listeExecution[0]);  // enfiler à la fin
                        listeExecution.RemoveAt(0);  // defiler 
                    }
                }
                if (tempsDebut == tempsFin)  // On est arrivé à tempsFin => la fin de l'exécution 
                {
                    return tempsFin;
                }
            }
            return tempsDebut;
        }


    }
}