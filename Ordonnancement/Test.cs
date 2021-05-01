using System;

namespace Ordonnancement
{
    class Test
    {
        static void Main(string[] args)
        {

            bool test = false;
            Processus Pro;
            int numAlgo = 0, nbNiveau = 0;
            Ordonnancement prgm = new PAPS();
            Random r1, r2, r3, r4;
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
                else if (numAlgo == 5)
                {
                    test = true;
                    int a1, a2;
                    Console.Write("Entrer le nombre de niveaux: ");
                    nbNiveau = int.Parse(Console.ReadLine());
                    Niveau[] niveaux = new Niveau[nbNiveau];
                    Console.WriteLine("\nVoulez-vous saisir les données des niveaux ? ");
                    Console.WriteLine("Remarque : Si vous choisissez Non , lse types d'algorithmes seront aléatoires ");
                    Console.Write("Votre reponse (y/n) : ");
                    char rep = char.Parse(Console.ReadLine());
                    for (int i = 0; i < nbNiveau; i++)
                    {
                        if (rep == 'n') { r1 = new Random(); a1 = r1.Next(0, 4); }
                        else
                        {
                            Console.WriteLine("Entrer l'Algorithme du niveau " + (i) + ".\n1.PAPS | 2.PCA | 3.PSP | 4.Tourniquet");
                            a1 = int.Parse(Console.ReadLine());
                        }
                        if (a1 == 3)
                        {
                            if (rep == 'n') { r2 = new Random(); a2 = r2.Next(1, 20); }
                            else
                            {
                                Console.WriteLine("Entrer le quantum du niveau " + (i));
                                a2 = int.Parse(Console.ReadLine());
                            }
                            niveaux[i] = new(a1, a2);
                            // Console.WriteLine("    ~ Niveau "+ (i+1) + ": Round robin avec quantum " + a2);
                        }
                        else
                        {
                            niveaux[i] = new(a1);
                            /*switch (a1)
                            {
                                case 0:
                                    Console.WriteLine("    ~ Niveau " + (i + 1) + ": PAPS");
                                    break;
                                case 1:
                                    Console.WriteLine("    ~ Niveau " + (i + 1) + ": PCA");
                                    break;
                                case 2:
                                    Console.WriteLine("    ~ Niveau " + (i + 1) + ": PSP");
                                    break;
                                default:
                                    Console.WriteLine("ERREUR. Veuillez choisir un numero entre 0 et 3");
                                    break;
                            }*/
                        }
                        Console.WriteLine("\n");

                    }
                    prgm = new MultiNiveau(nbNiveau, niveaux);

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
            int duree, prio, temparriv, id, niveau;
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
                   // Pro = new(id, temparriv, duree, prio);

                    if (numAlgo == 5)
                    {
                        error:
                        Console.Write("Donner son niveau : ");
                        niveau = int.Parse(Console.ReadLine());
                        if(niveau<0 || niveau >= nbNiveau)
                        {
                            Console.WriteLine("Entrer un numero entre 0 et " + (nbNiveau - 1));
                            goto error;
                        }
                        Pro = new ProcessusNiveau(id, temparriv, duree, prio, niveau);
                    }
                    else { Pro = new(id, temparriv, duree, prio); }
                    Console.WriteLine("");
                }
                else
                {
                    r1 = new Random();
                    r2 = new Random();
                    r3 = new Random();
                    r4 = new Random();
                    if (numAlgo == 5) { Pro = new ProcessusNiveau(i, r2.Next(0, 15), r3.Next(1, 15), r4.Next(1, 15), r1.Next(0, nbNiveau)); }
                    else { Pro = new(i, r2.Next(0, 15), r3.Next(1, 15), r4.Next(1, 5)); }
                }
                if (numAlgo != 5) prgm.Push(Pro);
                else ((MultiNiveau)prgm).Push((ProcessusNiveau)Pro);
            }
            Console.WriteLine("**** Iniatialisation **** ");
            if(numAlgo!=5) prgm.Affichage();
            else ((MultiNiveau)prgm).Affichage();
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
                case 5:
                    ((MultiNiveau)prgm).Executer();
                    break;
                default:
                    Console.WriteLine("error");
                    break;
            }
            Console.WriteLine("*****************************************");
            Console.WriteLine("\n\n\n");
            Console.WriteLine("Les resultats :");
            if (numAlgo != 5) prgm.Affichage();
            else ((MultiNiveau)prgm).Affichage();
            Console.WriteLine("\n\n\n");
            Console.ReadLine();


            /*ProcessusNiveau Pro;
            
            
            Console.WriteLine("\n\n");
            Console.WriteLine("Initialisation : \n");
            int id = 0;
            for (int niv = 0; niv < nbNiveau; niv++){
                r1 = new Random();
                r2 = new Random();
                r3 = new Random();
                int nbAlgo = r1.Next(1, 5);
                for (int i = 0; i < nbAlgo; i++)
                {
                    Pro = new(id, r1.Next(0, 15), r2.Next(1, 15), r3.Next(0, 15), niv);
                    Pro.Affichage();
                    prgm.Push(Pro);
                    id++;
                }
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
            */
        } 
    }

    }