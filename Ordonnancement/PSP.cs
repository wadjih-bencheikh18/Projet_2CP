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
            while (listeExecution.Count != 0 || indice < listeProcessus.Count) //s'il existe des processus non exécutés
            {
                indice = AjouterTous(temps, indice);  // Remplir listExecution
                temps++;
                if (listeExecution.Count != 0) //S'il y a des processus à exécuter
                {
                    listePrets.Sort(delegate (Processus x, Processus y)
                                           {
                                               if (x.prio.CompareTo(y.prio) == 0) return x.tempsArriv.CompareTo(y.tempsArriv); //si les processus ont la même priorité, on les trie selon le temps d'arrivée
                                               else return x.prio.CompareTo(y.prio); //sinon, on fait le tri par priorité.
                                           }
                                        );
                    listeExecution[0].tempsRestant--; //L'exécution courante du 1er processus de listeExecution => décrémenter tempsRestant
                    AfficheLigne(temps - 1, listeExecution[0].id); //affiche le temps et l'ID du processus entrain d'être executé
                    if (listeExecution[0].tempsRestant == 0)  // Si l'execution du premier processus de listeExecution est terminée :
                    {
                        listeExecution[0].tempsFin = temps; // temps de fin d'execution = temps actuel
                        listeExecution[0].tempsService = temps - listeExecution[0].tempsArriv; // temps de service = temps de fin d'execution - temps d'arrivé
                        listeExecution[0].tempsAtt = listeExecution[0].tempsService - listeExecution[0].duree; //temps d'attente = temps de service - durée d'execution
                        listeExecution.RemoveAt(0); //suprimer le processus lequel sa duree est écoulée
                    }
                }
                else AfficheLigne(temps - 1); //affiche le temps actuel et le mot "repos" ie le processeur n'execute aucun processus
            }
            return temps;
        }

        // des algorithmes nécessaires pour implémenter MultiNiveaux
        public void InitPSP(List<Processus> listeProcessus, List<Processus> listeExecution) //Initialiser ListProcessus et  ListExecution
        {
            this.listeProcessus = listeProcessus;
            this.listePrets = listeExecution;
        }
        public int Executer(int tempsDebut, int tempsFin, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral)
        {
            int temps = tempsDebut;  // initialisation du temps
            while (listeExecution.Count != 0 && (temps < tempsFin || tempsFin == -1))
            //s'il existe des processus non executés et ( On n'a pas encore arrivé à tempsFin ou il n'y a pas de temps fin )
            {
                niveaux[indiceNiveau].indice[0] = AjouterTous(temps, niveaux[indiceNiveau].indice[0], niveaux, listeGeneral, indiceNiveau);
                temps++;
                if (listeExecution.Count != 0) //S'il y a des processus à exécuter
                {
                    listePrets.Sort(
                                           delegate (Processus x, Processus y)
                                           {
                                               if (x.prio.CompareTo(y.prio) == 0) return x.tempsArriv.CompareTo(y.tempsArriv); //si les processus ont la même priorité, on les trie selon le temps d'arrivée
                                               else return x.prio.CompareTo(y.prio); //sinon, on fait le tri par priorité.
                                           }
                                        );
                    listeExecution[0].tempsRestant--;//L'exécution courante du 1er processus de listeExecution => décrémenter tempsRestant
                    AfficheLigne(temps - 1, listeExecution[0].id); //affiche le temps et l'ID du processus entrain d'être executé
                    if (listeExecution[0].tempsRestant == 0) // Si l'execution du premier processus de listeExecution est terminée :
                    {
                        listeExecution[0].tempsFin = temps; // temps de fin d'execution = temps actuel
                        listeExecution[0].tempsService = temps - listeExecution[0].tempsArriv; // temps de service = temps de fin d'execution - temps d'arrivé
                        listeExecution[0].tempsAtt = listeExecution[0].tempsService - listeExecution[0].duree; //temps d'attente = temps de service - durée d'execution
                        listeExecution.RemoveAt(0); //suprimer le processus lequel sa duree est écoulée
                    }
                }
            }
            return temps;
        }
    }

}