using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UiDoLockMask : MonoBehaviour
{
    private void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        var hornList = ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value.Split(BossServerTable.rewardSplit);

        //1~3 합 = 6
        //1~7 합 = 28
        int dokebiHornReward = 6;
        for (int i = 1;  i < hornList.Length;i++)
        {
            
            if (string.IsNullOrEmpty(hornList[i]))
            {
                continue;
            }
            else
            {
                dokebiHornReward -=  (int.Parse(hornList[i]) + 1);
            }
        }

        if(dokebiHornReward<=0)
        {
            this.gameObject.SetActive(false);
        }
    }

}
