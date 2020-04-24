//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;

//namespace Sudoku
//{
//    public class Genome
//    {
//        int[] m_Bits;
//        public int[] Bits
//        {
//            get { return m_Bits; }
//            set { m_Bits = value; }
//        }

//        double m_Fitness;
//        public double Fitness
//        {
//            get { return m_Fitness; }
//            set { m_Fitness = value; }
//        }

//        public int Size { get => m_Bits.Length; }

//        List<int> numbers;

//        public Genome(int size)
//        {
//            System.Random random = new System.Random((int)DateTime.UtcNow.Ticks);
//            SetListNumbers();

//            m_Bits = new int[size];
        
//            for (int i = 0; i < size; i += 9)
//            {
//                SetListNumbers();
//                for (int j = 0; j < 9; ++j)
//                {
//                    int randomNum = random.Next(1, 10);
//                    m_Bits[i + j] = numbers[randomNum];
//                    numbers.RemoveAt(randomNum);
//                }
//            }

//            m_Fitness = 0.0;
//        }

//        void SetListNumbers()
//        {
//            numbers = new List<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
//        }
//    }
//}