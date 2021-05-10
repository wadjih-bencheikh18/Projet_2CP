using FinalAppTest;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Ordonnancement
{
    public class Processus
    {
        //donnés
        public int id { get; set; } //ID du processus
        public int tempsArriv { get; set; }//temps d'arrivé
        public int duree { get; set; } //temps d'execution du processus (burst time)
        public int prio { get; set; }//priorite du processus
        //à remplir
        public int etat { get; set; } // 0:bloqué  1:prêt  2:en cours  3:fini
        public int tempsFin { get; set; }
        public int tempsAtt { get; set; }
        public int tempsService { get; set; }
        public int tempsRestant { get; set; }
        public int tempsReponse { get; set; }

        public Processus() { }

        public Processus(int id, int tempsArriv, int duree, int prio)  //constructeur pour l'algorithme de priorité
        {
            this.etat = 1;
            this.id = id;
            this.tempsArriv = tempsArriv;
            this.duree = duree;
            this.prio = prio;
            tempsRestant = duree;
        }

        public Processus(Processus P)  //constructeur pour l'algorithme de priorité
        {
            etat = P.etat;
            id = P.id;
            tempsArriv = P.tempsArriv;
            duree = P.duree;
            prio = P.prio;
            tempsRestant = P.tempsRestant;
            tempsAtt = P.tempsAtt;
            tempsService = P.tempsService;
            tempsReponse = P.tempsReponse;
            tempsFin = P.tempsFin;
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

        /*public void Affichage(Grid Table, int i)
        {
            ProcessusString proc;
            TableRowFinal item;
            RowDefinition rowdef = new RowDefinition();
            rowdef.Height = new GridLength(30);
            Table.RowDefinitions.Insert(i, rowdef);
            item = new TableRowFinal();
            proc = new ProcessusString(this);
            if (i % 2 == 0) proc.Background = "LightBlue";
            else proc.Background = "lightGray";
            item.DataContext = proc;
            Grid.SetRow(item, i + 1);
            Table.Children.Add(item);
        }
        public void Affichage(Grid Table, int i, int niveau)
        {
            ProcessusString proc;
            TableRowInit item;

            RowDefinition rowdef = new RowDefinition();
            rowdef.Height = new GridLength(30);
            Table.RowDefinitions.Insert(i, rowdef);
            item = new TableRowInit();
            item.Name = "5";
            proc = new ProcessusString(this);
            if (i % 2 == 0) proc.Background = "LightBlue";
            else proc.Background = "lightGray";
            item.DataContext = proc;
            Grid.SetRow(item, i + 1);
            Table.Children.Add(item);
        }*/
    }


    class ProcessusString
    {
        public string id { get; set; }
        public string tempsRestant { get; set; }
        public string tempsArriv { get; set; }
        public string duree { get; set; }
        public string tempsFin { get; set; }
        public string tempsAtt { get; set; }
        public string tempsService { get; set; }
        public string Background { get; set; }
        public string tempsReponse { get; set; }
        public string prio { set; get; }
        public string niveau { set; get; }

        public ProcessusString(Processus processus)
        {
            this.id = processus.id.ToString();
            this.tempsArriv = processus.tempsArriv.ToString();
            this.duree = processus.duree.ToString();
            this.tempsAtt = processus.tempsAtt.ToString();
            this.tempsFin = processus.tempsFin.ToString();
            this.tempsService = processus.tempsService.ToString();
            this.tempsReponse = processus.tempsReponse.ToString();

        }

        public ProcessusString(int id, int tempsRestant)
        {
            this.id = id.ToString();
            this.tempsRestant = tempsRestant.ToString();
        }
    }


    public abstract class Ordonnancement
    {
        public List<Processus> listeProcessus = new List<Processus>();  // liste des processus fournis par l'utilisateur
        public List<Processus> listePrets = new List<Processus>();  // liste des processus prêts
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
            Console.WriteLine(temps + "\t|\t" + id + "\t\t|");
        }
        public void AfficheLigne(int temps) //affiche le temps actuel et le mot "repos" ie le processeur n'execute aucun processus
        {
            Console.WriteLine(temps + "\t|\t   Repos   \t\t|");
        }
        public void Init(List<Processus> listeProcessus, List<Processus> listePrets)
        {
            this.listeProcessus = listeProcessus;
            this.listePrets = listePrets;
        }
        Action EmptyDelegate = delegate () { };
        /*public void AfficheLigne(int temps, List<Processus> listePrets, StackPanel ListProcessusView, StackPanel Processeur, TextBlock TempsView) //affiche le temps actuel et l'ID du processus entrain d'être executé
        {
            ProcessusDesign item;
            if (listePrets.Count != 0)
            {
                item = new ProcessusDesign();
                item.DataContext = listePrets[0];
                Processeur.Children.Add(item);
            }
            for (int i = 1; i < listePrets.Count; i++)
            {
                item = new ProcessusDesign();
                item.DataContext = listePrets[i];
                ListProcessusView.Children.Add(item);
            }
            TempsView.Text = temps.ToString();
            ListProcessusView.Dispatcher.Invoke(DispatcherPriority.Input, EmptyDelegate);
            Thread.Sleep(500);
            ListProcessusView.Children.Clear();
            Processeur.Children.Clear();

        }*/
    }
}