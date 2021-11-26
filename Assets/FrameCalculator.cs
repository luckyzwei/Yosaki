using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameCalculator : SingletonMono<FrameCalculator>
{
    public static int frameRate { get; private set; } = 30;

    private WaitForSeconds delay = new WaitForSeconds(0.1f);

    private IEnumerator Start()
    {
        while (true)
        {
            frameRate = (int)(1 / Time.unscaledDeltaTime);
            yield return null;
        }
    }
}
