using System;

namespace Perceptron
{


    class Neuronio
    {
      static float[] m_Inputs;
      static float[] m_Pesos;
      public static float m_Saida = 0.0f;

        public Neuronio(float[] Pesos, float[]Inputs)
        {
            m_Pesos = Pesos;
            m_Inputs = Inputs;


            Roberto();

        }

    

        static void Roberto()
        {


            float Somataria = 0.0f;

            for (int i = 0; i < m_Pesos.Length; i++)
            {
                Somataria += m_Pesos[i] * m_Inputs[i];

            }


            //1 / 1 + E^-X onde X = Somatoria
            m_Saida = SigmoidActive(Somataria);


        }

        static float SigmoidActive(float x)
        {
            return 1.0f / (1.0f + MathF.Pow(MathF.E, -x));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {      

        }
    }
}
