using System;

namespace Ordonnancement
{
    class Test
    {
        static void Main(string[] args)
        {
            Processus Pro;
            RoundRobin prgm = new(2);
            Random r1, r2, r3, r4;
            Console.WriteLine("Initialisation : \n");
            for (int i = 0; i < 6; i++)
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
        }
    }

}