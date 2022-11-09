using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiDokebiFireAbilDescription : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI description;

    private void OnEnable()
    {
        UpdateDescriptionText();
    }

    private void UpdateDescriptionText()
    {
        int level = (int)ServerData.goodsTable.GetTableData(GoodsTable.Cw).Value;

        description.SetText($"1개당\n{CommonString.GetStatusName(StatusType.AttackAddPer)} {Utils.ConvertBigNum(PlayerStats.GetDokebiFireAbilHasEffect(StatusType.AttackAddPer) / level)} \n{CommonString.GetStatusName(StatusType.SuperCritical5DamPer)} {(PlayerStats.GetChunAbilHasEffect(StatusType.SuperCritical5DamPer) / level) * 100f} 증가");
    }

}
