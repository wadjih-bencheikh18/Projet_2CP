using FinalAppTest;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Ordonnancement
{
    public partial class PAPS : Ordonnancement
    {

        public PAPS() { }
        public override async  Task<int> Executer(StackPanel ListProcessusView, StackPanel Processeur, TextBlock TempsView)
        {
            SortListeProcessus(); //tri des processus par ordre d'arrivé
            int temps = 0, indice = 0;
            bool anime = false;
            while (indice < listeProcessus.Count || listePrets.Count != 0) //s'il existe des processus non executés
            {
                if (listePrets.Count == 0) anime = true;
                int indiceDebut = indice;
                indice = AjouterTous(temps, indice, ListProcessusView); //remplir listePrets
                if (indiceDebut != indice)
                    await Task.Delay(1000);
                else await Task.Delay(500);
                if (listePrets.Count != 0 && anime)
                {
                    Processeur.Children.Clear();
                    ProcessusDesign item = new ProcessusDesign();
                    ProcessusString pro = new ProcessusString(listePrets[0]);
                    pro.X = "-89.6";
                    pro.Y = "-140.8";
                    item.DataContext = pro;
                    if (ListProcessusView.Children.Count != 0)
                    {
                        Storyboard AnimeProc = new Storyboard();
                        Storyboard AnimeList = new Storyboard();
                        AnimeList.Children.Add(ListProcessusView.FindResource("ListDecalage") as Storyboard);
                        AnimeProc.Children.Add(ListProcessusView.FindResource("up") as Storyboard);
                        AnimeProc.Begin((FrameworkElement)ListProcessusView.Children[0]);
                        await Task.Delay(1000);
                        Processeur.Children.Add(item);
                        ListProcessusView.Children[0].Visibility = Visibility.Hidden;
                        AnimeList.Begin(ListProcessusView);
                        await Task.Delay(1000);
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
                temps++; //incrementer le temps réel
                TempsView.Text = temps.ToString();
                if (listePrets.Count != 0) //s'il y a des processus prêts
                {
                    listePrets[0].etat = 2;
                    if (listePrets[0].tempsRestant == listePrets[0].duree) listePrets[0].tempsReponse = temps - 1 - listePrets[0].tempsArriv;
                    listePrets[0].tempsRestant--; //l'execution du 1er processus de listePrets commence               //AfficheLigne(temps,listePrets, ListProcessusView, Processeur, TempsView); //affiche le temps actuel et l'ID du processus entrain d'être executé
                    ProcessusDesign item = new ProcessusDesign();
                    ProcessusString pro = new ProcessusString(listePrets[0]);
                    item.DataContext = pro;
                    Processeur.Children.Clear();
                    Processeur.Children.Add(item);
                    if (listePrets[0].tempsRestant == 0) //si l'execution du premier processus de listePrets est terminée
                    {
                        listePrets[0].tempsFin = temps; //temps de fin d'execution = au temps actuel
                        listePrets[0].tempsService = temps - listePrets[0].tempsArriv; //temps de service = temps de fin d'execution - temps d'arrivé
                        listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree; //temps d'attente = temps de service - durée d'execution
                        listePrets[0].etat = 3;
                        listePrets.RemoveAt(0); //supprimer le premier processus executé

                        Storyboard AnimeDone = new Storyboard();
                        AnimeDone.Children.Add(item.FindResource("processusDone") as Storyboard);
                        AnimeDone.Begin((FrameworkElement)Processeur.Children[0]);
                        await Task.Delay(1000);

                        Processeur.Children.Clear();
                        if (listePrets.Count != 0)
                        {
                            anime = true;
                        }
                        else Processeur.Children.Clear();
                    }
                }
            }
            return temps;
        }
        public int Executer()
        {
            SortListeProcessus(); //tri des processus par ordre d'arrivé
            int temps = 0, indice = 0;
            while (indice < listeProcessus.Count || listePrets.Count != 0) //s'il existe des processus non executés
            {
                indice = AjouterTous(temps, indice); //remplir listePrets
                temps++; //incrementer le temps réel
                if (listePrets.Count != 0) //s'il y a des processus prêts
                {
                    listePrets[0].etat = 2;
                    if (listePrets[0].tempsRestant == listePrets[0].duree) listePrets[0].tempsReponse = temps - 1 - listePrets[0].tempsArriv;
                    listePrets[0].tempsRestant--; //l'execution du 1er processus de listePrets commence
                    AfficheLigne(temps - 1, listePrets[0].id); //affiche le temps actuel et l'ID du processus entrain d'être executé
                    if (listePrets[0].tempsRestant == 0) //si l'execution du premier processus de listePrets est terminée
                    {
                        listePrets[0].tempsFin = temps; //temps de fin d'execution = au temps actuel
                        listePrets[0].tempsService = temps - listePrets[0].tempsArriv; //temps de service = temps de fin d'execution - temps d'arrivé
                        listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree; //temps d'attente = temps de service - durée d'execution
                        listePrets[0].etat = 3;
                        listePrets.RemoveAt(0); //supprimer le premier processus executé
                    }
                }
                else AfficheLigne(temps - 1); //affiche le temps actuel et le mot "repos" ie le processeur n'execute aucun processus
            }
            return temps;
        }

        // à utiliser dans MultiNiveaux

        public int Executer(int tempsDebut, int tempsFin, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral)
        {
            int temps = tempsDebut;
            while (listePrets.Count != 0) //s'il existe des processus prêts 
            {
                niveaux[indiceNiveau].indice[0] = AjouterTous(temps, niveaux[indiceNiveau].indice[0], niveaux, listeGeneral, indiceNiveau); //remplir la liste des processus prêts de chaque niveau
                temps++; //incrementer le temps réel
                if (listePrets.Count != 0) //s'il y a des processus prêts
                {
                    listePrets[0].etat = 2;
                    if (listePrets[0].tempsRestant == listePrets[0].duree) listePrets[0].tempsReponse = temps - 1 - listePrets[0].tempsArriv;
                    listePrets[0].tempsRestant--; //l'execution du 1er processus de listePrets commence
                    AfficheLigne(temps - 1, listePrets[0].id); //affiche le temps actuel et l'ID du processus entrain d'être executé
                    if (listePrets[0].tempsRestant == 0) //si l'execution du premier processus de listePrets est terminée
                    {
                        listePrets[0].tempsFin = temps; //temps de fin d'execution = au temps actuel
                        listePrets[0].tempsService = temps - listePrets[0].tempsArriv; //temps de service = temps de fin d'execution - temps d'arrivé
                        listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree;  //temps d'attente = temps de service - durée d'execution
                        listePrets[0].etat = 3;
                        listePrets.RemoveAt(0); //supprimer le premier processus executé
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