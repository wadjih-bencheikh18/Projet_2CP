using System.Collections.Generic;

namespace Ordonnancement
{
    class RoundRobin : Ordonnancement
    {
        private int quantum { get; }
        public RoundRobin(int q,List<Processus> listeProcessus, List<Processus> listeExecution)
        {
            this.listeProcessus = listeProcessus;
            this.listeExecution = listeExecution;
        }
        public RoundRobin(int q)
        {
            quantum = q;
        }

        public int AjouterTous(int temps, int indice)  // collecter tous les processus a partit de "listeProcessus" (liste ordonnée) où leur temps d'arrivé est <= le temps réel d'execution, et les ajouter à la liste d'execution 
        {
            for (; indice < listeProcessus.Count; indice++)
            {
                if (listeProcessus[indice].tempsArriv <= temps) listeExecution.Add(listeProcessus[indice]);
                else break;
            }
            return indice;
        }

        public int Executer(int tempsDebut, int tempsFin, int i)  // executer la liste des processus selon l'algo RoundRobin
        // si l'interval du temps est précis (tempsFin != -1) donc on execute l'algo juste pendant cet intervalle, et a chaque fois on retourne l'indice du processus où on a arréter l'execution
        // sinon, on execute tous les processus et on retourne le temps total 
        {
            int indice = 0, temps;
            if (i == -1 || i > listeExecution.Count) i = 0;
            listeProcessus.Sort(delegate (Processus x, Processus y) { return x.tempsArriv.CompareTo(y.tempsArriv); }); //tri par ordre d'arrivé

            if (tempsDebut < listeProcessus[0].tempsArriv)  // si il y a pas de processus à executer au debut
            {
                temps = listeProcessus[0].tempsArriv;
                tempsDebut = listeProcessus[0].tempsArriv;
            }
            else temps = tempsDebut;

            indice = AjouterTous(temps, indice);

            while ((indice < listeProcessus.Count) || (listeExecution.Count != 0))
            {
                if ((indice < listeProcessus.Count) && (listeExecution.Count == 0))  // si le processeur a terminé mais il y a des processus arrivant
                {
                    if (temps + listeProcessus[indice].tempsArriv >= tempsFin) return -1;  //si le temps termine avant le processus suivant arrive donc on termine l'execution avec aucun indice
                    tempsDebut = listeProcessus[indice].tempsArriv;  // une nouvelle serie d'execution
                    temps += listeProcessus[indice].tempsArriv;
                    indice = AjouterTous(temps, indice);
                }
                else  // le processeur n'a pas terminé
                {
                    listeExecution[i].tempsAtt += temps - listeExecution[i].tempsFin;
                    if ((tempsFin != -1) && (((listeExecution[i].tempsRestant > quantum) && (temps + quantum >= tempsFin)) || (temps + listeExecution[i].tempsRestant >= tempsFin)))  // on a attent la fin d'execution
                    {
                        listeExecution[i].etat = 1;  // prés
                        listeExecution[i].tempsRestant -= tempsFin - temps;
                        listeExecution[i].tempsFin = tempsFin;  // stocker la fin d'execution du processus pour la prochain vague d'execution
                        if (listeExecution[i].tempsRestant == 0)  // processus terminé
                        {
                            // on supprime ce processus à partir de la liste d'execution mais on le mis à jour dans la liste des processus
                            int j = listeProcessus.FindIndex(p => ((p.id == listeExecution[i].id) && (p.prio == listeExecution[i].prio)));
                            listeProcessus[j] = listeExecution[i];
                            listeExecution.RemoveAt(i);
                        }
                        return i;  // fin d'execution: on retourne l'indice où on a arréter l'execution
                    }
                    else if (listeExecution[i].tempsRestant > quantum)  // il ne sera pas terminé pendant ce quantum
                    {
                        listeExecution[i].etat = 1;  // prés
                        listeExecution[i].tempsRestant -= quantum;
                        temps += quantum;
                        listeExecution[i].tempsFin = temps;  // stocker la fin d'execution du processus pour qu'on puisse calculer le temps d'att le prochain quantum
                        i++;  // passer au processus suivant
                    }
                    else  // sera terminé pendant ce quantum
                    {
                        temps += listeExecution[i].tempsRestant;
                        listeExecution[i].tempsRestant = 0;
                        listeExecution[i].tempsAtt -= tempsDebut;  // enlever le debut d'execution
                        listeExecution[i].etat = 0;  // terminé
                        // on supprime ce processus à partir de la liste d'execution mais on le mis à jour dans la liste des processus
                        int j = listeProcessus.FindIndex(p => ((p.id == listeExecution[i].id) && (p.prio == listeExecution[i].prio)));
                        listeProcessus[j] = listeExecution[i];
                        listeProcessus[j].tempsFin = temps;  // temps de fin d'execution
                        listeExecution.RemoveAt(i);
                    }
                    indice = AjouterTous(temps, indice);
                    if (i >= listeExecution.Count) i = 0;
                }
            }
            return temps;
        }
    }
}