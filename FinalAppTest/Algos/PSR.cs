﻿using System.Collections.Generic;
using FinalAppTest;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
namespace Ordonnancement
{
    public class PSR : Ordonnancement //Priorité Sans Réquisition
    {

        #region Constructeur
        public PSR() { }
        #endregion

        #region Visualisation
        public override async Task<int> Executer(StackPanel ListePretsView, StackPanel Processeur, TextBlock TempsView, StackPanel ListeBloqueView, StackPanel deroulement, WrapPanel GanttChart)
        {
            SortListeProcessus(); //tri des processus par ordre d'arrivé
            int temps = 0, indice = 0;
            bool sort = true; //est à vrai si un tri par priorité est necessaire
            while (indice < listeProcessus.Count || listePrets.Count != 0 || listebloque.Count != 0) //s'il existe des processus non executés
            {
                if (listePrets.Count == 0) sort = true; //les premiers processus arrivés => on fait un tri par priorité
                indice = await MAJListePrets(temps, indice, ListePretsView);  //remplir listePrets
                if (sort == true && listePrets.Count != 0) //si un tri par priorité est necessaire et il y a des processus prêts
                {
                    listePrets.Sort(delegate (Processus x, Processus y) { return x.prio.CompareTo(y.prio); }); //tri des processus de listePrets par priorité
                    sort = false; //le tri par priorité n'est plus necessaire (déja fait)
                    await MAJListePretsView(ListePretsView, 0);
                    listePrets[0].transition = 2;
                    await AfficherDeroulement(deroulement);
                    await Activation(ListePretsView, Processeur, listePrets[0]);
                }
                await InterruptionExecute(ListePretsView, ListeBloqueView, Processeur, deroulement);
                if (!SimulationPage.paused ) temps++; //incrementer le temps réel
                TempsView.Text = temps.ToString();
                sort = false;
                if (listePrets.Count != 0 && !SimulationPage.paused) //il y a des processus prêts
                {
                    if (listePrets[0].tempsRestant == listePrets[0].duree) listePrets[0].tempsReponse = temps - 1 - listePrets[0].tempsArriv;
                    listePrets[0].etat = 2;
                    AfficherEtat(GanttChart, temps);
                    listePrets[0].tempsRestant--; //execution du 1er processus de listePrets et donc décrémenter le tempsRestant
                    MAJProcesseur(Processeur);
                    if (listePrets[0].tempsRestant == 0) //si l'execution du premier processus de listePrets est terminée
                    {
                        listePrets[0].tempsFin = temps; //temps de fin d'execution = au temps actuel
                        listePrets[0].tempsService = temps - listePrets[0].tempsArriv; //temps de service = temps de fin d'execution - temps d'arrivé
                        listePrets[0].etat = 3;
                        listePrets[0].transition = 0;
                        await AfficherDeroulement(deroulement);
                        listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree; //temps d'attente = temps de service - durée d'execution
                        listePrets.RemoveAt(0); //supprimer le premier processus executé
                        await FinProcessus(Processeur);
                        sort = true; //donc il faut trier les processus restants dans listePrets

                    }

                }
                else if (!SimulationPage.paused)
                {
                    AfficheLigne(temps - 1);
                    AfficherEtat(GanttChart, temps);
                    tempsRepos++;
                }
            }
            TauxUtil(temps);
            return temps;
        }
        #endregion

