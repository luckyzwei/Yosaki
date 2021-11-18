using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using BackEnd;
public class UiYachaCraftBoard : SingletonMono<UiYachaCraftBoard>
{
    [SerializeField]
    private GameObject rootObject;

    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private TextMeshProUGUI createDescription;

    private WeaponData yachaData;

    [SerializeField]
    private WeaponView yachaView;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        yachaData = TableManager.Instance.WeaponTable.dataArray[21];

        yachaView.Initialize(yachaData, null);

        createDescription.SetText($"요물 능력치 레벨 합계 {GameBalance.YachaRequireLevel}이상일때 제작 가능 합니다.");
    }

    private void UpdateDescription()
    {
        var yomulAbilData = TableManager.Instance.YomulAbilTable.dataArray;

        string desc = string.Empty;

        for (int i = 0; i < yomulAbilData.Length; i++)
        {
            desc += $"{yomulAbilData[i].Abilname} : {ServerData.yomulServerTable.TableDatas[yomulAbilData[i].Stringid].level.Value}\n";
        }

        desc += $"합계 : {GetYomulTotalLevel()}";

        description.SetText(desc);
    }

    public void ShowPopup()
    {
        rootObject.SetActive(true);
        UpdateDescription();
    }

    private bool CanMakeYacha()
    {
        int level = GetYomulTotalLevel();
        return level >= GameBalance.YachaRequireLevel;
    }

    private int GetYomulTotalLevel()
    {
        int totalLevel = 0;

        var yomulAbilData = TableManager.Instance.YomulAbilTable.dataArray;

        for (int i = 0; i < yomulAbilData.Length; i++)
        {
            totalLevel += ServerData.yomulServerTable.TableDatas[yomulAbilData[i].Stringid].level.Value;
        }

        return totalLevel;
    }

    public void OnClickCraftButton()
    {
        var yachaServerData = ServerData.weaponTable.TableDatas[yachaData.Stringid];

        if (yachaServerData.hasItem.Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보유중 입니다.");
            return;
        }

        if (CanMakeYacha() == false)
        {
            PopupManager.Instance.ShowAlarmMessage("레벨이 부족 합니다.");
            return;
        }

        List<TransactionValue> transactions = new List<TransactionValue>();

        ServerData.weaponTable.TableDatas[yachaData.Stringid].amount.Value += 1;
        ServerData.weaponTable.TableDatas[yachaData.Stringid].hasItem.Value = 1;

        Param weaponParam = new Param();
        weaponParam.Add(yachaData.Stringid, ServerData.weaponTable.TableDatas[yachaData.Stringid].ConvertToString());

        transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            SoundManager.Instance.PlaySound("Reward");
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "너에게 힘을 주마", null);
            LogManager.Instance.SendLogType("야차제작", "", "");
        });
    }

}
