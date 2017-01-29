using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRPGeneticAlgorithm
{
    public class VRP
    {
        public int NumberOfCars { get; }
        public int NumberOfCustomers { get; }
        public int[][] AdjacencyMatrix { get; }

        private int vertexSetUpLastly = 0;  //to jest potrzebne tylko do wypełniania adjacencyMatrix w odpowiedni sposób
                                            //rozpoczynamy uzupełnianie od wierzchołka 0 czyli depot

        public VRP(int numberOfCars, int numberOfCustomers)
        {
            NumberOfCars = numberOfCars;   // musi być większy od 0 z resztą numberOfCustomers też
            NumberOfCustomers = numberOfCustomers;
            AdjacencyMatrix = new int[NumberOfCustomers + 1][];
            for (int firstVertex = 0; firstVertex < NumberOfCustomers + 1; firstVertex++)
            {
                AdjacencyMatrix[firstVertex] = new int[NumberOfCustomers + 1];
                for (int secondVertex = 0; secondVertex < NumberOfCustomers + 1; secondVertex++)
                {
                    AdjacencyMatrix[firstVertex][secondVertex] = 0;
                }
            }
        }

        public bool SetDistancesBetweenVertexAndOtherVertices(int[] distances)
        {
            if (distances == null || (distances != null && distances.Length != AdjacencyMatrix.Length - vertexSetUpLastly - 1) )
            {
                return false;   //po pierwsze nie wolno wrzucić nulla, lista musi być najmniej pusta 
                                //(choć żeby miało sens lista powinna być co najmniej dł. 1)
                                //po drugie tą funkcję należy wywoływać kilka razy po sobie od adjacencyMatrix zapełnionej zerami 
                                //po wypełnioną odległościami - żeby nie przekazywać za każdym razem odległości które podawaliśmy już wcześniej
                                //przy każdym wywołaniu tej funkcji lista musi być krótsza o 1 element poczynając od dł. AdjacencyMatrix.Length - 1
            }
            for (int i = 0; i < distances.Length; i++)
            {
                AdjacencyMatrix[vertexSetUpLastly][vertexSetUpLastly + 1 + i] = distances[i];   //wypełniamy rząd vertexSetUpLastly
                AdjacencyMatrix[vertexSetUpLastly + 1 + i][vertexSetUpLastly] = distances[i];   //wypełniamy kolumne vertexSetUpLastly 
                                                                                                //w obudwu przypadkach omijamy główną przekątną 
                                                                                                //i odległości dodane przy poprzednich 
                                                                                                //wywołaniach funkcji
            }
            vertexSetUpLastly++;
            return true;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("VRP:" + Environment.NewLine);
            sb.Append("numberOfCars:\t\t" + NumberOfCars + Environment.NewLine);
            sb.Append("numberOfClients:\t" + NumberOfCustomers + Environment.NewLine);
            sb.Append("adjacencyMatrix:" + Environment.NewLine);
            for (int firstVertex = 0; firstVertex < AdjacencyMatrix.Length; firstVertex++)
            {
                for (int secondVertex = 0; secondVertex < AdjacencyMatrix.Length; secondVertex++)
                {
                    sb.Append(AdjacencyMatrix[firstVertex][secondVertex] + "\t");
                }
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }
    }
}
