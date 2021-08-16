using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiMemory : SingletonMono<UiMemory>
{
    //[SerializeField]
    //private List<UiBossIconCell> bossIconCell;

    //[SerializeField]
    //private UIMemoryStatusView uIMemoryStatusView;

    //private void Start()
    //{
    //    Initialize();
    //}

    //private void Initialize()
    //{
    //    var tableDatas = TableManager.Instance.BossTable.dataArray;

    //    for (int i = 0; i < tableDatas.Length; i++)
    //    {
    //        bossIconCell[i].Initialize(tableDatas[i]);
    //    }

    //    OnClickCell(0);
    //}
    //public void OnClickCell(int idx)
    //{
    //    for (int i = 0; i < bossIconCell.Count; i++)
    //    {
    //        bossIconCell[i].SetSelectedFrame(i == idx);
    //    }

    //    uIMemoryStatusView.Initialize(TableManager.Instance.BossTable.dataArray[idx]);
    //}
}
