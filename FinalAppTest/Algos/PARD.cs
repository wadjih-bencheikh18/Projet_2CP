using System.Collections.Generic;
using FinalAppTest;
using System.Threading.Tasks;
using System.Windows.Controls;
namespace Ordonnancement
{
    public class PARD : Ordonnancement //Priorité Avec Réquisition Dynamique
    {
        public int refrechTemps;
        #region Constructeur
        public PARD(int refrechTemps) 
        {
            this.refrechTemps = refrechTemps;
        }
        #endregion

        #region Visualisation
        public override async Task<int> Executer(StackPanel ListePretsView, StackPanel Processeur, TextBlock TempsView, StackPanel ListeBloqueView, TextBlock deroulement, WrapPanel GanttChart)  // executer la liste des processus 
        {
            SortListeProcessus(); //tri des processus par ordre d'arrivé
            int temps = 0;
            int indice = 0;
            bool anime = true, debut = true;
            Processus proc;
            while (listePrets.Count != 0 || indice < listeProcessus.Count || listebloque.Count != 0) //Tant qu'il existe des processus prêts
            {
                int key = 0;
                foreach(Processus pro in listePrets)
                {
                    if (pro.Refrech(temps, refrechTemps,key)&& pro.prio!=0) pro.prio--;
                    key++;
                }

                if (debut) await MAJListePretsView(ListePretsView, 0);
                else await MAJListePretsView(ListePretsView, 1);

                if (listePrets.Count == 0) anime = true;
                indice = await MAJListePrets(temps, indice, ListePretsView);  // Remplir listePrets
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
                    for (int i = 0; i < P.Count; i++)
                    {
                        if (P[i].id != listePrets[i].id) sort = true;
                    }
                    if (sort)
                    {
                        int i;
                        if (debut)
                        {
                            i = 0;
                            anime = true;
                        }
                        else if (proc != listePrets[0])
                        {
                            listePrets[0].transition = 1; //Desactivation du 1er processus de listePrets
                            await AfficherDeroulement(deroulement);
                            await Desactivation(ListePretsView, Processeur, proc);
                            i = 0;
                            anime = true;
                        }
                        else i = 1;
                        await MAJListePretsView(ListePretsView, i);
                    }
                }
                if (anime && listePrets.Count != 0) //si un tri par durée est necessaire et il y a des processus prêts
                {
                    listePrets[0].transition = 2;
                    await AfficherDeroulement(deroulement);
                    await Activation(ListePretsView, Processeur, listePrets[0]);
                    anime = false;
                }
                await InterruptionExecute(ListePretsView, ListeBloqueView, Processeur, deroulement);
                anime = false;
                debut = false;
                if (!SimulationPage.paused) temps++;
                TempsView.Text = temps.ToString();
                if (listePrets.Count != 0 && !SimulationPage.paused) //S'il y a des processus prêts
                {
                    listePrets[0].transition = 2; //Activation du 1er processus de listePrets
                    listePrets[0].etat = 2;
                    AfficherEtat(GanttChart, temps);
                    if (listePrets[0].tempsRestant == listePrets[0].duree) listePrets[0].tempsReponse = temps - 1 - listePrets[0].tempsArriv;
                    listePrets[0].tempsRestant--; //L'exécution courante du 1er processus de listePrets => décrémenter tempsRestant
                    MAJProcesseur(Processeur);
                    if (listePrets[0].tempsRestant == 0)  // Si l'execution du premier processus de listePrets est terminée :
                    {
                        listePrets[0].tempsFin = temps; // temps de fin d'execution = temps actuel
                        listePrets[0].tempsService = temps - listePrets[0].tempsArriv; // temps de service = temps de fin d'execution - temps d'arrivé
                        listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree; //temps d'attente = temps de service - durée d'execution
                        listePrets[0].etat = 3;
                        await AfficherDeroulement(deroulement);
                        listePrets.RemoveAt(0); //supprimer le processus dont la duree est écoulée
                        await FinProcessus(Processeur);
                        debut = true;
                        anime = true;
                    }
                }
                else if (!SimulationPage.paused) AfficherEtat(GanttChart, temps);
            }
            return temps;
        }
        #endregion

