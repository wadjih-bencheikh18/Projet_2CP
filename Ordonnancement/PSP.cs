using System.Collections.Generic;

namespace Ordonnancement
{
    class PSP : Ordonnancement
    {
        public PSP() { }
        public int Executer()  // executer la liste des processus 
        {
            SortListeProcessus(); //tri des processus par ordre d'arrivé
            int temps = 0;  
            int indice = 0;
            while (listePrets.Count != 0 || indice < listeProcessus.Count) //Tant qu'il existe des processus prêts
            {
                indice = AjouterTous(temps, indice);  // Remplir listePrets
                temps++;
                if (listePrets.Count != 0) //S'il y a des processus prêts
                {
                    listePrets.Sort(delegate (Processus x, Processus y)
                                           {
                                               if (x.prio.CompareTo(y.prio) == 0) return x.tempsArriv.CompareTo(y.tempsArriv); //si les processus ont la même priorité, on les trie selon le temps d'arrivée
                                               else return x.prio.CompareTo(y.prio); //sinon, on fait le tri par priorité
                                           }
                                        );
                    listePrets[0].tempsRestant--; //L'exécution courante du 1er processus de listePrets => décrémenter tempsRestant
                    AfficheLigne(temps - 1, listePrets[0].id); //affiche le temps et l'ID du processus entrain d'être executé
                    if (listePrets[0].tempsRestant == 0)  // Si l'execution du premier processus de listePrets est terminée :
                    {
                        listePrets[0].tempsFin = temps; // temps de fin d'execution = temps actuel
                        listePrets[0].tempsService = temps - listePrets[0].tempsArriv; // temps de service = temps de fin d'execution - temps d'arrivé
                        listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree; //temps d'attente = temps de service - durée d'execution
                        listePrets.RemoveAt(0); //supprimer le processus dont la duree est écoulée
                    }
                }
                else AfficheLigne(temps - 1); //affiche le temps actuel et le mot "repos" ie le processeur n'execute aucun processus
            }
            return temps;
        }

        // des algorithmes nécessaires pour implémenter MultiNiveaux
        public void InitPSP(List<Processus> listeProcessus, List<Processus> listeExecution) //Initialiser listProcessus et listePrets
        {
            this.listeProcessus = listeProcessus;
            listePrets = listeExecution;
        }
        public int Executer(int tempsDebut, int tempsFin, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral)
        {
            int temps = tempsDebut;  // initialisation du temps
            while (listePrets.Count != 0 && (temps < tempsFin || tempsFin == -1))
            //s'il existe des processus prêts et ( On n'est pas encore arrivé à tempsFin ou il n'y a pas de temps fin )
            {
                niveaux[indiceNiveau].indice[0] = AjouterTous(temps, niveaux[indiceNiveau].indice[0], niveaux, listeGeneral, indiceNiveau);  //remplir la liste des processus prêts de chaque niveau
                temps++;
                if (listePrets.Count != 0) //S'il y a des processus prêts
                {
                    listePrets.Sort(
                                           delegate (Processus x, Processus y)
                                           {
                                               if (x.prio.CompareTo(y.prio) == 0) return x.tempsArriv.CompareTo(y.tempsArriv); //si les processus ont la même priorité, on les trie selon le temps d'arrivée
                                               else return x.prio.CompareTo(y.prio); //sinon, on fait le tri par priorité
                                           }
                                        );
                    listePrets[0].tempsRestant--;//L'exécution courante du 1er processus de listePrets => décrémenter tempsRestant
                    AfficheLigne(temps - 1, listePrets[0].id); //affiche le temps et l'ID du processus entrain d'être executé
                    if (listePrets[0].tempsRestant == 0) // Si l'execution du premier processus de listePrets est terminée :
                    {
                        listePrets[0].tempsFin = temps; // temps de fin d'execution = temps actuel
                        listePrets[0].tempsService = temps - listePrets[0].tempsArriv; // temps de service = temps de fin d'execution - temps d'arrivé
                        listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree; //temps d'attente = temps de service - durée d'execution
                        listePrets.RemoveAt(0); //supprimer le processus dont la duree est écoulée
                    }
                }
            }
            return temps;
        }
    }

}