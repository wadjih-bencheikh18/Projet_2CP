using FinalAppTest;
using FinalAppTest.Views;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Ordonnancement
{
    public class Processus
    {

        #region Abttributs
        //donnés
        public List<Interruption> listeInterruptions = new List<Interruption>();
        public int id { get; set; } //ID du processus
        public int tempsArriv { get; set; } //temps d'arrivé
        public int duree { get; set; } //temps d'execution du processus (burst time)
        public int prio { get; set; } //priorite du processus
        public int deadline { get; set; }
        //à remplir
        public int etat { get; set; }// 0:bloqué  1:prêt  2:en cours  3:fini
        public int transition { get; set; }// 0:blocage  1:désactivation  2:activation  3:reveil
        public int tempsFin { get; set; }
        public int tempsAtt { get; set; }
        public int tempsService { get; set; }
        public int tempsRestant { get; set; }
        public int tempsReponse { get; set; }
        public int slackTime { get; set; }
        public int[] indiceInterruptions = new int[2];
        #endregion

        #region Constructeur

        /// <summary>
        /// initialiser un processus
        /// </summary>

        public Processus(int id, int tempsArriv, int duree, int prio)  //constructeur pour l'algorithme de priorité
        {
            this.etat = 3;
            this.id = id;
            this.tempsArriv = tempsArriv;
            this.duree = duree;
            this.prio = prio;
            tempsRestant = duree;
        }
        public void CalculeSlackTime(int temps)
        {
            slackTime = deadline - temps - tempsRestant;
        }
        public Processus(int id, int tempsArriv, int duree, int prio,int deadline)  //constructeur pour l'algorithme de priorité
        {
            this.etat = 3;
            this.id = id;
            this.tempsArriv = tempsArriv;
            this.duree = duree;
            this.prio = prio;
            tempsRestant = duree;
            this.deadline = deadline;
            slackTime = deadline - 0 - tempsRestant;
        }
        public Processus(int id, int tempsArriv, int duree) //constructeur pour les autres algorithmes
        {
            this.etat = 3;
            this.id = id;
            this.tempsArriv = tempsArriv;
            this.duree = duree;
            prio = 0;
            tempsRestant = duree;
        }
        public Processus() { }
        #endregion

        #region Affichage

        /// <summary>
        /// le module utiliser pour l'affichage d'un processus
        /// </summary>

        public virtual void Affichage() //affiche les caracteristiques d'un processus
        {
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.Write("ID : " + id);
            Console.Write("\ttemps d'arrivé : " + tempsArriv);
            Console.Write("\tduree : " + duree);
            Console.Write("\tpriorité : " + prio);
            Console.Write("\ttemps d'attente : " + tempsAtt);
            Console.Write("\ttemps de fin :  " + tempsFin);
            Console.Write("\ttemps de service  : " + tempsService);
            //Console.Write("\ttemps restant : " + tempsRestant);
        }
        #endregion

        #region Interruption

        /// <summary>
        /// Les modules utiliser pour implimanter les interruptions
        /// </summary>

        public void Push(Interruption interruption)
        {
            listeInterruptions.Add(interruption);
        }

        public void SortListeInterruptions()
        {
            listeInterruptions.Sort
                    (
                        delegate (Interruption x, Interruption y)
                        {
                            if (x.tempsArriv.CompareTo(y.tempsArriv) == 0) return x.duree.CompareTo(y.duree); //si les processus ont la même priorité, on les trie selon le temps d'arrivée
                            else return x.tempsArriv.CompareTo(y.tempsArriv); //sinon, on fait le tri par priorité
                        }
                    );
        }

        public bool InterruptionExist()
        {
            for (; indiceInterruptions[1] < listeInterruptions.Count; indiceInterruptions[1]++)
            {
                if (!listeInterruptions[indiceInterruptions[1]].Prete(duree - tempsRestant)) break;
            }
            if (indiceInterruptions[0] == indiceInterruptions[1]) return false;
            else return true;
        }

        public void InterruptionExecute()
        {
            for (int i = indiceInterruptions[0]; i < indiceInterruptions[1]; i++)
            {
                listeInterruptions[i].Execute();
                if (listeInterruptions[i].tempsRestant == 0) indiceInterruptions[0]++;
            }
        }
        #endregion

        #region Visualisation

        public void Affichage(Grid Table, int i)
        {
            AffichageProcessus proc;
            TableRowFinal item;
            RowDefinition rowdef = new RowDefinition
            {
                Height = new GridLength(60)
            };
            Table.RowDefinitions.Insert(i, rowdef);
            item = new TableRowFinal();
            proc = new AffichageProcessus(this);
            if (i % 2 == 0) proc.Background = "LightBlue";
            else proc.Background = "lightGray";
            item.DataContext = proc;
            Grid.SetRow(item, i + 1);
            Table.Children.Add(item);
        }
        #endregion

    }

    public class ProcessusNiveau : Processus
    {
        #region Attributs
        public int niveau { get; set; }
        #endregion

        #region Constructeur
        public ProcessusNiveau(int id, int tempsArriv, int duree, int prio, int niveau) : base(id, tempsArriv, duree, prio)
        {
            this.niveau = niveau;
        }
        public ProcessusNiveau(int id, int tempsArriv, int duree, int prio) : base(id, tempsArriv, duree, prio) {}
        public ProcessusNiveau() { }
        #endregion

        #region Affichage
        public override void Affichage() //surdefinition : affiche les caracteristiques d'un processus en plus de son niveau
        {
            base.Affichage();
            Console.Write("\tNiveau : " + niveau);
        }
        #endregion

    }

    public class AffichageProcessus : ProcessusNiveau  // utilisé pour la simulation
    {

        #region Attributs
        public int tempsPasse { get; set; }
        public string quantum { get; set; }
        public string Background { get; set; }
        public double Speed { get; set; }
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }
        public double X3 { get; set; }
        public double Y3 { get; set; }
        public double X4 { get; set; }
        public double Y4 { get; set; }
        #endregion

        #region Constructeur
        public AffichageProcessus() { Speed = 1; }
        public AffichageProcessus(Processus processus)
        {
            this.id = processus.id;
            this.prio = processus.prio;
            this.tempsArriv = processus.tempsArriv;
            this.duree = processus.duree;
            this.tempsAtt = processus.tempsAtt;
            this.tempsFin = processus.tempsFin;
            this.tempsService = processus.tempsService;
            this.tempsReponse = processus.tempsReponse;
            this.tempsRestant = processus.tempsRestant;
            this.tempsPasse = processus.duree - processus.tempsRestant;
            Speed = 1;
        }
        public AffichageProcessus(ProcessusNiveau processus)
        {
            this.id = processus.id;
            this.prio = processus.prio;
            this.tempsArriv = processus.tempsArriv;
            this.duree = processus.duree;
            this.tempsAtt = processus.tempsAtt;
            this.tempsFin = processus.tempsFin;
            this.tempsService = processus.tempsService;
            this.tempsReponse = processus.tempsReponse;
            this.tempsRestant = processus.tempsRestant;
            this.tempsPasse = processus.duree - processus.tempsRestant;
            this.niveau = processus.niveau;
            Speed = 1;
        }
        #endregion

        #region Visualisation
        public PAPS_TabRow InsererPAPS(StackPanel Table, TextBox id, TextBox tempsArriv, TextBox duree, TextBlock Ajouter)  // inserer un processus dans Table à la i'éme ligne pour PAPS
        {
            PAPS_TabRow item = new PAPS_TabRow(id, tempsArriv, duree, Table, Ajouter);
            Background = "#FFEFF3F9";
            item.DataContext = this;
            Table.Children.Add(item);
            return item;
        }
        public void InsererProcML(StackPanel Table, TextBox id, TextBox tempsArriv, TextBox duree, TextBox prio, TextBox niveau, TextBlock Ajouter)  // inserer un processus dans Table à la i'éme ligne pour Ml proc
        {
            Multi_Niv_TabRow_Proc item = new Multi_Niv_TabRow_Proc(id, tempsArriv, duree, prio, niveau, Table, Ajouter);
            item.DataContext = this;
            Table.Children.Add(item);
        }
        public void InsererNivML(StackPanel Table, TextBox nivId, ComboBox algo, TextBox quantum, TextBlock Ajouter)  // inserer un processus dans Table à la i'éme ligne pour ML nv
        {
            Mult_Niv_TabRow item = new Mult_Niv_TabRow(nivId, algo, quantum, Table, Ajouter);
            item.DataContext = this;
            Table.Children.Add(item);
        }
        public PCA_TabRow InsererPCA(StackPanel Table, TextBox id, TextBox tempsArriv, TextBox duree, TextBlock Ajouter)  // inserer un processus dans Table à la i'éme ligne pour PCA
        {
            PCA_TabRow item = new PCA_TabRow(id, tempsArriv, duree, Table, Ajouter);
            Background = "#FFEFF3F9";
            item.DataContext = this;
            Table.Children.Add(item);
            return item;
        }
        public PLA_TabRow InsererPLA(StackPanel Table, TextBox id, TextBox tempsArriv, TextBox duree, TextBlock Ajouter)  // inserer un processus dans Table à la i'éme ligne pour PLA
        {
            PLA_TabRow item = new PLA_TabRow(id, tempsArriv, duree, Table, Ajouter);
            Background = "#FFEFF3F9";
            item.DataContext = this;
            Table.Children.Add(item);
            return item;
        }
        public PCTR_TabRow InsererPCTR(StackPanel Table, TextBox id, TextBox tempsArriv, TextBox duree, TextBlock Ajouter)  // inserer un processus dans Table à la i'éme ligne pour PCTR
        {
            PCTR_TabRow item = new PCTR_TabRow(id, tempsArriv, duree, Table, Ajouter);
            Background = "#FFEFF3F9";
            item.DataContext = this;
            Table.Children.Add(item);
            return item;
        }
        public PSP_TabRow InsererPSP(StackPanel Table, TextBox id, TextBox tempsArriv, TextBox duree, TextBox prio, TextBlock Ajouter)  // inserer un processus dans Table à la i'éme ligne pour PSP
        {
            PSP_TabRow item = new PSP_TabRow(id, tempsArriv, duree, prio, Table, Ajouter);
            Background = "#FFEFF3F9";
            item.DataContext = this;
            Table.Children.Add(item);
            return item;
        }
        public PRIO_TabRow InsererPRIO(StackPanel Table, TextBox id, TextBox tempsArriv, TextBox duree, TextBox prio, TextBlock Ajouter)  // inserer un processus dans Table à la i'éme ligne pour PRIO
        {
            PRIO_TabRow item = new PRIO_TabRow(id, tempsArriv, duree, prio, Table, Ajouter);
            Background = "#FFEFF3F9";
            item.DataContext = this;
            Table.Children.Add(item);
            return item;
        }
        public RoundRobin_TabRow InsererRR(StackPanel Table, TextBox id, TextBox tempsArriv, TextBox duree, TextBlock Ajouter)  // inserer un processus dans Table à la i'éme ligne pour RR
        {
            RoundRobin_TabRow item = new RoundRobin_TabRow(id, tempsArriv, duree, Table, Ajouter);
            Background = "#FFEFF3F9";
            item.DataContext = this;
            Table.Children.Add(item);
            return item;
        }
        public SlackTime_TabRow InsererSlackTime(StackPanel Table, TextBox id, TextBox tempsArriv, TextBox duree,TextBox deadline, TextBlock Ajouter)  // inserer un processus dans Table à la i'éme ligne pour PSPD
        {
            SlackTime_TabRow item = new SlackTime_TabRow(id, tempsArriv, duree,deadline, Table, Ajouter);
            Background = "#FFEFF3F9";
            item.DataContext = this;
            Table.Children.Add(item);
            return item;
        }
        #endregion

    }
}