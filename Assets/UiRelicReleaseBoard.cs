using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiRelicReleaseBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI description;


    private void OnEnable()
    {
        int usedKeyNum = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.usedRelicTicketNum].Value;

        description.SetText($"영혼열쇠 총{usedKeyNum}개 사용\n" +
            $"사용한 열쇠 1000개당 공격력(%) {Utils.ConvertBigNum(PlayerStats.relicReleaseValue*100f)} 증가 \n" +
            $"<color=yellow>총 {CommonString.GetStatusName(StatusType.AttackAddPer)} {Utils.ConvertBigNum(PlayerStats.GetRelicReleaseValue() * 100f)}증가");
    }
}
