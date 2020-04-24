
using System.Collections.Generic;
using UnityEngine;

namespace Sudoku
{

    public class Genome<T> : MonoBehaviour
    {
        int[] m_Bits;
        public int[] Bits
        {
            get { return m_Bits; }
            set { m_Bits = value; }
        }

        public double m_Fitness = 0.0;


        public int Size { get => m_Bits.Length; }

        List<int> numbers;

        public Genome(int size)
        {
            m_Bits = new int[size];


            for (int i = 0; i < size; i += 9)
            {
                numbers = SetListNumbers();

                for (int j = 0; j < 9; ++j)
                {
                    int randomNum = Random.Range(0, numbers.Count);
                    m_Bits[i + j] = numbers[randomNum];

                    numbers.RemoveAt(randomNum);
                }
            }

            List<int> SetListNumbers()
            {
                List<int> list = new List<int>();
                for (int i = 1; i < 10; i++)
                {
                    list.Add(i);
                }

                return list;
            }
        }
    }
    public class Methods<T> : MonoBehaviour
    {

        int m_PopulationSize;
        Genome<T>[] m_Genomes;
        public Genome<T>[] Genomes { get => m_Genomes; set => m_Genomes = value; }
        double m_CrossoverRate = 0.7;
        int m_ChromosomeLength;
        double m_MutationRate = 0.5;

        public Methods(int popSize, int chromosomeSize)
        {

            m_PopulationSize = popSize;
            m_ChromosomeLength = chromosomeSize;

            m_Genomes = new Genome<T>[m_PopulationSize];
            for (int i = 0; i < m_PopulationSize; i++)
            {
                m_Genomes[i] = new Genome<T>(m_ChromosomeLength);
            }

        }







        public Genome<T> TournamentSelection()

        {
            int index = 0;
            int SelectedGuy = 0;

            double BestFitness = 0;


            List<Genome<T>> SelectedPeople = new List<Genome<T>>();
            Genome<T> TheBest = null;
            while (index < 10)
            {

                SelectedGuy = Random.Range(0, m_PopulationSize);

                SelectedPeople.Add(m_Genomes[SelectedGuy]);
                index++;

            }
            for (int i = 0; i < SelectedPeople.Count; ++i)
            {
                if (SelectedPeople[i].m_Fitness > BestFitness)
                {
                    BestFitness = SelectedPeople[i].m_Fitness;

                    TheBest = SelectedPeople[i];
                }

            }


            return TheBest;
        }


        // Dois Pontos
        public void Crossover2Points(int[] parent1, int[] parent2, int[] child1, int[] child2)
        {


            int crossover1Point = Random.Range(0, m_ChromosomeLength - 1);
            int crossover2Point = Random.Range(0, m_ChromosomeLength - 1);

            for (int i = crossover1Point; i < crossover2Point; ++i)
            {
                child1[i] = parent1[i];
                child2[i] = parent2[i];
            }

            for (int i = 0; i < parent1.Length; ++i)
            {
                if (i < crossover1Point || i > crossover2Point)
                {
                    child1[i] = parent2[i];
                    child2[i] = parent1[i];
                }
            }
        }


        // Swap
        public void MutateTrade(int[] bits)
        {
            int chance = Random.Range(0, 11);

            int firstAllele = 0;
            int secoundAllele = 0;


            if (chance >= 0 && chance < 6)
            {
                while (firstAllele == secoundAllele)
                {
                    firstAllele = Random.Range(0, bits.Length);
                    secoundAllele = Random.Range(0, bits.Length);
                }

                int temp = bits[firstAllele];
                bits[firstAllele] = bits[secoundAllele];
                bits[secoundAllele] = temp;
            }


        }


    }


}
