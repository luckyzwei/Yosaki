using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameCalculator : SingletonMono<FrameCalculator>
{
    public static float frameRate { get; private set; } = 60;

    private WaitForSeconds delay = new WaitForSeconds(0.1f);

    //private IEnumerator Start()
    //{
    //    //while (true)
    //    //{
    //    //    frameRate = (1f / Time.unscaledDeltaTime);

    //    //    yield return null;

    //    //}
    //}
}
