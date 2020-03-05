using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Genome
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
        System.Random random = new System.Random((int)DateTime.UtcNow.Ticks);

        m_Bits = new bool[size];
        for (int i = 0; i < size; ++i)
        {
            m_Bits[i] = random.NextDouble() < 0.5;
        }
        m_Fitness = 0.0;
    }
}