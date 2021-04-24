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

    class MultiNiveau : Ordonnancement
    {
        private int nbNiveau;
        private Niveau[] niveaux;
        public MultiNiveau(int nbNiveau)
        {
            this.nbNiveau = nbNiveau;
            this.niveaux = new Niveau[nbNiveau];
        }
        public void Executer()
        {
            listeProcessus.Sort(delegate (Processus x, Processus y) { return x.tempsArriv.CompareTo(y.tempsArriv); }); //tri par ordre d'arrivé
            int p = 0, t = 0, i = 0, j = 0;
            bool avance=true;
            while (p < listeProcessus.Count)
            {
                if (t == listeProcessus[j].tempsArriv)
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
                        niveaux[i].algo = new PAPS();
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