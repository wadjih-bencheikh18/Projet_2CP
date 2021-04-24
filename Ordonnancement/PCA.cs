using System;

namespace Ordonnancement
{
    class PCA : Ordonnancement
    {
        public void Executer()
        {
            listeProcessus.Sort(delegate (Processus x, Processus y) { return x.tempsArriv.CompareTo(y.tempsArriv); }); //tri par ordre d'arrivé
            int temps = 0, indice = 0;
            bool sort;
            while (indice < listeProcessus.Count || listeExecution.Count != 0)
            {
                if (listeExecution.Count == 0) sort = true;
                else sort = false;
                indice = AjouterTous(temps, indice);
                if (sort == true && listeExecution.Count != 0)
                {
                    listeProcessus.Sort(delegate (Processus x, Processus y) { return y.duree.CompareTo(x.duree); }); //tri par duree
                }
                temps++;
                if (listeExecution.Count != 0)
                {
                    listeExecution[0].tempsRestant--;
                    Console.WriteLine(listeExecution[0].id + "-" + listeExecution[0].tempsRestant);
                    //for (int i = 1; i < listeExecution.Count; i++) listeExecution[i].tempsAtt++;
                    if (listeExecution[0].tempsRestant <= 0)
                    {
                        listeExecution[0].tempsFin = temps;
                        listeExecution[0].tempsService = temps - listeExecution[0].tempsArriv;
                        listeExecution.RemoveAt(0);
                        if (listeExecution.Count != 0) listeProcessus.Sort(delegate (Processus x, Processus y) { return x.duree.CompareTo(y.duree); }); //tri par duree
                    }
                }
            }

        }
        public int AjouterTous(int temps, int indice)  // collecter tous les processus a partit de "listeProcessus" (liste ordonnée) où leur temps d'arrivé est <= le temps réel d'execution, et les ajouter à la liste d'execution 
        {
            for (; indice < listeProcessus.Count; indice++)
            {
                if (listeProcessus[indice].tempsArriv > temps) break;
                else listeExecution.Add(listeProcessus[indice]);
            }
            return indice;
        }
    }
}