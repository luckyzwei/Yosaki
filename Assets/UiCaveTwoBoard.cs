using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UiCaveTwoBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI currentFloorDescription;
    [SerializeField]
    private TextMeshProUGUI currentFloorAbilDescription;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        string bossKey = "b91";

        var serverData = ServerData.bossServerTable.TableDatas[bossKey];

        serverData.score.AsObservable().Subscribe(e =>
        {
            if (string.IsNullOrEmpty(e))
            {
                currentFloorDescription.SetText($"점수 없음");
                currentFloorAbilDescription.SetText($"효과 없음");
            }
            else
            {

                if (int.TryParse(e, out var score))
                {
                    currentFloorDescription.SetText($"현재 단계 : {score}");
                    currentFloorAbilDescription.SetText($"십만동굴 허리띠 효과 : +{PlayerStats.GetTwoCaveBeltAbilPlusValue() * 100}%");
                }
                else
                {
                    currentFloorDescription.SetText($"점수 없음");
                    currentFloorAbilDescription.SetText($"효과 없음");
                }
            }
        }).AddTo(this);

        
    }
}
