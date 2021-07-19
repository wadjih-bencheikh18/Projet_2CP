using FinalAppTest;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Ordonnancement
{
    public class RR : Ordonnancement //Round Robin
    {

        #region Constructeur
        public int quantum { get; set; }

        public RR(int q) // Constructeur 
        {
            quantum = q;
        }
        #endregion

        #region Visualisation
        public override async Task<int> Executer(StackPanel ListePretsView, StackPanel Processeur, TextBlock TempsView, StackPanel ListeBloqueView, StackPanel deroulement, WrapPanel GanttChart)
        {
            bool anime = true;
            SortListeProcessus(); //Tri de la liste des processus par temps d'arrivée
            int indice = 0, temps = 0, q = 0;
            while ((indice < listeProcessus.Count || listePrets.Count != 0 || listebloque.Count != 0) && !SimulationPage.stop)  //s'il existe des processus prêts
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
                if (await InterruptionExecute(ListePretsView, ListeBloqueView, Processeur, deroulement)) q = 0;
                anime = false;
                if (!SimulationPage.paused) temps++; //incrementer le temps réel
                TempsView.Text = temps.ToString();
                if (listePrets.Count != 0 && !SimulationPage.paused)  // listePrets n'est pas vide 
                {
                    listePrets[0].etat = 2; //Le 1er processus de listePrets est actif
                    AfficherEtat(GanttChart, temps);
                    q++;  // on incrémente le quantum
                    if (listePrets[0].tempsRestant == listePrets[0].duree) listePrets[0].tempsReponse = temps - 1 - listePrets[0].tempsArriv;
                    listePrets[0].tempsRestant--; //L'exécution courante du 1er processus de listePrets => décrémenter tempsRestant
                    MAJProcesseur(Processeur);
                    if (listePrets[0].tempsRestant == 0) //fin d'exécution du processus 
                    {
                        listePrets[0].tempsFin = temps; // temps de fin d'execution = temps actuel
                        listePrets[0].tempsService = temps - listePrets[0].tempsArriv;// temps de service = temps de fin d'execution - temps d'arrivé
                        listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree;  //temps d'attente = temps de service - durée d'execution
                        listePrets[0].etat = 3;
                        listePrets[0].transition = 0;
                        await AfficherDeroulement(deroulement);
                        listePrets.RemoveAt(0);//supprimer le premier processus executé
                        anime = true;
                        await FinProcessus(Processeur);
                        Processeur.Children.Clear();
                        q = 0;  // un nouveau quantum va commencer
                    }
                    else if (q == quantum && listePrets.Count == 1) q = 0;
                    else if (q == quantum)  // on a terminé ce quantum => il faut passer au processus suivant => on defile, et à la fin, on enfile le processus courant
                    {
                        listePrets[0].tempsFin = temps;  // On sauvegarde le tempsFin puisqu'on a interrompu l'exécution de ce processus
                        q = 0;  //Un nouveau quantum
                        listePrets[0].transition = 1; //Desactivation du 1er processus de listePrets
                        await AfficherDeroulement(deroulement);
                        listePrets[0].etat = 1;
                        await Desactivation(ListePretsView, Processeur, listePrets[0]);
                        listePrets.Add(listePrets[0]);  //Enfilement à la fin
                        listePrets.RemoveAt(0);  // defiler 
                        anime = true;
                    }
                }
                else if (!SimulationPage.paused)
                {
                    AfficherEtat(GanttChart, temps);
                    tempsRepos++;
                }
            }
            TauxUtil(temps);
            SimulationPage.save.EndSimulation();
            return temps;
        }
        #endregion

        #region Test
        public override int Executer()  // exécuter la liste des processus et retourner le temps total pour terminer l'exécution
        {
            SortListeProcessus(); //Tri de la liste des processus par temps d'arrivée
            int indice = 0, temps = 0, q = 0;
            while (indice < listeProcessus.Count || listePrets.Count != 0)  //s'il existe des processus prêts
            {
                if (indice < listeProcessus.Count && listePrets.Count == 0)  // Si il y a des processus dans listeProcessus et la listePrets est vide
                {
                    tempsRepos++;
                    if (temps < listeProcessus[indice].tempsArriv)  // si aucun processus n'est arrivé
                    {
                        temps++;
                    }
                    else indice = MAJListePrets(temps, indice);  // sinon, on ajoute les processus arrivés à listePrets
                }
                else  // listePrets n'est pas vide 
                {
                    listePrets[0].transition = 2; //Activation du 1er processus de listePrets
                    listePrets[0].etat = 2;
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
                        listePrets[0].transition = 1; //Desactivation du 1er processus de listePrets
                        listePrets[0].etat = 1;
                        listePrets.Add(listePrets[0]);  //Enfilement à la fin
                        listePrets.RemoveAt(0);  // defiler 
                    }
                }
            }

            TauxUtil(temps);
            return temps;
        }

        #endregion

        #region MultiNiveau
        // Des algorithmes nécessaires pour implémenter MultiNiveaux
        public override async Task<int> Executer(int temps, int nbNiveau, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral, List<ProcessusNiveau> listebloqueGenerale, StackPanel[] ListesPretsViews, StackPanel Processeur, TextBlock TempsView, StackPanel ListeBloqueView, StackPanel deroulement)
        {
            niveaux[indiceNiveau].indice[1] = 0;
            bool anime = true,noDis=false;
            StackPanel ListePretsView = ListesPretsViews[indiceNiveau];
            while (listePrets.Count != 0 && PrioNiveaux(niveaux, indiceNiveau, nbNiveau) && !SimulationPage_MultiLvl.stop)  //tant qu'il existe des processus prêts
            {

                if (!SimulationPage_MultiLvl.paused)
                {
                    if (anime)
                    {
                        listePrets[0].transition = 2; //Activation du 1er processus de listePrets
                        await AfficherDeroulement(deroulement, listebloqueGenerale);
                        listePrets[0].transition = 0;
                        await Activation_MultiLvl(ListePretsView, Processeur, listePrets[0]);
                    }

                    anime = false;
                    listePrets[0].transition = 2; //Activation du 1er processus de listePrets
                    listePrets[0].etat = 2;
                    if (!SimulationPage_MultiLvl.paused) temps++;
                    TempsView.Text = temps.ToString();
                    AfficherEtat(listeGeneral, Ordonnancement.GanttChart, temps);
                    niveaux[indiceNiveau].indice[0] = await MAJListePrets(temps, niveaux[indiceNiveau].indice[0], niveaux, listeGeneral, indiceNiveau, ListesPretsViews);
                    if (await InterruptionExecute(niveaux, listebloqueGenerale, ListesPretsViews, indiceNiveau, ListeBloqueView, Processeur, deroulement, nbNiveau)) 
                    { 
                        niveaux[indiceNiveau].indice[1] = 0;
                        if (!PrioNiveaux(niveaux, indiceNiveau, nbNiveau))
                                noDis = true;
                    }
                    if (!PrioNiveaux(niveaux, indiceNiveau, nbNiveau)) break;
                    niveaux[indiceNiveau].indice[1]++;  // quantum++
                    if (listePrets[0].tempsRestant == listePrets[0].duree) listePrets[0].tempsReponse = temps - 1 - listePrets[0].tempsArriv;
                    listePrets[0].tempsRestant--; //L'exécution courante du 1er processus de listePrets => décrémenter tempsRestant
                    MAJProcesseur_MultiLvl(Processeur);
                    if (listePrets[0].tempsRestant == 0)  // fin d'exécution du processus 
                    {
                        listePrets[0].tempsFin = temps;
                        listePrets[0].tempsService = temps - listePrets[0].tempsArriv;
                        listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree;
                        listePrets[0].etat = 3; //Fin d'exécution du processus
                        listePrets[0].transition = 0;
                        await AfficherDeroulement(deroulement, listebloqueGenerale);
                        listePrets.RemoveAt(0); //supprimer le premier processus executé
                        niveaux[indiceNiveau].indice[1] = 0;  // un nouveau quantum va commencer
                        await FinProcessus_MultiLvl(Processeur);
                        anime = true;
                    }
                    else if (niveaux[indiceNiveau].indice[1] == quantum && listePrets.Count == 1) niveaux[indiceNiveau].indice[1] = 0;
                    else if (niveaux[indiceNiveau].indice[1] == quantum)  // on a terminé ce quantum => il faut passer au processus suivant => on defile, et à la fin, on enfile le processus courant
                    {
                        listePrets[0].transition = 1; //Désactivation du processus
                        listePrets[0].etat = 1;
                        await AfficherDeroulement(deroulement, listebloqueGenerale);
                        await Desactivation_MultiLvl(ListePretsView, Processeur, listePrets[0], indiceNiveau);
                        listePrets[0].tempsFin = temps;  // On sauvegarde le tempsFin puisqu'on a interrompu l'exécution de ce processus
                        niveaux[indiceNiveau].indice[1] = 0;  // nouveau quantum                        
                        listePrets.Add(listePrets[0]);  // enfiler à la fin
                        listePrets.RemoveAt(0);  // defiler 
                        anime = true;
                    }

                }
            }
            if (!PrioNiveaux(niveaux, indiceNiveau, nbNiveau) && listePrets.Count != 0 && !noDis)  // On est arrivé à tempsFin => la fin de l'exécution 
            {
                listePrets[0].transition = 1;//Desactivation du 1er processus de listePrets
                listePrets[0].etat = 1;
                await AfficherDeroulement(deroulement, listebloqueGenerale);
                await Desactivation_MultiLvl(ListePretsView, Processeur, listePrets[0], indiceNiveau);
                listePrets.Add(listePrets[0]);
                listePrets.RemoveAt(0);
                return temps;
            }
            return temps;
        }

        public override int Executer(int tempsDebut, int tempsFin, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral, List<ProcessusNiveau> listebloqueGenerale, StackPanel deroulement)
        // executer l'algo pendant un intervalle de temps où :
        // indices[0] est l'indice de listeProcessus où on doit reprendre l'exécution
        // indices[1] est le quantum du temps de la derniére exécution
        {
            while ((niveaux[indiceNiveau].indice[0] < listeProcessus.Count || listePrets.Count != 0) && !SimulationPage_MultiLvl.stop) //tant qu'il existe des processus prêts
            {
                if (niveaux[indiceNiveau].indice[0] < listeProcessus.Count && listePrets.Count == 0)  // Si il y a des processus dans listProcessus et listePrets est vide
                {
                    tempsDebut++;  // aucun processus n'est arrivé => on incrémente le temps

                    niveaux[indiceNiveau].indice[0] = MAJListePrets(tempsDebut, niveaux[indiceNiveau].indice[0], niveaux, listeGeneral, indiceNiveau); // On rempli listePrets
                }
                else  // listePrets n'est pas vide 
                {
                    listePrets[0].transition = 2; //Activation du 1er processus de listePrets
                    listePrets[0].etat = 2;
                    AfficheLigne(tempsDebut, listePrets[0].id); //affiche le temps actuel et l'ID du processus entrain d'être executé
                    tempsDebut++;
                    AfficherEtat(listeGeneral, Ordonnancement.GanttChart, tempsDebut);
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
                        listePrets[0].etat = 3; // fin d'exécution du processus 
                        listePrets.RemoveAt(0); //supprimer le premier processus executé
                        niveaux[indiceNiveau].indice[1] = 0;  // un nouveau quantum va commencer
                    }
                    else if (niveaux[indiceNiveau].indice[1] == quantum)  // on a terminé ce quantum => il faut passer au processus suivant => on defile, et à la fin, on enfile le processus courant
                    {
                        listePrets[0].tempsFin = tempsDebut;  // On sauvegarde le tempsFin puisqu'on a interrompu l'exécution de ce processus
                        niveaux[indiceNiveau].indice[1] = 0;  // nouveau quantum
                        listePrets[0].transition = 1; //Desactivation du processus 
                        listePrets[0].etat = 1;
                        listePrets.Add(listePrets[0]);  // enfiler à la fin
                        listePrets.RemoveAt(0);  // defiler 
                    }
                }
                if (tempsDebut == tempsFin)  // On est arrivé à tempsFin => la fin de l'exécution 
                {
                    listePrets[0].transition = 1; //Desactivation du processus
                    listePrets[0].etat = 1;
                    listePrets.Add(listePrets[0]);
                    listePrets.RemoveAt(0);
                    return tempsFin;
                }
            }
            return tempsDebut;
        }
        #endregion

        #region MultiNiveauRecyclage

        public override async Task<int> Executer(int temps, int nbNiveau, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral, List<ProcessusNiveau> listebloqueGenerale, StackPanel[] ListesPretsViews, StackPanel Processeur, TextBlock TempsView, StackPanel ListeBloqueView, StackPanel deroulement, int i)
        {
            niveaux[indiceNiveau].indice[1] = 0;
            bool anime = true,noDis=false;
            StackPanel ListePretsView = ListesPretsViews[indiceNiveau];
            while (listePrets.Count != 0 && PrioNiveaux(niveaux, indiceNiveau, nbNiveau) && !SimulationPage_MultiLvl.stop)  //tant qu'il existe des processus prêts
            {
                if (anime)
                {
                    listePrets[0].transition = 2; //Activation du 1er processus de listePrets
                    await AfficherDeroulement(deroulement, listebloqueGenerale);
                    await Activation_MultiLvl(ListePretsView, Processeur, listePrets[0]);
                }


                anime = false;
                listePrets[0].transition = 2; //Activation du 1er processus de listePrets
                listePrets[0].etat = 2;
                temps++;
                TempsView.Text = temps.ToString();
                AfficherEtat(listeGeneral, Ordonnancement.GanttChart, temps);
                niveaux[indiceNiveau].indice[0] = await MAJListePrets(temps, niveaux[indiceNiveau].indice[0], niveaux, listeGeneral, indiceNiveau, ListesPretsViews);
                if (await InterruptionExecute(niveaux, listebloqueGenerale, ListesPretsViews, indiceNiveau, ListeBloqueView, Processeur, deroulement, nbNiveau))
                {
                    niveaux[indiceNiveau].indice[1] = 0;
                    if (!PrioNiveaux(niveaux, indiceNiveau, nbNiveau))
                        noDis = true;
                }
                if (!PrioNiveaux(niveaux, indiceNiveau, nbNiveau)) break;
                niveaux[indiceNiveau].indice[1]++;  // quantum++
                if (listePrets[0].tempsRestant == listePrets[0].duree) listePrets[0].tempsReponse = temps - 1 - listePrets[0].tempsArriv;
                listePrets[0].tempsRestant--; //L'exécution courante du 1er processus de listePrets => décrémenter tempsRestant
                MAJProcesseur_MultiLvl(Processeur);
                if (listePrets[0].tempsRestant == 0)  // fin d'exécution du processus 
                {
                    listePrets[0].tempsFin = temps;
                    listePrets[0].tempsService = temps - listePrets[0].tempsArriv;
                    listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree;
                    listePrets[0].etat = 3; //Fin d'exécution du processus
                    listePrets[0].transition = 0;
                    await AfficherDeroulement(deroulement, listebloqueGenerale);
                    listePrets.RemoveAt(0); //supprimer le premier processus executé
                    niveaux[indiceNiveau].indice[1] = 0;  // un nouveau quantum va commencer
                    await FinProcessus_MultiLvl(Processeur);
                    anime = true;
                }
                else if (niveaux[indiceNiveau].indice[1] == quantum)  // on a terminé ce quantum => il faut passer au processus suivant => on defile, et à la fin, on enfile le processus courant
                {
                    listePrets[0].transition = 1; //Désactivation du processus
                    listePrets[0].etat = 1;
                    await AfficherDeroulement(deroulement, listebloqueGenerale);
                    listePrets[0].tempsFin = temps;  // On sauvegarde le tempsFin puisqu'on a interrompu l'exécution de ce processus
                    niveaux[indiceNiveau].indice[1] = 0;  // nouveau quantum
                    if (indiceNiveau + 1 < nbNiveau)
                    {
                        ((ProcessusNiveau)listePrets[0]).niveau++;
                        await Desactivation_MultiLvl(ListesPretsViews[indiceNiveau + 1], Processeur, listePrets[0], indiceNiveau + 1);
                        niveaux[indiceNiveau + 1].listePrets.Add(listePrets[0]);
                    }
                    else
                    {
                        await Desactivation_MultiLvl(ListePretsView, Processeur, listePrets[0], indiceNiveau);
                        listePrets.Add(listePrets[0]);
                    }
                    listePrets.RemoveAt(0);  // defiler 
                    anime = true;
                }


            }
            if (!PrioNiveaux(niveaux, indiceNiveau, nbNiveau) && listePrets.Count != 0 && noDis)  // On est arrivé à tempsFin => la fin de l'exécution 
            {
                listePrets[0].transition = 1;//Desactivation du 1er processus de listePrets
                listePrets[0].etat = 1;
                await AfficherDeroulement(deroulement, listebloqueGenerale);
                if (indiceNiveau + 1 < nbNiveau)
                {
                    ((ProcessusNiveau)listePrets[0]).niveau++;
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