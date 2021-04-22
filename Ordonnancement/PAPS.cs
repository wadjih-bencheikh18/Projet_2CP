namespace Ordonnancement
{
    class PAPS : Ordonnancement
    {
        public void Faire()
        {
            listeProcessus.Sort(delegate (Processus x, Processus y) { return x.tempsArriv.CompareTo(y.tempsArriv); }); //tri par ordre d'arrivé
            CalculFin();
            CalculAtt();
            CalculService();
            Affichage();
        }

        public void CalculFin() //temps de fin d'execution FIN
        {
            listeProcessus[0].tempsFin = listeProcessus[0].duree + listeProcessus[0].tempsArriv;
            for (int i = 1; i < listeProcessus.Count; i++)
            {
                if (listeProcessus[i].tempsArriv < listeProcessus[i - 1].tempsFin)
                {
                    listeProcessus[i].tempsFin = listeProcessus[i - 1].tempsFin + listeProcessus[i].duree;
                }
                else
                {
                    listeProcessus[i].tempsFin = listeProcessus[i].duree + listeProcessus[i].tempsArriv;
                }
            }
        }

        public void CalculAtt() //Waiting Time WT (pour FCFS waiting time = response time)
        {
            listeProcessus[0].tempsAtt = 0;
            for (int i = 1; i < listeProcessus.Count; i++)
            {
                if (listeProcessus[i].tempsArriv < listeProcessus[i - 1].tempsFin)
                {
                    listeProcessus[i].tempsAtt = listeProcessus[i - 1].tempsFin - listeProcessus[i].tempsArriv;
                }
                else
                {
                    listeProcessus[i].tempsAtt = 0;
                }
            }
        }

        public void CalculService() //Turn Around Time TAT
        {
            for (int i = 0; i < listeProcessus.Count; i++)
            {
                listeProcessus[i].tempsService = listeProcessus[i].tempsAtt + listeProcessus[i].duree;
            }
        }
    }
}