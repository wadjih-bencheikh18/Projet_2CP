namespace Ordonnancement
{
    class PCA : Ordonnancement
    {
        public void Faire()
        {
            TriArriv(); //tri par ordre d'arrivé
            CalculFin();
            CalculService();
            CalculAtt();
            for (int i = 0; i < nb; i++) listeProcessus[i].Affichage();
        }
        private void TriArriv() // trier le tableau des processus selon le temps d'arrivée
        {
            listeProcessus.Sort(delegate (Processus P1, Processus P2) {
                return P1.tempsArriv.CompareTo(P2.tempsArriv);
            });
        }

        public override void CalculFin() //temps de fin d'execution FIN
        {
            int tmp, i, j, val = -1, low;
            listeProcessus[0].tempsFin = listeProcessus[0].tempsArriv + listeProcessus[0].duree;
            for (i = 1; i < nb; i++)
            {
                tmp = listeProcessus[i - 1].tempsFin;
                low = listeProcessus[i].duree;
                for (j = i; j < nb; j++)
                {
                    if (tmp >= listeProcessus[j].tempsArriv && low >= listeProcessus[j].duree) val = j;
                }
                listeProcessus[val].tempsFin = tmp + listeProcessus[val].duree;
                Processus tem; //variable temporaire
                tem = listeProcessus[val];
                listeProcessus[val] = listeProcessus[i];
                listeProcessus[i] = tem;
            }
        }

<<<<<<< HEAD
        public override void CalculService() 
=======
        public override void CalculService()
>>>>>>> 47ff79a26b527be3084ab78179c94ffb25615599
        {
            int tmp, i, j, val = -1, low;
            listeProcessus[0].tempsService = listeProcessus[0].tempsFin - listeProcessus[0].tempsArriv;
            for (i = 1; i < nb; i++)
            {
                tmp = listeProcessus[i - 1].tempsFin;
                low = listeProcessus[i].duree;
                for (j = i; j < nb; j++)
                {
                    if (tmp >= listeProcessus[j].tempsArriv && low >= listeProcessus[j].duree) val = j;
                }
                listeProcessus[val].tempsService = listeProcessus[val].tempsFin - listeProcessus[val].tempsArriv;
                Processus tem; //variable temporaire
                tem = listeProcessus[val];
                listeProcessus[val] = listeProcessus[i];
                listeProcessus[i] = tem;
            }
        }

<<<<<<< HEAD
        public override void CalculAtt() 
=======
        public override void CalculAtt()
>>>>>>> 47ff79a26b527be3084ab78179c94ffb25615599
        {
            int tmp, i, j, val = -1, low;
            listeProcessus[0].tempsAtt = listeProcessus[0].tempsService - listeProcessus[0].duree;
            for (i = 1; i < nb; i++)
            {
                tmp = listeProcessus[i - 1].tempsFin;
                low = listeProcessus[i].duree;
                for (j = i; j < nb; j++)
                {
                    if (tmp >= listeProcessus[j].tempsArriv && low >= listeProcessus[j].duree) val = j;
                }
                listeProcessus[val].tempsAtt = listeProcessus[val].tempsService - listeProcessus[val].duree;
                Processus tem; //variable temporaire
                tem = listeProcessus[val];
                listeProcessus[val] = listeProcessus[i];
                listeProcessus[i] = tem;
            }
        }
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> 47ff79a26b527be3084ab78179c94ffb25615599
