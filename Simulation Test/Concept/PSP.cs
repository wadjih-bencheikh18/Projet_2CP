using Simulation_Test;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Ordonnancement
{
    public class PSP : Ordonnancement
    {
        public PSP() { }
        public async Task<int> Executer(StackPanel ListProcessusView, StackPanel Processeur, TextBlock TempsView)  // executer la liste des processus 
        {
            ProcessusDesign item;
            ProcessusString pro;
            SortListeProcessus(); //tri des processus par ordre d'arrivé
            int temps = 0;
            int indice = 0;
            bool anime = true,debut=true;
            Processus proc;
            while (listePrets.Count != 0 || indice < listeProcessus.Count) //Tant qu'il existe des processus prêts
            {
                if (listePrets.Count == 0) anime = true;
                int indiceDebut = indice;
                indice = AjouterTous(temps, indice, ListProcessusView);  // Remplir listePrets
                if (indiceDebut != indice)
                    await Task.Delay(1000);
                else await Task.Delay(500);
                if (listePrets.Count != 0)
                {
                    bool sort = false;
                    proc = listePrets[0];
                    List<Processus> P = new List<Processus>();
                    foreach (Processus Pro in listePrets)
                    {
                        P.Add(Pro);
                    }
                    listePrets.Sort(delegate (Processus x, Processus y)
                    {
                        if (x.prio.CompareTo(y.prio) == 0) return x.tempsArriv.CompareTo(y.tempsArriv); //si les processus ont la même priorité, on les trie selon le temps d'arrivée
                        else return x.prio.CompareTo(y.prio); //sinon, on fait le tri par priorité
                    }
                                        );
                    for(int i = 0; i < P.Count; i++)
                    {
                        if (P[i].id!=listePrets[i].id) sort = true;
                    }
                    if (sort)
                    {
                        if (!debut && proc!= listePrets[0])
                        {
                            Storyboard animeDis = new Storyboard();
                            animeDis.Children.Add(Processeur.FindResource("Disactive") as Storyboard);
                            animeDis.Begin((FrameworkElement)Processeur.Children[0]);
                            await Task.Delay(1000);
                            Processeur.Children.Clear();
                            pro = new ProcessusString(proc);
                            pro.X = 600 - 60 * ListProcessusView.Children.Count;
                            item = new ProcessusDesign();
                            item.DataContext = pro;
                            ListProcessusView.Children.Add(item);  
                            await Task.Delay(1000);
                        }
                        
                        ListProcessusView.Children.Clear();
                        int i;
                        if (proc != listePrets[0])
                        {
                            i = 0;
                            anime = true;
                        }
                        else i = 1;
                        for (; i < listePrets.Count; i++)
                        {
                            item = new ProcessusDesign();
                            pro = new ProcessusString(listePrets[i]);
                            item.DataContext = pro;
                            ListProcessusView.Children.Add(item);
                        }
                        await Task.Delay(500);
                    }
                }
                if (anime && listePrets.Count != 0) //si un tri par durée est necessaire et il y a des processus prêts
                {
                    Processeur.Children.Clear();
                    item = new ProcessusDesign();
                    pro = new ProcessusString(listePrets[0]);
                    pro.X = -88;
                    pro.Y = -130;
                    item.DataContext = pro;
                    if (ListProcessusView.Children.Count != 0)
                    {
                        Storyboard AnimeProc = new Storyboard();
                        Storyboard AnimeList = new Storyboard();
                        AnimeList.Children.Add(ListProcessusView.FindResource("ListDecalage") as Storyboard);
                        AnimeProc.Children.Add(ListProcessusView.FindResource("up") as Storyboard);
                        AnimeProc.Begin((FrameworkElement)ListProcessusView.Children[0]);
                        await Task.Delay(500);
                        Processeur.Children.Add(item);
                        ListProcessusView.Children[0].Visibility = Visibility.Hidden;
                        AnimeList.Begin(ListProcessusView);
                        await Task.Delay(1000);
                        AnimeList.Children.Clear();
                        AnimeList.Children.Add(ListProcessusView.FindResource("Listback") as Storyboard);
                        AnimeList.Begin(ListProcessusView);
                        ListProcessusView.Children.RemoveAt(0);
                        await Task.Delay(1000);
                    }
                    else
                    {
                        Processeur.Children.Add(item);
                        await Task.Delay(2000);
                    }
                    anime = false;
                }
                debut = false;
                temps++;
                TempsView.Text = temps.ToString();
                if (listePrets.Count != 0) //S'il y a des processus prêts
                {
                    listePrets[0].etat = 2;
                    if (listePrets[0].tempsRestant == listePrets[0].duree) listePrets[0].tempsReponse = temps - 1 - listePrets[0].tempsArriv;
                    listePrets[0].tempsRestant--; //L'exécution courante du 1er processus de listePrets => décrémenter tempsRestant
                    item = new ProcessusDesign();
                    pro = new ProcessusString(listePrets[0]);
                    item.DataContext = pro;
                    Processeur.Children.Clear();
                    Processeur.Children.Add(item);
                    AfficheLigne(temps - 1, listePrets[0].id); //affiche le temps et l'ID du processus entrain d'être executé
                    if (listePrets[0].tempsRestant == 0)  // Si l'execution du premier processus de listePrets est terminée :
                    {
                        listePrets[0].tempsFin = temps; // temps de fin d'execution = temps actuel
                        listePrets[0].tempsService = temps - listePrets[0].tempsArriv; // temps de service = temps de fin d'execution - temps d'arrivé
                        listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree; //temps d'attente = temps de service - durée d'execution
                        listePrets[0].etat = 3;
                        listePrets.RemoveAt(0); //supprimer le processus dont la duree est écoulée
                        anime = true;
                        Storyboard AnimeDone = new Storyboard();
                        AnimeDone.Children.Add(item.FindResource("processusDone") as Storyboard);
                        AnimeDone.Begin((FrameworkElement)Processeur.Children[0]);
                        await Task.Delay(1000);
                        Processeur.Children.Clear();
                        debut = true;
                    }
                }
                else AfficheLigne(temps - 1); //affiche le temps actuel et le mot "repos" ie le processeur n'execute aucun processus
            }
            return temps;
        }
        public int Executer()  // executer la liste des processus 
        {
            SortListeProcessus(); //tri des processus par ordre d'arrivé
            int temps = 0;  
            int indice = 0;
            while (listePrets.Count != 0 || indice < listeProcessus.Count) //Tant qu'il existe des processus prêts
            {
                indice = AjouterTous(temps, indice);  // Remplir listePrets
                temps++;
                if (listePrets.Count != 0) //S'il y a des processus prêts
                {
                    listePrets.Sort(delegate (Processus x, Processus y)
                                           {
                                               if (x.prio.CompareTo(y.prio) == 0) return x.tempsArriv.CompareTo(y.tempsArriv); //si les processus ont la même priorité, on les trie selon le temps d'arrivée
                                               else return x.prio.CompareTo(y.prio); //sinon, on fait le tri par priorité
                                           }
                                        );
                    listePrets[0].etat = 2;
                    if (listePrets[0].tempsRestant == listePrets[0].duree) listePrets[0].tempsReponse = temps-1 - listePrets[0].tempsArriv;
                    listePrets[0].tempsRestant--; //L'exécution courante du 1er processus de listePrets => décrémenter tempsRestant
                    AfficheLigne(temps - 1, listePrets[0].id); //affiche le temps et l'ID du processus entrain d'être executé
                    if (listePrets[0].tempsRestant == 0)  // Si l'execution du premier processus de listePrets est terminée :
                    {
                        listePrets[0].tempsFin = temps; // temps de fin d'execution = temps actuel
                        listePrets[0].tempsService = temps - listePrets[0].tempsArriv; // temps de service = temps de fin d'execution - temps d'arrivé
                        listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree; //temps d'attente = temps de service - durée d'execution
                        listePrets[0].etat = 3;
                        listePrets.RemoveAt(0); //supprimer le processus dont la duree est écoulée
                    }
                }
                else AfficheLigne(temps - 1); //affiche le temps actuel et le mot "repos" ie le processeur n'execute aucun processus
            }
            return temps;
        }

        // des algorithmes nécessaires pour implémenter MultiNiveaux

        public int Executer(int tempsDebut, int tempsFin, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral)
        {
            int temps = tempsDebut;  // initialisation du temps
            while (listePrets.Count != 0 && (temps < tempsFin || tempsFin == -1))
            //s'il existe des processus prêts et ( On n'est pas encore arrivé à tempsFin ou il n'y a pas de temps fin )
            {
                niveaux[indiceNiveau].indice[0] = AjouterTous(temps, niveaux[indiceNiveau].indice[0], niveaux, listeGeneral, indiceNiveau);  //remplir la liste des processus prêts de chaque niveau
                temps++;
                if (listePrets.Count != 0) //S'il y a des processus prêts
                {
                    listePrets.Sort(
                                           delegate (Processus x, Processus y)
                                           {
                                               if (x.prio.CompareTo(y.prio) == 0) return x.tempsArriv.CompareTo(y.tempsArriv); //si les processus ont la même priorité, on les trie selon le temps d'arrivée
                                               else return x.prio.CompareTo(y.prio); //sinon, on fait le tri par priorité
                                           }
                                        );
                    listePrets[0].etat = 2;
                    if (listePrets[0].tempsRestant == listePrets[0].duree) listePrets[0].tempsReponse = temps-1 - listePrets[0].tempsArriv;
                    listePrets[0].tempsRestant--;//L'exécution courante du 1er processus de listePrets => décrémenter tempsRestant
                    AfficheLigne(temps - 1, listePrets[0].id); //affiche le temps et l'ID du processus entrain d'être executé
                    if (listePrets[0].tempsRestant == 0) // Si l'execution du premier processus de listePrets est terminée :
                    {
                        listePrets[0].tempsFin = temps; // temps de fin d'execution = temps actuel
                        listePrets[0].tempsService = temps - listePrets[0].tempsArriv; // temps de service = temps de fin d'execution - temps d'arrivé
                        listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree; //temps d'attente = temps de service - durée d'execution
                        listePrets[0].etat = 3;
                        listePrets.RemoveAt(0); //supprimer le processus dont la duree est écoulée
                    }
                }
                if (temps == tempsFin)
                {
                    listePrets[0].etat = 1;
                    listePrets.Add(listePrets[0]);
                    listePrets.RemoveAt(0);
                    return temps;
                }
            }
            return temps;
        }
    }

}