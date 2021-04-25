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
        public int quantum; //a khemem
        public int[] indice = new int[4];
        public Niveau(int numAlgo)
        {
            this.numAlgo = numAlgo;
            switch (this.numAlgo)
            {
                case 0:
                    // code block
                    break;
                case 1:
                    // code block
                    break;
                case 2:
                    // code block
                    break;
                case 3:
                    // code block
                    break;
                default:
                    // code block
                    break;
            }
        }
        public Niveau(int numAlgo, int quantum)
        {
            this.numAlgo = numAlgo;
            this.quantum = quantum;
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
        //public Ordonnancement[] algo = new Ordonnancement[4];
        private int nbNiveau;
        private Niveau[] niveaux;

        public MultiNiveau(int nbNiveau)
        {
            Random r;
            this.nbNiveau = nbNiveau;
            this.niveaux = new Niveau[nbNiveau];
            for (int i = 0; i < nbNiveau; i++)
            {
                r = new Random();
                Console.WriteLine(i + "=>" + r.Next(0, 3));
                niveaux[i] = new(r.Next(0,3));
            }
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
            SortListeProcessus();
            InitNiveaux();
            int temps = 0, indice = 0, indiceNiveau = -1, tempsFin, tempsAtt = 0, tempsDebut = 0;
            bool conditionTempsAtt = false;
            while (indice < listeProcessus.Count || indiceNiveau < nbNiveau)
            {
                indice = AjouterTous(temps, indice);
                if (conditionTempsAtt)
                {
                    for (int i = 0; i < nbNiveau; i++)
                    {
                        if (i != indiceNiveau)
                        {
                            for (int j = 0; j < niveaux[i].listeExecution.Count; j++)
                            {
                                if (niveaux[i].listeExecution[j].tempsArriv > tempsDebut)
                                    niveaux[i].listeExecution[j].tempsAtt = temps - niveaux[i].listeExecution[j].tempsArriv;
                                else niveaux[i].listeExecution[j].tempsAtt += tempsAtt;
                            }
                        }

                    }
                    conditionTempsAtt = false;
                }
                for (indiceNiveau = 0; indiceNiveau < nbNiveau && niveaux[indiceNiveau].listeExecution.Count == 0; indiceNiveau++) ; //la recherche de permier niveau non vide
                if (indiceNiveau < nbNiveau)  //a vider
                {
                    conditionTempsAtt = true;
                    tempsFin = TempsFin(temps, indice, indiceNiveau);
                    tempsDebut = temps;
                    if (niveaux[indiceNiveau].numAlgo == 0)
                    {
                        niveaux[indiceNiveau].algo = new PAPS(niveaux[indiceNiveau].listeProcessus, niveaux[indiceNiveau].listeExecution);
                        niveaux[indiceNiveau].indice[0] = indice;
                        temps = ((PAPS)niveaux[indiceNiveau].algo).Executer(temps, tempsFin, niveaux, indiceNiveau, listeProcessus);
                        indice = niveaux[indiceNiveau].indice[0];
                    }
                    else if (niveaux[indiceNiveau].numAlgo == 1)
                    {
                        niveaux[indiceNiveau].algo = new PCA(niveaux[indiceNiveau].listeProcessus, niveaux[indiceNiveau].listeExecution);
                        niveaux[indiceNiveau].indice[0] = indice;
                        temps = ((PCA)niveaux[indiceNiveau].algo).Executer(temps, tempsFin, niveaux, indiceNiveau, listeProcessus);
                        indice = niveaux[indiceNiveau].indice[0];
                    }
                    else if (niveaux[indiceNiveau].numAlgo == 2)
                    {
                        niveaux[indiceNiveau].algo = new PSP(niveaux[indiceNiveau].listeProcessus, niveaux[indiceNiveau].listeExecution);
                        niveaux[indiceNiveau].indice[0] = indice;
                        temps = ((PSP)niveaux[indiceNiveau].algo).Executer(temps, tempsFin, niveaux, indiceNiveau, listeProcessus);
                        indice = niveaux[indiceNiveau].indice[0];
                    }
                    else if (niveaux[indiceNiveau].numAlgo == 3)
                    {
                        niveaux[indiceNiveau].algo = new RoundRobin(niveaux[indiceNiveau].quantum, niveaux[indiceNiveau].listeProcessus, niveaux[indiceNiveau].listeExecution);
                        temps = ((RoundRobin)niveaux[indiceNiveau].algo).Executer(temps, tempsFin, niveaux[indiceNiveau].indice);
                    }
                    tempsAtt = temps - tempsDebut;
                }
                else temps++;
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
                    //listeExecution.Add(listeProcessus[indice]);
                    niveaux[listeProcessus[indice].niveau].listeExecution.Add(listeProcessus[indice]);
                }
            }
            return indice;
        }
        public int TempsFin(int temps, int indice, int i)
        {
            for (; indice < listeProcessus.Count; indice++)
            {
                if (listeProcessus[indice].niveau < i) break;
            }
            if (indice < listeProcessus.Count) return listeProcessus[indice].tempsArriv;
            else return 999;
        }
    }
}