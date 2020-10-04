using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmorePostaggioMatchBrain
{
    public static class CoppieUtility
    {
        public static List<Coppia> VariMatch (CSVElement element)
        {
            List<Coppia> temporanea = new List<Coppia>();
            
            foreach (CSVElement el in CSVUtility.csvList)
            {
                temporanea.Add(new Coppia
                {
                    PrimoNome = element.Nome,
                    SecondoNome = el.Nome,
                    Punteggio = CSVUtility.ConfrontaElementi(element, el)
                });
            }

            List<Coppia> temporaneaSorted = temporanea.OrderByDescending(s => s.Punteggio).ToList();

            return temporaneaSorted;

        }

        public static int PointDifference(double first, double second)
        {
            double risultato = Math.Abs(first - second);

            switch (risultato)
            {
                case 0:
                    return 20;
                case 1:
                    return 10;
            }

            return 0;
        }


    }
}
