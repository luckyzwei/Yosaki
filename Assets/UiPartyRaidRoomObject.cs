using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
public class UiPartyRaidRoomObject : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI description;

    private void OnEnable()
    {
        if (PartyRaidManager.Instance.NetworkManager.IsPartyTowerBoss() == true)
        {
            description.SetText(string.Empty);
        }
        else if (PartyRaidManager.Instance.NetworkManager.IsGuildBoss() == true)
        {
            description.SetText(string.Empty);
        }
        else
        {
            int recCount = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.canRecommendCount].Value;
            description.SetText($"매주{GameBalance.recommendCountPerWeek}회 다른 유저를 추천하실 수 있습니다.\n남은 추천 : {ServerData.userInfoTable.TableDatas[UserInfoTable.canRecommendCount].Value}");
        }
    }
}
