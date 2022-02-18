using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiSinBoard : MonoBehaviour
{
    [SerializeField]
    private UiTwelveBossContentsView bossContentsView;

    public ObscuredInt rewardGrade = 0;

    [SerializeField]
    private Button recordButton;

    void Start()
    {
        Initialize();
    }
    private void OnEnable()
    {
        this.transform.SetAsLastSibling();
    }

    private void Initialize()
    {
        bossContentsView.Initialize(TableManager.Instance.TwelveBossTable.dataArray[13]);
    }
}
