using System.Collections.Generic;
using FinalAppTest;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Ordonnancement
{
    public class RoundRobin : Ordonnancement
    {

        #region Constructeur
        public int quantum { get; set; }

        public RoundRobin(int q) // Constructeur 
        {
            quantum = q;
        }
        #endregion

        #region Visualisation
        public override async Task<int> Executer(StackPanel ListePretsView, StackPanel Processeur, TextBlock TempsView, StackPanel ListeBloqueView)
        {
            ProcessusDesign item;
            AffichageProcessus pro;
            bool anime = false;
            SortListeProcessus(); //Tri de la liste des processus par temps d'arrivée
            int indice = 0, temps = 0, q = 0;
            while (indice < listeProcessus.Count || listePrets.Count != 0)  //s'il existe des processus prêts
            {
                if (indice < listeProcessus.Count && listePrets.Count == 0)  // Si il y a des processus dans listeProcessus et la listePrets est vide
                {
                    if (temps < listeProcessus[indice].tempsArriv)  // si aucun processus n'est arrivé
                    {
                        temps++;
                        InterruptionExecute(ListePretsView, ListeBloqueView, Processeur);
                    }
                    else
                    {
                        indice = await MAJListePrets(temps, indice, ListePretsView);  // Remplir listePrets
                        anime = true;
                    }  // sinon, on ajoute les processus arrivés à listePrets
                }
                else  // listePrets n'est pas vide 
                {
                    if (anime)
                    {
                        Processeur.Children.Clear();
                        item = new ProcessusDesign();
                        pro = new AffichageProcessus(listePrets[0]);
                        pro.X = -89.6;
                        pro.Y = -140.8;
                        item.DataContext = pro;
                        if (ListePretsView.Children.Count != 0)
                        {
                            Storyboard AnimeProc = new Storyboard();
                            Storyboard AnimeList = new Storyboard();
                            AnimeList.Children.Add(ListePretsView.FindResource("ListDecalage") as Storyboard);
                            AnimeProc.Children.Add(ListePretsView.FindResource("up") as Storyboard);
                            AnimeProc.Begin((FrameworkElement)ListePretsView.Children[0]);
                            await Task.Delay(500);
                            Processeur.Children.Add(item);
                            ListePretsView.Children[0].Visibility = Visibility.Hidden;
                            AnimeList.Begin(ListePretsView);
                            await Task.Delay(1000);
                            AnimeList.Children.Clear();
                            AnimeList.Children.Add(ListePretsView.FindResource("Listback") as Storyboard);
                            AnimeList.Begin(ListePretsView);
                            ListePretsView.Children.RemoveAt(0);
                            await Task.Delay(1000);
                        }
                        else
                        {
                            Processeur.Children.Add(item);
                            await Task.Delay(2000);
                        }
                        anime = false;
                    }
                    temps++;
                    TempsView.Text = temps.ToString();
                    InterruptionExecute(ListePretsView, ListeBloqueView, Processeur);
                    q++;  // on incrémente le quantum
                    if (listePrets[0].tempsRestant == listePrets[0].duree) listePrets[0].tempsReponse = temps - 1 - listePrets[0].tempsArriv;
                    listePrets[0].tempsRestant--; //L'exécution courante du 1er processus de listePrets => décrémenter tempsRestant
                    item = new ProcessusDesign();
                    pro = new AffichageProcessus(listePrets[0]);
                    item.DataContext = pro;
                    Processeur.Children.Clear();
                    Processeur.Children.Add(item);
                    indice = await MAJListePrets(temps, indice, ListePretsView);  // Remplir listePrets

                    if (listePrets[0].tempsRestant == 0) //fin d'exécution du processus 
                    {
                        listePrets[0].tempsFin = temps; // temps de fin d'execution = temps actuel
                        listePrets[0].tempsService = temps - listePrets[0].tempsArriv;// temps de service = temps de fin d'execution - temps d'arrivé
                        listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree;  //temps d'attente = temps de service - durée d'execution
                        listePrets[0].etat = 3;
                        listePrets.RemoveAt(0);//supprimer le premier processus executé
                        anime = true;
                        Storyboard AnimeDone = new Storyboard();
                        AnimeDone.Children.Add(item.FindResource("processusDone") as Storyboard);
                        AnimeDone.Begin((FrameworkElement)Processeur.Children[0]);
                        await Task.Delay(1000);
                        Processeur.Children.Clear();
                        q = 0;  // un nouveau quantum va commencer
                    }
                    else if (q == quantum && listePrets.Count == 1) q = 0;
                    else if (q == quantum)  // on a terminé ce quantum => il faut passer au processus suivant => on defile, et à la fin, on enfile le processus courant
                    {
                        listePrets[0].tempsFin = temps;  // On sauvegarde le tempsFin puisqu'on a interrompu l'exécution de ce processus
                        q = 0;  //Un nouveau quantum
                        listePrets[0].etat = 1;
                        Storyboard animeDis = new Storyboard();
                        animeDis.Children.Add(Processeur.FindResource("Disactive") as Storyboard);
                        animeDis.Begin((FrameworkElement)Processeur.Children[0]);
                        await Task.Delay(1000);
                        Processeur.Children.Clear();
                        pro = new AffichageProcessus(listePrets[0]);
                        pro.X = 600 - 60 * ListePretsView.Children.Count;
                        item = new ProcessusDesign();
                        item.DataContext = pro;
                        ListePretsView.Children.Add(item);
                        await Task.Delay(1000);
                        listePrets.Add(listePrets[0]);  //Enfilement à la fin
                        listePrets.RemoveAt(0);  // defiler 
                        anime = true;
                    }
                }
            }
            return temps;
        }
        #endregion

        #region Test
        public int Executer()  // exécuter la liste des processus et retourner le temps total pour terminer l'exécution
        {
            SortListeProcessus(); //Tri de la liste des processus par temps d'arrivée
            int indice = 0, temps = 0, q = 0;
            while (indice < listeProcessus.Count || listePrets.Count != 0)  //s'il existe des processus prêts
            {
                if (indice < listeProcessus.Count && listePrets.Count == 0)  // Si il y a des processus dans listeProcessus et la listePrets est vide
                {
                    if (temps < listeProcessus[indice].tempsArriv)  // si aucun processus n'est arrivé
                    {
                        AfficheLigne(temps); //affiche le temps actuel et le mot "repos", i.e le processeur n'execute aucun processus
                        temps++;
                    }
                    else indice = MAJListePrets(temps, indice);  // sinon, on ajoute les processus arrivés à listePrets
                }
                else  // listePrets n'est pas vide 
                {
                    listePrets[0].etat = 2;
                    AfficheLigne(temps, listePrets[0].id); //affiche le temps actuel et l'ID du processus entrain d'être executé
                    temps++;
                    InterruptionExecute();
                    q++;  // on incrémente le quantum
                    if (listePrets[0].tempsRestant == listePrets[0].duree) listePrets[0].tempsReponse = temps - 1 - listePrets[0].tempsArriv;
                    listePrets[0].tempsRestant--; //L'exécution courante du 1er processus de listePrets => décrémenter tempsRestant
                    indice = MAJListePrets(temps, indice);  // ajouter les processus arrivés à listePrets

                    if (listePrets[0].tempsRestant == 0) //fin d'exécution du processus 
                    {
                        listePrets[0].tempsFin = temps; // temps de fin d'execution = temps actuel
                        listePrets[0].tempsService = temps - listePrets[0].tempsArriv;// temps de service = temps de fin d'execution - temps d'arrivé
                        listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree;  //temps d'attente = temps de service - durée d'execution
                        listePrets[0].etat = 3;
                        listePrets.RemoveAt(0);//supprimer le premier processus executé
                        q = 0;  // un nouveau quantum va commencer
                    }
                    else if (q == quantum)  // on a terminé ce quantum => il faut passer au processus suivant => on defile, et à la fin, on enfile le processus courant
                    {
                        listePrets[0].tempsFin = temps;  // On sauvegarde le tempsFin puisqu'on a interrompu l'exécution de ce processus
                        q = 0;  //Un nouveau quantum
                        listePrets[0].etat = 1;
                        listePrets.Add(listePrets[0]);  //Enfilement à la fin
                        listePrets.RemoveAt(0);  // defiler 
                    }
                }
            }
            return temps;
        }

        #endregion

        #region MultiNiveau
        // Des algorithmes nécessaires pour implémenter MultiNiveaux

        public override int Executer(int tempsDebut, int tempsFin, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral, List<ProcessusNiveau> listebloqueGenerale)
        // executer l'algo pendant un intervalle de temps où :
        // indices[0] est l'indice de listeProcessus où on doit reprendre l'exécution
        // indices[1] est le quantum du temps de la derniére exécution
        {
            while (niveaux[indiceNiveau].indice[0] < listeProcessus.Count || listePrets.Count != 0)  //tant qu'il existe des processus prêts
            {
                if (niveaux[indiceNiveau].indice[0] < listeProcessus.Count && listePrets.Count == 0)  // Si il y a des processus dans listProcessus et listePrets est vide
                {
                    tempsDebut++;  // aucun processus n'est arrivé => on incrémente le temps

                    niveaux[indiceNiveau].indice[0] = MAJListePrets(tempsDebut, niveaux[indiceNiveau].indice[0], niveaux, listeGeneral, indiceNiveau); // On rempli listePrets
                }
                else  // listePrets n'est pas vide 
                {
                    listePrets[0].etat = 2;
                    AfficheLigne(tempsDebut, listePrets[0].id); //affiche le temps actuel et l'ID du processus entrain d'être executé
                    tempsDebut++;
                    InterruptionExecute(listebloqueGenerale);
                    niveaux[indiceNiveau].indice[1]++;  // quantum++
                    if (listePrets[0].tempsRestant == listePrets[0].duree) listePrets[0].tempsReponse = tempsDebut - 1 - listePrets[0].tempsArriv;
                    listePrets[0].tempsRestant--; //L'exécution courante du 1er processus de listePrets => décrémenter tempsRestant
                    niveaux[indiceNiveau].indice[0] = MAJListePrets(tempsDebut, niveaux[indiceNiveau].indice[0], niveaux, listeGeneral, indiceNiveau);  // On rempli listePrets

                    if (listePrets[0].tempsRestant == 0)  // fin d'exécution du processus 
                    {
                        listePrets[0].tempsFin = tempsDebut;
                        listePrets[0].tempsService = tempsDebut - listePrets[0].tempsArriv;
                        listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree;
                        listePrets[0].etat = 3;
                        listePrets.RemoveAt(0); //supprimer le premier processus executé
                        niveaux[indiceNiveau].indice[1] = 0;  // un nouveau quantum va commencer
                    }
                    else if (niveaux[indiceNiveau].indice[1] == quantum)  // on a terminé ce quantum => il faut passer au processus suivant => on defile, et à la fin, on enfile le processus courant
                    {
                        listePrets[0].tempsFin = tempsDebut;  // On sauvegarde le tempsFin puisqu'on a interrompu l'exécution de ce processus
                        niveaux[indiceNiveau].indice[1] = 0;  // nouveau quantum
                        listePrets[0].etat = 1;
                        listePrets.Add(listePrets[0]);  // enfiler à la fin
                        listePrets.RemoveAt(0);  // defiler 
                    }
                }
                if (tempsDebut == tempsFin)  // On est arrivé à tempsFin => la fin de l'exécution 
                {
                    listePrets[0].etat = 1;
                    listePrets.Add(listePrets[0]);
                    listePrets.RemoveAt(0);
                    return tempsFin;
                }
            }
            return tempsDebut;
        }
        #endregion

    }
}