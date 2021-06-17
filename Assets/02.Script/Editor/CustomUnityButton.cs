using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BackEnd;

public class CustomUnityButton : MonoBehaviour
{
    [MenuItem("Utils/Delete all playerPrefs")]
    public static void Edit()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
