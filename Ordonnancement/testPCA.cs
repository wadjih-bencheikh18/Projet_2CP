using System;
using System.Collections.Generic;
namespace Ordonnancement
{
    class testPCA
    {
        static void Main(string[] args)
        {
            Processus Pro;
            PCA programme = new();
            int r1, r2, r3;
            for (int i = 0; i < 5; i++)
            {
                string val; // lecture des parametres des processus (ID, BT, AT)
                val = Console.ReadLine();
                r1 = Convert.ToInt32(val);
                val = Console.ReadLine();
                r2 = Convert.ToInt32(val);
                val = Console.ReadLine();
                r3 = Convert.ToInt32(val);
                Pro = new(r1, r2, r3);
                programme.Push(Pro);
                Console.WriteLine("===================");
            }
            programme.Faire();
        }
    }
}

