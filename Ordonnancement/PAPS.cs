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
            while (indice < listeProcessus.Count || listeExecution.Count != 0) //s'il existe des processus non executés et le temps < le temps de fin
            {
                indice = AjouterTous(temps, indice); //remplir listeExecution
                temps++; //incrementer le temps réel
                if (listeExecution.Count != 0) //s'il y a des processus à executer
                {
                    listeExecution[0].tempsRestant--; //l'execution du 1er processus de listeExecution commence
                    AfficheLigne(temps - 1, listeExecution[0].id);
                    if (listeExecution[0].tempsRestant == 0) //si l'execution du premier processus de listeExecution est terminée
                    {
                        listeExecution[0].tempsFin = temps; //temps de fin d'execution = au temps actuel
                        listeExecution[0].tempsService = temps - listeExecution[0].tempsArriv; //temps de service = temps de fin d'execution - temps d'arrivé
                        listeExecution[0].tempsAtt = listeExecution[0].tempsService - listeExecution[0].duree;
                        listeExecution.RemoveAt(0); //supprimer le premier processus executé
                    }
                }
                else AfficheLigne(temps - 1);
            }
            return temps;
        }


        // des algos pour utiliser dans MultiNiveaux
        public void InitPAPS(List<Processus> listeProcessus, List<Processus> listeExecution)
        {
            this.listeProcessus = listeProcessus;
            this.listeExecution = listeExecution;
        }
        public int Executer(int tempsDebut, int tempsFin, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral)
        {
            SortListeProcessus(); //tri des processus par ordre d'arrivé
            int temps = tempsDebut;
            while (listeExecution.Count != 0 && temps < tempsFin) //s'il existe des processus non executés et le temps < le temps de fin
            {
                niveaux[indiceNiveau].indice[0] = AjouterTous(temps, niveaux[indiceNiveau].indice[0], niveaux, listeGeneral, indiceNiveau); //remplir listeExecution
                temps++; //incrementer le temps réel
                if (listeExecution.Count != 0) //s'il y a des processus à executer
                {
                    listeExecution[0].tempsRestant--; //l'execution du 1er processus de listeExecution commence
                    AfficheLigne(temps - 1, listeExecution[0].id);
                    if (listeExecution[0].tempsRestant == 0) //si l'execution du premier processus de listeExecution est terminée
                    {
                        listeExecution[0].tempsFin = temps; //temps de fin d'execution = au temps actuel
                        listeExecution[0].tempsService = temps - listeExecution[0].tempsArriv; //temps de service = temps de fin d'execution - temps d'arrivé
                        listeExecution[0].tempsAtt = listeExecution[0].tempsService - listeExecution[0].duree;
                        listeExecution.RemoveAt(0); //supprimer le premier processus executé
                    }
                }
            }
            return temps;
        }
    }
}