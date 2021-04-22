using System.Collections.Generic;

namespace Ordonnancement
{
    class Niveau
    {
        public List<Processus> listeProcessus = new List<Processus>();
        public int algo;  //0 PAPS  1 SJF  2 PRIO 3 RR
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
        public void Excuter()
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
                    for (i = 0; i < nbNiveau || niveaux[i].listeProcessus.Count == 0; i++) ;
                    if (niveaux[i].algo == 0)
                    {

                    }
                    else if (niveaux[i].algo == 1)
                    {

                    }
                    else if (niveaux[i].algo == 2)
                    {

                    }
                    else if(niveaux[i].algo == 3)
                    {

                    }
                }
            }

        }
    }
}