using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreScene : MonoBehaviour
{
    void Awake()
    {
#if !UNITY_EDITOR
        Application.targetFrameRate = 60;
#endif


    }
}
