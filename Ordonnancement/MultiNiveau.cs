using System.Collections.Generic;

namespace Ordonnancement
{
    class Niveau
    {
        public List<Processus> listeProcessus = new List<Processus>();
        public List<Processus> listeExecution = new List<Processus>();
        public Ordonnancement algo;
        public int numAlgo; //0 PAPS  1 SJF  2 PRIO 3 RR
    }
    class ProcessusNiveau : Processus
    {
        public int niveau;
        public ProcessusNiveau(int id, int tempsArriv, int duree, int prio, int niveau) : base(id, tempsArriv, duree, prio)
        {
            this.tempsRestant = duree;
            this.niveau = niveau;
        }
    }
    class MultiNiveau : Ordonnancement
    {
        protected new List<ProcessusNiveau> listeProcessus = new List<ProcessusNiveau>();
        private int nbNiveau;
        private Niveau[] niveaux;
        public MultiNiveau(int nbNiveau)
        {
            this.nbNiveau = nbNiveau;
            this.niveaux = new Niveau[nbNiveau];
        }
        public void InitNiveaux()
        {
            for (int i=0; i<listeProcessus.Count;i++)
                niveaux[listeProcessus[i].niveau].listeProcessus.Add(listeProcessus[i]);
        }
        public void Executer()
        {
            SortListeProcessus();
            InitNiveaux();
            int p = 0, temps = 0, i = 0, j = 0;
            bool avance=true;
            while (p < listeProcessus.Count)
            {
                if (temps == listeProcessus[j].tempsArriv)
                {
                    niveaux[listeProcessus[j].niveau].listeProcessus.Add(listeProcessus[j]);
                    j++;
                    avance = false;
                }
                else if (t < listeProcessus[j].tempsArriv)
                {
                    t++;
                    avance = true;
                }
                while (i != nbNiveau && avance)
                {
                    for (i = 0; i < nbNiveau || niveaux[i].listeProcessus.Count == 0; i++) ; //la recherche de permier niveau non vide
                    if (niveaux[i].numAlgo == 0)
                    {
                        niveaux[i].algo = new PAPS(niveaux[i].listeProcessus, niveaux[i].listeExecution);
                        ((PAPS) niveaux[i].algo).Executer(1,5);
                    }
                    else if (niveaux[i].numAlgo == 1)
                    {

                    }
                    else if (niveaux[i].numAlgo == 2)
                    {

                    }
                    else if(niveaux[i].numAlgo == 3)
                    {

                    }
                }
            }

        }
    }
}