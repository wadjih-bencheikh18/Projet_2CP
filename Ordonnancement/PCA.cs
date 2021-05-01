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
            while ((indice < listeProcessus.Count || listePrets.Count != 0)) //s'il existe des processus non executés
            {
                if (listePrets.Count == 0) sort = true; //les premiers processus arrivés => on fait un tri par durée (croissant)
                indice = AjouterTous(temps, indice);  //remplir listePrets
                if (sort == true && listePrets.Count != 0) //si un tri par durée est necessaire et il y a des processus prêts
                {
                    listePrets.Sort(delegate (Processus x, Processus y) { return y.duree.CompareTo(x.duree); }); //tri des processus de listePrets par durée
                    sort = false; //le tri par durée n'est plus necessaire (déja fait)
                }
                temps++; //incrementer le temps réel
                if (listePrets.Count != 0) //il y a des processus prêts
                {
                    if (listePrets[0].tempsRestant == listePrets[0].duree) listePrets[0].tempsReponse = temps - 1 - listePrets[0].tempsArriv;
                    listePrets[0].tempsRestant--; //execution du 1er processus de listePrets et donc décrémenter le tempsRestant
                    AfficheLigne(temps - 1, listePrets[0].id); //affiche le temps actuel et l'ID du processus entrain d'être executé
                    if (listePrets[0].tempsRestant == 0) //si l'execution du premier processus de listePrets est terminée
                    {
                        listePrets[0].tempsFin = temps; //temps de fin d'execution = au temps actuel
                        listePrets[0].tempsService = temps - listePrets[0].tempsArriv; //temps de service = temps de fin d'execution - temps d'arrivé
                        listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree; //temps d'attente = temps de service - durée d'execution
                        listePrets.RemoveAt(0); //supprimer le premier processus executé
                        if (listePrets.Count != 0) sort = true; //donc il faut trier les processus restants dans listePrets
                    }
                }
                else AfficheLigne(temps - 1); //affiche le temps actuel et le mot "repos" ie le processeur n'execute aucun processus
            }
            return temps;
        }

        // à utiliser dans MultiNiveaux
        public int Executer(int tempsDebut, int tempsFin, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral)
        {
            /*dans cette methode on utilisera :
             * niveau[].indice[1] est à 1 si un tri par durée est necessaire, à 0 sinon (déja trié)
             * niveau[].indice[2] est à 1 si au moins un processus de ce niveau a été executé, à 0 sinon (aucun processus de ce niveau n'a été executé)*/

            int temps = tempsDebut;
            if (niveaux[indiceNiveau].indice[2] == 0) niveaux[indiceNiveau].indice[1] = 1; //si aucun processus du niveau actuel n'a été executé alors il faut trier les processus de listePrets de ce niveau par durée
            niveaux[indiceNiveau].indice[2] = 1; //l'execution d'un processus de ce niveau commence
            while (listePrets.Count != 0 && (temps < tempsFin || tempsFin == -1)) //s'il existe des processus prêts et le temps < le temps de fin  ou il n'y a pas de temps fin
            {
                niveaux[indiceNiveau].indice[0] = AjouterTous(temps, niveaux[indiceNiveau].indice[0], niveaux, listeGeneral,indiceNiveau); //remplir la liste des processus prêts de chaque niveau
                if (listePrets.Count != 0 && niveaux[indiceNiveau].indice[1] == 1) //s'il y a des processus prêts et un tri par durée est necessaire
                {
                    listePrets.Sort(delegate (Processus x, Processus y) { return y.duree.CompareTo(x.duree); }); //tri des processus par durée
                    niveaux[indiceNiveau].indice[1] = 0; //le tri par durée n'est plus necessaire (déja fait)
                }
                temps++; //incrementer le temps réel
                if (listePrets.Count != 0) //il y a des processus prêts
                {
                    if (listePrets[0].tempsRestant == listePrets[0].duree) listePrets[0].tempsReponse = temps - 1 - listePrets[0].tempsArriv;
                    listePrets[0].tempsRestant--; //le processus est entrain de s'exécuter donc on décrémente le tempsRestant
                    AfficheLigne(temps - 1, listePrets[0].id); //affiche le temps actuel et l'ID du processus entrain d'être executé
                    if (listePrets[0].tempsRestant == 0) // fin d'exécution du processus 
                    {
                        listePrets[0].tempsFin = temps; //temps de fin d'execution = au temps actuel
                        listePrets[0].tempsService = temps - listePrets[0].tempsArriv; //temps de service = temps de fin d'execution - temps d'arrivé
                        listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree;  //temps d'attente = temps de service - durée d'execution
                        listePrets.RemoveAt(0); //supprimer le premier processus executé
                        if (listePrets.Count != 0) niveaux[indiceNiveau].indice[1] = 1; //il faut trier les processus restants dans listePrets par durée
                    }
                }
            }
            return temps;
        }
    }
}