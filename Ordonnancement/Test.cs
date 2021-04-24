using System;
using System.Collections.Generic;

namespace Ordonnancement
{
    class Test
    {
        static void Main(string[] args)
        {
            Processus Pro;
            PSP prgm = new ();
            Random r1, r2, r3, r4;
            for (int i = 0; i < 6; i++)
            {
                r1 = new Random();
                r2 = new Random();
                r3 = new Random();
                r4 = new Random();
                Pro = new(r1.Next(0, 10), r2.Next(0, 10), r3.Next(1, 10), r4.Next(0, 5));
                prgm.Push(Pro);
            }
            prgm.Executer(0,prgm.Somme());
            prgm.Affichage();
            Console.ReadLine();
        }
    }
}