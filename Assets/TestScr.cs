using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScr : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        for (int i = 0; i < 1000; i++)
        {
            int prob = 130 - i;
            int test = Mathf.Clamp(prob, 50, 100);
            Debug.LogError(test);
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
