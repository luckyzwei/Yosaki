using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiHellRelicBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI bestScoreText;

    [SerializeField]
    private TextMeshProUGUI abilDescription;

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        bestScoreText.SetText($"최고점수:{(int)ServerData.userInfoTable.TableDatas[UserInfoTable.hellRelicKillCount].Value}");

        abilDescription.SetText($"최고점수 {PlayerStats.HellRelicAbilDivide}당 지옥베기 피해량 {PlayerStats.HellRelicAbilValue * 100f}% 증가\n<color=red>{PlayerStats.GetHellRelicAbilValue() * 100f}%증가됨</color>");
    }

    public void OnClickEnterButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "입장 하시겠습니까?", () =>
        {
            GameManager.Instance.LoadContents(GameManager.ContentsType.HellRelic);
        }, () => { });
    }

}
