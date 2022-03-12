using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiContentsPopup : MonoBehaviour
{
    [SerializeField]
    private UiBossContentsView bossContentsView;

    [SerializeField]
    private GameObject tower1;

    [SerializeField]
    private GameObject tower2;

    void Start()
    {
        bossContentsView.Initialize(TableManager.Instance.BossTable.dataArray[0]);

        tower1.SetActive(ServerData.userInfoTable.IsLastFloor() == false);
        tower2.SetActive(ServerData.userInfoTable.IsLastFloor());
    }
}
