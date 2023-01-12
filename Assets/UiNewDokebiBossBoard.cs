using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiNewDokebiBossBoard : MonoBehaviour
{
    [SerializeField]
    private List<UiTwelveBossContentsView> dokebiBoss;


    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        for(int i = 0;  i < dokebiBoss.Count;i++)
        {
            if (dokebiBoss[i] != null)
            {
                dokebiBoss[i].Initialize(TableManager.Instance.TwelveBossTable.dataArray[85 + i]);
            }
        }
    }
}
