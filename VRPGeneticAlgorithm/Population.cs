using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRPGeneticAlgorithm
{
    public class Population
    {
        private List<Chromosome> chromosomes;

        public Population(List<Chromosome> chromosomes)
        {
            this.chromosomes = chromosomes;
        }

        public Population(int populationSize, VRP problem)
        {
            chromosomes = new List<Chromosome>();
            for (int i = 0; i < populationSize; i++)
            {
                chromosomes.Add(new Chromosome(problem.NumberOfCars, problem.NumberOfCustomers));
            }
            ////Console.WriteLine("Creating initial population...");
            ////Console.WriteLine(this);
        }

        public void Evaluate(int[][] adjacencyMatrix)
        {
            foreach (Chromosome chromosome in chromosomes)
            {
                chromosome.Evaluate(adjacencyMatrix);
            }
            //Console.WriteLine("Evaluating population...");
            //Console.WriteLine(this);
        }

        public int GetBestEvaluation() {
            int bestEvaluation = chromosomes[0].Evaluation;
            for (int i = 1; i < chromosomes.Count; i++) {
                if (chromosomes[i].Evaluation < bestEvaluation) {
                    bestEvaluation = chromosomes[i].Evaluation;
                }
            }
            return bestEvaluation;
        }

        public float GetAverageEvaluation() {
            int sumOfEvaluations = 0;
            foreach (Chromosome chromosome in chromosomes) {
                sumOfEvaluations += chromosome.Evaluation;
            }
            return sumOfEvaluations / chromosomes.Count;
        }

        public void Select()
        {
            //ta funkcja tworzy nową populację - populację rodziców - na bezie starej (stara znika)
            //wykorzystuje ranking
            //tasuje populację po selekcji
            List<Chromosome> parents = new List<Chromosome>();
            List<Chromosome> sortedChromosomes = chromosomes.OrderBy(o => o.Evaluation).ToList();   //najpierw sortujemy chromosomes rosnąco
            //i przepisujemy pierwszą połowę posortowanej listy na listę parents dwa razy
            for (int i = 0; i < sortedChromosomes.Count / 2; i++)
            {
                parents.Add(sortedChromosomes[i]);
                parents.Add(sortedChromosomes[i]);
            }
            parents = parents.OrderBy(a => Guid.NewGuid()).ToList();    //następnie tasujemy osobników
            chromosomes = parents;
            //Console.WriteLine("Selecting parents...");
            //Console.WriteLine(this);
        }

        public void Cross() {
            //to jest OX crossover
            //Console.WriteLine("Crossing over parents...");
            int chromsomeLength = chromosomes[0].Length;
            for (int i = 0; i < chromosomes.Count; i += 2) {
                int cutStartingIndex = RandomIntGenerator.Generate(0, chromsomeLength / 2);
                int cutEndingIndex = RandomIntGenerator.Generate(chromsomeLength / 2, chromsomeLength);
                Chromosome firstChild = chromosomes[i].CrossWith(chromosomes[i + 1], cutStartingIndex, cutEndingIndex);
                Chromosome secondChild = chromosomes[i + 1].CrossWith(chromosomes[i], cutStartingIndex, cutEndingIndex);
                chromosomes[i] = firstChild;
                chromosomes[i + 1] = secondChild;
            }
            //Console.WriteLine(this);
        }

        public void Mutate(int mutationProbability)
        {
            //Console.WriteLine("Mutating children...");
            for (int i = 0; i < chromosomes.Count; i++)
            {
                //dla każdego chromosomu losoujemy czy ma się zmutować czy nie
                //przykładowo prawdopodobieństwo równe jest 10% to generujemy jakąś liczbęz przedziału od 0 do 99
                //jeśli jest ona w przedziale 0 - 9 to trafiliśmy (a mogliśmy to trafić z prawdopodobieństwem 10% w ten przedział)
                if (RandomIntGenerator.Generate(0, 100) < mutationProbability)
                {
                    //Console.WriteLine("Chromosom {0} is being mutated", i);
                    chromosomes[i] = chromosomes[i].Mutate();
                }
            }
            //Console.WriteLine(this);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Population:" + Environment.NewLine);
            foreach (Chromosome chromosome in chromosomes)
            {
                sb.Append(chromosome + Environment.NewLine);
            }
            return sb.ToString();
        }
    }
}
