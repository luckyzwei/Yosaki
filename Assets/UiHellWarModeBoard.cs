using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class UiHellWarModeBoard : MonoBehaviour
{
    [SerializeField]
    private Button enterButton;

    [SerializeField]
    private TextMeshProUGUI scoreDescription;

    private void Start()
    {
        scoreDescription.SetText($"최고 점수 : {Utils.ConvertBigNum(ServerData.userInfoTable.TableDatas[UserInfoTable.hellWarScore].Value * GameBalance.BossScoreConvertToOrigin)}");
    }

    public void OnClickEnterButton()
    {
        PopupManager.Instance.ShowYesNoPopup("알림", "입장 할까요?", () =>
        {

            GameManager.Instance.LoadContents(ContentsType.HellWarMode);

            enterButton.interactable = false;

        }, () => { });
    }
}
