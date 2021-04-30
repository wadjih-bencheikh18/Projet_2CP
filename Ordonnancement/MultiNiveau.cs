using System;
using System.Collections.Generic;

namespace Ordonnancement
{
    class Niveau
    {
        public List<Processus> listeProcessus = new List<Processus>();
        public List<Processus> listeExecution = new List<Processus>();
        public Ordonnancement algo;
        public int numAlgo; //0 PAPS  1 SJF  2 PRIO 3 RR
        public int[] indice = new int[4];
        public Niveau(int numAlgo)
        {
            this.numAlgo = numAlgo;
            switch (this.numAlgo)
            {
                case 0:
                    algo = new PAPS();
                    break;
                case 1:
                    algo = new PCA();
                    break;
                case 2:
                    algo = new PSP();
                    break;
                default:
                    Console.WriteLine("we need quantum");
                    break;
            }
        }
        public Niveau(int numAlgo, int quantum)
        {
            this.numAlgo = numAlgo;
            switch (this.numAlgo)
            {
                case 0:
                    algo = new PAPS();
                    break;
                case 1:
                    algo = new PCA();
                    break;
                case 2:
                    algo = new PSP();
                    break;
                case 3:
                    algo = new RoundRobin(quantum);
                    break;
                default:
                    Console.WriteLine("Error");
                    break;
            }
        }
    }
    class ProcessusNiveau : Processus
    {
        public int niveau;
        public ProcessusNiveau(int id, int tempsArriv, int duree, int prio, int niveau) : base(id, tempsArriv, duree, prio)
        {
            this.niveau = niveau;
        }
        public override void Affichage()
        {
            base.Affichage();
            Console.Write("\tNiveau : " + niveau);
        }
    }
    class MultiNiveau : Ordonnancement
    {
        protected new List<ProcessusNiveau> listeProcessus = new List<ProcessusNiveau>();
        private int nbNiveau;
        private Niveau[] niveaux;

        public MultiNiveau(int nbNiveau, Niveau[] niveaux)
        {
            this.nbNiveau = nbNiveau;
            this.niveaux = new Niveau[nbNiveau];
            this.niveaux = niveaux;
        }
        public override void Affichage()
        {
            for (int i = 0; i < listeProcessus.Count; i++) listeProcessus[i].Affichage();
        }
        public void InitNiveaux()
        {
            for (int i = 0; i < listeProcessus.Count; i++)
                niveaux[listeProcessus[i].niveau].listeProcessus.Add(listeProcessus[i]);
        }
        public void Push(ProcessusNiveau pro) //ajout d'un processus à la liste P
        {
            listeProcessus.Add(pro);
        }
        public override void SortListeProcessus()
        {
            listeProcessus.Sort(delegate (ProcessusNiveau x, ProcessusNiveau y) { return x.tempsArriv.CompareTo(y.tempsArriv); }); //tri par ordre d'arrivé
        }
        public int Executer()
        {
            SortListeProcessus();  //ordoner la liste des processus
            InitNiveaux();   // remplir la liste des processus dans chaque niveau
            int temps = 0, indice = 0, indiceNiveau = 0, tempsFin;
            while (indice < listeProcessus.Count || indiceNiveau < nbNiveau)
            {
                indice = AjouterTous(temps, indice);  //remplir les niveaux
                for (indiceNiveau = 0; indiceNiveau < nbNiveau && niveaux[indiceNiveau].listeExecution.Count == 0; indiceNiveau++) ; //la recherche de permier niveau non vide
                if (indiceNiveau < nbNiveau)  //il exist un niveau non vide
                {
                    tempsFin = TempsFin(indice, indiceNiveau);  //calcule de temps fin
                    niveaux[indiceNiveau].indice[0] = indice;   //sauvgarder indice dans le niveau
                    temps = NiveauExecute(temps, tempsFin, niveaux, indiceNiveau, listeProcessus);  //executer le niveau "indiceNiveau"
                    indice = niveaux[indiceNiveau].indice[0];  //recuperer l'indice 
                }
                else
                {
                    if (indice < listeProcessus.Count) AfficheLigne(temps);  //afficher repos (aucun niveau est executer)
                    temps++;  
                }
            }
            return temps;

        }
        public int NiveauExecute(int temps,int tempsFin,Niveau[] niveaux,int indiceNiveau, List<ProcessusNiveau> listeProcessus)  //executer le niveau "indiceNiveau"
        {
            switch (niveaux[indiceNiveau].numAlgo)
            {
                case 0:
                    ((PAPS)niveaux[indiceNiveau].algo).InitPAPS(niveaux[indiceNiveau].listeProcessus, niveaux[indiceNiveau].listeExecution);
                    temps = ((PAPS)niveaux[indiceNiveau].algo).Executer(temps, tempsFin, niveaux, indiceNiveau, listeProcessus);
                    break;
                case 1:
                    ((PCA)niveaux[indiceNiveau].algo).InitPCA(niveaux[indiceNiveau].listeProcessus, niveaux[indiceNiveau].listeExecution);
                    temps = ((PCA)niveaux[indiceNiveau].algo).Executer(temps, tempsFin, niveaux, indiceNiveau, listeProcessus);
                    break;
                case 2:
                    ((PSP)niveaux[indiceNiveau].algo).InitPSP(niveaux[indiceNiveau].listeProcessus, niveaux[indiceNiveau].listeExecution);
                    temps = ((PSP)niveaux[indiceNiveau].algo).Executer(temps, tempsFin, niveaux, indiceNiveau, listeProcessus);
                    break;
                case 3:
                    ((RoundRobin)niveaux[indiceNiveau].algo).InitRoundRobin(niveaux[indiceNiveau].listeProcessus, niveaux[indiceNiveau].listeExecution);
                    temps = ((RoundRobin)niveaux[indiceNiveau].algo).Executer(temps, tempsFin, niveaux[indiceNiveau].indice, niveaux, indiceNiveau, listeProcessus);
                    break;
                default:
                    Console.WriteLine("Error");
                    break;
            }
            return temps;
        }
        public override int AjouterTous(int temps, int indice)  // collecter tous les processus a partit de "listeProcessus" (liste ordonnée) où leur temps d'arrivé est <= le temps réel d'execution, et les ajouter à la liste d'execution de chaque niveau
        {
            for (; indice < listeProcessus.Count; indice++)
            {
                if (listeProcessus[indice].tempsArriv > temps) break;
                else
                {
                    niveaux[listeProcessus[indice].niveau].listeExecution.Add(listeProcessus[indice]);
                }
            }
            return indice;
        }
        public int TempsFin(int indice, int i)   //calcule de temps fin d'execution ( si il n ya pas de temps fin alors return -1)
        {
            for (; indice < listeProcessus.Count; indice++)
            {
                if (listeProcessus[indice].niveau < i) break;
            }
            if (indice < listeProcessus.Count) return listeProcessus[indice].tempsArriv;
            else return -1;
        }
    }
}