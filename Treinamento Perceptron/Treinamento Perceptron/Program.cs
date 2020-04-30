using System;
using System.Collections.Generic;

namespace Treinamento_Perceptron
{
    class Program
    {

        public static void Trainamento(Neuronio Neuronio, double classe, double aprendizado,double[] pesos )
        {

            double TaxaDeErro = 0;

            if (Neuronio.output != classe)
            {
                TaxaDeErro = classe - Neuronio.output;

                for(int i = 0; i < Neuronio.Inputs.Length; i++)
                {
                    pesos[i] = pesos[i] + (TaxaDeErro * aprendizado * Neuronio.Inputs[i]);
              

                }

            }


        }

        public class Neuronio
        {
            double Vies = 0;
            public double[] Inputs;
            public double[] Pesos;
            public double output = 0;
            double Soma = 0;

            public Neuronio(double vies, double[] inputs, double[] pesos)
            {
                Vies = vies;
                Inputs = inputs;
                Pesos = pesos;

                for (int i = 0; i < Inputs.Length; i++)
                {
                    Soma += Inputs[i] * Pesos[i];

                }
                Soma += Vies;

                if (Soma >= 0)
                {
                    output = 1;
                }
                else
                {
                    output = 0;
                }
            }



        }

        static void Main(string[] args)
        {


            List<double[]> ListVector = new List<double[]>() { new double[] { 0.3, 0.7 }, new double[] { -0.6, 0.3 }, new double[] { -0.1, -0.8 }, new double[] { 0.1, -0.45 } };

            double[] ClassesReciever = new double[] { 1, 0, 0, 1 };

            double[] PesosVector = new double[] { 0.8, -0.5 };

            double TaxaDeAprendizado = 0.5;

            double Vies = 0f;


            for(int i = 0; i < 4; i++)
            {
                Neuronio temp = new Neuronio(Vies, ListVector[i], PesosVector);
                Trainamento(temp, ClassesReciever[i], TaxaDeAprendizado, PesosVector);
                foreach(double w in PesosVector)
                {
                    Console.WriteLine("Peso att " + w);

                }
            }


            Console.ReadKey();
        }

    }
}
