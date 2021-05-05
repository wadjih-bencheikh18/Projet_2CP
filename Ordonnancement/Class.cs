using System;
using System.Collections.Generic;

namespace Ordonnancement
{
    class Interruption
    {
        public string type;
        public int duree;
        public int tempsRestant;
        public int tempsArriv;
        public Interruption(string type, int duree, int tempsArriv)
        {
            this.type = type;
            this.duree = duree;
            tempsRestant = duree;
            this.tempsArriv = tempsArriv;
        }
        public Interruption(int duree, int tempsArriv)
        {
            this.duree = duree;
            tempsRestant = duree;
            this.tempsArriv = tempsArriv;
        }
        public bool Prete(int temps)
        {
            if (tempsArriv == temps) return true;
            else return false;
        }
        public void Execute()
        {
            tempsRestant--;
        }
    }
    class Processus
    {
        //donnés
        public int id { get; } //ID du processus
        public int tempsArriv { get; } //temps d'arrivé
        public int duree { get; } //temps d'execution du processus (burst time)
        public int prio { get; } //priorite du processus
        //à remplir
        public int etat; // 0:bloqué  1:prêt  2:en cours  3:fini
        public int tempsFin;
        public int tempsAtt;
        public int tempsService;
        public int tempsRestant;
        public int tempsReponse;
        public List<Interruption> listeInterruptions = new List<Interruption>();
        public int[] indice = new int[2];
        public Processus(int id, int tempsArriv, int duree, int prio)  //constructeur pour l'algorithme de priorité
        {
            this.etat = 1;
            this.id = id;
            this.tempsArriv = tempsArriv;
            this.duree = duree;
            this.prio = prio;
            tempsRestant = duree;
        }
        public Processus(int id, int tempsArriv, int duree) //constructeur pour les autres algorithmes
        {
            this.etat = 1;
            this.id = id;
            this.tempsArriv = tempsArriv;
            this.duree = duree;
            prio = 0;
            tempsRestant = duree;
        }
        public virtual void Affichage() //affiche les caracteristiques d'un processus
        {
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.Write("ID : " + id);
            Console.Write("\ttemps d'arrivé : " + tempsArriv);
            Console.Write("\tduree : " + duree);
            Console.Write("\tpriorité : " + prio);
            Console.Write("\ttemps d'attente : " + tempsAtt);
            Console.Write("\ttemps de fin :  " + tempsFin);
            Console.Write("\ttemps de service  : " + tempsService);
            //Console.Write("\ttemps restant : " + tempsRestant);
        }
        public bool InterruptionExist()
        {
            for (; indice[1] < listeInterruptions.Count; indice[1]++)
            {
                if (!listeInterruptions[indice[1]].Prete(duree - tempsRestant)) break;
            }
            if (indice[0] == indice[1]) return false;
            else return true;
        }
        public void InterruptionExecute()
        {
            for (int i = indice[0]; i < indice[1]; i++)
            {
                listeInterruptions[i].Execute();
                if (listeInterruptions[i].tempsRestant == 0) indice[0]++;
            }
        }
        public void SortListeInterruptions()
        {
            listeInterruptions.Sort
                    (
                        delegate (Interruption x, Interruption y)
                        {
                            if (x.tempsArriv.CompareTo(y.tempsArriv) == 0) return x.duree.CompareTo(y.duree); //si les processus ont la même priorité, on les trie selon le temps d'arrivée
                            else return x.tempsArriv.CompareTo(y.tempsArriv); //sinon, on fait le tri par priorité
                        }
                    );
        }
        public void Push(Interruption interruption)
        {
            listeInterruptions.Add(interruption);
        }
    }

    abstract class Ordonnancement
    {
        public List<Processus> listeProcessus = new List<Processus>();  // liste des processus fournis par l'utilisateur
        public List<Processus> listePrets = new List<Processus>();  // liste des processus prêts
        public List<Processus> listebloque = new List<Processus>();

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
        public void ListBloqueExecute()
        {
            for (int i=0; i<listebloque.Count;i++)
            {
                listebloque[i].InterruptionExecute();
                if (listebloque[i].indice[0]== listebloque[i].indice[1])
                {
                    listebloque[i].etat = 1;
                    listePrets.Add(listebloque[i]);
                    listebloque.RemoveAt(i);
                }
            }

        }
        public void InterruptionExecute()
        {
            ListBloqueExecute();
            while (listePrets.Count != 0 && listePrets[0].InterruptionExist())
            {
                listePrets[0].etat = 0;
                listebloque.Add(listePrets[0]);
                listePrets.RemoveAt(0);
            }
        }
        public virtual int AjouterTous(int temps, int indice) //ajouter à la liste des processus prêts tous les processus de "listeProcessus" (liste ordonnée) dont le temps d'arrivé est <= au temps réel d'execution
        {
            for (; indice < listeProcessus.Count; indice++) //parcours de listeProcessus à partir du processus d'indice "indice"
            {
                if (listeProcessus[indice].tempsArriv > temps) break; //si le processus n'est pas encore arrivé on sort
                else listePrets.Add(listeProcessus[indice]); //sinon on ajoute le processus à la liste des processus prêts
            }
            return indice;
        }
        public int AjouterTous(int temps, int indice, Niveau[] niveaux, List<ProcessusNiveau> listeGeneral, int indiceNiveau) //ajouter à la liste des processus prêts tous les processus de "listeGeneral" (liste ordonnée) dont le temps d'arrivé est <= au temps réel d'execution de MultiNiveaux
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
        public void Init(List<Processus> listeProcessus, List<Processus> listePrets, List<Processus> listebloque)
        {
            this.listeProcessus = listeProcessus;
            this.listePrets = listePrets;
            this.listebloque = listebloque;
        }
    }
}