using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

    public class GA
    {
        Genome[] m_Genomes;
        public Genome[] Genomes { get => m_Genomes; }

        double m_CrossoverRate;
        double m_MutationRate;
        int m_PopulationSize;

        int m_ChromosomeLength;

        int m_GeneLength;
        public GameObject roberto;

        double m_BestFitnessScore;
        double m_TotalFitnessScore;
        int m_FittestGenome;
        public int FittestGenome { get => m_FittestGenome; }

        int m_Generation;
        public int Generation { get => m_Generation; }

        Map m_Brain;
        public Map Brain { get => m_Brain; set => m_Brain = value; }

        bool m_IsRunning;
        public bool IsRunning { get => m_IsRunning; set => m_IsRunning = value; }

        private System.Random m_Random;

        public GA(double crossoverRate, double mutationRate, int populationSize, int chromosomeLength, int geneLength)
        {
            m_Random = new System.Random((int)Time.deltaTime);
            m_Brain = GameObject.Find("Mapa").GetComponent<Map>();

            m_CrossoverRate = crossoverRate;
            m_MutationRate = mutationRate;
            m_PopulationSize = populationSize;
            m_ChromosomeLength = chromosomeLength;
            m_GeneLength = geneLength;

            m_IsRunning = false;

            CreateInitialPopulation();
        }

        public void CreateInitialPopulation()
        {
            m_Genomes = new Genome[m_PopulationSize];
            for (int i = 0; i < m_PopulationSize; ++i)
            {
                m_Genomes[i] = new Genome(m_ChromosomeLength);

            }

            m_BestFitnessScore = 0.0;
            m_TotalFitnessScore = 0.0;
            m_FittestGenome = 0;

            m_Generation = 0;
        }


        private void UpdateFitnessScores()
        {
            m_FittestGenome = 0;
            m_BestFitnessScore = 0.0f;
            m_TotalFitnessScore = 0.0f;



            int[] directions;

            for (int i = 0; i < m_PopulationSize; ++i)
            {
                directions = Decode(m_Genomes[i].Bits);

                m_Genomes[i].Fitness = m_Brain.TestRoute(directions); //retorna uma fitness entre 0 e 1

                m_TotalFitnessScore += m_Genomes[i].Fitness;
                if (m_Genomes[i].Fitness > m_BestFitnessScore)
                {
                    m_BestFitnessScore = m_Genomes[i].Fitness;
                    m_FittestGenome = i;


                    if (m_Genomes[i].Fitness >= 1.0)
                    {
                        m_IsRunning = false;
                    }
                }

            }
        }

        public int[] Decode(bool[] bits)
        {
            List<int> directions = new List<int>();
            bool[] gene = new bool[m_GeneLength];

            for (int currentGene = 0; currentGene < bits.Length; currentGene += m_GeneLength)
            {
                for (int bit = 0; bit < m_GeneLength; ++bit)
                {
                    gene[bit] = bits[currentGene + bit];

                }
                directions.Add(BinToInt(gene));
            }
            return directions.ToArray();
        }

        public int BinToInt(bool[] bits)
        {
            int value = 0;
            int multiplier = 1;
            for (int i = bits.Length; i > 0; --i)
            {
                value += bits[i-1] ? multiplier : 0;
                multiplier *= 2;
            }
            return value;
        }



        public Genome RouletteWheelSelection()
        {
            double fitnessSlice = m_Random.NextDouble() * m_TotalFitnessScore;
            double fitnessTotal = 0.0;
            int selectedGenome = 0;

            for (int i = 0; i < m_PopulationSize; ++i)
            {
                fitnessTotal += m_Genomes[i].Fitness;
                if (fitnessTotal > fitnessSlice)
                {
                    selectedGenome = i;
                    break;
                }
            }

            return m_Genomes[selectedGenome];
        }

        public void Crossover(bool[] parent1, bool[] parent2, bool[] child1, bool[] child2)
        {
            if (m_Random.NextDouble() > m_CrossoverRate || parent1 == parent2)
            {
                for (int i = 0; i < parent1.Length; ++i)
                {
                    child1[i] = parent1[i];
                    child2[i] = parent2[i];
                }
                return;
            }

            int crossoverPoint = m_Random.Next(0, m_ChromosomeLength - 1);

            for (int i = 0; i < crossoverPoint; ++i)
            {
                child1[i] = parent1[i];
                child2[i] = parent2[i];
            }

            for (int i = crossoverPoint; i < parent1.Length; ++i)
            {
                child1[i] = parent1[i];
                child2[i] = parent2[i];
            }
        }

        public void Mutate(bool[] bits)
        {
            for (int currentBit = 0; currentBit < bits.Length; currentBit++)
            {
                if (m_Random.NextDouble() < m_MutationRate)
                {
                    bits[currentBit] = !bits[currentBit];
                }
            }
        }

        public void Epoch()
        {

            UpdateFitnessScores();

            int populationCurrentSize = 0;
            Genome[] newGenomes = new Genome[m_PopulationSize];

            while (populationCurrentSize < m_PopulationSize)
            {
                Genome parent1 = RouletteWheelSelection();
                Genome parent2 = RouletteWheelSelection();

                Genome child1 = new Genome(parent1.Size);
                Genome child2 = new Genome(parent1.Size);
                Crossover(parent1.Bits, parent2.Bits, child1.Bits, child2.Bits);

                Mutate(child1.Bits);
                Mutate(child2.Bits);

                newGenomes[populationCurrentSize] = child1;
                newGenomes[populationCurrentSize + 1] = child2;

                populationCurrentSize += 2;
            }

            for (int i = 0; i < m_Genomes.Length; i++)
            {
                m_Genomes[i] = newGenomes[i];
            }
            ++m_Generation;
        }

    }