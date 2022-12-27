using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class SusanoUpgradeDescription : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI marbleDescription;


    void Start()
    {

        Subscribe();

    }

    private void Subscribe()
    {
  
        ServerData.goodsTable.GetTableData(GoodsTable.SusanoTreasure).AsObservable().Subscribe(e =>
        {
            int grade = PlayerStats.GetSusanoGrade();
            if (grade == -1)
            {
                marbleDescription.SetText("악귀 퇴치 기록이 없습니다!");
                return;
            }
            else
            {
                var tableData = TableManager.Instance.susanoTable.dataArray[grade];
                marbleDescription.SetText(
                    $"{CommonString.GetStatusName((StatusType)tableData.Abiltype1)}  {Utils.ConvertBigNum(tableData.Abilvalue0 * PlayerStats.GetSusanoUpgradeAbilPlusValue() * 100f)}%\n" +
                    $"{CommonString.GetStatusName((StatusType)tableData.Abiltype2)}  {tableData.Abilvalue1 * PlayerStats.GetSusanoUpgradeAbilPlusValue() * 100}% 강화됨)"
                    );
            }
        }).AddTo(this);

    }

}
