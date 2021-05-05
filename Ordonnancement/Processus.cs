using System;
using System.Collections.Generic;

namespace Ordonnancement
{
    class Processus
    {
        //donnés
        public List<Interruption> listeInterruptions = new List<Interruption>();
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
        public int[] indiceInterruptions = new int[2];

        #region Constructeur

         /// <summary>
         /// initialiser un processus
         /// </summary>
  
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
        #endregion

        #region Affichage

        /// <summary>
        /// le module utiliser pour l'affichage d'un processus
        /// </summary>

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
        #endregion

        #region Interruption

        /// <summary>
        /// Les modules utiliser pour implimanter les interruptions
        /// </summary>

        public void Push(Interruption interruption)
        {
            listeInterruptions.Add(interruption);
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

        public bool InterruptionExist()
        {
            for (; indiceInterruptions[1] < listeInterruptions.Count; indiceInterruptions[1]++)
            {
                if (!listeInterruptions[indiceInterruptions[1]].Prete(duree - tempsRestant)) break;
            }
            if (indiceInterruptions[0] == indiceInterruptions[1]) return false;
            else return true;
        }

        public void InterruptionExecute()
        {
            for (int i = indiceInterruptions[0]; i < indiceInterruptions[1]; i++)
            {
                listeInterruptions[i].Execute();
                if (listeInterruptions[i].tempsRestant == 0) indiceInterruptions[0]++;
            }
        }
        #endregion

    }

    class ProcessusNiveau : Processus
    {
        public int niveau;

        public ProcessusNiveau(int id, int tempsArriv, int duree, int prio, int niveau) : base(id, tempsArriv, duree, prio)
        {
            this.niveau = niveau;
        }
        public override void Affichage() //surdefinition : affiche les caracteristiques d'un processus en plus de son niveau
        {
            base.Affichage();
            Console.Write("\tNiveau : " + niveau);
        }
    }
}