using System;

namespace Ordonnancement
{
    class Test
    {
        static void Main(string[] args)
        {
            Processus Pro;
            PSP prgm = new();
            Random r1, r2, r3, r4;
            for (int i = 0; i < 20; i++)
            {
                r1 = new Random();
                r2 = new Random();
                r3 = new Random();
                r4 = new Random();
                Pro = new(r1.Next(0, 100), r2.Next(0, 100), r3.Next(1, 100), r4.Next(0, 10));
                prgm.Push(Pro);
            }
            prgm.Faire();
            Console.ReadLine();
        }
    }
}