using FinalAppTest;
using FinalAppTest.Views;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Ordonnancement
{
    public class Niveau
    {
        public List<Processus> listeProcessus = new List<Processus>();
        public List<Processus> listePrets = new List<Processus>();
        public Ordonnancement algo;
        public int numAlgo; //0:PAPS  1:PCA  2:Priorité  3:RoundRobin
        public int[] indice = new int[4];

        public Niveau(int numAlgo)
        {
            this.numAlgo = numAlgo;
            switch (this.numAlgo)
            {
                case 0:
                    algo = new PAPS();
                    break;
                case 1:
                    algo = new PCA();
                    break;
                case 2:
                    algo = new PSP();
                    break;
                case 3:
                    Console.WriteLine("Il manque un quantum. Veuillez reessayer");
                    break;
                default:
                    Console.WriteLine("ERREUR. Veuillez choisir un numero entre 0 et 3");
                    break;
            }
        }
        public Niveau(int numAlgo, int quantum)
        {
            this.numAlgo = numAlgo;
            switch (this.numAlgo)
            {
                case 0:
                    algo = new PAPS();
                    break;
                case 1:
                    algo = new PCA();
                    break;
                case 2:
                    algo = new PSP();
                    break;
                case 3:
                    algo = new RoundRobin(quantum);
                    break;
                default:
                    Console.WriteLine("ERREUR. Veuillez choisir un numero entre 0 et 3");
                    break;
            }
        }
    }

    public class ProcessusNiveau : Processus
    {
        public int niveau;

        public ProcessusNiveau() { }
        
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

    public class AffichageProcessus : ProcessusNiveau  // utilisé pour la simulation
    {
        public string Background { get; set; }
        public AffichageProcessus() { }

        public void Inserer(Grid Table, int i)  // inserer un processus dans Table à la i'éme ligne
        {
            PAPS_TabRow item = new PAPS_TabRow();
            RowDefinition rowdef = new RowDefinition(); rowdef.Height = new GridLength(50);
            Table.RowDefinitions.Insert(i+1, rowdef);
            if (i % 2 == 0) this.Background = "#FFEFF3F9";
            else this.Background = "#FFEFF3F9";
            item.DataContext = this;
            Grid.SetRow(item, i + 1);
            Table.Children.Add(item);
        }
    }

    class MultiNiveau : Ordonnancement
    {
        protected new List<ProcessusNiveau> listeProcessus = new List<ProcessusNiveau>();
        private int nbNiveau;
        private Niveau[] niveaux;

        public MultiNiveau(int nbNiveau, Niveau[] niveaux)
        {
            this.nbNiveau = nbNiveau;
            this.niveaux = new Niveau[nbNiveau];
            this.niveaux = niveaux;
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
        public int Executer()
        {
            SortListeProcessus();  //trier la liste des processus
            InitNiveaux();   //remplir les niveaux
            int temps = 0, indice = 0, indiceNiveau = 0, tempsFin;
            while (indice < listeProcessus.Count || indiceNiveau < nbNiveau) //tant que le processus est dans listeProcessus ou il existe un niveau non vide
            {
                indice = AjouterTous(temps, indice);  //remplir la liste des processus prêts de chaque niveau
                for (indiceNiveau = 0; indiceNiveau < nbNiveau && niveaux[indiceNiveau].listePrets.Count == 0; indiceNiveau++) ; //la recherche du permier niveau non vide
                if (indiceNiveau < nbNiveau)  //il existe un niveau non vide
                {
                    tempsFin = TempsFin(indice, indiceNiveau);  //calcul du temps de fin d'execution
                    niveaux[indiceNiveau].indice[0] = indice;   //pour sauvegarder l'indice "indice" (temporairement)
                    temps = NiveauExecute(temps, tempsFin, niveaux, indiceNiveau, listeProcessus);  //temps de fin d'execution du niveau "indiceNiveau"
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
        public int NiveauExecute(int temps, int tempsFin, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeProcessus)  //executer le niveau "indiceNiveau"
        {
            niveaux[indiceNiveau].algo.Init(niveaux[indiceNiveau].listeProcessus, niveaux[indiceNiveau].listePrets); //initialisation de algo avec la liste des processus et la liste des processus prêts du niveau
            switch (niveaux[indiceNiveau].numAlgo)
            {
                case 0:
                    temps = ((PAPS)niveaux[indiceNiveau].algo).Executer(temps, tempsFin, niveaux, indiceNiveau, listeProcessus); //executer le niveau jusqu'à tempsfin en incrementant le temps
                    break;
                case 1:
                    temps = ((PCA)niveaux[indiceNiveau].algo).Executer(temps, tempsFin, niveaux, indiceNiveau, listeProcessus); //(pareil que case 0)
                    break;
                case 2:
                    temps = ((PSP)niveaux[indiceNiveau].algo).Executer(temps, tempsFin, niveaux, indiceNiveau, listeProcessus); //(pareil que case 0)
                    break;
                case 3:
                    temps = ((RoundRobin)niveaux[indiceNiveau].algo).Executer(temps, tempsFin, niveaux[indiceNiveau].indice, niveaux, indiceNiveau, listeProcessus); //(pareil que case 0)
                    break;
                default:
                    Console.WriteLine("ERREUR. Le num d'algo doit être entre 0 et 3");
                    break;
            }
            return temps;
        }
        public override int AjouterTous(int temps, int indice) //ajouter à la liste des processus prêts de chaque niveau tous les processus de "listeProcessus" (liste ordonnée) dont le temps d'arrivé est <= au temps réel d'execution
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
        public int TempsFin(int indice, int indiceNiveau) //calcul du temps de fin d'execution d'un niveau indiceNiveau (si il n'y a pas de temps de fin alors return -1)
        {
            for (; indice < listeProcessus.Count; indice++) //parcours de listeProcessus à partir du processus d'indice "indice"
            {
                if (listeProcessus[indice].niveau < indiceNiveau) break; //s'il existe un niveau plus prioritaire on sort (tq niv0 plus prio que niv1..)
            }
            if (indice < listeProcessus.Count) return listeProcessus[indice].tempsArriv; //pour eviter le cas ou il n'y a pas eu de parcours de listeProcessus
            else return -1; //l'indice est negatif ou superieur aux nombre de processus (impossible)
        }
    }
}