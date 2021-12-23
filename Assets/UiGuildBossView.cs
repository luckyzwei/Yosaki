using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiGuildBossView : MonoBehaviour
{
    [SerializeField]
    private UiTwelveBossContentsView bossContentsView;

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
        bossContentsView.Initialize(TableManager.Instance.TwelveBossTable.dataArray[TableManager.Instance.TwelveBossTable.dataArray.Length - 1]);
    }
}
