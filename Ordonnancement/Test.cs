using System;

namespace Ordonnancement
{
    class Test
    {
        static void Main(string[] args)
        {
            Processus Pro;
            PAPS prgm = new();
            Random r1, r2, r3, r4;
            Console.WriteLine("Initialisation : \n");
            for (int i = 0; i < 5; i++)
            {
                r1 = new Random();
                r2 = new Random();
                r3 = new Random();
                r4 = new Random();
                Pro = new(r1.Next(0, 15), r2.Next(0, 15), r3.Next(1, 15), r4.Next(0, 15));
                Console.WriteLine("id: " + Pro.id + "  arriv: " + Pro.tempsArriv + "  duree: " + Pro.duree);
                prgm.Push(Pro);
            }
            Console.WriteLine("***********************");
            prgm.Executer();
            prgm.Affichage();
            prgm.Executer();
            prgm.Affichage();
        }
    }

}