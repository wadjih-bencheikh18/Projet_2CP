using System;

namespace Ordonnancement
{
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
            Console.Write("Niveau : " + niveau);
        }
    }
}