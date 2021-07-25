using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiContentsPopup : MonoBehaviour
{
    [SerializeField]
    private UiBossContentsView bossContentsView;

    void Start()
    {
        bossContentsView.Initialize(TableManager.Instance.BossTable.dataArray[0]);
    }
}
