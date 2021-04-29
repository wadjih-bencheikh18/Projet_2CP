﻿using System;
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
                    Console.WriteLine(temps + "\t|\t " + listeExecution[0].id+ "\t\t\t|");
                    for (int i = 1; i < listeExecution.Count; i++) listeExecution[i].tempsAtt++; //incrementer le temps d'attente des processus de listeExecution à partir du 2eme 
                    if (listeExecution[0].tempsRestant == 0) //si l'execution du premier processus de listeExecution est terminée
                    {
                        listeExecution[0].tempsFin = temps; //temps de fin d'execution = au temps actuel
                        listeExecution[0].tempsService = temps - listeExecution[0].tempsArriv; //temps de service = temps de fin d'execution - temps d'arrivé
                        listeExecution[0].tempsAtt = listeExecution[0].tempsService - listeExecution[0].duree;
                        listeExecution.RemoveAt(0); //supprimer le premier processus executé
                    }
                }
                else Console.WriteLine(temps + "\t|\t Ropos\t\t\t|");
            }
            return temps;
        }



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
                niveaux[indiceNiveau].indice[0] = AjouterTous(temps,niveaux[indiceNiveau].indice[0],niveaux,listeGeneral,indiceNiveau); //remplir listeExecution
                temps++; //incrementer le temps réel
                if (listeExecution.Count != 0) //s'il y a des processus à executer
                {
                    listeExecution[0].tempsRestant--; //l'execution du 1er processus de listeExecution commence
                    Console.WriteLine(temps + "-" + listeExecution[0].id);
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