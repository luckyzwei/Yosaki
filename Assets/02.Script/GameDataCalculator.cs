using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameDataCalculator
{
    public static float GetMaxExp(int level)
    {
        return Mathf.Pow(level, 2.3f)*6f + Mathf.Pow(level, 1.5f) * 50;
    }
}
