using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameDataCalculator
{
    public static float GetMaxExp(int level)
    {
        return Mathf.Pow(level, level < 10000 ? 2.3f : 2.38f) * 6f + Mathf.Pow(level, 1.5f) * 50;
    }
}
