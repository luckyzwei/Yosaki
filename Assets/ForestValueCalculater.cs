using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestValueCalculater : MonoBehaviour
{

    private ObscuredFloat divideNum = 100f;
    private ObscuredFloat addValue = 0.07f;

    void Start()
    {
        UpdateForestValue();
    }

    private void UpdateForestValue()
    {
        float diveidedValue = (float)ServerData.userInfoTable.TableDatas[UserInfoTable.relicKillCount].Value / divideNum;

        GameBalance.forestValue = 1f + (diveidedValue* addValue);

       // GameBalance.forestValue = 1f;
    }
}
