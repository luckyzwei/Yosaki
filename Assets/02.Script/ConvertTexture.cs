using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ConvertTexture : MonoBehaviour
{
    [SerializeField]
    private Texture2D tex2D;
    void Start()
    {
        byte[] bytes = tex2D.EncodeToPNG();

        Debug.Log(bytes);

        File.WriteAllBytes(Application.dataPath + "/" + "test.png", bytes);
    }

   
}
