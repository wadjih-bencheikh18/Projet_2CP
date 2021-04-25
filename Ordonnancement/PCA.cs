using System.Collections.Generic;

namespace Ordonnancement
{
    class PCA : Ordonnancement
    {
        public PCA(List<Processus> listeProcessus, List<Processus> listeExecution)
        {
            this.listeProcessus = listeProcessus;
            this.listeExecution = listeExecution;
        }
        public PCA() { }
        public int Executer()
        {
            SortListeProcessus(); //tri par ordre d'arrivé
            int temps = 0, indice = 0;
            bool sort=true;
            while ((indice < listeProcessus.Count || listeExecution.Count != 0))
            {
                if (listeExecution.Count == 0) sort = true; //les premiers processus arrivés => on fait le tri pour avoir la plus courte durée
                indice = AjouterTous(temps, indice);
                if (sort == true && listeExecution.Count != 0)
                {
                    listeExecution.Sort(delegate (Processus x, Processus y) { return y.duree.CompareTo(x.duree); }); //tri par duree
                    sort = false;
                }
                temps++;
                if (listeExecution.Count != 0) //il y a des processus à exécuter
                {
                    listeExecution[0].tempsRestant--; //le processus est entrain de s'exécuter => décrémenter le tempsRestant
                    for (int i = 1; i < listeExecution.Count; i++) listeExecution[i].tempsAtt++;  //incrementer le tempsAtt pour les autres processus
                    if (listeExecution[0].tempsRestant == 0) // fin d'exécution du processus 
                    {
                        listeExecution[0].tempsFin = temps;
                        listeExecution[0].tempsService = temps - listeExecution[0].tempsArriv;
                        listeExecution.RemoveAt(0);
                        // on tri les processus restants
                        if (listeExecution.Count != 0) sort = true;
                    }
                }
            }
            return temps;
        }
        public int Executer(int tempsDebut, int tempsFin, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral)
        {
            SortListeProcessus(); //tri par ordre d'arrivé
            int temps = tempsDebut;
            while (listeExecution.Count != 0 && temps < tempsFin)
            {
                if (listeExecution[0]==listeProcessus[0] && niveaux[indiceNiveau].indice[2]==0) niveaux[indiceNiveau].indice[1] = 1; //les premiers processus arrivés => on fait le tri pour avoir la plus courte durée
                niveaux[indiceNiveau].indice[2] = 1;
                niveaux[indiceNiveau].indice[0] = AjouterTous(temps, niveaux[indiceNiveau].indice[0], niveaux, listeGeneral);
                if (niveaux[indiceNiveau].indice[1] == 1 && listeExecution.Count != 0)
                {
                    listeExecution.Sort(delegate (Processus x, Processus y) { return y.duree.CompareTo(x.duree); }); //tri par duree
                    niveaux[indiceNiveau].indice[1] = 0;
                }
                temps++;
                if (listeExecution.Count != 0) //il y a des processus à exécuter
                {
                    listeExecution[0].tempsRestant--; //le processus est entrain de s'exécuter => décrémenter le tempsRestant
                    for (int i = 1; i < listeExecution.Count; i++) listeExecution[i].tempsAtt++;  //incrementer le tempsAtt pour les autres processus
                    if (listeExecution[0].tempsRestant == 0) // fin d'exécution du processus 
                    {
                        listeExecution[0].tempsFin = temps;
                        listeExecution[0].tempsService = temps - listeExecution[0].tempsArriv;
                        listeExecution.RemoveAt(0);
                        // on tri les processus restants
                        if (listeExecution.Count != 0) niveaux[indiceNiveau].indice[1] = 1;
                    }
                }
            }
            return temps;
        }
        public int AjouterTous(int temps, int indice)  // collecter tous les processus a partit de "listeProcessus" (liste ordonnée) où leur temps d'arrivé est <= le temps réel d'execution, et les ajouter à la liste d'execution 
        {
            for (; indice < listeProcessus.Count; indice++)
            {
                if (listeProcessus[indice].tempsArriv > temps) break;
                else listeExecution.Add(listeProcessus[indice]);
            }
            return indice;
        }
        public int AjouterTous(int temps, int indice, Niveau[] niveaux, List<ProcessusNiveau> listeGeneral)  // collecter tous les processus a partit de "listeProcessus" (liste ordonnée) où leur temps d'arrivé est <= le temps réel d'execution, et les ajouter à la liste d'execution 
        {
            for (; indice < listeProcessus.Count; indice++)
            {
                if (listeProcessus[indice].tempsArriv > temps) break;
                else
                {
                    //listeExecution.Add(listeProcessus[indice]);
                    niveaux[listeGeneral[indice].niveau].listeExecution.Add(listeGeneral[indice]);
                }
            }
            return indice;
        }
    }
}