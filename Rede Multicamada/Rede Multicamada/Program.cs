using System;
using System.IO;
using System.Globalization;


namespace Rede_Multicamada
{


    class Neuronio
    {
        static float[] m_Inputs;
        static float[] m_Pesos;
        public float Saida { get => SigmoidActive(); }
        private float m_saida;

        public Neuronio(float[] Pesos, float[] Inputs)
        {
            m_Pesos = Pesos;
            m_Inputs = Inputs;
            m_saida = 0;

            //foreach (float f in m_Pesos)
            //{
            //    Console.WriteLine("Peso: " + f);
            //}
            //foreach (float f in m_Inputs)
            //{
            //    Console.WriteLine("Input: " + f);
            //}
        }

        static float Roberto()
        {
            float Somataria = 0.0f;

            for (int i = 0; i < m_Pesos.Length; i++)
            {
                Somataria += m_Pesos[i] * m_Inputs[i];
            }

            //1 / 1 + E^-X onde X = Somatoria
            return Somataria;
        }

        static float SigmoidActive()
        {
            return 1.0f / (1.0f + MathF.Exp(-Roberto()));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            NumberFormatInfo number = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
            number.NumberDecimalSeparator = ".";

            string Treino = Directory.GetCurrentDirectory() + "\\..\\..\\..\\..\\irisTrain.txt";
            string Teste = Directory.GetCurrentDirectory() + "\\..\\..\\..\\..\\irisTest.txt" ;
            
            string[] fileTreino = File.ReadAllLines(Treino); 

            float[][] valores_inputs = new float[fileTreino.Length][];
            string[] resultadosEsperados = new string[fileTreino.Length];
            float[] florNumero = new float[fileTreino.Length];

            for (int i = 0; i < valores_inputs.Length; i++)
            {
                valores_inputs[i] = new float[4];
            }

            //lendo arquivo de treino
            for (int i = 0; i < valores_inputs.Length; i++)
            {
                string[] line = fileTreino[i].Split(",");

                for (int j = 0; j < line.Length; j++)
                {
                    if (j != 4)
                    {
                        valores_inputs[i][j] = float.Parse(line[j], number);
                       // Console.WriteLine(valores_inputs[i][j]);
                    }
                    else
                    {
                        resultadosEsperados[i] = line[j];
                        florNumero[i] = resultadosEsperados[i].Converter();
                    }
                }
            }

            //perceptrons
            int neuroniosOculta = 5;
            int neuroniosSaida = 3;
            int numeroInputs = 4;
            float taxaAprendizado = 0.1f;
            float valorParar  = 0.5f;
            int valorPararContador = 90;
            int maximoGeracoes = 30000;
            int geracaoAtual = 0;

            float[][] valores_pesosOculta = new float[neuroniosOculta][];
            float[][] valores_pesosSaida = new float[neuroniosSaida][];

            for (int i = 0; i < valores_pesosOculta.Length; i++)
            {
                valores_pesosOculta[i] = new float[numeroInputs];
            }
            for (int i = 0; i < valores_pesosSaida.Length; i++)
            {
                valores_pesosSaida[i] = new float[neuroniosOculta];
            }

            Random rnd = new Random((int)DateTime.UtcNow.Ticks);

            //inicializando pesos
            for (int i = 0; i < valores_pesosOculta.Length; i++)
            {
                for (int j = 0; j < numeroInputs; j++)
                {
                    valores_pesosOculta[i][j] = (float)rnd.NextDouble();
                }
            }

            for (int i = 0; i < valores_pesosSaida.Length; i++)
            {
                for (int j = 0; j < neuroniosOculta; j++)
                {
                    valores_pesosSaida[i][j] = (float)rnd.NextDouble();
                }
            }

            //Mostrando pesos antes do treino
            Console.WriteLine("PESOS CAMADA OCULTA ANTES DO TREINO");
            for (int i = 0; i < valores_pesosOculta.Length; i++)
            {
                for (int j = 0; j < valores_pesosOculta[i].Length; j++)
                {
                    Console.WriteLine(valores_pesosOculta[i][j]);
                }
            }

            Console.WriteLine("\nPESOS CAMADA SAIDA ANTES DO TREINO");
            for (int i = 0; i < valores_pesosSaida.Length; i++)
            {
                for (int j = 0; j < valores_pesosSaida[i].Length; j++)
                {
                    Console.WriteLine(valores_pesosSaida[i][j]);
                }
            }

            
            Treinamento();

            void Treinamento()
            {

                Console.ReadLine();
                while (true)
                {
                    int contadorErroQuadratico = 0;
                    for (int i = 0; i < valores_inputs.Length; i++)
                    {
                        Neuronio[] camadaOculta = new Neuronio[neuroniosOculta];
                        Neuronio[] camadaSaida = new Neuronio[neuroniosSaida];
                        float[] saidasCamadaOculta = new float[neuroniosOculta];

                        //calculo camada oculta
                        for (int j = 0; j < camadaOculta.Length; j++)
                        {
                            camadaOculta[j] = new Neuronio(valores_pesosOculta[j], valores_inputs[i]);
                            saidasCamadaOculta[j] = camadaOculta[j].Saida;
                        }

                        //calculo camada saida
                        for (int j = 0; j < camadaSaida.Length; j++)
                        {
                            camadaSaida[j] = new Neuronio(valores_pesosSaida[j], saidasCamadaOculta);
                        }

                        float[] errosCamadaSaida = new float[neuroniosSaida];
                        float[] derivadasSaida = new float[neuroniosSaida];
                        float[] errosSaida = new float[neuroniosSaida];
                        float erroTotalSaida = 0f;
                        for (int j = 0; j < errosCamadaSaida.Length; j++)
                        {
                            errosCamadaSaida[j] = florNumero[i] - camadaSaida[j].Saida;
                           // Console.WriteLine(florNumero[i] + " "+ camadaSaida[j].Saida);
                            derivadasSaida[j] = camadaSaida[j].Saida * (1f - camadaSaida[j].Saida);
                            errosSaida[j] = errosCamadaSaida[j] * derivadasSaida[j];
                            erroTotalSaida += errosSaida[j];
                          
                        }

                        float erroQuadratico = 0f;
                        for (int j = 0; j < errosCamadaSaida.Length; j++)
                        {
                            erroQuadratico += errosCamadaSaida[j] * errosCamadaSaida[j];
                        }

                        if (erroQuadratico < valorParar)
                        {
                            ++contadorErroQuadratico;
                        }
                        Console.WriteLine(erroQuadratico);

                        for (int j = 0; j < errosCamadaSaida.Length; j++)
                        {
                            for (int w = 0; w < valores_pesosSaida[j].Length; w++)
                            {
                                //atualizando pesos entre camada oculta e saida
                                valores_pesosSaida[j][w] += taxaAprendizado * errosSaida[j] * saidasCamadaOculta[w];
                            }
                        }

                        float[] derivadasOculta = new float[neuroniosOculta];
                        float erroOculta = 0f;
                        for (int j = 0; j < derivadasOculta.Length; j++)
                        {
                            derivadasOculta[j] = camadaOculta[j].Saida * (1.0f - camadaOculta[j].Saida);
                            erroOculta = derivadasOculta[j] * erroTotalSaida;

                            for (int w = 0; w < valores_pesosOculta[j].Length; w++)
                            {
                                //atualizando pesos entre camada de input e oculta
                                valores_pesosOculta[j][w] += taxaAprendizado * erroOculta * valores_inputs[i][w];
                            }
                        }
                       // Console.ReadLine();

                        geracaoAtual++;
                    }

                    if (contadorErroQuadratico > valorPararContador)
                    {
                        break;
                    }
                    else if (geracaoAtual >= maximoGeracoes)
                    {
                        geracaoAtual = 0;

                        //reinicializando pesos
                        Console.WriteLine("Não consegue né");
                        for (int i = 0; i < valores_pesosOculta.Length; i++)
                        {
                            for (int j = 0; j < numeroInputs; j++)
                            {
                                valores_pesosOculta[i][j] = (float)rnd.NextDouble();
                            }
                        }

                        for (int i = 0; i < valores_pesosSaida.Length; i++)
                        {
                            for (int j = 0; j < neuroniosOculta; j++)
                            {
                                valores_pesosSaida[i][j] = (float)rnd.NextDouble();
                            }
                        }

                        Treinamento();

                        break;
                    }
                }
            }

            //Mostrando pesos treinados
            Console.WriteLine("PESOS CAMADA OCULTA TREINADOS");
            for (int i = 0; i < valores_pesosOculta.Length; i++)
            {
                for (int j = 0; j < valores_pesosOculta[i].Length; j++)
                {
                    Console.WriteLine(valores_pesosOculta[i][j]);
                }
            }

            Console.WriteLine("PESOS CAMADA SAIDA TREINADOS");
            for (int i = 0; i < valores_pesosSaida.Length; i++)
            {
                for (int j = 0; j < valores_pesosSaida[i].Length; j++)
                {
                    Console.WriteLine(valores_pesosSaida[i][j]);
                }
            }

            //testando
            string[] fileTeste = File.ReadAllLines(Teste);

            valores_inputs = new float[fileTeste.Length][];
            resultadosEsperados = new string[fileTeste.Length];
            florNumero = new float[fileTeste.Length];

            for (int i = 0; i < valores_inputs.Length; i++)
            {
                valores_inputs[i] = new float[4];
            }

            //lendo arquivo de teste
            for (int i = 0; i < valores_inputs.Length; i++)
            {
                string[] line = fileTreino[i].Split(",");

                for (int j = 0; j < line.Length; j++)
                {
                    if (j != 4)
                    {
                        valores_inputs[i][j] = float.Parse(line[j], number);
                    }
                    else
                    {
                        resultadosEsperados[i] = line[j];
                        florNumero[i] = resultadosEsperados[i].Converter();
                    }
                }
            }

            Neuronio[] testeCamadaOculta = new Neuronio[neuroniosOculta];
            Neuronio[] testeCamadaSaida = new Neuronio[neuroniosSaida];
            float[] testeSaidasCamadaOculta = new float[neuroniosOculta];

            //calculo camada saida
            for (int i = 0; i < valores_inputs.Length; i++)
            {
                //calculo camada oculta
                for (int j = 0; j < testeCamadaOculta.Length; j++)
                {
                    testeCamadaOculta[j] = new Neuronio(valores_pesosOculta[j], valores_inputs[j]);
                    testeSaidasCamadaOculta[j] = testeCamadaOculta[j].Saida;
                }

                for (int j = 0; j < testeCamadaSaida.Length; j++)
                {
                    testeCamadaSaida[j] = new Neuronio(valores_pesosSaida[j], testeSaidasCamadaOculta);
                    if (testeCamadaSaida[j].Saida < 0.3f)
                    {
                        Console.WriteLine("Resultado é iris-setosa");
                    }
                    else if (testeCamadaSaida[j].Saida >= 0.3f && testeCamadaSaida[j].Saida <= 0.6f)
                    {
                        Console.WriteLine("Resultado é iris-versicolor");
                    }
                    else if (testeCamadaSaida[j].Saida > 0.6f)
                    {
                        Console.WriteLine("Resultado é iris-virginica");
                    }
                }
            }
            Console.ReadKey();
        }
    }

    public static class Extensions
    {
        public static float Converter(this String flor)
        {
            if (flor == "Iris-setosa")
            {
                return 0f;
            }
            if (flor == "Iris-versicolor")
            {
                return 0.5f;
            }
            if (flor == "Iris-virginica")
            {
                return 1f;
            }

            return -1f;
        }
    }
}