        #region Test
        public int Executer()
        {
            SortListeProcessus(); //tri des processus par ordre d'arrivé
            int temps = 0, indice = 0;
            bool sort = true; //est à vrai si un tri par priorité est necessaire
            while (indice < listeProcessus.Count || listePrets.Count != 0 || listebloque.Count != 0) //s'il existe des processus non executés
            {
                if (listePrets.Count == 0) sort = true; //les premiers processus arrivés => on fait un tri par priorité (croissant)
                indice = MAJListePrets(temps, indice);  //remplir listePrets
                InterruptionExecute();
                temps++; //incrementer le temps réel

                if (sort == true && listePrets.Count != 0) //si un tri par priorité est necessaire et il y a des processus prêts
                {
                    listePrets.Sort(
                            delegate (Processus x, Processus y)
                            {
                                if (x.prio.CompareTo(y.prio) == 0) return x.tempsArriv.CompareTo(y.tempsArriv); //si les processus ont la même priorité, on les trie selon le temps d'arrivée
                                else return x.prio.CompareTo(y.prio); //sinon, on fait le tri par prio
                            }
                        ); //tri des processus de listePrets par priorité
                    sort = false; //le tri par priorité n'est plus necessaire (déja fait)
                }
                if (listePrets.Count != 0) //il y a des processus prêts
                {
                    listePrets[0].transition = 2; //Activation du 1er processus de listePrets
                    listePrets[0].etat = 2;
                    if (listePrets[0].tempsRestant == listePrets[0].duree) listePrets[0].tempsReponse = temps - 1 - listePrets[0].tempsArriv;
                    listePrets[0].tempsRestant--; //execution du 1er processus de listePrets et donc décrémenter le tempsRestant
                    AfficheLigne(temps - 1, listePrets[0].id); //affiche le temps actuel et l'ID du processus entrain d'être executé
                    if (listePrets[0].tempsRestant == 0) //si l'execution du premier processus de listePrets est terminée
                    {
                        listePrets[0].tempsFin = temps; //temps de fin d'execution = au temps actuel
                        listePrets[0].tempsService = temps - listePrets[0].tempsArriv; //temps de service = temps de fin d'execution - temps d'arrivé
                        listePrets[0].etat = 3;
                        listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree; //temps d'attente = temps de service - durée d'execution
                        listePrets.RemoveAt(0); //supprimer le premier processus executé
                        if (listePrets.Count != 0) sort = true; //donc il faut trier les processus restants dans listePrets
                    }
                }
                else AfficheLigne(temps - 1); //affiche le temps actuel et le mot "repos" ie le processeur n'execute aucun processus
            }
            return temps;
        }
        #endregion

