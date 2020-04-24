using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sudoku
{

    public class Sudoku : MonoBehaviour
    {

        public int Offset = 69;


        public int IPosX = 0;


        public int IPosY = 0;

        [SerializeField]
        Text TextPrefab;

        [SerializeField]
        GameObject Canvas;

        int[,] sudoku;

        public Genome<int>[] genomes;

        public int popSize;
        public int chromosomeSize;

        public float fitIdeal;
        public float desconto;

        private int bestGenome;
        private float bestFitness;
        private double totalFitness;

        public int generation;

        Methods<int> gaMethods;

        bool runGA;
        bool canPlay = false;

        List<Text> listTexts;

        public GameObject Roberto;

        void Start()
        {
            Roberto.SetActive(false);
            gaMethods = new Methods<int>(popSize, chromosomeSize);
            runGA = true;
            listTexts = new List<Text>();
        }

        public void CheckErrors()
        {
            int[,] temp = new int[9, 9];
            int errors;

            for (int g = 0; g < popSize; g++)
            {
                //Fazer a lógica de checar os erros aqui dentro

                //Copiando o vetor de 81 posições na matriz temp
                errors = 0;
                temp = ConvertToMatrix(gaMethods.Genomes[g]);

                string temp2 = "";
                for (int i = 0; i < gaMethods.Genomes[g].Bits.Length; i++)
                {
                    temp2 += gaMethods.Genomes[g].Bits[i] + " ";
                }

                //Checar os erros

                bool HasRowError(int row, int col)
                {
                    var used = new int[10];
                    for (var x = 0; x < 9; ++x)
                    {
                        var val = temp[row, x];
                        used[val]++;
                    }
                    var colVal = temp[row, col];
                    return used[colVal] > 1;
                }
                bool HasColumnError(int row, int col)
                {
                    var used = new int[10];
                    for (var x = 0; x < 9; ++x)
                    {
                        var val = temp[x, col];
                        used[val]++;
                    }
                    var colVal = temp[row, col];
                    return used[colVal] > 1;
                }

                bool HasSubGridError(int row, int col)
                {
                    var gridY = (int)(row / 3);
                    var gridX = (int)(col / 3);

                    var used = new int[10];
                    for (var y = 0; y < 3; ++y)
                    {
                        for (var x = 0; x < 3; ++x)
                        {
                            var val = temp[gridY * 3 + y, gridX * 3 + x];
                            used[val]++;
                        }
                    }
                    var colVal = temp[row, col];
                    return used[colVal] > 1;
                }

                int GetErrorCount()
                {
                    int numOfErrors = 0;
                    for (var row = 0; row < Rows(temp); ++row)
                    {
                        for (var col = 0; col < Columns(temp); ++col)
                        {
                            if (HasRowError(row, col)) numOfErrors++;
                            if (HasColumnError(row, col)) numOfErrors++;
                            if (HasSubGridError(row, col)) numOfErrors++;
                        }
                    }
                    return numOfErrors;
                }
           
                errors = GetErrorCount();
       
                Debug.Log("Número de erros foi: " + errors);
                gaMethods.Genomes[g].m_Fitness = ReturnFitness(errors);

                if (gaMethods.Genomes[g].m_Fitness > bestFitness)
                {
                    bestFitness = (float)gaMethods.Genomes[g].m_Fitness;
           
                    bestGenome = g;

                }

                if( generation >= 2500 && gaMethods.Genomes[g].m_Fitness >= 993)
                {
                    canPlay = true;
                    runGA = false;
                    return;
                }
               else if (gaMethods.Genomes[g].m_Fitness >= fitIdeal)
                {
                    canPlay = true;
                    runGA = false;
                    return;
                }
            }
            ShowBestGenome(ConvertToMatrix(gaMethods.Genomes[bestGenome]));
        }

        private int[,] ConvertToMatrix(Genome<int> genomeBits)
        {
            int[,] temp = new int[9, 9];
            for (int i = 0; i < chromosomeSize; i += 9)
            {
                for (int j = 0; j < 9; j++)
                {
                    temp[i / 9, j] = genomeBits.Bits[i + j];
                }
            }

            return temp;
        }

        public double ReturnFitness(int errors)
        {
            return fitIdeal - (desconto * errors);
        }

        public int Rows(int[,] matrix)
        {
            return matrix.GetLength(0);
        }

        public int Columns(int[,] matrix)
        {
            return matrix.GetLength(1);
        }

        public void Epoch()
        {
            if (runGA == true)
            {
                CheckErrors();
  


                int populationCurrentSize = 0;
                Genome<int>[] newGenomes = new Genome<int>[popSize];

                while (populationCurrentSize < popSize)
                {

                    Genome<int> parent1 = gaMethods.TournamentSelection();
                    Genome<int> parent2;
                    if (bestGenome == popSize - 1)
                    {
                        parent2 = gaMethods.Genomes[0];
                    }
                    else
                    {
                        parent2 = gaMethods.Genomes[bestGenome + 1];
                    }


                    Genome<int> child1 = new Genome<int>(parent1.Size);
                    Genome<int> child2 = new Genome<int>(parent1.Size);

                    gaMethods.Crossover2Points(parent1.Bits, parent2.Bits, child1.Bits, child2.Bits);


                    gaMethods.MutateTrade(child1.Bits);
                    gaMethods.MutateTrade(child2.Bits);


                    newGenomes[populationCurrentSize] = child1;
                    newGenomes[populationCurrentSize + 1] = child2;

                    populationCurrentSize += 2;

                }
                Debug.Log("Melhor fitness da geração " + generation + " foi: " + gaMethods.Genomes[bestGenome].m_Fitness);
                for (int i = 0; i < gaMethods.Genomes.Length; i++)
                {
                    gaMethods.Genomes[i] = newGenomes[i];
                }
                bestFitness = 0;
                bestGenome = 0;

                ++generation;
            }


        }


        public void Erease()
        {
            int limit = 81;
            for (int i = 0; i < 64; i++)
            {
                int R = Random.Range(0, limit);

                Text temp = listTexts[R];

                listTexts.Remove(temp);

                temp.gameObject.SetActive(false);
                limit--;
            }



        }

        void Update()
        {
            if (canPlay == true)
            {
                Roberto.SetActive(true);
            }

            if (runGA)
            {

                Epoch();

            }
        }

        void ShowBestGenome(int[,] matrix)
        {
            DestroyList();
            Genome<int> melhor = gaMethods.Genomes[bestGenome];
            Text temp;

            for (var col = 0; col < Columns(matrix); ++col)
            {
                for (var row = 0; row < Rows(matrix); ++row)
                {
                    temp = (Text)Instantiate(TextPrefab);
                    temp.text = matrix[col, row].ToString();
                    temp.rectTransform.anchoredPosition = new Vector3(IPosX + (Offset * row), IPosY + (Offset * col), 0);
                    temp.transform.SetParent(Canvas.transform);
                    listTexts.Add(temp);
                }
            }
        }

        void DestroyList()
        {
            foreach (Text text in listTexts)
            {
                Destroy(text.gameObject);
            }

            listTexts.Clear();
        }
    }
}