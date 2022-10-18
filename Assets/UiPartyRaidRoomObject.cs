using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
public class UiPartyRaidRoomObject : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI RecommendCount;
    [SerializeField]
    public TextMeshProUGUI RecommendCountDescription;

    // Start is called before the first frame update
    void Start()
    {
        Subscribe();
        
        RecommendCountDescription.text = "추천은 결과창에서 가능합니다!\n매 주 " + GameBalance.recommendCountPerWeek.ToString() + " 번 추천 가능합니다!";
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.TableDatas[UserInfoTable.canRecommendCount].AsObservable().Subscribe(e=>
        {
            RecommendCount.text = "추천 가능 수 : " + ServerData.userInfoTable.TableDatas[UserInfoTable.canRecommendCount].Value.ToString();
        }).AddTo(this);
    }
}