        #region MultiNiveau
        // à utiliser dans MultiNiveaux
        public override async Task<int> Executer(int tempsDebut, int nbNiveau, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral, List<ProcessusNiveau> listebloqueGenerale, StackPanel[] ListesPretsViews, StackPanel Processeur, TextBlock TempsView, StackPanel ListeBloqueView, StackPanel deroulement)
        {
            StackPanel ListePretsView = ListesPretsViews[indiceNiveau];
            int temps = tempsDebut;
            if (niveaux[indiceNiveau].indice[2] == 0) niveaux[indiceNiveau].indice[1] = 1; //si aucun processus du niveau actuel n'a été executé alors il faut trier les processus de listePrets de ce niveau par priorité
            niveaux[indiceNiveau].indice[2] = 1; //l'execution d'un processus de ce niveau commence
            while (listePrets.Count != 0 && PrioNiveaux(niveaux, indiceNiveau, nbNiveau)) //s'il existe des processus prêts et le temps < le temps de fin  ou il n'y a pas de temps fin
            {
                niveaux[indiceNiveau].indice[0] = await MAJListePrets(temps, niveaux[indiceNiveau].indice[0], niveaux, listeGeneral, indiceNiveau, ListesPretsViews); //remplir la liste des processus prêts de chaque niveau
                if (listePrets.Count != 0 && niveaux[indiceNiveau].indice[1] == 1) //s'il y a des processus prêts et un tri par priorité est necessaire
                {
                    listePrets.Sort(delegate (Processus x, Processus y) { return x.prio.CompareTo(y.prio); }); //sinon, on fait le tri par priorité
                    niveaux[indiceNiveau].indice[1] = 0; //le tri par priorité n'est plus necessaire (déja fait)
                    await MAJListePretsView_MultiLvl(ListePretsView, 0);
                    listePrets[0].transition = 2; //Activation du 1er processus de ListePrets
                    await AfficherDeroulement(deroulement);
                    await Activation_MultiLvl(ListePretsView, Processeur, listePrets[0]);
                }
                await InterruptionExecute(listebloqueGenerale, ListesPretsViews, indiceNiveau, ListeBloqueView, Processeur, deroulement);
                niveaux[indiceNiveau].indice[1] = 0;
                if(!SimulationPage_MultiLvl.paused) temps++; //incrementer le temps réel
                TempsView.Text = temps.ToString();
                if (listePrets.Count != 0 && !SimulationPage_MultiLvl.paused) //il y a des processus prêts
                {
                    listePrets[0].transition = 2; //Activation du 1er processus de listePrets
                    listePrets[0].etat = 2;
                    AfficherEtat(listeGeneral, Ordonnancement.GanttChart, temps);
                    if (listePrets[0].tempsRestant == listePrets[0].duree) listePrets[0].tempsReponse = temps - 1 - listePrets[0].tempsArriv;
                    listePrets[0].tempsRestant--; //le processus est entrain de s'exécuter donc on décrémente le tempsRestant
                    MAJProcesseur_MultiLvl(Processeur);
                    if (listePrets[0].tempsRestant == 0) // fin d'exécution du processus 
                    {
                        listePrets[0].tempsFin = temps; //temps de fin d'execution = au temps actuel
                        listePrets[0].tempsService = temps - listePrets[0].tempsArriv; //temps de service = temps de fin d'execution - temps d'arrivé
                        listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree;  //temps d'attente = temps de service - durée d'execution
                        listePrets[0].etat = 3; //Fin d'exécution du processus
                        listePrets[0].transition = 0;
                        await AfficherDeroulement(deroulement);
                        listePrets.RemoveAt(0); //supprimer le premier processus executé
                        await FinProcessus_MultiLvl(Processeur);
                        niveaux[indiceNiveau].indice[1] = 1; //il faut trier les processus restants dans listePrets par priorité
                    }
                }
                else if(!SimulationPage_MultiLvl.paused) AfficherEtat(listeGeneral, Ordonnancement.GanttChart, temps);
            }
            if (!PrioNiveaux(niveaux, indiceNiveau, nbNiveau) && listePrets.Count != 0)
            {
                niveaux[indiceNiveau].indice[1] = 1;
                listePrets[0].transition = 1; //Désactivation du processus entrain d'exécution
                listePrets[0].etat = 1;
                await AfficherDeroulement(deroulement);
                await Desactivation_MultiLvl(ListePretsView, Processeur, listePrets[0], indiceNiveau);
                listePrets.Add(listePrets[0]);
                listePrets.RemoveAt(0);
                return temps;
            }
            return temps;
        }

