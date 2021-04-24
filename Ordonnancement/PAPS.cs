using System;

namespace Ordonnancement
{
    class PAPS : Ordonnancement
    {
        public void Executer(int tempsDebut, int tempsFin)
        {

            listeProcessus.Sort(delegate (Processus x, Processus y) { return x.tempsArriv.CompareTo(y.tempsArriv); }); //tri par ordre d'arrivé
            int temps = tempsDebut, indice = 0;
            while ((indice < listeProcessus.Count || listeExecution.Count != 0) && temps < tempsFin)
            {
                indice = AjouterTous(temps, indice);   //remplire listeExecution
                temps++;
                if (listeExecution.Count != 0)
                {
                    listeExecution[0].tempsRestant--;
                    for (int i = 1; i < listeExecution.Count; i++) listeExecution[i].tempsAtt++;
                    if (listeExecution[0].tempsRestant == 0)
                    {
                        listeExecution[0].tempsFin = temps;
                        listeExecution[0].tempsService = temps - listeExecution[0].tempsArriv;
                        listeExecution.RemoveAt(0);
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