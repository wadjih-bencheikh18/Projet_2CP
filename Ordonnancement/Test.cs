using System;

namespace Ordonnancement
{
    class Test
    {
        static void Main(string[] args)
        {
            /*
            bool test = false;
            Processus Pro;
            int numAlgo = 0;
            Ordonnancement prgm = new PAPS();
            while (!test)
            {
                Console.WriteLine("Voici les algorithmes d'ordonnancement:\n");
                Console.WriteLine("\t1 - Premier Arrivé Premier Servie (PAPS)");
                Console.WriteLine("\t2 - Plus Court d’Abord (PCA)");
                Console.WriteLine("\t3 - Priorité Avec Réquisition (PSP)");
                Console.WriteLine("\t4 - Le Tourniquet (Round Robin)");
                Console.WriteLine("\t5 - Multi-Niveau \n");
                Console.Write(" > ");
                numAlgo = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("");
                if (numAlgo == 1)
                {
                    prgm = new PAPS();
                    test = true;
                }
                else if (numAlgo == 2)
                {
                    prgm = new PCA();
                    test = true;
                }
                else if (numAlgo == 3)
                {
                    prgm = new PSP();
                    test = true;
                }
                else if (numAlgo == 4)
                {
                    Console.Write("Donner un quantum : ");
                    int quantum = Convert.ToInt32(Console.ReadLine());
                    prgm = new RoundRobin(quantum);
                    test = true;
                }
                else { Console.WriteLine("Votre choix n'est pas valide . Veuillez réesseyer "); }

            }
            Console.WriteLine("Voulez-vous saisir les données des processus ? \n");
            Console.WriteLine("Remarque : Si vous choisissez Non , la génération des données des processus sera aléatoire \n");
            Console.Write("Votre reponse (y/n) : ");
            string reponse = Console.ReadLine();
            Console.WriteLine("Combien de processus voulez-vous ajouter ?");
            Console.Write(" > ");
            int numPro = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("");
            Random r1, r2, r3, r4;
            int duree, prio, temparriv, id;
            for (int i = 0; i < numPro; i++)
            {
                if (reponse == "y")
                {
                    id = i;
                    Console.Write("Donner son temps d'arrivé : ");
                    temparriv = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Donner sa duree : ");
                    duree = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Donner sa priorité : ");
                    prio = Convert.ToInt32(Console.ReadLine());
                    Pro = new(id, temparriv, duree, prio);
                    Console.WriteLine("");
                }
                else
                {
                    r1 = new Random();
                    r2 = new Random();
                    r3 = new Random();
                    r4 = new Random();
                    Pro = new(i, r2.Next(0, 15), r3.Next(1, 15), r4.Next(1, 5));
                }
                prgm.Push(Pro);
            }
            Console.WriteLine("**** Iniatialisation **** ");
            prgm.Affichage();
            Console.WriteLine("\n\n*****************************************\n");
            Console.WriteLine("Le temps| Id de processus executer\t|\n");
            Console.WriteLine("*****************************************");
            switch (numAlgo)
            {
                case 1:
                    ((PAPS)prgm).Executer();
                    break;
                case 2:
                    ((PCA)prgm).Executer();
                    break;
                case 3:
                    ((PSP)prgm).Executer();
                    break;
                case 4:
                    ((RoundRobin)prgm).Executer();
                    break;
            }
            Console.WriteLine("*****************************************");
            Console.WriteLine("\n\n\n");
            Console.WriteLine("Les resultats :");
            prgm.Affichage();
            Console.WriteLine("\n\n\n");
            Console.ReadLine();
            */
            
            ProcessusNiveau Pro;
            Random r1, r2, r3, r4;
            int a1, a2;
            int nbNiveau = 6;
            Niveau[] niveaux = new Niveau[nbNiveau];
            Console.WriteLine("Les niveaux : \n");
            for (int i = 0; i < nbNiveau; i++)
            {
                r1 = new Random();
                a1 = r1.Next(0, 4);
                if (a1 == 3)
                {
                    r2 = new Random();
                    a2 = r2.Next(0, 6);
                    niveaux[i] = new(a1, a2);
                    Console.WriteLine(i + "=>" + a1 + "=>" + a2);
                }
                else
                {
                    niveaux[i] = new(a1);
                    Console.WriteLine(i + "=>" + a1);
                }

            }
            MultiNiveau prgm = new(nbNiveau, niveaux);
            Console.WriteLine("\n\n");
            Console.WriteLine("Initialisation : \n");
            for (int i = 0; i < 10; i++)
            {
                r1 = new Random();
                r2 = new Random();
                r3 = new Random();
                r4 = new Random();
                Pro = new(i, r1.Next(0, 15), r2.Next(1, 15), r3.Next(0, 15), r4.Next(0, 4));
                Pro.Affichage();
                prgm.Push(Pro);
            }
            Console.WriteLine("\n\n*****************************************");
            Console.WriteLine("Le temps| Id de processus executer\t|\n");
            Console.WriteLine("*****************************************");
            prgm.Executer();
            Console.WriteLine("*****************************************");
            Console.WriteLine("\n\n\n");
            Console.WriteLine("Les resultats :");
            prgm.Affichage();
            Console.WriteLine("\n\n\n");
            
        }
    }

}