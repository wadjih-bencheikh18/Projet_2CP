using System.Collections.Generic;

namespace Ordonnancement
{
    class PSP : Ordonnancement
    {
        public PSP() { }
        public int Executer()  // executer la liste des processus 
        {
            SortListeProcessus(); //tri par ordre d'arrivé
            int temps = 0;  // horloge
            int indice = 0;
            while (listeExecution.Count != 0 || indice < listeProcessus.Count)
            {
                indice = AjouterTous(temps, indice);  //remplir list Execution
                temps++;
                if (listeExecution.Count != 0)
                {
                    listeExecution.Sort(delegate (Processus x, Processus y)
                                           {
                                               if (x.prio.CompareTo(y.prio) == 0) return x.tempsArriv.CompareTo(y.tempsArriv); //si priorité egale, on trie par arrivé
                                               else return x.prio.CompareTo(y.prio); //sinon, tri par priorité
                                           }
                                        );
                    listeExecution[0].tempsRestant--;
                    AfficheLigne(temps - 1, listeExecution[0].id);
                    if (listeExecution[0].tempsRestant == 0) // si le temps restant de l'execution = 0
                    {
                        listeExecution[0].tempsFin = temps; // temps de fin d'execution
                        listeExecution[0].tempsService = temps - listeExecution[0].tempsArriv; // temps de service d'execution
                        listeExecution[0].tempsAtt = listeExecution[0].tempsService - listeExecution[0].duree;
                        listeExecution.RemoveAt(0); //suprimer le processus lequel sa duree est ecoulé
                    }
                }
                else AfficheLigne(temps - 1);
            }
            return temps;
        }

        // des algos pour utiliser dans MultiNiveaux
        public void InitPSP(List<Processus> listeProcessus, List<Processus> listeExecution)
        {
            this.listeProcessus = listeProcessus;
            this.listeExecution = listeExecution;
        }
        public int Executer(int tempsDebut, int tempsFin, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral)  // executer la liste des processus 
        {
            int temps = tempsDebut;  // horloge
            while (listeExecution.Count != 0 && temps < tempsFin)
            {
                niveaux[indiceNiveau].indice[0] = AjouterTous(temps, niveaux[indiceNiveau].indice[0], niveaux, listeGeneral, indiceNiveau);
                temps++;
                if (listeExecution.Count != 0)
                {
                    listeExecution.Sort(
                                           delegate (Processus x, Processus y)
                                           {
                                               if (x.prio.CompareTo(y.prio) == 0) return x.tempsArriv.CompareTo(y.tempsArriv); //si priorité egale
                                               else return x.prio.CompareTo(y.prio); //tri par priorité
                                           }
                                        );
                    listeExecution[0].tempsRestant--;
                    AfficheLigne(temps - 1, listeExecution[0].id);
                    if (listeExecution[0].tempsRestant == 0) // si le temps restant de l'execution = 0
                    {
                        listeExecution[0].tempsFin = temps; // temps de fin d'execution
                        listeExecution[0].tempsService = temps - listeExecution[0].tempsArriv; // temps de service d'execution
                        listeExecution[0].tempsAtt = listeExecution[0].tempsService - listeExecution[0].duree;
                        listeExecution.RemoveAt(0); //suprimer le processus lequel sa duree est ecoulé
                    }
                }
            }
            return temps;
        }
    }

}