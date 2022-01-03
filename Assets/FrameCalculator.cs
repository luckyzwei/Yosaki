using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameCalculator : SingletonMono<FrameCalculator>
{
    private List<float> averageFrame = new List<float>();
    public static float frameRate { get; private set; } = 60;

    private float frameDivedeNum = 150f;

    private WaitForSeconds delay = new WaitForSeconds(0.1f);

    private IEnumerator Start()
    {
        while (true)
        {
            averageFrame.Add((1f / Time.unscaledDeltaTime));

            if (averageFrame.Count >= frameDivedeNum)
            {
                float sum = 0f;

                for (int i = 0; i < averageFrame.Count; i++)
                {
                    sum += averageFrame[i];
                }

                frameRate = sum / frameDivedeNum;

                averageFrame.Clear();

#if UNITY_EDITOR
                Debug.LogError($"Frame {frameRate}");
#endif
            }
            yield return null;
        }
    }
}
