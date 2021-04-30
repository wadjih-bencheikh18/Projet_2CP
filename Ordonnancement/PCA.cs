using System;
using System.Collections.Generic;

namespace Ordonnancement
{
    class PCA : Ordonnancement
    {
        public PCA() { }
        public int Executer()
        {
            SortListeProcessus(); //tri par ordre d'arrivé
            int temps = 0, indice = 0;
            bool sort = true;
            while ((indice < listeProcessus.Count || listeExecution.Count != 0))
            {
                if (listeExecution.Count == 0) sort = true; //les premiers processus arrivés => on fait le tri pour avoir la plus courte durée
                indice = AjouterTous(temps, indice);  //remplir list Execution
                if (sort == true && listeExecution.Count != 0)
                {
                    listeExecution.Sort(delegate (Processus x, Processus y) { return y.duree.CompareTo(x.duree); }); //tri par duree
                    sort = false;
                }
                temps++;
                if (listeExecution.Count != 0) //il y a des processus à exécuter
                {
                    listeExecution[0].tempsRestant--; //le processus est entrain de s'exécuter => décrémenter le tempsRestant
                    if (listeExecution[0].tempsRestant == 0) // fin d'exécution du processus 
                    {
                        listeExecution[0].tempsFin = temps;
                        listeExecution[0].tempsService = temps - listeExecution[0].tempsArriv;
                        listeExecution[0].tempsAtt = listeExecution[0].tempsService - listeExecution[0].duree;
                        listeExecution.RemoveAt(0);
                        // on tri les processus restants
                        if (listeExecution.Count != 0) sort = true;
                    }
                }
            }
            return temps;
        }

        // des algos pour utiliser dans MultiNiveaux
        public void InitPCA(List<Processus> listeProcessus, List<Processus> listeExecution)
        {
            this.listeProcessus = listeProcessus;
            this.listeExecution = listeExecution;
        }
        public int Executer(int tempsDebut, int tempsFin, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral)
        {
            SortListeProcessus(); //tri par ordre d'arrivé
            int temps = tempsDebut;
            while (listeExecution.Count != 0 && temps < tempsFin)
            {
                if (listeExecution[0]==listeProcessus[0] && niveaux[indiceNiveau].indice[2]==0) niveaux[indiceNiveau].indice[1] = 1; //les premiers processus arrivés => on fait le tri pour avoir la plus courte durée
                niveaux[indiceNiveau].indice[2] = 1;
                niveaux[indiceNiveau].indice[0] = AjouterTous(temps, niveaux[indiceNiveau].indice[0], niveaux, listeGeneral,indiceNiveau);
                if (niveaux[indiceNiveau].indice[1] == 1 && listeExecution.Count != 0)
                {
                    listeExecution.Sort(delegate (Processus x, Processus y) { return y.duree.CompareTo(x.duree); }); //tri par duree
                    niveaux[indiceNiveau].indice[1] = 0;
                }
                temps++;
                if (listeExecution.Count != 0) //il y a des processus à exécuter
                {
                    listeExecution[0].tempsRestant--; //le processus est entrain de s'exécuter => décrémenter le tempsRestant
                    Console.WriteLine(temps + "-" + listeExecution[0].id);
                    if (listeExecution[0].tempsRestant == 0) // fin d'exécution du processus 
                    {
                        listeExecution[0].tempsFin = temps;
                        listeExecution[0].tempsService = temps - listeExecution[0].tempsArriv;
                        listeExecution[0].tempsAtt = listeExecution[0].tempsService - listeExecution[0].duree;
                        listeExecution.RemoveAt(0);
                        // on tri les processus restants
                        if (listeExecution.Count != 0) niveaux[indiceNiveau].indice[1] = 1;
                    }
                }
            }
            return temps;
        }
    }
}