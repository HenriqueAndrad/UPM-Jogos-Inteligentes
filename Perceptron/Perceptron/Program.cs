using System;

namespace Perceptron
{
    class Program
    {
        static void Main(string[] args)
        {
       

            int InputNumber = 0;
            float Vies = 0f;

            Console.WriteLine("Escreva o numero de inputs");
            InputNumber = int.Parse(Console.ReadLine());

            float[] Inputs = new float[InputNumber];
            float[] Pesos = new float[InputNumber];
            

            Console.WriteLine("Escreva o valor dos pesos separadamente por espaço:");
            string[] PesosString  = Console.ReadLine().Split();

            Console.WriteLine("Escreva o valor dos inputs separadamente por espaço:");
            string[] InputString = Console.ReadLine().Split();

            Console.WriteLine("Escreva o vies:");
            Vies = float.Parse(Console.ReadLine());

            for(int i = 0; i < InputNumber; i++)
            {
                Pesos[i] = float.Parse(PesosString[i]);

                Inputs[i] = float.Parse(InputString[i]);
            }

            float Somataria = -Vies;

            for (int i = 0; i < InputNumber; i++)
            {
                Somataria += Pesos[i] * Inputs[i];

            }

         
            //1 / 1 + E^-X onde X = Somatoria
            float Output = SigmoidActive(Somataria);

            Console.WriteLine(Output);
            Console.ReadKey();
        }

      static  float SigmoidActive(float x)
        {
            return 1.0f / (1.0f + MathF.Pow(MathF.E, -x));
        }

    }
}
