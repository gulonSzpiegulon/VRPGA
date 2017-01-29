using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRPGeneticAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            VRP problem = new VRP(3, 8);
            problem.SetDistancesBetweenVertexAndOtherVertices(new int[] { 10, 20, 30, 40, 50, 60, 70, 80 });
            problem.SetDistancesBetweenVertexAndOtherVertices(new int[] { 90, 80, 45, 35, 20, 20, 25 });
            problem.SetDistancesBetweenVertexAndOtherVertices(new int[] { 45, 45, 45, 45, 50, 60 });
            problem.SetDistancesBetweenVertexAndOtherVertices(new int[] { 10, 10, 20, 90, 80 });
            problem.SetDistancesBetweenVertexAndOtherVertices(new int[] { 90, 30, 5, 35 });
            problem.SetDistancesBetweenVertexAndOtherVertices(new int[] { 5, 30, 60 });
            problem.SetDistancesBetweenVertexAndOtherVertices(new int[] { 80, 100 });
            problem.SetDistancesBetweenVertexAndOtherVertices(new int[] { 5 });

            GeneticAlgorithm ga = new GeneticAlgorithm();
            int[] bestSolution = ga.GetBestSolution(problem, 100, 6, 10);
            if (bestSolution != null) {
                Console.Write("Best solution: ");
                foreach (var gene in bestSolution) {
                    Console.Write("{0}", gene);
                }
                Console.WriteLine();
            }
            Console.ReadLine();
        }
    }
}
