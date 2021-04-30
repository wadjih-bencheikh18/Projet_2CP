using System.Collections.Generic;

namespace Ordonnancement
{
    class PCA : Ordonnancement
    {
        public PCA() { }
        public int Executer()
        {
            SortListeProcessus(); //tri des processus par ordre d'arrivé
            int temps = 0, indice = 0;
            bool sort = true; //est à vrai si un tri par durée est necessaire
            while ((indice < listeProcessus.Count || listeExecution.Count != 0)) //s'il existe des processus non executés
            {
                if (listeExecution.Count == 0) sort = true; //les premiers processus arrivés => on fait le tri pour avoir la plus courte durée
                indice = AjouterTous(temps, indice);  //remplir listeExecution
                if (sort == true && listeExecution.Count != 0) //si un tri est necessaire et il y a des processus à executer
                {
                    listeExecution.Sort(delegate (Processus x, Processus y) { return y.duree.CompareTo(x.duree); }); //tri des processus de listeExecution par durée
                    sort = false; //le tri par durée n'est plus necessaire (déja fait)
                }
                temps++; //incrementer le temps réel
                if (listeExecution.Count != 0) //il y a des processus à exécuter
                {
                    listeExecution[0].tempsRestant--; //execution du 1er processus de listeExecution => décrémenter le tempsRestant
                    AfficheLigne(temps - 1, listeExecution[0].id); //affiche le temps actuel et l'ID du processus entrain d'être executé
                    if (listeExecution[0].tempsRestant == 0) //si l'execution du premier processus de listeExecution est terminée
                    {
                        listeExecution[0].tempsFin = temps; //temps de fin d'execution = au temps actuel
                        listeExecution[0].tempsService = temps - listeExecution[0].tempsArriv; //temps de service = temps de fin d'execution - temps d'arrivé
                        listeExecution[0].tempsAtt = listeExecution[0].tempsService - listeExecution[0].duree; //temps d'attente = temps de service - durée d'execution
                        listeExecution.RemoveAt(0); //supprimer le premier processus executé
                        if (listeExecution.Count != 0) sort = true; //donc il faut trier les processus restants dans listeExecution
                    }
                }
                else AfficheLigne(temps - 1); //affiche le temps actuel et le mot "repos" ie le processeur n'execute aucun processus
            }
            return temps;
        }

        // à utiliser dans MultiNiveaux
        public void InitPCA(List<Processus> listeProcessus, List<Processus> listeExecution)
        {
            this.listeProcessus = listeProcessus;
            this.listeExecution = listeExecution;
        }
        public int Executer(int tempsDebut, int tempsFin, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral)
        {
            int temps = tempsDebut;
            while (listeExecution.Count != 0 && temps < tempsFin) //s'il existe des processus non executés et le temps < le temps de fin
            {
                if (listeExecution[0]==listeProcessus[0] && niveaux[indiceNiveau].indice[2]==0) niveaux[indiceNiveau].indice[1] = 1; //
                niveaux[indiceNiveau].indice[2] = 1; //
                niveaux[indiceNiveau].indice[0] = AjouterTous(temps, niveaux[indiceNiveau].indice[0], niveaux, listeGeneral,indiceNiveau); //remplir la liste d'execution de chaque niveau
                if (niveaux[indiceNiveau].indice[1] == 1 && listeExecution.Count != 0) //
                {
                    listeExecution.Sort(delegate (Processus x, Processus y) { return y.duree.CompareTo(x.duree); }); //tri des processus par durée
                    niveaux[indiceNiveau].indice[1] = 0; //
                }
                temps++; //incrementer le temps réel
                if (listeExecution.Count != 0) //il y a des processus à exécuter
                {
                    listeExecution[0].tempsRestant--; //le processus est entrain de s'exécuter => décrémenter le tempsRestant
                    AfficheLigne(temps - 1, listeExecution[0].id); //affiche le temps actuel et l'ID du processus entrain d'être executé
                    if (listeExecution[0].tempsRestant == 0) // fin d'exécution du processus 
                    {
                        listeExecution[0].tempsFin = temps; //temps de fin d'execution = au temps actuel
                        listeExecution[0].tempsService = temps - listeExecution[0].tempsArriv; //temps de service = temps de fin d'execution - temps d'arrivé
                        listeExecution[0].tempsAtt = listeExecution[0].tempsService - listeExecution[0].duree;  //temps d'attente = temps de service - durée d'execution
                        listeExecution.RemoveAt(0); //supprimer le premier processus executé
                        if (listeExecution.Count != 0) niveaux[indiceNiveau].indice[1] = 1; //il faut trier les processus restants dans listeExecution
                    }
                }
            }
            return temps;
        }
    }
}