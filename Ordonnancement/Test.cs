using System;

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
            int nbNiveau = 3;
            Random r1, r2, r3, r4;
            Niveau[] niveaux = new Niveau[nbNiveau];
            for (int i = 0; i < nbNiveau; i++)
            {
                r1 = new Random();
                Console.WriteLine(i + "=>" + r1.Next(0, 2));
                niveaux[i] = new(r1.Next(0, 2));
            }
            MultiNiveau prgm = new(nbNiveau, niveaux);
            for (int i = 0; i < 4; i++)
            {
                r1 = new Random();
                r2 = new Random();
                r3 = new Random();
                r4 = new Random();
                Pro = new(i, r2.Next(0, 15), r3.Next(1, 15), r4.Next(0, 15), r1.Next(0, 3));
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