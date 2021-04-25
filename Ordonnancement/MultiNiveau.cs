using System.Collections.Generic;

namespace Ordonnancement
{
    class Niveau
    {
        public List<Processus> listeProcessus = new List<Processus>();
        public List<Processus> listeExecution = new List<Processus>();
        public Ordonnancement algo;
        public int numAlgo; //0 PAPS  1 SJF  2 PRIO 3 RR
        public int quantum; //a khemem
        public int indice;
    }
    class ProcessusNiveau : Processus
    {
        public int niveau;
        public ProcessusNiveau(int id, int tempsArriv, int duree, int prio, int niveau) : base(id, tempsArriv, duree, prio)
        {
            this.niveau = niveau;
        }
    }
    class MultiNiveau : Ordonnancement
    {
        protected new List<ProcessusNiveau> listeProcessus = new List<ProcessusNiveau>();
        public Ordonnancement[] algo=new Ordonnancement[4];
        private int nbNiveau;
        private Niveau[] niveaux;
        public MultiNiveau(int nbNiveau)
        {
            this.nbNiveau = nbNiveau;
            this.niveaux = new Niveau[nbNiveau];
        }
        public void InitNiveaux()
        {
            for (int i = 0; i < listeProcessus.Count; i++)
                niveaux[listeProcessus[i].niveau].listeProcessus.Add(listeProcessus[i]);
        }
        public int Executer()
        {
            SortListeProcessus();
            InitNiveaux();
            int temps = 0, indice = 0, i,tempsFin;
            int[] l = { 0, 0, 0 };
            while (indice < listeProcessus.Count || listeExecution.Count != 0)
            {
                AjouterTous(temps, indice);
                temps++;
                if (listeExecution.Count != 0)  //a vider
                {
                    for (i = 0; i < nbNiveau || niveaux[i].listeProcessus.Count == 0; i++) ; //la recherche de permier niveau non vide
                    tempsFin = TempsFin(temps, indice, i);
                    if (niveaux[i].numAlgo == 0)
                    {
                        niveaux[i].algo = new PAPS(niveaux[i].listeProcessus, niveaux[i].listeExecution);
                        temps=((PAPS)niveaux[i].algo).Executer(temps, tempsFin);
                    }
                    else if (niveaux[i].numAlgo == 1)
                    {
                        niveaux[i].algo = new PCA(niveaux[i].listeProcessus, niveaux[i].listeExecution);
                        temps = ((PCA)niveaux[i].algo).Executer(temps,tempsFin,niveaux[i]);
                    }
                    else if (niveaux[i].numAlgo == 2)
                    {
                        niveaux[i].algo = new PSP(niveaux[i].listeProcessus, niveaux[i].listeExecution);
                        temps = ((PSP)niveaux[i].algo).Executer(temps,tempsFin,niveaux[i]);
                    }
                    else if (niveaux[i].numAlgo == 3)
                    {
                        niveaux[i].algo = new RoundRobin(niveaux[i].quantum, niveaux[i].listeProcessus, niveaux[i].listeExecution);
                        temps = ((RoundRobin)niveaux[i].algo).Executer(temps, tempsFin, l);
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
                else
                {
                    listeExecution.Add(listeProcessus[indice]);
                    niveaux[listeProcessus[indice].niveau].listeExecution.Add(listeProcessus[indice]);
                }
            }
            return indice;
        }
        public int TempsFin(int temps,int indice,int i)
        {
            for (; indice < listeProcessus.Count; indice++)
            {
                if (listeProcessus[indice].niveau > i) break;
            }
            return listeProcessus[indice].tempsArriv;
        }
    }
}