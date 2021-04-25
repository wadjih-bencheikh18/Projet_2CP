using System.Collections.Generic;

namespace Ordonnancement
{
    partial class PAPS : Ordonnancement
    {
        public PAPS(List<Processus> listeProcessus, List<Processus> listeExecution)
        {
            this.listeProcessus = listeProcessus;
            this.listeExecution = listeExecution;
        }
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
                    for (int i = 1; i < listeExecution.Count; i++) listeExecution[i].tempsAtt++; //incrementer le temps d'attente des processus de listeExecution à partir du 2eme 
                    if (listeExecution[0].tempsRestant == 0) //si l'execution du premier processus de listeExecution est terminée
                    {
                        listeExecution[0].tempsFin = temps; //temps de fin d'execution = au temps actuel
                        listeExecution[0].tempsService = temps - listeExecution[0].tempsArriv; //temps de service = temps de fin d'execution - temps d'arrivé
                        listeExecution.RemoveAt(0); //supprimer le premier processus executé
                    }
                }
            }
            return temps;
        }
        public int Executer(int tempsDebut, int tempsFin, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral)
        {
            SortListeProcessus(); //tri des processus par ordre d'arrivé
            int temps = tempsDebut;
            while (listeExecution.Count != 0 && temps < tempsFin) //s'il existe des processus non executés et le temps < le temps de fin
            {
                niveaux[indiceNiveau].indice[0] = AjouterTous(temps,niveaux[indiceNiveau].indice[0],niveaux,listeGeneral);
                //indice[0] = AjouterTous(temps, indice[0]); //remplir listeExecution
                temps++; //incrementer le temps réel
                if (listeExecution.Count != 0) //s'il y a des processus à executer
                {
                    listeExecution[0].tempsRestant--; //l'execution du 1er processus de listeExecution commence
                    for (int i = 1; i < listeExecution.Count; i++) listeExecution[i].tempsAtt++; //incrementer le temps d'attente des processus de listeExecution à partir du 2eme 
                    if (listeExecution[0].tempsRestant == 0) //si l'execution du premier processus de listeExecution est terminée
                    {
                        listeExecution[0].tempsFin = temps; //temps de fin d'execution = au temps actuel
                        listeExecution[0].tempsService = temps - listeExecution[0].tempsArriv; //temps de service = temps de fin d'execution - temps d'arrivé
                        listeExecution.RemoveAt(0); //supprimer le premier processus executé
                    }
                }
            }
            return temps;
        }
        public int AjouterTous(int temps, int indice) //ajouter à la liste d'execution tous les processus de "listeProcessus" (liste ordonnée) dont le temps d'arrivé est <= au temps réel d'execution
        {
            for (; indice < listeProcessus.Count; indice++) //parcours de listeProcessus à partir du processus d'indice "indice"
            {
                if (listeProcessus[indice].tempsArriv > temps) break; //si le processus n'est pas encore arrivé on sort
                else listeExecution.Add(listeProcessus[indice]); //sinon on ajoute le processus à la liste d'execution et on passe au suivant
            }
            return indice;
        }
        public int AjouterTous(int temps, int indice,Niveau[] niveaux, List<ProcessusNiveau> listeGeneral)  // collecter tous les processus a partit de "listeProcessus" (liste ordonnée) où leur temps d'arrivé est <= le temps réel d'execution, et les ajouter à la liste d'execution 
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