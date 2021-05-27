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

        public abstract Task<int> Executer(StackPanel ListePretsView, StackPanel Processeur, TextBlock TempsView, StackPanel ListeBloqueView,TextBlock deroulement);
        public abstract Task<int> Executer(int tempsDebut, int tempsFin, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral, List<ProcessusNiveau> listebloqueGenerale, StackPanel[] ListesPretsView, StackPanel Processeur, TextBlock TempsView, StackPanel ListeBloqueView, TextBlock deroulement, int i);

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
                    pro.X1 = 700;
                    pro.Y1 = 0;
                    pro.Speed = SimulationPage.Speed;
                    item.DataContext = pro;
                    ListePretsView.Children.Add(item);
                    listePrets.Add(listeProcessus[indice]); //sinon on ajoute le processus à la liste des processus prêts
                }
            }
            if (ajout) await Task.Delay(Convert.ToInt32(1000/ SimulationPage.Speed));
            else await Task.Delay(Convert.ToInt32(500 / SimulationPage.Speed));
            return indice;
        }
        
        public async Task<bool> MAJListBloque(StackPanel ListePretsView, StackPanel ListeBloqueView, TextBlock deroulement)
        {
            bool Anime = false;
            for (int i = 0; i < listebloque.Count; i++)
            {
                listebloque[i].InterruptionExecute();
                if (listebloque[i].indiceInterruptions[0] == listebloque[i].indiceInterruptions[1])
                {
                    
                    listebloque[i].transition = 3; //Reviel du ieme processus de listebloque
                    await AfficherDeroulement(deroulement);
                    await Reveil(ListePretsView, ListeBloqueView, i);
                    listebloque[i].etat = 1;
                    listePrets.Add(listebloque[i]);
                    listebloque.RemoveAt(i);
                    Anime = true;
                }
            }
            return Anime;
        }
        public async Task<bool> InterruptionExecute(StackPanel ListePretsView, StackPanel ListeBloqueView, StackPanel Processeur, TextBlock deroulement)
        {
            bool interupt = false;
            bool vide=false;
            if (listePrets.Count == 0) vide = true;
            bool Anime=await MAJListBloque(ListePretsView, ListeBloqueView,deroulement);
            if (listePrets.Count != 0 && listePrets[0].InterruptionExist())
            {
                interupt = true;
                listePrets[0].transition = 0; //Blocage du ieme processus de listebloque
                listePrets[0].etat = 0;
                listebloque.Add(listePrets[0]);
                await AfficherDeroulement(deroulement);
                await Blocage(ListeBloqueView, Processeur);
                listePrets.RemoveAt(0);
                if (listePrets.Count != 0)
                {
                    listePrets[0].transition = 2;
                    await AfficherDeroulement(deroulement);
                    await Activation(ListePretsView, Processeur, listePrets[0]);
                }
            }
            else if (Anime && vide)
            {
                listePrets[0].transition = 2;
                await AfficherDeroulement(deroulement);
                await Activation(ListePretsView, Processeur, listePrets[0]);
            }
            return interupt;
        }

        #endregion

        #region Animation

        public async Task Activation(StackPanel ListePretsView, StackPanel Processeur,Processus proc)
        {
            Processeur.Children.Clear();
            ProcessusDesign item = new ProcessusDesign();
            AffichageProcessus pro = new AffichageProcessus(proc);
            pro.X1 = -100;
            pro.Y1 = -140;
            pro.Speed = SimulationPage.Speed;
            item.DataContext = pro;
            Storyboard AnimeProc = new Storyboard();
            Storyboard AnimeList = new Storyboard();
            AnimeList.Children.Add(ListePretsView.FindResource("ListDecalage") as Storyboard);
            AnimeList.SpeedRatio = SimulationPage.Speed;
            AnimeProc.Children.Add(ListePretsView.FindResource("up") as Storyboard);
            AnimeProc.SpeedRatio = SimulationPage.Speed;
            AnimeProc.Begin((FrameworkElement)ListePretsView.Children[0]);
            await Task.Delay(Convert.ToInt32(500 / SimulationPage.Speed));
            Processeur.Children.Add(item);
            ListePretsView.Children[0].Visibility = Visibility.Hidden;
            for(int j=1;j< ListePretsView.Children.Count;j++)
            {
                AnimeList.Begin((FrameworkElement)ListePretsView.Children[j]);
            }
            await Task.Delay(Convert.ToInt32(1000 / SimulationPage.Speed));
            AnimeList = new Storyboard();
            AnimeList.Children.Add(ListePretsView.FindResource("Listback") as Storyboard);
            AnimeList.SpeedRatio = SimulationPage.Speed;
            ListePretsView.Children.RemoveAt(0);
            for (int j = 0; j < ListePretsView.Children.Count; j++)
            {
                AnimeList.Begin((FrameworkElement)ListePretsView.Children[j]);
            }
        }

        public async Task Activation_MultiLvl(StackPanel ListePretsView, StackPanel Processeur, Processus proc)
        {
            Processeur.Children.Clear();
            ProcessusDesignMultiLvl item = new ProcessusDesignMultiLvl();
            AffichageProcessus pro = new AffichageProcessus(proc);
            pro.X1 = -136.5;
            pro.Y1 = -75;
            pro.Speed = SimulationPage_MultiLvl.Speed;
            item.DataContext = pro;
            Storyboard AnimeProc = new Storyboard();
            Storyboard AnimeList = new Storyboard();
            AnimeList.Children.Add(ListePretsView.FindResource("ListDecalage") as Storyboard);
            AnimeList.SpeedRatio = SimulationPage_MultiLvl.Speed;
            AnimeProc.Children.Add(ListePretsView.FindResource("up") as Storyboard);
            AnimeProc.SpeedRatio = SimulationPage_MultiLvl.Speed;
            AnimeProc.Begin((FrameworkElement)ListePretsView.Children[0]);
            await Task.Delay(Convert.ToInt32(1500 / SimulationPage_MultiLvl.Speed));
            Processeur.Children.Add(item);
            ListePretsView.Children[0].Visibility = Visibility.Hidden;
            for (int j = 1; j < ListePretsView.Children.Count; j++)
            {
                AnimeList.Begin((FrameworkElement)ListePretsView.Children[j]);
            }
            await Task.Delay(Convert.ToInt32(1000/ SimulationPage_MultiLvl.Speed));
            AnimeList = new Storyboard();
            AnimeList.Children.Add(ListePretsView.FindResource("Listback") as Storyboard);
            AnimeList.SpeedRatio = SimulationPage_MultiLvl.Speed;
            ListePretsView.Children.RemoveAt(0);
            for (int j = 0; j < ListePretsView.Children.Count; j++)
            {
                AnimeList.Begin((FrameworkElement)ListePretsView.Children[j]);
            }
        }

        public void MAJProcesseur(StackPanel Processeur)
        {
            ProcessusDesign item = new ProcessusDesign();
            AffichageProcessus pro = new AffichageProcessus(listePrets[0]);
            pro.Speed = SimulationPage.Speed;
            item.DataContext = pro;
            Processeur.Children.Clear();
            Processeur.Children.Add(item);
        }

        public void MAJProcesseur_MultiLvl(StackPanel Processeur)
        {
            ProcessusDesignMultiLvl item = new ProcessusDesignMultiLvl();
            AffichageProcessus pro = new AffichageProcessus(listePrets[0]);
            pro.Speed = SimulationPage_MultiLvl.Speed;
            item.DataContext = pro;
            Processeur.Children.Clear();
            Processeur.Children.Add(item);
        }

        public async Task FinProcessus(StackPanel Processeur)
        {
            ProcessusDesign item = new ProcessusDesign();
            Storyboard AnimeDone = new Storyboard();
            AnimeDone.Children.Add(item.FindResource("processusDone") as Storyboard);
            AnimeDone.SpeedRatio = SimulationPage.Speed;
            AnimeDone.Begin((FrameworkElement)Processeur.Children[0]);
            await Task.Delay(Convert.ToInt32(1000 / SimulationPage.Speed));
            Processeur.Children.Clear();
        }

        public async Task FinProcessus_MultiLvl(StackPanel Processeur)
        {
            ProcessusDesignMultiLvl item = new ProcessusDesignMultiLvl();
            Storyboard AnimeDone = new Storyboard();
            AnimeDone.Children.Add(item.FindResource("processusDone") as Storyboard);
            AnimeDone.SpeedRatio = SimulationPage_MultiLvl.Speed;
            AnimeDone.Begin((FrameworkElement)Processeur.Children[0]);
            await Task.Delay(Convert.ToInt32(1000 / SimulationPage_MultiLvl.Speed));
            Processeur.Children.Clear();
        }

        public async Task MAJListePretsView(StackPanel ListePretsView, int i)
        {
            ListePretsView.Children.Clear();
            for (; i < listePrets.Count; i++)
            {
                ProcessusDesign item = new ProcessusDesign();
                AffichageProcessus pro = new AffichageProcessus(listePrets[i]);
                pro.Speed = SimulationPage.Speed;
                item.DataContext = pro;
                ListePretsView.Children.Add(item);
            }
            await Task.Delay(Convert.ToInt32(500 / SimulationPage.Speed));
        }

        public async Task MAJListePretsView_MultiLvl(StackPanel ListePretsView, int i)
        {
            ListePretsView.Children.Clear();
            for (; i < listePrets.Count; i++)
            {
                ProcessusDesignMultiLvl item = new ProcessusDesignMultiLvl();
                AffichageProcessus pro = new AffichageProcessus(listePrets[i]);
                pro.Speed = SimulationPage_MultiLvl.Speed;
                item.DataContext = pro;
                ListePretsView.Children.Add(item);
            }
            await Task.Delay(Convert.ToInt32(500 / SimulationPage_MultiLvl.Speed));
        }

        public async Task Reveil(StackPanel ListePretsView, StackPanel ListeBloqueView,int i)
        {
            ProcessusDesign item = new ProcessusDesign();
            AffichageProcessus pro = new AffichageProcessus(listebloque[i]);
            pro.X1 = - 60 * ListePretsView.Children.Count + 60 * i;
            pro.Y1 = 340;
            pro.Y2 = 340;
            pro.X2 = 600 - 60 * ListePretsView.Children.Count;
            pro.X3 = pro.X2;
            pro.Speed= SimulationPage.Speed;
            item.DataContext = pro; 
            Storyboard animeReveil = new Storyboard();
            animeReveil.Children.Add(ListeBloqueView.FindResource("up") as Storyboard);
            animeReveil.SpeedRatio=  SimulationPage.Speed;
            animeReveil.Begin((FrameworkElement)ListeBloqueView.Children[i]);
            await Task.Delay(Convert.ToInt32(500 / SimulationPage.Speed));

            animeReveil = (Storyboard)ListeBloqueView.FindResource("decalage");
            animeReveil.SpeedRatio=  SimulationPage.Speed;
            for (int j=i+1;j< ListeBloqueView.Children.Count; j++)
                animeReveil.Begin((FrameworkElement)ListeBloqueView.Children[j]);

            await Task.Delay(Convert.ToInt32(1000 / SimulationPage.Speed));
            ListeBloqueView.Children.RemoveAt(i);
            animeReveil = ListeBloqueView.FindResource("Retour") as Storyboard;
            animeReveil.SpeedRatio=  SimulationPage.Speed;
            for (int j = i; j < ListeBloqueView.Children.Count; j++)
                animeReveil.Begin((FrameworkElement)ListeBloqueView.Children[j]);
            
            ListePretsView.Children.Add(item);
            await Task.Delay(Convert.ToInt32(3000 / SimulationPage.Speed));
        }

        public async Task Reveil_MultiLvl(StackPanel ListePretsView, StackPanel ListeBloqueView, int i)
        {
            ProcessusDesignMultiLvl item = new ProcessusDesignMultiLvl();
            AffichageProcessus pro = new AffichageProcessus(listebloque[i]);
            pro.X1 = -60 * ListePretsView.Children.Count + 60 * i;
            pro.Y1 = 340;
            pro.Y2 = 340;
            pro.X2 = 600 - 60 * ListePretsView.Children.Count;
            pro.X3 = pro.X2;
            pro.Speed = SimulationPage_MultiLvl.Speed;
            item.DataContext = pro;
            Storyboard animeReveil = new Storyboard();
            animeReveil.Children.Add(ListeBloqueView.FindResource("up") as Storyboard);
            animeReveil.SpeedRatio = SimulationPage_MultiLvl.Speed;
            animeReveil.Begin((FrameworkElement)ListeBloqueView.Children[i]);
            await Task.Delay(Convert.ToInt32(500 / SimulationPage_MultiLvl.Speed));

            animeReveil = (Storyboard)ListeBloqueView.FindResource("decalage"); 
            animeReveil.SpeedRatio = SimulationPage_MultiLvl.Speed;
            for (int j = i + 1; j < ListeBloqueView.Children.Count; j++)
                animeReveil.Begin((FrameworkElement)ListeBloqueView.Children[j]);

            await Task.Delay(Convert.ToInt32(1000 / SimulationPage_MultiLvl.Speed));
            ListeBloqueView.Children.RemoveAt(i);
            animeReveil = ListeBloqueView.FindResource("Retour") as Storyboard;
            animeReveil.SpeedRatio = SimulationPage_MultiLvl.Speed;
            for (int j = i; j < ListeBloqueView.Children.Count; j++)
                animeReveil.Begin((FrameworkElement)ListeBloqueView.Children[j]);

            ListePretsView.Children.Add(item);
            await Task.Delay(Convert.ToInt32(3000 / SimulationPage_MultiLvl.Speed));
        }

        public async Task Blocage(StackPanel ListeBloqueView, StackPanel Processeur)
        {
            Storyboard animeBloque = new Storyboard();
            animeBloque.Children.Add(Processeur.FindResource("Blocage") as Storyboard);
            animeBloque.SpeedRatio= SimulationPage.Speed;
            animeBloque.Begin((FrameworkElement)Processeur.Children[0]);
            await Task.Delay(Convert.ToInt32(1000 / SimulationPage.Speed));
            Processeur.Children.Clear();
            AffichageProcessus pro = new AffichageProcessus(listePrets[0]);
            pro.X1 = 600 - 45 * ListeBloqueView.Children.Count;
            ProcessusDesign item = new ProcessusDesign();
            pro.Speed = SimulationPage.Speed;
            item.DataContext = pro;
            ListeBloqueView.Children.Add(item);
            await Task.Delay(Convert.ToInt32(1000 / SimulationPage.Speed));
        }

        public async Task Blocage_MultiLvl(StackPanel ListeBloqueView, StackPanel Processeur)
        {
            Storyboard animeBloque = new Storyboard();
            animeBloque.Children.Add(Processeur.FindResource("Blocage") as Storyboard);
            animeBloque.SpeedRatio = SimulationPage_MultiLvl.Speed;
            animeBloque.Begin((FrameworkElement)Processeur.Children[0]);
            await Task.Delay(Convert.ToInt32(1000 / SimulationPage_MultiLvl.Speed));
            Processeur.Children.Clear();
            AffichageProcessus pro = new AffichageProcessus(listePrets[0]);
            pro.X1 = 600 - 60 * ListeBloqueView.Children.Count;
            ProcessusDesignMultiLvl item = new ProcessusDesignMultiLvl();
            pro.Speed= SimulationPage_MultiLvl.Speed;
            item.DataContext = pro;
            ListeBloqueView.Children.Add(item);
        }

        public async Task Desactivation(StackPanel ListePretsView, StackPanel Processeur, Processus proc)
        {
            Storyboard animeDis = new Storyboard();
            animeDis.Children.Add(Processeur.FindResource("Disactive") as Storyboard);
            animeDis.SpeedRatio = SimulationPage.Speed;
            animeDis.Begin((FrameworkElement)Processeur.Children[0]);
            await Task.Delay(Convert.ToInt32(1000 / SimulationPage.Speed));
            Processeur.Children.Clear();
            AffichageProcessus pro = new AffichageProcessus(proc);
            pro.X1 = 600 - 60 * ListePretsView.Children.Count;
            pro.Speed= SimulationPage.Speed;
            ProcessusDesign item = new ProcessusDesign();
            item.DataContext = pro;
            ListePretsView.Children.Add(item);
            await Task.Delay(Convert.ToInt32(1000 / SimulationPage.Speed));
        }

        public async Task Desactivation_MultiLvl(StackPanel ListePretsView, StackPanel Processeur, Processus proc, int indiceNiveau)
        {
            Storyboard animeDis = new Storyboard();
            animeDis.Children.Add(Processeur.FindResource("Disactive") as Storyboard);
            animeDis.SpeedRatio = SimulationPage_MultiLvl.Speed;
            animeDis.Begin((FrameworkElement)Processeur.Children[0]);
            await Task.Delay(Convert.ToInt32(1500 / SimulationPage_MultiLvl.Speed));
            Processeur.Children.Clear();
            AffichageProcessus pro = new AffichageProcessus(proc);
            pro.X1 = 600 - 45 * ListePretsView.Children.Count;
            pro.X2 = pro.X1;
            pro.Y1 = 60 * (3 - indiceNiveau) + 50;
            pro.Speed = SimulationPage_MultiLvl.Speed;
            ProcessusDesignMultiLvl item = new ProcessusDesignMultiLvl();
            item.DataContext = pro;
            ListePretsView.Children.Add(item);
            await Task.Delay(Convert.ToInt32(2500 / SimulationPage_MultiLvl.Speed));
        }

        public async Task AfficherDeroulement(TextBlock deroulement) //Affiche les transitions des états des processus
        {
            if (listePrets.Count!=0)
            {   
                if (listePrets[0].etat == 3)
                {
                    deroulement.Text = $"Fin du processus de l'ID = {listePrets[0].id}";
                    await Task.Delay(100);
                }
                else if (listePrets[0].transition == 1)
                {
                    deroulement.Text = $"Désactivation du processus de l'ID = {listePrets[0].id}";
                    await Task.Delay(100);
                }
                else if (listePrets[0].transition == 2)
                    {
                        deroulement.Text = $"Activation du processus de l'ID = {listePrets[0].id}";
                    await Task.Delay(100);
                }
            }
            if (listebloque.Count != 0)
            {
                foreach (Processus pro in listebloque)
                {
                    if (pro.transition == 0)
                    {
                        deroulement.Text = $"Blocage du processus de l'ID = {pro.id}";
                        pro.transition = -1;
                        await Task.Delay(100);
                    }
                    else if (pro.transition == 3)
                    {
                        deroulement.Text = $"Réveil du processus de l'ID = {pro.id}";
                        pro.transition = -1;
                        await Task.Delay(100);
                    }
                }
                
            }
            
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
                    listebloque[i].transition = 1; //Desactivation du ieme processus de listebloque
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
                listePrets[0].transition = 0; //Blocage du 1er processus de listePrets
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
        
        public abstract int Executer(int tempsDebut, int tempsFin, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral, List<ProcessusNiveau> listebloqueGenerale, TextBlock deroulement);
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
                    listebloque[i].transition = 1; //Desactivation du ieme processus de listebloque
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
                listePrets[0].transition = 0; //Blocage du ieme processus de listePrets
                listePrets[0].etat = 0;
                listebloqueGenerale.Add((ProcessusNiveau)listePrets[0]);
                listebloque.Add(listePrets[0]);
                listePrets.RemoveAt(0);
            }
        }
        #region Animation-Multi
        public bool PrioNiveaux(Niveau[] niveaux,int indiceNiveau,int nbNiveau)
        {
            int i;
            for (i = 0; i < nbNiveau && niveaux[i].listePrets.Count == 0; i++) ;
            if (i < indiceNiveau) return false;
            return true;
        }
        public abstract Task<int> Executer(int tempsDebut, int tempsFin, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral, List<ProcessusNiveau> listebloqueGenerale, StackPanel[] ListsPretsViews, StackPanel Processeur, TextBlock TempsView, StackPanel ListeBloqueView, TextBlock deroulement);
        public async Task<int> MAJListePrets(int temps, int indice, Niveau[] niveaux, List<ProcessusNiveau> listeGeneral, int indiceNiveau, StackPanel[] ListesPretsViews) //ajouter à la liste des processus prêts tous les processus de "listeGeneral" (liste ordonnée) dont le temps d'arrivé est <= au temps réel d'execution de MultiNiveaux
        {
            bool ajout = false;
            for (; indice < listeGeneral.Count; indice++) //parcours de listeGeneral à partir du processus d'indice "indice"
            {
                if (listeGeneral[indice].tempsArriv > temps) break; //si le processus n'est pas encore arrivé on sort
                else
                {
                    ajout = true;
                    if (listeGeneral[indice].niveau == indiceNiveau) listePrets.Add(listeGeneral[indice]); //si le niveau du processus = indiceNiveau (niveau actuel) on ajoute ce processus à la liste des processus prêts de ce niveau
                    else niveaux[listeGeneral[indice].niveau].listePrets.Add(listeGeneral[indice]); //sinon on ajoute le processus à la liste des processus prêts de son niveau
                    AffichageProcessus pro = new AffichageProcessus(listeGeneral[indice]);
                    ProcessusDesignMultiLvl item = new ProcessusDesignMultiLvl();
                    item.DataContext = pro;
                    pro.X1 = 700;
                    pro.Y1 = 0;
                    pro.Speed = SimulationPage_MultiLvl.Speed;
                    item.DataContext = pro;
                    ListesPretsViews[listeGeneral[indice].niveau].Children.Add(item);
                }
            }
            if (ajout) await Task.Delay(Convert.ToInt32(1000 / SimulationPage_MultiLvl.Speed));
            else await Task.Delay(Convert.ToInt32(500 / SimulationPage_MultiLvl.Speed));
            return indice;
        }
        public async Task<bool> MAJListBloque(List<ProcessusNiveau> listebloqueGenerale,StackPanel[] ListesPretsViews, StackPanel ListeBloqueView, TextBlock deroulement)
        {
            bool Anime = false;
            for (int i = 0; i < listebloqueGenerale.Count; i++)
            {
                listebloqueGenerale[i].InterruptionExecute();
                if (listebloqueGenerale[i].indiceInterruptions[0] == listebloqueGenerale[i].indiceInterruptions[1])
                {
                    listebloqueGenerale[i].transition = 3; //Réveil du ieme processus de listebloqueGenerale
                    listebloqueGenerale[i].etat = 1;
                    listePrets.Add(listebloqueGenerale[i]);
                    await AfficherDeroulement(deroulement);
                    await Reveil_MultiLvl(ListesPretsViews[listebloqueGenerale[i].niveau], ListeBloqueView, i);
                    listebloqueGenerale.RemoveAt(i);
                    listebloque.RemoveAt(i);
                    Anime = true;
                }
            }
            return Anime;
        }
        public async Task<bool> InterruptionExecute(List<ProcessusNiveau> listebloqueGenerale, StackPanel[] ListesPretsViews,int indiceNiveau, StackPanel ListeBloqueView, StackPanel Processeur, TextBlock deroulement)
        {
            bool interupt = false;
            bool vide = false;
            if (listePrets.Count == 0) vide = true;
            bool Anime=await MAJListBloque(listebloqueGenerale,ListesPretsViews, ListeBloqueView, deroulement);
            if (listePrets.Count != 0 && listePrets[0].InterruptionExist())
            {
                interupt = true;
                listePrets[0].transition = 0; //Blocage du processus qui était entrain d'exécution
                listePrets[0].etat = 0;
                await AfficherDeroulement(deroulement);
                listebloqueGenerale.Add((ProcessusNiveau)listePrets[0]);
                await Blocage_MultiLvl(ListeBloqueView, Processeur);
                listebloque.Add(listePrets[0]);
                listePrets.RemoveAt(0);
                if (listePrets.Count != 0)
                {
                    listePrets[0].transition = 2; //Activation du 1er processus de ListePrets
                    await AfficherDeroulement(deroulement);
                    await Activation_MultiLvl(ListesPretsViews[indiceNiveau], Processeur, listePrets[0]);
                }
            }
            else if (Anime && vide)
            {
                listePrets[0].transition = 2; //Activation du 1er processus de ListePrets
                await AfficherDeroulement(deroulement);
                await Activation_MultiLvl(ListesPretsViews[indiceNiveau], Processeur, listePrets[0]);
            }
            return interupt;
        }
        #endregion
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

        #region PrioDyn
        public void UpdateStackTime(int temps)
        {

            foreach (Processus pro in listePrets)
                pro.CalculeSlackTime(temps);

        }
        #endregion
    }
}