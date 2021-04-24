using System;

namespace Ordonnancement
{
    class PAPS : Ordonnancement
    {
        /*public void Faire()
        {
            listeProcessus.Sort(delegate (Processus x, Processus y) { return x.tempsArriv.CompareTo(y.tempsArriv); }); //tri par ordre d'arrivé
            CalculFin();
            CalculAtt();
            CalculService();
        }*/
        public void Executer(int tempsDebut , int tempsFin)
        {

            listeProcessus.Sort(delegate (Processus x, Processus y) { return x.tempsArriv.CompareTo(y.tempsArriv); }); //tri par ordre d'arrivé
            int temps = tempsDebut, indice = 0;
            while (indice < listeProcessus.Count || listeExecution.Count != 0 || temps < tempsFin)
            {
                indice = AjouterTous(temps, indice);
                temps++;
                if (listeExecution.Count != 0 )
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
        /*public void CalculFin() //temps de fin d'execution FIN
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
        }*/
    }
}