using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageAbilDescription : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI description;

    void Start()
    {
        int currentStage = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.topClearStageId].Value;

        float divide = (int)(currentStage / PlayerStats.divideNum);

        description.SetText($"x{PlayerStats.GetStageAddValue()}배");
    }

}
