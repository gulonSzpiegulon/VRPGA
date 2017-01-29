using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRPGeneticAlgorithm
{
    public class RandomIntGenerator
    {   // aby generować w porządny sposób liczby pseudolosowe najlepiej jest tylko raz zainicjalizować zmienną random,
        //a potem już korzystać z next, next, next... ta klasa to zapewnia
        private static Random random;

        public static int Generate(int lowerBound, int upperBound)
        {
            if (random == null)
            {
                random = new Random();
            }
            return random.Next(lowerBound, upperBound);
        }
    }
}
