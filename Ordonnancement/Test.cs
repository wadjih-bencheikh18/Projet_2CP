using System;
using System.Collections.Generic;

namespace Ordonnancement
{
    class Test
    {
        static void Main(string[] args)
        {
            /*Processus Pro;
            RoundRobin prgm = new RoundRobin(10);
            Random r1, r2, r3, r4;
            for (int i = 0; i < 3; i++)
            {
                r1 = new Random();
                r2 = new Random();
                r3 = new Random();
                r4 = new Random();
                Pro = new(r1.Next(0, 15), r2.Next(0, 15), r3.Next(1, 15), r4.Next(0, 15));
                Console.WriteLine("id: " + Pro.id + "  arriv: " + Pro.tempsArriv + "  duree: " + Pro.duree);
                prgm.Push(Pro);
            }
            //prgm.Faire();
            //prgm.Affichage();
            int[] l = { 0, 0, 0 };
            Console.WriteLine("***********************" + prgm.Executer(0, 20, l));
            prgm.Affichage();
            Console.WriteLine("***********************" + prgm.Executer(250, 400, l));
            prgm.Affichage();*/
            //*******************
            //   MultiLvl Test
            //*******************
            ProcessusNiveau Pro;
            MultiNiveau prgm = new(3);
            Random r1, r2, r3, r4;
            for (int i = 0; i < 3; i++)
            {
                r1 = new Random();
                r2 = new Random();
                r3 = new Random();
                r4 = new Random();
                Pro = new(r1.Next(0, 15), r2.Next(0, 15), r3.Next(1, 15), r4.Next(0, 15), i);
                Console.WriteLine("id: " + Pro.id + "  arriv: " + Pro.tempsArriv + "  duree: " + Pro.duree + " niveau: " + Pro.niveau);
                prgm.Push(Pro);
            }
            prgm.Affichage();
            Console.WriteLine(" ");
            Console.WriteLine("***********************");
            prgm.Executer();
            Console.WriteLine("***********************");
            prgm.Affichage();
        }
    }

}