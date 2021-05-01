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
            while (listePrets.Count != 0 || indice < listeProcessus.Count)
            {
                indice = AjouterTous(temps, indice);  //remplir listePrets
                temps++;
                if (listePrets.Count != 0)
                {
                    listePrets.Sort(delegate (Processus x, Processus y)
                                           {
                                               if (x.prio.CompareTo(y.prio) == 0) return x.tempsArriv.CompareTo(y.tempsArriv); //si priorité egale, on trie par arrivé
                                               else return x.prio.CompareTo(y.prio); //sinon, tri par priorité
                                           }
                                        );
                    listePrets[0].tempsRestant--;
                    AfficheLigne(temps - 1, listePrets[0].id);
                    if (listePrets[0].tempsRestant == 0) // si le temps restant de l'execution = 0
                    {
                        listePrets[0].tempsFin = temps; // temps de fin d'execution
                        listePrets[0].tempsService = temps - listePrets[0].tempsArriv; // temps de service d'execution
                        listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree;
                        listePrets.RemoveAt(0); //supprimer le processus lequel sa duree est ecoulé
                    }
                }
                else AfficheLigne(temps - 1);
            }
            return temps;
        }

        // à utiliser dans MultiNiveaux
        public void InitPSP(List<Processus> listeProcessus, List<Processus> listeExecution)
        {
            this.listeProcessus = listeProcessus;
            this.listePrets = listeExecution;
        }
        public int Executer(int tempsDebut, int tempsFin, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral)  // executer la liste des processus 
        {
            int temps = tempsDebut;  // horloge
            while (listePrets.Count != 0 && (temps < tempsFin || tempsFin == -1))
            {
                niveaux[indiceNiveau].indice[0] = AjouterTous(temps, niveaux[indiceNiveau].indice[0], niveaux, listeGeneral, indiceNiveau);
                temps++;
                if (listePrets.Count != 0)
                {
                    listePrets.Sort(
                                           delegate (Processus x, Processus y)
                                           {
                                               if (x.prio.CompareTo(y.prio) == 0) return x.tempsArriv.CompareTo(y.tempsArriv); //si priorité egale
                                               else return x.prio.CompareTo(y.prio); //tri par priorité
                                           }
                                        );
                    listePrets[0].tempsRestant--;
                    AfficheLigne(temps - 1, listePrets[0].id);
                    if (listePrets[0].tempsRestant == 0) // si le temps restant de l'execution = 0
                    {
                        listePrets[0].tempsFin = temps; // temps de fin d'execution
                        listePrets[0].tempsService = temps - listePrets[0].tempsArriv; // temps de service d'execution
                        listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree;
                        listePrets.RemoveAt(0); //supprimer le processus dont la duree est ecoulée
                    }
                }
            }
            return temps;
        }
    }

}