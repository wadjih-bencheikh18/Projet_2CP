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
        
        #region Constructeur
        public PAPS() { }
        #endregion

        #region Visualisation
        public override async  Task<int> Executer(StackPanel ListePretsView, StackPanel Processeur, TextBlock TempsView, StackPanel ListeBloqueView, StackPanel deroulement, WrapPanel GanttChart)
        {
            SortListeProcessus(); //tri des processus par ordre d'arrivé
            int temps = 0, indice = 0;
            bool anime = true;
            while (indice < listeProcessus.Count || listePrets.Count != 0 || listebloque.Count != 0) //s'il existe des processus non executés
            {
                if (listePrets.Count == 0) anime = true;
                indice = await MAJListePrets(temps, indice, ListePretsView); //remplir listePrets
                if (listePrets.Count != 0 && anime)
                {
                    listePrets[0].transition = 2;
                    await AfficherDeroulement(deroulement);
                    listePrets[0].transition = 0;
                    await Activation(ListePretsView, Processeur, listePrets[0]);
                }
                await InterruptionExecute(ListePretsView, ListeBloqueView, Processeur,deroulement);
                anime = false;
                if (!SimulationPage.paused)  temps++; //incrementer le temps réel
                TempsView.Text = temps.ToString();
                if (listePrets.Count != 0 && !SimulationPage.paused) //s'il y a des processus prêts
                {
                    //listePrets[0].transition = 2; //Activation du 1er processus de listePrets
                    listePrets[0].etat = 2;
                    AfficherEtat(GanttChart, temps);
                    if (listePrets[0].tempsRestant == listePrets[0].duree) listePrets[0].tempsReponse = temps - 1 - listePrets[0].tempsArriv;
                    listePrets[0].tempsRestant--; //l'execution du 1er processus de listePrets commence               //AfficheLigne(temps,listePrets, ListProcessusView, Processeur, TempsView); //affiche le temps actuel et l'ID du processus entrain d'être executé
                    MAJProcesseur(Processeur);
                    if (listePrets[0].tempsRestant == 0) //si l'execution du premier processus de listePrets est terminée
                    {
                        listePrets[0].tempsFin = temps; //temps de fin d'execution = au temps actuel
                        listePrets[0].tempsService = temps - listePrets[0].tempsArriv; //temps de service = temps de fin d'execution - temps d'arrivé
                        listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree; //temps d'attente = temps de service - durée d'execution
                        listePrets[0].etat = 3;
                        listePrets[0].transition = 0;
                        await AfficherDeroulement(deroulement);
                        listePrets.RemoveAt(0); //supprimer le premier processus executé
                        await FinProcessus(Processeur);
                        if (listePrets.Count != 0)
                        {
                            anime = true;
                        }
                    }
                }
                else if (!SimulationPage.paused)
                {
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
            while (indice < listeProcessus.Count || listePrets.Count != 0 || listebloque.Count != 0) //s'il existe des processus non executés
            {
                indice = MAJListePrets(temps, indice); //remplir listePrets
                temps++; //incrementer le temps réel
                InterruptionExecute();
                if (listePrets.Count != 0) //s'il y a des processus prêts
                {
                    listePrets[0].transition = 2; //Activation du 1er processus de listePrets
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
        #endregion

        #region MultiNiveau
        // à utiliser dans MultiNiveaux
        public override async Task<int> Executer(int tempsDebut, int nbNiveau, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral, List<ProcessusNiveau> listebloqueGenerale, StackPanel[] ListesPretsViews, StackPanel Processeur, TextBlock TempsView, StackPanel ListeBloqueView, StackPanel deroulement)
        {
            StackPanel ListePretsView = ListesPretsViews[indiceNiveau];
            int temps = tempsDebut;
            bool anime = true;
            while (listePrets.Count != 0 && PrioNiveaux(niveaux,indiceNiveau,nbNiveau)) //s'il existe des processus prêts 
            {
                if(anime)
                {
                    listePrets[0].transition = 2; //Activation du 1er procussus dans ListePrets 
                    await AfficherDeroulement(deroulement);
                    listePrets[0].transition = 0;
                    await Activation_MultiLvl(ListePretsView, Processeur, listePrets[0]);
                }
                await InterruptionExecute(listebloqueGenerale, ListesPretsViews,indiceNiveau, ListeBloqueView, Processeur, deroulement);
                anime = false;
                if (!SimulationPage_MultiLvl.paused) temps++; //incrementer le temps réel
                TempsView.Text = temps.ToString();
                niveaux[indiceNiveau].indice[0] = await MAJListePrets(temps, niveaux[indiceNiveau].indice[0], niveaux, listeGeneral, indiceNiveau, ListesPretsViews); //remplir la liste des processus prêts de chaque niveau
                if (listePrets.Count != 0 && !SimulationPage_MultiLvl.paused) //s'il y a des processus prêts
                {
                    listePrets[0].transition = 2; //Activation du 1er processus de listePrets
                    listePrets[0].etat = 2;
                    AfficherEtat(listeGeneral, Ordonnancement.GanttChart, temps);
                    if (listePrets[0].tempsRestant == listePrets[0].duree) listePrets[0].tempsReponse = temps - 1 - listePrets[0].tempsArriv;
                    listePrets[0].tempsRestant--; //l'execution du 1er processus de listePrets commence
                    MAJProcesseur_MultiLvl(Processeur);
                    if (listePrets[0].tempsRestant == 0) //si l'execution du premier processus de listePrets est terminée
                    {
                        listePrets[0].tempsFin = temps; //temps de fin d'execution = au temps actuel
                        listePrets[0].tempsService = temps - listePrets[0].tempsArriv; //temps de service = temps de fin d'execution - temps d'arrivé
                        listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree;  //temps d'attente = temps de service - durée d'execution
                        listePrets[0].etat = 3; //Fin d'exécution du processus
                        listePrets[0].transition = 0;
                        await AfficherDeroulement(deroulement);
                        await FinProcessus_MultiLvl(Processeur);
                        listePrets.RemoveAt(0); //supprimer le premier processus executé
                        anime = true;
                    }
                }
                else if (!SimulationPage_MultiLvl.paused) AfficherEtat(listeGeneral, Ordonnancement.GanttChart, temps);
            }
            if (!PrioNiveaux(niveaux, indiceNiveau, nbNiveau)&& listePrets.Count!=0)
            {
                listePrets[0].transition = 1; //Désactivation du processus qui etait entrain d'exécution
                listePrets[0].etat = 1;
                await AfficherDeroulement(deroulement);
                await Desactivation_MultiLvl(ListePretsView, Processeur, listePrets[0],indiceNiveau);
                listePrets.Add(listePrets[0]);
                listePrets.RemoveAt(0);
                return temps;
            }
            return temps;
        }




        public override int Executer(int tempsDebut, int tempsFin, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral, List<ProcessusNiveau> listebloqueGenerale, StackPanel deroulement)
        {
            int temps = tempsDebut;
            while (listePrets.Count != 0) //s'il existe des processus prêts 
            {
                niveaux[indiceNiveau].indice[0] = MAJListePrets(temps, niveaux[indiceNiveau].indice[0], niveaux, listeGeneral, indiceNiveau); //remplir la liste des processus prêts de chaque niveau
                if (!SimulationPage_MultiLvl.paused) temps++; //incrementer le temps réel
                InterruptionExecute(listebloqueGenerale);
                if (listePrets.Count != 0 && !SimulationPage_MultiLvl.paused) //s'il y a des processus prêts
                {
                    listePrets[0].transition = 2; //Activation du 1er processus de listePrets
                    listePrets[0].etat = 2;
                    AfficherEtat(listeGeneral, Ordonnancement.GanttChart, temps);
                    if (listePrets[0].tempsRestant == listePrets[0].duree) listePrets[0].tempsReponse = temps - 1 - listePrets[0].tempsArriv;
                    listePrets[0].tempsRestant--; //l'execution du 1er processus de listePrets commence
                    AfficheLigne(temps - 1, listePrets[0].id); //affiche le temps actuel et l'ID du processus entrain d'être executé
                    if (listePrets[0].tempsRestant == 0) //si l'execution du premier processus de listePrets est terminée
                    {
                        listePrets[0].tempsFin = temps; //temps de fin d'execution = au temps actuel
                        listePrets[0].tempsService = temps - listePrets[0].tempsArriv; //temps de service = temps de fin d'execution - temps d'arrivé
                        listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree;  //temps d'attente = temps de service - durée d'execution
                        listePrets[0].etat = 3; //Fin d'exécution du processus
                        listePrets.RemoveAt(0); //supprimer le premier processus executé
                    }
                }
                else if(!SimulationPage_MultiLvl.paused) AfficherEtat(listeGeneral, Ordonnancement.GanttChart, temps);
                if (temps == tempsFin)
                {
                    listePrets[0].transition = 1; //Désactivation du processus entrain d'exécution
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
        public override async Task<int> Executer(int tempsDebut, int nbNiveau, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral, List<ProcessusNiveau> listebloqueGenerale, StackPanel[] ListesPretsViews, StackPanel Processeur, TextBlock TempsView, StackPanel ListeBloqueView, StackPanel deroulement,int i)
        {
            StackPanel ListePretsView = ListesPretsViews[indiceNiveau];
            int temps = tempsDebut;
            bool anime = true;
            while (listePrets.Count != 0 && PrioNiveaux(niveaux, indiceNiveau, nbNiveau)) //s'il existe des processus prêts 
            {
                if (anime)
                {
                    listePrets[0].transition = 2; //Activation du 1er procussus dans ListePrets 
                    await AfficherDeroulement(deroulement);
                    listePrets[0].transition = 0;
                    await Activation_MultiLvl(ListePretsView, Processeur, listePrets[0]);
                }
                await InterruptionExecute(listebloqueGenerale, ListesPretsViews, indiceNiveau, ListeBloqueView, Processeur, deroulement);
                anime = false;
                if(!SimulationPage_MultiLvl.paused) temps++; //incrementer le temps réel
                TempsView.Text = temps.ToString();
                niveaux[indiceNiveau].indice[0] = await MAJListePrets(temps, niveaux[indiceNiveau].indice[0], niveaux, listeGeneral, indiceNiveau, ListesPretsViews); //remplir la liste des processus prêts de chaque niveau
                if (listePrets.Count != 0 && !SimulationPage_MultiLvl.paused) //s'il y a des processus prêts
                {
                    listePrets[0].transition = 2; //Activation du 1er processus de listePrets
                    listePrets[0].etat = 2;
                    AfficherEtat(listeGeneral, Ordonnancement.GanttChart, temps);
                    if (listePrets[0].tempsRestant == listePrets[0].duree) listePrets[0].tempsReponse = temps - 1 - listePrets[0].tempsArriv;
                    listePrets[0].tempsRestant--; //l'execution du 1er processus de listePrets commence
                    MAJProcesseur_MultiLvl(Processeur);
                    if (listePrets[0].tempsRestant == 0) //si l'execution du premier processus de listePrets est terminée
                    {
                        listePrets[0].tempsFin = temps; //temps de fin d'execution = au temps actuel
                        listePrets[0].tempsService = temps - listePrets[0].tempsArriv; //temps de service = temps de fin d'execution - temps d'arrivé
                        listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree;  //temps d'attente = temps de service - durée d'execution
                        listePrets[0].etat = 3; //Fin d'exécution du processus
                        listePrets[0].transition = 0;
                        await AfficherDeroulement(deroulement);
                        await FinProcessus_MultiLvl(Processeur);
                        listePrets.RemoveAt(0); //supprimer le premier processus executé
                        anime = true;
                    }
                }
                else if (!SimulationPage_MultiLvl.paused) AfficherEtat(listeGeneral, Ordonnancement.GanttChart, temps);
            }
            if (!PrioNiveaux(niveaux, indiceNiveau, nbNiveau) && listePrets.Count != 0)
            {
                listePrets[0].transition = 1; //Désactivation du processus qui etait entrain d'exécution
                listePrets[0].etat = 1;
                await AfficherDeroulement(deroulement);
                if(indiceNiveau+1<nbNiveau)
                {
                    await Desactivation_MultiLvl(ListesPretsViews[indiceNiveau + 1], Processeur, listePrets[0], indiceNiveau+1);
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