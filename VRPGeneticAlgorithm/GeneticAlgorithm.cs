using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRPGeneticAlgorithm
{
    public class GeneticAlgorithm
    {
        private VRP problem;
        private Population population;

        public int[] GetBestSolution(VRP problem, int numberOfIterations, int populationSize, int mutationProbability)
        {
            Console.WriteLine("GeneticAlgorithm's GetBestSolution method started:");
            Console.WriteLine();

            Console.WriteLine("Our problem:");
            this.problem = problem;
            Console.WriteLine(problem);

            population = new Population(populationSize, problem); //TODO: population size musi być parzysta
            for (int i = 0; i < numberOfIterations; i++) {
                population.Evaluate(problem.AdjacencyMatrix);
                Console.WriteLine("Best evaluation: {0}; AverageEvaluation: {1}", population.GetBestEvaluation(), population.GetAverageEvaluation());
                population.Select();
                population.Cross();
                population.Mutate(mutationProbability);
            }
            
            

            return null;    // TODO: zmienić - to tylko tak żeby błędu kompilacji nie było
        }
    }
}
