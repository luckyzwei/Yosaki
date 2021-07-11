using CodeStage.AntiCheat.ObscuredTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : SingletonMono<BuffManager>
{
    private ObscuredInt updateDelay = 1;

    void Start()
    {
        StartBuffTime();
    }

    private void StartBuffTime()
    {
        StartCoroutine(BuffTimerRoutine());
    }

    private IEnumerator BuffTimerRoutine()
    {
        WaitForSeconds updateTick = new WaitForSeconds(updateDelay);

        while (true)
        {
            yield return updateTick;

            UpdateBuffTime();
        }
    }

    public void UpdateBuffTime() 
    {
        var tableDatas = TableManager.Instance.BuffTable.dataArray;

        DateTime currentTime = DateTime.Now.ToUniversalTime();

        int elapsedSeconds = updateDelay;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            //영구버프
            if (tableDatas[i].Buffseconds <= 0f) continue;

            //시간끝난버프
            if (ServerData.buffServerTable.TableDatas[tableDatas[i].Stringid].remainSec.Value == 0) continue;

            ServerData.buffServerTable.TableDatas[tableDatas[i].Stringid].remainSec.Value -= elapsedSeconds;

            if (ServerData.buffServerTable.TableDatas[tableDatas[i].Stringid].remainSec.Value <= 0f)
            {
                ServerData.buffServerTable.TableDatas[tableDatas[i].Stringid].remainSec.Value = 0;

                //서버에 저장
                ServerData.buffServerTable.SyncData(tableDatas[i].Stringid);
            }
        }
    }
}
