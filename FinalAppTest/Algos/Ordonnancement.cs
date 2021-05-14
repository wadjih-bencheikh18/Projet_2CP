using FinalAppTest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Ordonnancement
{
    public abstract class Ordonnancement
    {

        #region Attributs
        public List<Processus> listeProcessus = new List<Processus>();  // liste des processus fournis par l'utilisateur
        public List<Processus> listePrets = new List<Processus>();  // liste des processus prêts
        public List<Processus> listebloque = new List<Processus>();
        #endregion

        #region Visualisation

        public abstract Task<int> Executer(StackPanel ListePretsView, StackPanel Processeur, TextBlock TempsView, StackPanel ListeBloqueView);

        public async Task<int> MAJListePrets(int temps, int indice, StackPanel ListePretsView) //ajouter à la liste des processus prêts tous les processus de "listeProcessus" (liste ordonnée) dont le temps d'arrivé est <= au temps réel d'execution
        {
            bool ajout = false;
            for (; indice < listeProcessus.Count; indice++) //parcours de listeProcessus à partir du processus d'indice "indice"
            {
                if (listeProcessus[indice].tempsArriv > temps) break; //si le processus n'est pas encore arrivé on sort
                else
                {
                    ajout = true;
                    ProcessusDesign item = new ProcessusDesign();
                    AffichageProcessus pro = new AffichageProcessus(listeProcessus[indice]);
                    pro.X = 700;
                    pro.Y = 0;
                    item.DataContext = pro;
                    ListePretsView.Children.Add(item);
                    listePrets.Add(listeProcessus[indice]); //sinon on ajoute le processus à la liste des processus prêts
                }
            }
            if (ajout) await Task.Delay(1000);
            else await Task.Delay(500);
            return indice;
        }
        
        public async Task MAJListBloque(StackPanel ListePretsView, StackPanel ListeBloqueView)
        {
            for (int i = 0; i < listebloque.Count; i++)
            {
                listebloque[i].InterruptionExecute();
                if (listebloque[i].indiceInterruptions[0] == listebloque[i].indiceInterruptions[1])
                {
                    await Reveil(ListePretsView, ListeBloqueView, i);
                    listebloque[i].etat = 1;
                    //ProcessusDesign item = new ProcessusDesign();
                    //AffichageProcessus pro = new AffichageProcessus(listebloque[i]);
                    //item.DataContext = pro;
                    //ListeBloqueView.Children.RemoveAt(i);
                    listePrets.Add(listebloque[i]);
                    //ListePretsView.Children.Add(item);
                    listebloque.RemoveAt(i);
                }
            }
        }
        public async Task InterruptionExecute(StackPanel ListePretsView, StackPanel ListeBloqueView, StackPanel Processeur)
        {
            await MAJListBloque(ListePretsView, ListeBloqueView);
            while (listePrets.Count != 0 && listePrets[0].InterruptionExist())
            {
                ProcessusDesign item = new ProcessusDesign();
                AffichageProcessus pro = new AffichageProcessus(listePrets[0]);
                item.DataContext = pro;
                Processeur.Children.Clear();
                listePrets[0].etat = 0;
                ListeBloqueView.Children.Add(item);
                listebloque.Add(listePrets[0]);
                listePrets.RemoveAt(0);
                await Activation(ListePretsView, Processeur);
            }
        }

        #endregion

        #region Animation

        public async Task Activation(StackPanel ListePretsView, StackPanel Processeur)
        {
            Processeur.Children.Clear();
            ProcessusDesign item = new ProcessusDesign();
            AffichageProcessus pro = new AffichageProcessus(listePrets[0]);
            pro.X = -89.6;
            pro.Y = -140.8;
            item.DataContext = pro;
            Storyboard AnimeProc = new Storyboard();
            Storyboard AnimeList = new Storyboard();
            AnimeList.Children.Add(ListePretsView.FindResource("ListDecalage") as Storyboard);
            AnimeProc.Children.Add(ListePretsView.FindResource("up") as Storyboard);
            AnimeProc.Begin((FrameworkElement)ListePretsView.Children[0]);
            await Task.Delay(1000);
            Processeur.Children.Add(item);
            ListePretsView.Children[0].Visibility = Visibility.Hidden;
            AnimeList.Begin(ListePretsView);
            await Task.Delay(1000);
            AnimeList.Children.Add(ListePretsView.FindResource("Listback") as Storyboard);
            AnimeList.Begin(ListePretsView);
            ListePretsView.Children.RemoveAt(0);
            await Task.Delay(1000);
        }

        public void MAJProcesseur(StackPanel Processeur)
        {
            ProcessusDesign item = new ProcessusDesign();
            AffichageProcessus pro = new AffichageProcessus(listePrets[0]);
            item.DataContext = pro;
            Processeur.Children.Clear();
            Processeur.Children.Add(item);
        }

        public async Task FinProcessus(StackPanel Processeur)
        {
            ProcessusDesign item = new ProcessusDesign();
            Storyboard AnimeDone = new Storyboard();
            AnimeDone.Children.Add(item.FindResource("processusDone") as Storyboard);
            AnimeDone.Begin((FrameworkElement)Processeur.Children[0]);
            await Task.Delay(1000);
            Processeur.Children.Clear();
        }

        public async Task MAJListePretsView(StackPanel ListePretsView, int i)
        {
            ListePretsView.Children.Clear();
            for (; i < listePrets.Count; i++)
            {
                ProcessusDesign item = new ProcessusDesign();
                AffichageProcessus pro = new AffichageProcessus(listePrets[i]);
                item.DataContext = pro;
                ListePretsView.Children.Add(item);
            }
            await Task.Delay(500);
        }
        public async Task Reveil(StackPanel ListePretsView, StackPanel ListeBloqueView,int i)
        {
            ProcessusDesign item = new ProcessusDesign();
            AffichageProcessus pro = new AffichageProcessus(listebloque[i]);
            item.DataContext = pro;
            Storyboard animeReveil = new Storyboard();
            animeReveil.Children.Add(ListeBloqueView.FindResource("up") as Storyboard);
            animeReveil.Begin((FrameworkElement)ListeBloqueView.Children[i]);
            animeReveil = new Storyboard();
            await Task.Delay(1000);
            for(int j=i+1;j< ListeBloqueView.Children.Count; j++)
            {
                animeReveil.Children.Add(ListeBloqueView.FindResource("decalage") as Storyboard);
                animeReveil.Begin((FrameworkElement)ListeBloqueView.Children[j]);
            }
            await Task.Delay(1000);
            ListeBloqueView.Children.RemoveAt(i);
            animeReveil = new Storyboard();
            for (int j = i; j < ListeBloqueView.Children.Count; j++)
            {
                animeReveil.Children.Add(ListeBloqueView.FindResource("Retour") as Storyboard);
                animeReveil.Begin((FrameworkElement)ListeBloqueView.Children[j]);
            }
            ListePretsView.Children.Add(item);
        }
        public async Task Blocage()
        {

        }
        public async Task Desactivation(StackPanel ListePretsView, StackPanel Processeur, Processus proc)
        {
            Storyboard animeDis = new Storyboard();
            animeDis.Children.Add(Processeur.FindResource("Disactive") as Storyboard);
            animeDis.Begin((FrameworkElement)Processeur.Children[0]);
            await Task.Delay(1000);
            Processeur.Children.Clear();
            AffichageProcessus pro = new AffichageProcessus(proc);
            pro.X = 600 - 60 * ListePretsView.Children.Count;
            ProcessusDesign item = new ProcessusDesign();
            item.DataContext = pro;
            ListePretsView.Children.Add(item);
            await Task.Delay(1000);
        }
        #endregion

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
        public abstract int Executer(int tempsDebut, int tempsFin, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral, List<ProcessusNiveau> listebloqueGenerale);
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
        public void MAJListBloque(List<ProcessusNiveau> listebloqueGenerale)
        {
            for (int i = 0; i < listebloque.Count; i++)
            {
                listebloque[i].InterruptionExecute();
                if (listebloque[i].indiceInterruptions[0] == listebloque[i].indiceInterruptions[1])
                {
                    listebloque[i].etat = 1;
                    listePrets.Add(listebloque[i]);
                    listebloqueGenerale.RemoveAll(p => p.id == listebloque[i].id);
                    listebloque.RemoveAt(i);
                }
            }
        }
        public void InterruptionExecute(List<ProcessusNiveau> listebloqueGenerale)
        {
            MAJListBloque(listebloqueGenerale);
            while (listePrets.Count != 0 && listePrets[0].InterruptionExist())
            {
                listePrets[0].etat = 0;
                listebloqueGenerale.Add((ProcessusNiveau)listePrets[0]);
                listebloque.Add(listePrets[0]);
                listePrets.RemoveAt(0);
            }
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