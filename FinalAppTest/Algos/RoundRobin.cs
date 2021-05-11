using System.Collections.Generic;

namespace Ordonnancement
{
    public class RoundRobin : Ordonnancement
    {
        public int quantum { get; set; }

        public RoundRobin(int q) // Constructeur 
        {
            quantum = q;
        }

        public int Executer()  // exécuter la liste des processus et retourner le temps total pour terminer l'exécution
        {
            SortListeProcessus(); //Tri de la liste des processus par temps d'arrivée
            int indice = 0, temps = 0, q = 0;
            while (indice < listeProcessus.Count || listePrets.Count != 0)  //s'il existe des processus prêts
            {
                if (indice < listeProcessus.Count && listePrets.Count == 0)  // Si il y a des processus dans listeProcessus et la listePrets est vide
                {
                    if (temps < listeProcessus[indice].tempsArriv)  // si aucun processus n'est arrivé
                    {
                        AfficheLigne(temps); //affiche le temps actuel et le mot "repos", i.e le processeur n'execute aucun processus
                        temps++;
                    }
                    else indice = AjouterTous(temps, indice);  // sinon, on ajoute les processus arrivés à listePrets
                }
                else  // listePrets n'est pas vide 
                {
                    AfficheLigne(temps, listePrets[0].id); //affiche le temps actuel et l'ID du processus entrain d'être executé
                    temps++;
                    q++;  // on incrémente le quantum
                    if (listePrets[0].tempsRestant == listePrets[0].duree) listePrets[0].tempsReponse = temps - 1 - listePrets[0].tempsArriv;
                    listePrets[0].tempsRestant--; //L'exécution courante du 1er processus de listePrets => décrémenter tempsRestant
                    indice = AjouterTous(temps, indice);  // ajouter les processus arrivés à listePrets

                    if (listePrets[0].tempsRestant == 0) //fin d'exécution du processus 
                    {
                        listePrets[0].tempsFin = temps; // temps de fin d'execution = temps actuel
                        listePrets[0].tempsService = temps - listePrets[0].tempsArriv;// temps de service = temps de fin d'execution - temps d'arrivé
                        listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree;  //temps d'attente = temps de service - durée d'execution
                        listePrets[0].etat = 3;
                        listePrets.RemoveAt(0);//supprimer le premier processus executé
                        q = 0;  // un nouveau quantum va commencer
                    }
                    else if (q == quantum)  // on a terminé ce quantum => il faut passer au processus suivant => on defile, et à la fin, on enfile le processus courant
                    {
                        listePrets[0].tempsFin = temps;  // On sauvegarde le tempsFin puisqu'on a interrompu l'exécution de ce processus
                        q = 0;  //Un nouveau quantum
                        listePrets[0].etat = 1;
                        listePrets.Add(listePrets[0]);  //Enfilement à la fin
                        listePrets.RemoveAt(0);  // defiler 
                    }
                }
            }
            return temps;
        }

        // Des algorithmes nécessaires pour implémenter MultiNiveaux

        public int Executer(int tempsDebut, int tempsFin, int[] indices, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral)
        // executer l'algo pendant un intervalle de temps où :
        // indices[0] est l'indice de listeProcessus où on doit reprendre l'exécution
        // indices[1] est le quantum du temps de la derniére exécution
        {
            while (indices[0] < listeProcessus.Count || listePrets.Count != 0)  //tant qu'il existe des processus prêts
            {
                if (indices[0] < listeProcessus.Count && listePrets.Count == 0)  // Si il y a des processus dans listProcessus et listePrets est vide
                {
                    tempsDebut++;  // aucun processus n'est arrivé => on incrémente le temps
                    indices[0] = AjouterTous(tempsDebut, indices[0], niveaux, listeGeneral, indiceNiveau);  // On rempli listePrets
                }
                else  // listePrets n'est pas vide 
                {
                    AfficheLigne(tempsDebut, listePrets[0].id); //affiche le temps actuel et l'ID du processus entrain d'être executé
                    tempsDebut++;
                    indices[1]++;  // quantum++
                    listePrets[0].etat = 2;
                    if (listePrets[0].tempsRestant == listePrets[0].duree) listePrets[0].tempsReponse = tempsDebut - 1 - listePrets[0].tempsArriv;
                    listePrets[0].tempsRestant--; //L'exécution courante du 1er processus de listePrets => décrémenter tempsRestant
                    indices[0] = AjouterTous(tempsDebut, indices[0], niveaux, listeGeneral, indiceNiveau);  // On rempli listePrets

                    if (listePrets[0].tempsRestant == 0)  // fin d'exécution du processus 
                    {
                        listePrets[0].tempsFin = tempsDebut;
                        listePrets[0].tempsService = tempsDebut - listePrets[0].tempsArriv;
                        listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree;
                        listePrets[0].etat = 3;
                        listePrets.RemoveAt(0); //supprimer le premier processus executé
                        indices[1] = 0;  // un nouveau quantum va commencer
                    }
                    else if (indices[1] == quantum)  // on a terminé ce quantum => il faut passer au processus suivant => on defile, et à la fin, on enfile le processus courant
                    {
                        listePrets[0].tempsFin = tempsDebut;  // On sauvegarde le tempsFin puisqu'on a interrompu l'exécution de ce processus
                        indices[1] = 0;  // nouveau quantum
                        listePrets[0].etat = 1;
                        listePrets.Add(listePrets[0]);  // enfiler à la fin
                        listePrets.RemoveAt(0);  // defiler 
                    }
                }
                if (tempsDebut == tempsFin)  // On est arrivé à tempsFin => la fin de l'exécution 
                {
                    listePrets[0].etat = 1;
                    listePrets.Add(listePrets[0]);
                    listePrets.RemoveAt(0);
                    return tempsFin;
                }
            }
            return tempsDebut;
        }
    }
}