using System;
using System.Collections.Generic;

namespace Ordonnancement
{
    public class Niveau
    {
        #region Attributs
        public List<Processus> listeProcessus = new List<Processus>();
        public List<Processus> listePrets = new List<Processus>();
        public List<Processus> listebloque = new List<Processus>();
        public Ordonnancement algo;

        public int numAlgo; //0:PAPS  1:PCA  2:Priorité  3:RoundRobin
        public int[] indice = new int[4];
        #endregion

        #region Contructeur

        /// <summary>
        /// initialiser un niveau
        /// </summary>

        public Niveau(int numAlgo)
        {
            this.numAlgo = numAlgo;
            switch (this.numAlgo)
            {
                case 0:
                    algo = new PAPS();
                    break;
                case 1:
                    algo = new PCA();
                    break;
                case 2:
                    algo = new PSP();
                    break;
                case 3:
                    Console.WriteLine("Il manque un quantum. Veuillez reessayer");
                    break;
                default:
                    Console.WriteLine("ERREUR. Veuillez choisir un numero entre 0 et 3");
                    break;
            }
        }
        public Niveau(int numAlgo, int quantum)
        {
            this.numAlgo = numAlgo;
            switch (this.numAlgo)
            {
                case 0:
                    algo = new PAPS();
                    break;
                case 1:
                    algo = new PCA();
                    break;
                case 2:
                    algo = new PSP();
                    break;
                case 3:
                    algo = new RoundRobin(quantum);
                    break;
                default:
                    Console.WriteLine("ERREUR. Veuillez choisir un numero entre 0 et 3");
                    break;
            }
        }
        #endregion
    }
}