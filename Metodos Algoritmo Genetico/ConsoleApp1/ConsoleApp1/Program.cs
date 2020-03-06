using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
 
    public class Genome<T>
    {
        bool[] m_Bits;
        public bool[] Bits
        {
            get { return m_Bits; }
            set { m_Bits = value; }
        }

        double m_Fitness;
        public double Fitness
        {
            get { return m_Fitness; }
            set { m_Fitness = value; }
        }

        public int Size { get => m_Bits.Length; }

        public Genome(int size)
        {
            Random random = new Random((int)DateTime.UtcNow.Ticks);

            m_Bits = new bool[size];
            for (int i = 0; i < size; ++i)
            {
                m_Bits[i] = random.NextDouble() < 0.5;
            }
            m_Fitness = 0.0;
        }
    }

    public class Methods<T> {

        int m_PopulationSize = 120;
        int Genes = 50;
        Genome<T>[] m_Genomes;
        Random m_Random;
        int m_CrossoverRate = 0;
        int m_ChromosomeLength = 0;
        int m_MutationRate = 0;

        public Methods() {

            m_Genomes = new Genome<T>[m_PopulationSize];
            for (int i = 0; i < m_PopulationSize; i++)
            {
                m_Genomes[i] = new Genome<T>(Genes);
            }


            m_Random = new Random((int)DateTime.UtcNow.Ticks);
        }


        // Elitismo
        public Genome<T>[] ElitsmSelection()
        {
            //Manter os melhores de cada geração 


            int index = 0;
           
            double BestFitness = 0;
            Genome<T>[] SelectedPeople = new Genome<T>[2];
            while (index < 2)
            { 
                for (int i = 0; i < m_PopulationSize; ++i)
                {
            
                    if ( m_Genomes[i].Fitness > BestFitness)
                    {
                        BestFitness = m_Genomes[i].Fitness;
                       
                        SelectedPeople.SetValue(m_Genomes[i], index);
                        m_Genomes[i].Fitness = 0.0f;
                        BestFitness = 0.0f;
                        index++;
                    }

                
                }
            }
            return SelectedPeople;
        }

        // Estado Estacionario
        public Genome<T>[] SteadyStateSelection()
        {
            //Matar pessoas aleatoriamente
            int SelectedGuy = 0;
            int index = 0;
            List<Genome<T>> SelectedPeopleToDeath = new List<Genome<T>>();
            List<Genome<T>> Genomes = new List<Genome<T>>();

            while (index < 30)
            {
                SelectedGuy = m_Random.Next(0, m_PopulationSize);
                index++;

                SelectedPeopleToDeath.Add(m_Genomes[SelectedGuy]);
            }
           for ( int i = 0; i < m_PopulationSize; i++)
            {

                Genomes.Add(m_Genomes[i]);
            }
            for (int i = 0; i <Genomes.Count; i++)
            {

                if (SelectedPeopleToDeath.Contains(Genomes[i]))
                {
                    Genomes.Remove(Genomes[i]);
                }
            }





            return Genomes.ToArray();
        }
        // Roleta

        public Genome<T> RouletteWheelSelection()

        {
            double m_TotalFitnessScore = 0;
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


        // SUS 

        // Torneio
        public Genome<T> TournamentSelection()

        {
            int SelectedGuy = 0;
            int index = 0;
            double BestFitness = 0;


            Genome<T>[] SelectedPeople = new Genome<T>[1];
            Genome<T> TheBest = null;
            while (index < 10)
            {


                SelectedGuy = m_Random.Next(0, m_PopulationSize);

                SelectedPeople.SetValue(SelectedGuy, index);

            }
            for (int i = 0; i < SelectedPeople.Length; ++i)
            {
                if (SelectedPeople[i].Fitness > BestFitness)
                {
                    BestFitness = SelectedPeople[i].Fitness;

                    TheBest = SelectedPeople[i] ;
                }

            }
           

            return TheBest;
        }

        // Crossover - Ponto Unico 

        public void Crossover1Point(bool[] parent1, bool[] parent2, bool[] child1, bool[] child2)
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
        // Dois Pontos
        public void Crossover2Points(bool[] parent1, bool[] parent2, bool[] child1, bool[] child2)
        {


            int crossover1Point = m_Random.Next(0, m_ChromosomeLength - 1);
            int crossover2Point = m_Random.Next(0, m_ChromosomeLength - 1);

            for (int i = crossover1Point; i < crossover2Point; ++i)
            {
                child1[i] = parent1[i];
                child2[i] = parent2[i];
            }

            for (int i = 0; i < parent1.Length; ++i)
            {
                if (i < crossover1Point || i > crossover2Point) { 
                child1[i] = parent1[i];
                child2[i] = parent2[i];
                }
            }
        }

        // Multi Point

        // Aritmético Único
        public void AritimeticoUnico(double[] parent1, double[] parent2, double[] child1, double[] child2)
        {

            
            double crossoverValue = m_Random.NextDouble();
            int SelectedGuy = m_Random.Next(0, m_PopulationSize);

            child1[SelectedGuy] = crossoverValue * parent2[SelectedGuy] + (1 - crossoverValue) * parent1[SelectedGuy];
            child2[SelectedGuy] = crossoverValue * parent1[SelectedGuy] + (1 - crossoverValue) * parent2[SelectedGuy];
            
        }

        // Aritmético Simples

        public void AritimeticoSimples(double[] parent1, double[] parent2, double[] child1, double[] child2)
        {

        
            double crossoverValue = m_Random.NextDouble();
            int SelectedGuy = m_Random.Next(0, m_PopulationSize);

            for (int i = SelectedGuy; i < m_PopulationSize; i++) { 
            child1[i] = crossoverValue * parent2[i] + (1 - crossoverValue) * parent1[i];
            child2[i] = crossoverValue * parent1[i] + (1 - crossoverValue) * parent2[i];
            }
        }

        // Aritmético Completo
        public void AritimeticoCompleto(double[] parent1, double[] parent2, double[] child1, double[] child2)
        {

         
            double crossoverValue = m_Random.NextDouble();
      

            for (int i = 0; i < m_PopulationSize; i++)
            {
                child1[i] = crossoverValue * parent2[i] + (1 - crossoverValue) * parent1[i];
                child2[i] = crossoverValue * parent1[i] + (1 - crossoverValue) * parent2[i];
            }
        }

        // PMX

        // Order-Based OBX

        // Position Based
        public void PositionBased(double[] parent1, double[] parent2, double[] child1, double[] child2)
        {
            int GeneRandom1 = 0;
            int index = 0;
            List<int> Position = new List<int>();

            while (index < 10)
            {
                GeneRandom1 = m_Random.Next(0, m_ChromosomeLength - 1);

                Position.Add((int)parent1[GeneRandom1]);
                index++;
            }
            for (int i = 0; i < Position.Count; i++)
            {
                if (Position.Contains(i))
                {
                    child1[i] = parent1[i];
                    child2[i] = parent2[i];

                }
                         
                    child2[i] = parent1[i];
                    child1[i] = parent2[i];
                
            }

        }

        // Mutação - Inversão de bit

        public void MutateInversao(bool[] bits)
        {
            
            for (int currentBit = 0; currentBit < bits.Length; currentBit++)
            {
                if (m_Random.NextDouble() < m_MutationRate)
                {
                    bits[currentBit] = !bits[currentBit];
                }
            }
        }

        // Troca
        public void MutateTrade(bool[] bits)
        {
            List<int> Position = new List<int>();
            
            int index = 0;

            if (m_Random.NextDouble() < m_MutationRate)
                {
                while (index < 2)
                {
                    int GeneRandom1 = m_Random.Next(0, m_PopulationSize);
                    Position.Add(GeneRandom1);
                    index++;

                }
                int tempPos = 0;


                tempPos = Position[0];
                Position[0] = Position[1];
                Position[1] = tempPos;


            }
            
        }

        // Embaralhamento
        public void MutateSM(bool[] bits)
        {
            List<int> Position = new List<int>();

            int index = 0;

            

            if (m_Random.NextDouble() < m_MutationRate)
            {
             
                    int n = bits.Length;
                    for (int i = 0; i < (n - 1); i++)
                    {
                   
                        int r = i + m_Random.Next(n - i);
                        bool t = bits[r];
                        bits[r] = bits[i];
                        bits[i] = t;
                    }
            }

        }

        // Deslocamento

        // Inserção

        // Inversão
        public void MutateInverso(bool[] bits)
        {
           int Position1 = m_Random.Next(0, m_PopulationSize);
           int Position2 = m_Random.Next(0, m_PopulationSize);

            if (m_Random.NextDouble() < m_MutationRate)
            {

                Array.Reverse(bits, Position1, Position2);
            
            }

        }
        // Inversão Deslocada




    }

   

}