        public override int Executer(int tempsDebut, int tempsFin, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral, List<ProcessusNiveau> listebloqueGenerale, StackPanel deroulement)
        {
            /*dans cette methode on utilisera :
             * niveau[].indice[1] est à 1 si un tri par priorité est necessaire, à 0 sinon (déja trié)
             * niveau[].indice[2] est à 1 si au moins un processus de ce niveau a été executé, à 0 sinon (aucun processus de ce niveau n'a été executé)*/

            int temps = tempsDebut;
            if (niveaux[indiceNiveau].indice[2] == 0) niveaux[indiceNiveau].indice[1] = 1; //si aucun processus du niveau actuel n'a été executé alors il faut trier les processus de listePrets de ce niveau par priorité
            niveaux[indiceNiveau].indice[2] = 1; //l'execution d'un processus de ce niveau commence
            while (listePrets.Count != 0 && (temps < tempsFin || tempsFin == -1)) //s'il existe des processus prêts et le temps < le temps de fin  ou il n'y a pas de temps fin
            {
                niveaux[indiceNiveau].indice[0] = MAJListePrets(temps, niveaux[indiceNiveau].indice[0], niveaux, listeGeneral, indiceNiveau); //remplir la liste des processus prêts de chaque niveau
                if(!SimulationPage_MultiLvl.paused) temps++; //incrementer le temps réel
                InterruptionExecute(listebloqueGenerale);
                if (listePrets.Count != 0 && niveaux[indiceNiveau].indice[1] == 1) //s'il y a des processus prêts et un tri par priorité est necessaire
                {
                    listePrets.Sort(
                           delegate (Processus x, Processus y)
                           {
                               if (x.prio.CompareTo(y.prio) == 0) return x.tempsArriv.CompareTo(y.tempsArriv); //si les processus ont la même priorité, on les trie selon le temps d'arrivée
                               else return x.prio.CompareTo(y.prio); //sinon, on fait le tri par prio
                           }
                       ); //tri des processus de listePrets par priorité
                    niveaux[indiceNiveau].indice[1] = 0; //le tri par priorité n'est plus necessaire (déja fait)
                }

                if (listePrets.Count != 0 && !SimulationPage_MultiLvl.paused) //il y a des processus prêts
                {
                    listePrets[0].transition = 2; //Activation du 1er processus de listePrets
                    listePrets[0].etat = 2;
                    AfficherEtat(listeGeneral, Ordonnancement.GanttChart, temps);
                    if (listePrets[0].tempsRestant == listePrets[0].duree) listePrets[0].tempsReponse = temps - 1 - listePrets[0].tempsArriv;
                    listePrets[0].tempsRestant--; //le processus est entrain de s'exécuter donc on décrémente le tempsRestant
                    AfficheLigne(temps - 1, listePrets[0].id); //affiche le temps actuel et l'ID du processus entrain d'être executé
                    if (listePrets[0].tempsRestant == 0) // fin d'exécution du processus 
                    {
                        listePrets[0].tempsFin = temps; //temps de fin d'execution = au temps actuel
                        listePrets[0].tempsService = temps - listePrets[0].tempsArriv; //temps de service = temps de fin d'execution - temps d'arrivé
                        listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree;  //temps d'attente = temps de service - durée d'execution
                        listePrets[0].etat = 3; //Fin d'exécution du processus
                        listePrets.RemoveAt(0); //supprimer le premier processus executé
                        if (listePrets.Count != 0) niveaux[indiceNiveau].indice[1] = 1; //il faut trier les processus restants dans listePrets par durée
                    }
                }
                else if(!SimulationPage_MultiLvl.paused) AfficherEtat(listeGeneral, Ordonnancement.GanttChart, temps);
                if (temps == tempsFin)
                {
                    listePrets[0].transition = 1; //Desctivation du 1er processus de listePrets
                    listePrets[0].etat = 1;
                    listePrets.Add(listePrets[0]);
                    listePrets.RemoveAt(0);
                    return temps;
                }
            }
            return temps;
        }
        #endregion
        #region MultiNiveauRecyclage
        public override async Task<int> Executer(int tempsDebut, int nbNiveau, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral, List<ProcessusNiveau> listebloqueGenerale, StackPanel[] ListesPretsViews, StackPanel Processeur, TextBlock TempsView, StackPanel ListeBloqueView, StackPanel deroulement, int i)
        {
            StackPanel ListePretsView = ListesPretsViews[indiceNiveau];
            int temps = tempsDebut;
            if (niveaux[indiceNiveau].indice[2] == 0) niveaux[indiceNiveau].indice[1] = 1; //si aucun processus du niveau actuel n'a été executé alors il faut trier les processus de listePrets de ce niveau par priorité
            niveaux[indiceNiveau].indice[2] = 1; //l'execution d'un processus de ce niveau commence
            while (listePrets.Count != 0 && PrioNiveaux(niveaux, indiceNiveau, nbNiveau)) //s'il existe des processus prêts et le temps < le temps de fin  ou il n'y a pas de temps fin
            {
                niveaux[indiceNiveau].indice[0] = await MAJListePrets(temps, niveaux[indiceNiveau].indice[0], niveaux, listeGeneral, indiceNiveau, ListesPretsViews); //remplir la liste des processus prêts de chaque niveau
                if (listePrets.Count != 0 && niveaux[indiceNiveau].indice[1] == 1) //s'il y a des processus prêts et un tri par durée est necessaire
                {
                    listePrets.Sort(delegate (Processus x, Processus y) { return x.prio.CompareTo(y.prio); }); //sinon, on fait le tri par priorité
                    niveaux[indiceNiveau].indice[1] = 0; //le tri par priorité n'est plus necessaire (déja fait)
                    await MAJListePretsView_MultiLvl(ListePretsView, 0);
                    listePrets[0].transition = 2; //Activation du 1er processus de ListePrets
                    await AfficherDeroulement(deroulement);
                    await Activation_MultiLvl(ListePretsView, Processeur, listePrets[0]);
                }
                await InterruptionExecute(listebloqueGenerale, ListesPretsViews, indiceNiveau, ListeBloqueView, Processeur, deroulement);
                niveaux[indiceNiveau].indice[1] = 0;
                if(!SimulationPage_MultiLvl.paused) temps++; //incrementer le temps réel
                TempsView.Text = temps.ToString();
                if (listePrets.Count != 0 && !SimulationPage_MultiLvl.paused) //il y a des processus prêts
                {
                    listePrets[0].transition = 2; //Activation du 1er processus de listePrets
                    listePrets[0].etat = 2;
                    AfficherEtat(listeGeneral, Ordonnancement.GanttChart, temps);
                    if (listePrets[0].tempsRestant == listePrets[0].duree) listePrets[0].tempsReponse = temps - 1 - listePrets[0].tempsArriv;
                    listePrets[0].tempsRestant--; //le processus est entrain de s'exécuter donc on décrémente le tempsRestant
                    MAJProcesseur_MultiLvl(Processeur);
                    if (listePrets[0].tempsRestant == 0) // fin d'exécution du processus 
                    {
                        listePrets[0].tempsFin = temps; //temps de fin d'execution = au temps actuel
                        listePrets[0].tempsService = temps - listePrets[0].tempsArriv; //temps de service = temps de fin d'execution - temps d'arrivé
                        listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree;  //temps d'attente = temps de service - durée d'execution
                        listePrets[0].etat = 3; //Fin d'exécution du processus
                        listePrets[0].transition = 0;
                        await AfficherDeroulement(deroulement);
                        listePrets.RemoveAt(0); //supprimer le premier processus executé
                        await FinProcessus_MultiLvl(Processeur);
                        niveaux[indiceNiveau].indice[1] = 1; //il faut trier les processus restants dans listePrets par durée
                    }
                }
                else if(!SimulationPage_MultiLvl.paused) AfficherEtat(listeGeneral, Ordonnancement.GanttChart, temps);
            }
            if (!PrioNiveaux(niveaux, indiceNiveau, nbNiveau) && listePrets.Count != 0)
            {
                niveaux[indiceNiveau].indice[1] = 1;
                listePrets[0].transition = 1; //Désactivation du processus entrain d'exécution
                listePrets[0].etat = 1;
                await AfficherDeroulement(deroulement);
                if (indiceNiveau + 1 < nbNiveau)
                {
                    await Desactivation_MultiLvl(ListesPretsViews[indiceNiveau + 1], Processeur, listePrets[0], indiceNiveau + 1);
                    niveaux[indiceNiveau + 1].listePrets.Add(listePrets[0]);
                }
                else
                {
                    await Desactivation_MultiLvl(ListePretsView, Processeur, listePrets[0], indiceNiveau);
                    listePrets.Add(listePrets[0]);
                }
                listePrets.RemoveAt(0);
                return temps;
            }
            return temps;
        }
        #endregion

    }
}