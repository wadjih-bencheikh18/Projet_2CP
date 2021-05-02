using System.Collections.Generic;

namespace Ordonnancement
{
    partial class PAPS : Ordonnancement
    {

        public PAPS() { }
        public int Executer()
        {
            SortListeProcessus(); //tri des processus par ordre d'arrivé
            int temps = 0, indice = 0;
            while (indice < listeProcessus.Count || processusActif.tempsRestant != 0) //s'il existe des processus non executés
            {
                indice = AjouterTous(temps, indice); //remplir listePrets
                temps++; //incrementer le temps réel
                if (listePrets.Count != 0 && processusActif.tempsRestant == 0)
                {
                    processusActif = listePrets[0];
                    listePrets.RemoveAt(0); //supprimer le premier processus executé
                }
                if (processusActif.tempsRestant != 0) //s'il y a des processus prêts
                {
                    if (processusActif.tempsRestant == processusActif.duree) processusActif.tempsReponse = temps - 1 - processusActif.tempsArriv;
                    processusActif.tempsRestant--; //l'execution du 1er processus de listePrets commence
                    AfficheLigne(temps - 1, processusActif.id); //affiche le temps actuel et l'ID du processus entrain d'être executé
                    if (processusActif.tempsRestant == 0) //si l'execution du premier processus de listePrets est terminée
                    {
                        processusActif.tempsFin = temps; //temps de fin d'execution = au temps actuel
                        processusActif.tempsService = temps - processusActif.tempsArriv; //temps de service = temps de fin d'execution - temps d'arrivé
                        processusActif.tempsAtt = processusActif.tempsService - processusActif.duree; //temps d'attente = temps de service - durée d'execution
                        if (listePrets.Count != 0)
                        {
                            processusActif = listePrets[0];
                            listePrets.RemoveAt(0); //supprimer le premier processus executé
                        }
                    }
                }
                else
                {
                    AfficheLigne(temps - 1); //affiche le temps actuel et le mot "repos" ie le processeur n'execute aucun processus
                }
            }
            return temps;
        }

        // à utiliser dans MultiNiveaux

        public int Executer(int tempsDebut, int tempsFin, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral)
        {
            int temps = tempsDebut;

            processusActif = listePrets[0];
            listePrets.RemoveAt(0); //supprimer le premier processus executé
            while (processusActif.tempsRestant != 0) //s'il existe des processus prêts et le temps < le temps de fin d'execution ou il n'y a pas de temps fin d'execution
            {
                niveaux[indiceNiveau].indice[0] = AjouterTous(temps, niveaux[indiceNiveau].indice[0], niveaux, listeGeneral, indiceNiveau); //remplir la liste des processus prêts de chaque niveau
                temps++; //incrementer le temps réel
                if (processusActif.tempsRestant == processusActif.duree) processusActif.tempsReponse = temps - 1 - processusActif.tempsArriv;
                processusActif.tempsRestant--; //l'execution du 1er processus de listePrets commence
                AfficheLigne(temps - 1, processusActif.id); //affiche le temps actuel et l'ID du processus entrain d'être executé
                if (processusActif.tempsRestant == 0) //si l'execution du premier processus de listePrets est terminée
                {
                    processusActif.tempsFin = temps; //temps de fin d'execution = au temps actuel
                    processusActif.tempsService = temps - processusActif.tempsArriv; //temps de service = temps de fin d'execution - temps d'arrivé
                    processusActif.tempsAtt = processusActif.tempsService - processusActif.duree;  //temps d'attente = temps de service - durée d'execution
                    if (listePrets.Count != 0)
                    {
                        processusActif = listePrets[0];
                        listePrets.RemoveAt(0); //supprimer le premier processus executé
                    }
                }

                if (temps == tempsFin)
                {
                    listePrets.Add(processusActif);
                    return tempsFin;
                }
            }
            return temps;
        }
    }
}