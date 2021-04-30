using System;

namespace Ordonnancement
{
    class Test
    {
        static void Main(string[] args)
        {
            /*
            Processus Pro;
            RoundRobin prgm = new(5);
            Random r1, r2, r3, r4;
            Console.WriteLine("Initialisation : \n");
            for (int i = 0; i < 5; i++)
            {
                r1 = new Random();
                r2 = new Random();
                r3 = new Random();
                r4 = new Random();
                Pro = new(i, r2.Next(0, 15), r3.Next(1, 15));
                Console.WriteLine("id: " + Pro.id + "  arriv: " + Pro.tempsArriv + "  duree: " + Pro.duree);
                prgm.Push(Pro);
            }
            Console.WriteLine("\n*****************************************\n");
            Console.WriteLine("Le temps| Id de processus executer\t|\n");
            Console.WriteLine("*****************************************");
            prgm.Executer();
            Console.WriteLine("*****************************************");
            Console.WriteLine("\n\n\n");
            Console.WriteLine("Les resultats :");
            prgm.Affichage();
            Console.WriteLine("\n\n\n");
            */


            ProcessusNiveau Pro;
            Random r1, r2, r3, r4;
            int a1, a2;
            int nbNiveau = 5;
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
                    Console.WriteLine(i + "=>" + a1 );
                }

            }
            MultiNiveau prgm = new(nbNiveau, niveaux);
            Console.WriteLine("\n\n");
            Console.WriteLine("Initialisation : \n");
            for (int i = 0; i < 6; i++)
            {
                r1 = new Random();
                r2 = new Random();
                r3 = new Random();
                r4 = new Random();
                Pro = new(i, r1.Next(0, 15), r2.Next(1, 15), r3.Next(0, 15), r3.Next(0, 4));
                Pro.Affichage();
                prgm.Push(Pro);
            }
            Console.WriteLine("\n\n");
            prgm.Executer();
            prgm.Affichage();
            Console.WriteLine("\n\n\n");

        }
    }

}