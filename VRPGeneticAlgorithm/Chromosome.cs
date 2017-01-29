using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRPGeneticAlgorithm
{
    public class Chromosome
    {
        public int Length { get { return genes.Length; } }

        private string[] genes; // 0 1 2 30 0 11 0 23 4 oznacza następujące trasy dla kolejnych pojazdów:
                                // pojazd 0: Depot -> Depot (czyli nie rusza się wgl)
                                // pojazd 1: Depot -> 1 -> 2 -> 30 -> Depot
                                // pojazd 2: Depot -> 11 -> Depot
                                // pojazd 3: Depot -> 23 -> 4 -> Depot
        public int Evaluation = 0;

        public Chromosome(int numberOfCars, int numberOfCustomers)
        {
            List<string> elementsToPickUp = new List<string>(); //stąd będziemy pobierać losowo elementy 
                                                                //i wsadzać je na kolejne miejsca w customersOrder
            for (int i = 0; i < numberOfCars - 1; i++)  //zer w rozwiązaniu jest zawsze o 1 mniej niż samochodów
            {        
                elementsToPickUp.Add("0");
            }
            for (int i = 0; i < numberOfCustomers; i++)
            {
                elementsToPickUp.Add((i + 1) + "");
            }
            genes = new string[numberOfCars + numberOfCustomers - 1];
            int j = 0;
            while (elementsToPickUp.Count > 0)
            {
                int index = RandomIntGenerator.Generate(0, elementsToPickUp.Count);
                genes[j] = elementsToPickUp[index];
                elementsToPickUp.RemoveAt(index);
                j++;
            }
        }

        private Chromosome(string[] genes) {
            this.genes = genes;
        }

        public void Evaluate(int[][] adjacencyMatrix)
        {
            Evaluation = 0;
            //najpierw pierwszy wierzchołek - przed nim jest jeszcze depot (domyślnie) - bierzemy odległość między nimi
            int firstVertex = Int32.Parse(genes[0]);
            Evaluation += adjacencyMatrix[0][firstVertex];
            //następnie iterujemy przez całe rozwiązanie - bierzemy odległości międyz lewym a prawym wierzchołkiem (czyli od 0 do genes.Length - 1)
            for (int i = 0; i < genes.Length - 1; i++)
            {
                int leftVertex = Int32.Parse(genes[i]);
                int rightVertex = Int32.Parse(genes[i + 1]);
                Evaluation += adjacencyMatrix[leftVertex][rightVertex];
            }
            //na końcu ostatni wierzchołek - za nim jest jeszcze depot (domyślnie) - bierzemy odległość między nimi
            int lastVertex = Int32.Parse(genes[genes.Length - 1]);
            Evaluation += adjacencyMatrix[lastVertex][0];
        }

        public Chromosome CrossWith(Chromosome secondChromosome, int cutStartingIndex, int cutEndingIndex)
        {
            string[] childGenes = new string[Length];
            List<string> clientsCopiedFromThisChromosome = new List<string>();
            int baseCounter = 0;
            foreach (string gene in genes) {
                if (gene.Equals("0")) {
                    baseCounter++;
                }
            }
            int c = 0; //indeks interujący po dziecku 

            //najpierw przepisujemy geny z tego chromosomu na dziecko
            //a także zapamiętujemy je
            for (int t = cutStartingIndex; t <= cutEndingIndex; t++) //t - indeks iterujący po pierwszym (czyli tym) chromosomie
            { 
                c = t;
                childGenes[c] = this.genes[t];
                if (this.genes[t] != "0")
                {
                    clientsCopiedFromThisChromosome.Add(this.genes[t]);
                }
                else {
                    baseCounter--;
                }
            }

            c++;    //przesuwamy na pierwyszy element który jeszcze nie został przepisany zaraz za tymi z tego chromosomu
            int s = c; //indeks interujący po drugim chromosomie
            //dopuki nie dojdziemy do ostatniego przepisanego elementu z tego chromosomu
            bool firstTime = true;
            while (s != cutEndingIndex + 1 || firstTime) {
                firstTime = false;
                if (s >= Length) //jeśli s poza chromosomem
                {
                    s = 0;
                }
                if (c >= Length)    //jesli c poza chromosomem
                {
                    c = 0;
                }
                string candidateToAdd = secondChromosome.genes[s];
                //jeśli aktualnie kandydatem do dodania jest klient i nie jest on jeszcze zawarty w rozwiązaniu to możemy go dodać
                if (candidateToAdd != "0" && !clientsCopiedFromThisChromosome.Contains(candidateToAdd)) {
                    childGenes[c] = candidateToAdd;
                    c++;
                }
                //jeśli aktualnie kandydatem do dodania jest baza i można jeszcze jakąś dodać
                else if (candidateToAdd == "0" && baseCounter > 0) {
                    baseCounter--;
                    childGenes[c] = candidateToAdd;
                    c++;
                } 
                s++;
            }
            Chromosome child = new Chromosome(childGenes);
            return child;
        }

        public Chromosome Mutate()
        {
            //mutacja przez inwersję

            //najpierw przepisujemy wszystkie geny
            string[] mutatedGenes = new string[Length];
            for (int i = 0; i < Length; i++) {
                mutatedGenes[i] = genes[i];
            }

            int cutStartingIndex = RandomIntGenerator.Generate(0, Length / 2);
            int cutEndingIndex = RandomIntGenerator.Generate(Length / 2, Length);
            //Console.WriteLine("Chromosome has following indices: {0}, {1}", cutStartingIndex, cutEndingIndex);
            string[] reversedPart = new string[cutEndingIndex - cutStartingIndex + 1];

            //sczytujemy genes i zapisujemy do reversedPart w odwróconej kolejności
            int j = cutStartingIndex;
            for (int i = reversedPart.Length - 1; i >= 0; i--) {
                reversedPart[i] = genes[j];
                j++;
            }

            //zapisujemy z powrotem z reversedPart do genes
            j = cutStartingIndex;
            foreach (string reversedElement in reversedPart) {
                mutatedGenes[j] = reversedElement;
                j++;
            }

            Chromosome mutant = new Chromosome(mutatedGenes);
            return mutant;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Chromosome: ");
            for (int i = 0; i < genes.Length; i++)
            {
                sb.Append(genes[i]);
                if (i < genes.Length - 1)
                {
                    sb.Append("-");
                }
            }
            sb.Append(" Evaluation: " + Evaluation);
            return sb.ToString();
        }
    }
}
