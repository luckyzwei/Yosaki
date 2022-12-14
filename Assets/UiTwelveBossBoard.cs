using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiTwelveBossBoard : MonoBehaviour
{

    [SerializeField]
    private UiTwelveBossContentsView twelveBossContentsView;

    [SerializeField]
    private int bossNum = -1;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (bossNum != -1)
        {
            twelveBossContentsView.Initialize(TableManager.Instance.TwelveBossTable.dataArray[bossNum]);
        }
        else
        {
            Debug.Log("Input SerializeFiled");
        }
    }
}
    
