using Simulation_Test;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Ordonnancement
{
    public partial class PAPS : Ordonnancement
    {

        public PAPS() { }
        public async Task<int> Executer(StackPanel ListProcessusView, StackPanel Processeur,TextBlock TempsView)
        {
            SortListeProcessus(); //tri des processus par ordre d'arrivé
            int temps = 0, indice = 0;
            Action EmptyDelegate = delegate () { };
            while (indice < listeProcessus.Count || listePrets.Count != 0) //s'il existe des processus non executés
            {
                indice = AjouterTous(temps, indice, ListProcessusView, Processeur); //remplir listePrets
                temps++; //incrementer le temps réel
                TempsView.Text = temps.ToString();
                await Task.Delay(2000);
                if (listePrets.Count != 0) //s'il y a des processus prêts
                {
                    listePrets[0].etat = 2;
                    if (listePrets[0].tempsRestant == listePrets[0].duree) listePrets[0].tempsReponse = temps-1 - listePrets[0].tempsArriv;
                    //AfficheLigne(temps,listePrets, ListProcessusView, Processeur, TempsView); //affiche le temps actuel et l'ID du processus entrain d'être executé
                    listePrets[0].tempsRestant--; //l'execution du 1er processus de listePrets commence
                    //AfficheLigne(temps,listePrets, ListProcessusView, Processeur, TempsView); //affiche le temps actuel et l'ID du processus entrain d'être executé
                    if (listePrets[0].tempsRestant == 0) //si l'execution du premier processus de listePrets est terminée
                    {
                        listePrets[0].tempsFin = temps; //temps de fin d'execution = au temps actuel
                        listePrets[0].tempsService = temps - listePrets[0].tempsArriv; //temps de service = temps de fin d'execution - temps d'arrivé
                        listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree; //temps d'attente = temps de service - durée d'execution
                        listePrets[0].etat = 3;
                        Processeur.Children.Clear();
                        if (listePrets.Count > 1)
                        {
                            var item = ListProcessusView.Children[0];
                            ListProcessusView.Children.RemoveAt(0);
                            Processeur.Children.Add(item);
                        }
                        listePrets.RemoveAt(0); //supprimer le premier processus executé
                    }
                }
                else AfficheLigne(temps,listePrets, ListProcessusView, Processeur, TempsView);//affiche le temps actuel et le mot "repos" ie le processeur n'execute aucun processus
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
                    if (listePrets[0].tempsRestant == listePrets[0].duree) listePrets[0].tempsReponse = temps-1- listePrets[0].tempsArriv;
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
                else AfficheLigne(temps-1); //affiche le temps actuel et le mot "repos" ie le processeur n'execute aucun processus
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
                    if (listePrets[0].tempsRestant == listePrets[0].duree) listePrets[0].tempsReponse = temps - 1- listePrets[0].tempsArriv;
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
                if (temps== tempsFin)
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