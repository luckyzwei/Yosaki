using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiLibraryBoard : MonoBehaviour
{
    [SerializeField]
    private float needKillAmount = 1000000;

    [SerializeField]
    private UiTwelveBossContentsView twelveBossContentsView;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        twelveBossContentsView.Initialize(TableManager.Instance.TwelveBossTable.dataArray[57]);
    }

    private void OnEnable()
    {
        if (ServerData.userInfoTable.GetTableData(UserInfoTable.gumGiSoulClear).Value < needKillAmount)
        {
            PopupManager.Instance.ShowAlarmMessage($"서재 점수 {Utils.ConvertBigNum(needKillAmount)} 이상일때 해금 됩니다.");

            this.gameObject.SetActive(false);

            return;
        }
    }
}
