using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SeolAttenButton2 : MonoBehaviour
{
    private static bool active = true;

    void Start()
    {
        if (active == true)
        {
            CheckEnable();
        }

        this.gameObject.SetActive(active);
    }

    private void CheckEnable()
    {
        var tableData = TableManager.Instance.SulPass.dataArray;

        string freeKey = SulPassServerTable.MonthlypassFreeReward;

        var freeSplits = GetSplitData(freeKey);

        bool allReceived = true;

        for (int i = 0; i < tableData.Length; i++)
        {
            if (HasReward(freeSplits, tableData[i].Id) == false)
            {
                allReceived = false;
                break;
            }
        }

        active = (allReceived == false);
    }

    public bool HasReward(List<string> splitData, int data)
    {
        return splitData.Contains(data.ToString());
    }

    public List<string> GetSplitData(string key)
    {
        return ServerData.sulPassServerTable.TableDatas[key].Value.Split(',').ToList();
    }
}
