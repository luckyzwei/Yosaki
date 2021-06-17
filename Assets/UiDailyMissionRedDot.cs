using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class UiDailyMissionRedDot : UiRedDotBase
{
    private ReactiveDictionary<string, int> activeObject = new ReactiveDictionary<string, int>();

    protected override void Subscribe()
    {
        bool hasActive = false;

        var tableDatas = TableManager.Instance.DailyMission.dataArray;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            var tableData = tableDatas[i];

            if (tableData.Enable == false) continue;

            var serverTableData = DatabaseManager.dailyMissionTable.TableDatas[tableData.Stringid];

            if (hasActive == false)
            {
                hasActive = serverTableData.Value >= tableData.Rewardrequire;
            }

            serverTableData.AsObservable().Subscribe(account =>
            {
                bool active = account >= tableData.Rewardrequire;

                if (active)
                {

                    if (activeObject.ContainsKey(tableData.Stringid) == false)
                    {
                        activeObject.Add(tableData.Stringid, 0);
                    }
                }
                else
                {
                    if (activeObject.ContainsKey(tableData.Stringid) == true)
                    {
                        activeObject.Remove(tableData.Stringid);
                    }
                }

            }).AddTo(this);
        }

        //맨처음 초기화
        rootObject.SetActive(hasActive);

        activeObject.ObserveAdd().Subscribe(add =>
        {
            rootObject.SetActive(activeObject.Count != 0);
        }).AddTo(this);

        activeObject.ObserveRemove().Subscribe(remove =>
        {
            rootObject.SetActive(activeObject.Count != 0);
        }).AddTo(this);
    }
}
