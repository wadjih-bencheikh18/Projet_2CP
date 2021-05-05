using System;
using System.Collections.Generic;

namespace Ordonnancement
{
    abstract class Ordonnancement
    {
        public List<Processus> listeProcessus = new List<Processus>();  // liste des processus fournis par l'utilisateur
        public List<Processus> listePrets = new List<Processus>();  // liste des processus prêts
        public List<Processus> listebloque = new List<Processus>();
        #region Liste Processus

        /// <summary>
        /// les modules pour changer et afficher Liste Processus (initialisation) donne saise par l'utilisateur
        /// </summary>

        public void Push(Processus pro) //ajout d'un processus à la liste listeProcessus
        {
            listeProcessus.Add(pro);
        }
        public virtual void Affichage() //affichage de listeProcessus
        {
            for (int i = 0; i < listeProcessus.Count; i++) listeProcessus[i].Affichage();
        }
        public virtual void SortListeProcessus() //tri des processus par ordre d'arrivé
        {
            listeProcessus.Sort(delegate (Processus x, Processus y) { return x.tempsArriv.CompareTo(y.tempsArriv); });

            foreach (Processus processus in listeProcessus)
                processus.SortListeInterruptions();
        }
        #endregion

        #region Liste Prets

        /// <summary>
        /// Chnagement sur la liste Prets
        /// </summary>

        public virtual int MAJListePrets(int temps, int indice) //ajouter à la liste des processus prêts tous les processus de "listeProcessus" (liste ordonnée) dont le temps d'arrivé est <= au temps réel d'execution
        {
            for (; indice < listeProcessus.Count; indice++) //parcours de listeProcessus à partir du processus d'indice "indice"
            {
                if (listeProcessus[indice].tempsArriv > temps) break; //si le processus n'est pas encore arrivé on sort
                else listePrets.Add(listeProcessus[indice]); //sinon on ajoute le processus à la liste des processus prêts
            }
            return indice;
        }
        #endregion

        #region Liste Bloque

        /// <summary>
        /// Changement sur la list bloqué
        /// </summary>
        
        public void MAJListBloque()
        {
            for (int i = 0; i < listebloque.Count; i++)
            {
                listebloque[i].InterruptionExecute();
                if (listebloque[i].indiceInterruptions[0] == listebloque[i].indiceInterruptions[1])
                {
                    listebloque[i].etat = 1;
                    listePrets.Add(listebloque[i]);
                    listebloque.RemoveAt(i);
                }
            }

        }
        public void InterruptionExecute()
        {
            MAJListBloque();
            while (listePrets.Count != 0 && listePrets[0].InterruptionExist())
            {
                listePrets[0].etat = 0;
                listebloque.Add(listePrets[0]);
                listePrets.RemoveAt(0);
            }
        }
        #endregion

        #region Pour Multi niveau

        /// <summary>
        /// les algorithme utuliser pour implimenter Algo multi niveau
        /// </summary>

        public int MAJListePrets(int temps, int indice, Niveau[] niveaux, List<ProcessusNiveau> listeGeneral, int indiceNiveau) //ajouter à la liste des processus prêts tous les processus de "listeGeneral" (liste ordonnée) dont le temps d'arrivé est <= au temps réel d'execution de MultiNiveaux
        {
            for (; indice < listeGeneral.Count; indice++) //parcours de listeGeneral à partir du processus d'indice "indice"
            {
                if (listeGeneral[indice].tempsArriv > temps) break; //si le processus n'est pas encore arrivé on sort
                else
                {
                    if (listeGeneral[indice].niveau == indiceNiveau) listePrets.Add(listeGeneral[indice]); //si le niveau du processus = indiceNiveau (niveau actuel) on ajoute ce processus à la liste des processus prêts de ce niveau
                    else niveaux[listeGeneral[indice].niveau].listePrets.Add(listeGeneral[indice]); //sinon on ajoute le processus à la liste des processus prêts de son niveau
                }
            }
            return indice;
        }
        public void Init(List<Processus> listeProcessus, List<Processus> listePrets, List<Processus> listebloque)
        {
            this.listeProcessus = listeProcessus;
            this.listePrets = listePrets;
            this.listebloque = listebloque;
        }
        #endregion

        #region Affichage

        /// <summary>
        /// Affichage dans le console pour les tests
        /// </summary>

        public void AfficheLigne(int temps, int id) //affiche le temps actuel et l'ID du processus entrain d'être executé
        {
            Console.Write(temps + "\t|\t " + id + "\t\t\t|=> ");
            foreach (Processus processus in listebloque)
                Console.Write(processus.id + " | ");
            Console.WriteLine();
        }
        public void AfficheLigne(int temps) //affiche le temps actuel et le mot "repos" ie le processeur n'execute aucun processus
        {
            Console.Write(temps + "\t|\t   Repos   \t\t|=> ");
            foreach (Processus processus in listebloque)
                Console.Write(processus.id + " | ");
            Console.WriteLine();
        }
        #endregion
        
    }
}