        #region Test
        public int Executer()  // executer la liste des processus 
        {
            SortListeProcessus(); //tri des processus par ordre d'arrivé
            int temps = 0;
            int indice = 0;
            while (listePrets.Count != 0 || indice < listeProcessus.Count || listebloque.Count != 0) //Tant qu'il existe des processus prêts
            {
                indice = MAJListePrets(temps, indice);  // Remplir listePrets
                temps++;
                InterruptionExecute();
                if (listePrets.Count != 0) //S'il y a des processus prêts
                {
                    listePrets.Sort(delegate (Processus x, Processus y)
                    {
                        if (x.prio.CompareTo(y.prio) == 0) return x.tempsArriv.CompareTo(y.tempsArriv); //si les processus ont la même priorité, on les trie selon le temps d'arrivée
                        else return x.prio.CompareTo(y.prio); //sinon, on fait le tri par priorité
                    }
                                        );
                    listePrets[0].transition = 2; //Activation du 1er processus de listePrets
                    listePrets[0].etat = 2;
                    if (listePrets[0].tempsRestant == listePrets[0].duree) listePrets[0].tempsReponse = temps - 1 - listePrets[0].tempsArriv;
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
        #endregion

        #region MultiNiveau
        // des algorithmes nécessaires pour implémenter MultiNiveaux
        public override async Task<int> Executer(int tempsDebut, int nbNiveau, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral, List<ProcessusNiveau> listebloqueGenerale, StackPanel[] ListesPretsViews, StackPanel Processeur, TextBlock TempsView, StackPanel ListeBloqueView, TextBlock deroulement)
        {
            bool anime = true, debut = true;
            Processus proc;
            StackPanel ListePretsView = ListesPretsViews[indiceNiveau];
            int temps = tempsDebut;  // initialisation du temps
            while (listePrets.Count != 0 && PrioNiveaux(niveaux, indiceNiveau, nbNiveau))
            //s'il existe des processus prêts et ( On n'est pas encore arrivé à tempsFin ou il n'y a pas de temps fin )
            {
                int key = 0;
                foreach (Processus pro in listePrets)
                {
                    if (pro.Refrech(temps, refrechTemps,key) && pro.prio != 0) pro.prio--;
                    key++;
                }
                if (debut) await MAJListePretsView_MultiLvl(ListePretsView, 0);
                else await MAJListePretsView_MultiLvl(ListePretsView, 1);
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
                    for (int i = 0; i < P.Count; i++)
                    {
                        if (P[i].id != listePrets[i].id) sort = true;
                    }
                    if (sort)
                    {
                        int i;
                        if (debut)
                        {
                            i = 0;
                            anime = true;
                        }
                        else if (proc != listePrets[0])
                        {
                            listePrets[0].transition = 1; //désactivation du processus qui était entrain d'être exécuté 
                            await AfficherDeroulement(deroulement);
                            await Desactivation_MultiLvl(ListePretsView, Processeur, proc, indiceNiveau);
                            i = 0;
                            anime = true;
                        }
                        else i = 1;
                        await MAJListePretsView_MultiLvl(ListePretsView, i);
                    }
                }
                if (anime && listePrets.Count != 0) //si un tri par durée est necessaire et il y a des processus prêts
                {
                    listePrets[0].transition = 2; //Activation du 1er processus dans listePrets
                    await AfficherDeroulement(deroulement);
                    await Activation_MultiLvl(ListePretsView, Processeur, listePrets[0]);
                    anime = false;
                }
                await InterruptionExecute(listebloqueGenerale, ListesPretsViews, indiceNiveau, ListeBloqueView, Processeur, deroulement);
                temps++;
                TempsView.Text = temps.ToString();
                niveaux[indiceNiveau].indice[0] = await MAJListePrets(temps, niveaux[indiceNiveau].indice[0], niveaux, listeGeneral, indiceNiveau, ListesPretsViews); //remplir la liste des processus prêts de chaque niveau
                anime = false;
                debut = false;
                if (listePrets.Count != 0) //S'il y a des processus prêts
                {
                    listePrets[0].transition = 2; //Activation du 1er processus de listePrets
                    listePrets[0].etat = 2;
                    if (listePrets[0].tempsRestant == listePrets[0].duree) listePrets[0].tempsReponse = temps - 1 - listePrets[0].tempsArriv;
                    listePrets[0].tempsRestant--;//L'exécution courante du 1er processus de listePrets => décrémenter tempsRestant
                    MAJProcesseur_MultiLvl(Processeur);
                    if (listePrets[0].tempsRestant == 0) // Si l'execution du premier processus de listePrets est terminée :
                    {
                        listePrets[0].tempsFin = temps; // temps de fin d'execution = temps actuel
                        listePrets[0].tempsService = temps - listePrets[0].tempsArriv; // temps de service = temps de fin d'execution - temps d'arrivé
                        listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree; //temps d'attente = temps de service - durée d'execution
                        listePrets[0].etat = 3; //Fin d'exécution du processus
                        await AfficherDeroulement(deroulement);
                        await FinProcessus_MultiLvl(Processeur);
                        debut = true;
                        anime = true;
                        listePrets.RemoveAt(0); //supprimer le processus dont la duree est écoulée
                    }
                }

            }
            if (!PrioNiveaux(niveaux, indiceNiveau, nbNiveau) && listePrets.Count != 0)
            {
                listePrets[0].transition = 1; //Désactivation du processus qui était entrain d'être exécuté
                listePrets[0].etat = 1;
                await AfficherDeroulement(deroulement);
                await Desactivation_MultiLvl(ListePretsView, Processeur, listePrets[0], indiceNiveau);
                listePrets.Add(listePrets[0]);
                listePrets.RemoveAt(0);
                return temps;
            }
            return temps;
        }

        public override int Executer(int tempsDebut, int tempsFin, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral, List<ProcessusNiveau> listebloqueGenerale, TextBlock deroulement)
        {
            int temps = tempsDebut;  // initialisation du temps
            while (listePrets.Count != 0 && (temps < tempsFin || tempsFin == -1))
            //s'il existe des processus prêts et ( On n'est pas encore arrivé à tempsFin ou il n'y a pas de temps fin )
            {
                niveaux[indiceNiveau].indice[0] = MAJListePrets(temps, niveaux[indiceNiveau].indice[0], niveaux, listeGeneral, indiceNiveau);  //remplir la liste des processus prêts de chaque niveau
                temps++;
                InterruptionExecute(listebloqueGenerale);
                if (listePrets.Count != 0) //S'il y a des processus prêts
                {
                    listePrets.Sort(
                                           delegate (Processus x, Processus y)
                                           {
                                               if (x.prio.CompareTo(y.prio) == 0) return x.tempsArriv.CompareTo(y.tempsArriv); //si les processus ont la même priorité, on les trie selon le temps d'arrivée
                                               else return x.prio.CompareTo(y.prio); //sinon, on fait le tri par priorité
                                           }
                                    );
                    listePrets[0].transition = 2; //Activation du 1er processus de listePrets
                    listePrets[0].etat = 2;
                    if (listePrets[0].tempsRestant == listePrets[0].duree) listePrets[0].tempsReponse = temps - 1 - listePrets[0].tempsArriv;
                    listePrets[0].tempsRestant--;//L'exécution courante du 1er processus de listePrets => décrémenter tempsRestant
                    AfficheLigne(temps - 1, listePrets[0].id); //affiche le temps et l'ID du processus entrain d'être executé
                    if (listePrets[0].tempsRestant == 0) // Si l'execution du premier processus de listePrets est terminée :
                    {
                        listePrets[0].tempsFin = temps; // temps de fin d'execution = temps actuel
                        listePrets[0].tempsService = temps - listePrets[0].tempsArriv; // temps de service = temps de fin d'execution - temps d'arrivé
                        listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree; //temps d'attente = temps de service - durée d'execution
                        listePrets[0].etat = 3; //Fin d'exécution du processus
                        listePrets.RemoveAt(0); //supprimer le processus dont la duree est écoulée
                    }
                }
                if (temps == tempsFin)
                {
                    listePrets[0].transition = 1; //Désactivation du processus qui était entrain d'être exécuté
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

        public override async Task<int> Executer(int tempsDebut, int nbNiveau, Niveau[] niveaux, int indiceNiveau, List<ProcessusNiveau> listeGeneral, List<ProcessusNiveau> listebloqueGenerale, StackPanel[] ListesPretsViews, StackPanel Processeur, TextBlock TempsView, StackPanel ListeBloqueView, TextBlock deroulement, int k)
        {
            bool anime = true, debut = true;
            Processus proc;
            StackPanel ListePretsView = ListesPretsViews[indiceNiveau];
            int temps = tempsDebut;  // initialisation du temps
            while (listePrets.Count != 0 && PrioNiveaux(niveaux, indiceNiveau, nbNiveau))
            //s'il existe des processus prêts et ( On n'est pas encore arrivé à tempsFin ou il n'y a pas de temps fin )
            {
                int key = 0;
                foreach (Processus pro in listePrets)
                {
                    if (pro.Refrech(temps, refrechTemps,key) && pro.prio != 0) pro.prio--;
                    key++;
                }
                if (debut) await MAJListePretsView_MultiLvl(ListePretsView, 0);
                else await MAJListePretsView_MultiLvl(ListePretsView, 1);
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
                    for (int i = 0; i < P.Count; i++)
                    {
                        if (P[i].id != listePrets[i].id) sort = true;
                    }
                    if (sort)
                    {
                        int i;
                        if (debut)
                        {
                            i = 0;
                            anime = true;
                        }
                        else if (proc != listePrets[0])
                        {
                            listePrets[0].transition = 1; //désactivation du processus qui était entrain d'être exécuté 
                            await AfficherDeroulement(deroulement);
                            if (indiceNiveau + 1 < nbNiveau)
                            {
                                await Desactivation_MultiLvl(ListesPretsViews[indiceNiveau + 1], Processeur, proc, indiceNiveau + 1);
                                niveaux[indiceNiveau + 1].listePrets.Add(proc);
                                listePrets.Remove(proc);
                            }
                            else
                            {
                                await Desactivation_MultiLvl(ListePretsView, Processeur, proc, indiceNiveau);
                            }
                            i = 0;
                            anime = true;
                        }
                        else i = 1;
                        await MAJListePretsView_MultiLvl(ListePretsView, i);
                    }
                }
                if (anime && listePrets.Count != 0) //si un tri par durée est necessaire et il y a des processus prêts
                {
                    listePrets[0].transition = 2; //Activation du 1er processus dans listePrets
                    await AfficherDeroulement(deroulement);
                    await Activation_MultiLvl(ListePretsView, Processeur, listePrets[0]);
                    anime = false;
                }
                await InterruptionExecute(listebloqueGenerale, ListesPretsViews, indiceNiveau, ListeBloqueView, Processeur, deroulement);
                temps++;
                TempsView.Text = temps.ToString();
                niveaux[indiceNiveau].indice[0] = await MAJListePrets(temps, niveaux[indiceNiveau].indice[0], niveaux, listeGeneral, indiceNiveau, ListesPretsViews); //remplir la liste des processus prêts de chaque niveau
                anime = false;
                debut = false;
                if (listePrets.Count != 0) //S'il y a des processus prêts
                {
                    listePrets[0].transition = 2; //Activation du 1er processus de listePrets
                    listePrets[0].etat = 2;
                    if (listePrets[0].tempsRestant == listePrets[0].duree) listePrets[0].tempsReponse = temps - 1 - listePrets[0].tempsArriv;
                    listePrets[0].tempsRestant--;//L'exécution courante du 1er processus de listePrets => décrémenter tempsRestant
                    MAJProcesseur_MultiLvl(Processeur);
                    if (listePrets[0].tempsRestant == 0) // Si l'execution du premier processus de listePrets est terminée :
                    {
                        listePrets[0].tempsFin = temps; // temps de fin d'execution = temps actuel
                        listePrets[0].tempsService = temps - listePrets[0].tempsArriv; // temps de service = temps de fin d'execution - temps d'arrivé
                        listePrets[0].tempsAtt = listePrets[0].tempsService - listePrets[0].duree; //temps d'attente = temps de service - durée d'execution
                        listePrets[0].etat = 3; //Fin d'exécution du processus
                        await AfficherDeroulement(deroulement);
                        await FinProcessus_MultiLvl(Processeur);
                        debut = true;
                        anime = true;
                        listePrets.RemoveAt(0); //supprimer le processus dont la duree est écoulée
                    }
                }

            }
            if (!PrioNiveaux(niveaux, indiceNiveau, nbNiveau) && listePrets.Count != 0)
            {
                listePrets[0].transition = 1; //Désactivation du processus qui était entrain d'être exécuté
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