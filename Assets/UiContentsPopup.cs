using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiContentsPopup : MonoBehaviour
{
    [SerializeField]
    private UiBossContentsView bossContentsView;

    [SerializeField]
    private GameObject bandit1;

    [SerializeField]
    private GameObject bandit2;

    [SerializeField]
    private GameObject tower1;

    [SerializeField]
    private GameObject tower2;

    [SerializeField]
    private TextMeshProUGUI banditDescription;

    void Start()
    {
        bossContentsView.Initialize(TableManager.Instance.BossTable.dataArray[0]);

        tower1.SetActive(ServerData.userInfoTable.IsLastFloor() == false);
        tower2.SetActive(ServerData.userInfoTable.IsLastFloor());
    }


    private void OnEnable()
    {
        RefreshBandit();
    }

    private void RefreshBandit()
    {
        int level = ServerData.statusTable.GetTableData(StatusTable.Level).Value;
        int requireLv = GameBalance.banditUpgradeLevel;
        bandit1.SetActive(level < requireLv);
        bandit2.SetActive(level >= requireLv);

        banditDescription.SetText($"레벨 {Utils.ConvertBigNum(GameBalance.banditUpgradeLevel)}에 대왕반딧불전 해금!");
    }

    private void OnDisable()
    {
        PlayerStats.ResetAbilDic();
    }
}
