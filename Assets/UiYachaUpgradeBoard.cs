using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;

public class UiYachaUpgradeBoard : SingletonMono<UiYachaUpgradeBoard>
{
    [SerializeField]
    private GameObject rootObject;

    [SerializeField]
    private WeaponView yachaWeaponView;

    [SerializeField]
    private TextMeshProUGUI basicAbilDescription;

    private void Start()
    {
        Initialize();
        Subscribe();
    }

    private void Subscribe() 
    {
        ServerData.statusTable.GetTableData(StatusTable.Level).AsObservable().Subscribe(e=> 
        {
            basicAbilDescription.SetText($"{CommonString.GetStatusName(StatusType.SkillDamage)} {PlayerStats.GetYachaSkillPercentValue()*100f} 증가");
        }).AddTo(this);
    }

    private void Initialize()
    {
        yachaWeaponView.Initialize(TableManager.Instance.WeaponTable.dataArray[21], null, null);
    }


    public void ShowUpgradePopup(bool show)
    {
        rootObject.SetActive(show);
    }
}
