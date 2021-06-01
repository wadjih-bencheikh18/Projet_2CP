using FinalAppTest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Ordonnancement
{
    public class MultiNiveauRecyclage : Ordonnancement
    {
        #region Attributs
        protected new List<ProcessusNiveau> listeProcessus = new List<ProcessusNiveau>();
        protected new List<ProcessusNiveau> listebloque = new List<ProcessusNiveau>();
        public int nbNiveau;
        private Niveau[] niveaux;
        public StackPanel[] ListesPretsViews;
        public StackPanel deroulement;
        #endregion
        public void InitVisualisation(StackPanel[] ListesPretsViews)
        {
            this.ListesPretsViews = ListesPretsViews;
        }
        #region Constructeur
        /// <summary>
        /// initialisation
        /// </summary>

        public MultiNiveauRecyclage(int nbNiveau, Niveau[] niveaux)
        {
            this.nbNiveau = nbNiveau;
            this.niveaux = new Niveau[nbNiveau];
            ListesPretsViews = new StackPanel[nbNiveau];
            this.niveaux = niveaux;
        }
        #endregion

        #region Redefinition

        /// <summary>
        /// la redéfinition des modules déclarer dans la class ordannancement
        /// </summary>
        public new void Famine(int tempsRestantMax)
        {
            foreach (ProcessusNiveau pro in listeProcessus)
                if (pro.tempsRestant >= tempsRestantMax)
                {
                    pro.famine = true;
                    nbFamine++;
                }
        }
        public override void Affichage() //affiche les caracteristiques d'un processus et son niveau
        {
            for (int i = 0; i < listeProcessus.Count; i++) listeProcessus[i].Affichage();
        }

        public void InitNiveaux() //initialisation des niveaux
        {
            for (int i = 0; i < listeProcessus.Count; i++)
                niveaux[listeProcessus[i].niveau].listeProcessus.Add(listeProcessus[i]); //on ajoute le processus à la liste des processus de son niveau
        }

        public void Push(ProcessusNiveau pro) //ajout d'un processus à la liste "listeProcessus"
        {
            listeProcessus.Add(pro);
        }

        public override void SortListeProcessus() //tri de listeProcessus par ordre d'arrivé
        {
            listeProcessus.Sort(delegate (ProcessusNiveau x, ProcessusNiveau y) { return x.tempsArriv.CompareTo(y.tempsArriv); });
        }

        public override int MAJListePrets(int temps, int indice) //ajouter à la liste des processus prêts de chaque niveau tous les processus de "listeProcessus" (liste ordonnée) dont le temps d'arrivé est <= au temps réel d'execution
        {
            for (; indice < listeProcessus.Count; indice++) //parcours de listeProcessus à partir du processus d'indice "indice"
            {
                if (listeProcessus[indice].tempsArriv > temps) break; //si le processus n'est pas encore arrivé on sort
                else
                {
                    niveaux[listeProcessus[indice].niveau].listePrets.Add(listeProcessus[indice]); //sinon on ajoute le processus à la liste des processus prêts de son niveau
                }
            }
            return indice;
        }

        public override int Executer(int tempsDebut, int tempsFin, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral, List<ProcessusNiveau> listebloqueGenerale, StackPanel deroulement) { return 0; }
        public override async Task<int> Executer(int tempsDebut, int tempsFin, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral, List<ProcessusNiveau> listebloqueGenerale, StackPanel[] ListesPretsView, StackPanel Processeur, TextBlock TempsView, StackPanel ListeBloqueView, StackPanel deroulement)
        {
            await Task.CompletedTask;
            return 0;
        }
        public override async Task<int> Executer(int tempsDebut, int tempsFin, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral, List<ProcessusNiveau> listebloqueGenerale, StackPanel[] ListesPretsView, StackPanel Processeur, TextBlock TempsView, StackPanel ListeBloqueView, StackPanel deroulement,int i)
        {
            await Task.CompletedTask;
            return 0;
        }

        #endregion

        #region Visualisation
        public override async Task<int> Executer(StackPanel ListProcessusView, StackPanel Processeur, TextBlock TempsView, StackPanel ListeBloqueView, StackPanel deroulement, WrapPanel GanttChart)
        {
            SortListeProcessus();  //trier la liste des processus
            InitNiveaux();   //remplir les niveaux
            int temps = 0, indice = 0, indiceNiveau = nbNiveau;
            while (indice < listeProcessus.Count || indiceNiveau < nbNiveau || listebloque.Count != 0) //tant que le processus est dans listeProcessus ou il existe un niveau non vide
            {
                indice = await MAJListePrets(temps, indice, ListesPretsViews);  //remplir la liste des processus prêts de chaque niveau
                //await InterruptionExecute(listebloque, ListesPretsViews, indiceNiveau, ListeBloqueView, Processeur);
                for (indiceNiveau = 0; indiceNiveau < nbNiveau && niveaux[indiceNiveau].listePrets.Count == 0; indiceNiveau++) ; //la recherche du permier niveau non vide
                if (indiceNiveau < nbNiveau)  //il existe un niveau non vide
                {
                    niveaux[indiceNiveau].indice[0] = indice;   //pour sauvegarder l'indice "indice" (temporairement)
                    temps = await NiveauExecute(temps, indiceNiveau, Processeur, TempsView, ListeBloqueView, deroulement);  //temps de fin d'execution du niveau "indiceNiveau"
                    indice = niveaux[indiceNiveau].indice[0];  //recuperer l'indice sauvegardé precedemment
                }
                else
                {
                    temps++;
                    TempsView.Text = temps.ToString();
                    AfficherEtat(listeProcessus, Ordonnancement.GanttChart, temps);
                    tempsRepos++;
                }
            }
            TauxUtil(temps);
            return temps;
        }
        public async Task<int> NiveauExecute(int temps, int indiceNiveau, StackPanel Processeur, TextBlock TempsView, StackPanel ListeBloqueView, StackPanel deroulement)  //executer le niveau "indiceNiveau"
        {
            niveaux[indiceNiveau].algo.Init(niveaux[indiceNiveau].listeProcessus, niveaux[indiceNiveau].listePrets, niveaux[indiceNiveau].listebloque); //initialisation de algo avec la liste des processus et la liste des processus prêts du niveau
            temps = await niveaux[indiceNiveau].algo.Executer(temps, nbNiveau, niveaux, indiceNiveau, listeProcessus, listebloque, ListesPretsViews, Processeur, TempsView, ListeBloqueView, deroulement,1);
            return temps;
        }
        public async Task<int> MAJListePrets(int temps, int indice, StackPanel[] ListesPretsViews) //ajouter à la liste des processus prêts de chaque niveau tous les processus de "listeProcessus" (liste ordonnée) dont le temps d'arrivé est <= au temps réel d'execution
        {
            bool ajout = false;
            for (; indice < listeProcessus.Count; indice++) //parcours de listeProcessus à partir du processus d'indice "indice"
            {
                if (listeProcessus[indice].tempsArriv > temps) break; //si le processus n'est pas encore arrivé on sort
                else
                {
                    ajout = true;
                    niveaux[listeProcessus[indice].niveau].listePrets.Add(listeProcessus[indice]); //sinon on ajoute le processus à la liste des processus prêts de son niveau
                    AffichageProcessus pro = new AffichageProcessus(listeProcessus[indice]);
                    ProcessusDesignMultiLvl item = new ProcessusDesignMultiLvl();
                    item.DataContext = pro;
                    pro.X1 = 700;
                    pro.Y1 = 0;
                    pro.Speed = SimulationPage_MultiLvl.Speed;
                    item.DataContext = pro;
                    ListesPretsViews[listeProcessus[indice].niveau].Children.Add(item);
                }
            }
            if (ajout) await Task.Delay(Convert.ToInt32(1000 / SimulationPage_MultiLvl.Speed));
            else await Task.Delay(Convert.ToInt32(500 / SimulationPage_MultiLvl.Speed));
            return indice;
        }
        #endregion

        #region Test
        public int Executer()
        {
            SortListeProcessus();  //trier la liste des processus
            InitNiveaux();   //remplir les niveaux
            int temps = 0, indice = 0, indiceNiveau = 0, tempsFin;
            while (indice < listeProcessus.Count || indiceNiveau < nbNiveau) //tant que le processus est dans listeProcessus ou il existe un niveau non vide
            {
                indice = MAJListePrets(temps, indice);  //remplir la liste des processus prêts de chaque niveau
                for (indiceNiveau = 0; indiceNiveau < nbNiveau && niveaux[indiceNiveau].listePrets.Count == 0; indiceNiveau++) ; //la recherche du permier niveau non vide
                if (indiceNiveau < nbNiveau)  //il existe un niveau non vide
                {
                    tempsFin = TempsFin(indice, indiceNiveau);  //calcul du temps de fin d'execution
                    niveaux[indiceNiveau].indice[0] = indice;   //pour sauvegarder l'indice "indice" (temporairement)
                    temps = NiveauExecute(temps, tempsFin, niveaux, indiceNiveau, listeProcessus, deroulement);  //temps de fin d'execution du niveau "indiceNiveau"
                    indice = niveaux[indiceNiveau].indice[0];  //recuperer l'indice sauvegardé precedemment
                }
                else
                {
                    if (indice < listeProcessus.Count) AfficheLigne(temps);  //affiche le temps actuel et le mot "repos" ie aucun niveau n'est executé
                    temps++;
                }
            }
            return temps;
        }
        public int NiveauExecute(int temps, int tempsFin, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeProcessus, StackPanel deroulement)  //executer le niveau "indiceNiveau"
        {
            niveaux[indiceNiveau].algo.Init(niveaux[indiceNiveau].listeProcessus, niveaux[indiceNiveau].listePrets, niveaux[indiceNiveau].listebloque); //initialisation de algo avec la liste des processus et la liste des processus prêts du niveau
            temps = niveaux[indiceNiveau].algo.Executer(temps, tempsFin, niveaux, indiceNiveau, listeProcessus, listebloque, deroulement);
            return temps;
        }

        public int TempsFin(int indice, int indiceNiveau) //calcul du temps de fin d'execution d'un niveau indiceNiveau (si il n'y a pas de temps de fin alors return -1)
        {
            for (; indice < listeProcessus.Count; indice++) //parcours de listeProcessus à partir du processus d'indice "indice"
            {
                if (listeProcessus[indice].niveau < indiceNiveau) break; //s'il existe un niveau plus prioritaire on sort (tq niv0 plus prio que niv1..)
            }
            if (indice < listeProcessus.Count) return listeProcessus[indice].tempsArriv; //pour eviter le cas ou il n'y a pas eu de parcours de listeProcessus
            else return -1; //l'indice est negatif ou superieur aux nombre de processus (impossible)
        }
        #endregion
    }
}