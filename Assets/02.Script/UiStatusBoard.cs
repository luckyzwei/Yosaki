using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UiStatusBoard : MonoBehaviour
{
    [SerializeField]
    private UiStatusUpgradeCell statusCellPrefab;

    [SerializeField]
    private Transform goldAbilParent;

    [SerializeField]
    private Transform skillPointAbilParent;

    [SerializeField]
    private Transform memoryParent;

    [SerializeField]
    private UiTopRankerCell topRankerCell;

    private void Awake()
    {
        Initialize();
    }



    private void Initialize()
    {
        var e = TableManager.Instance.StatusDatas.GetEnumerator();

        while (e.MoveNext())
        {
            if (e.Current.Value.Active == false) continue;
            Transform cellParent = null;

            if (e.Current.Value.STATUSWHERE == StatusWhere.gold)
            {
                cellParent = goldAbilParent;
            }
            else if (e.Current.Value.STATUSWHERE == StatusWhere.statpoint)
            {
                cellParent = skillPointAbilParent;
            }
            else if (e.Current.Value.STATUSWHERE == StatusWhere.memory)
            {
                cellParent = memoryParent;
            }

            var cell = MakeCell(cellParent);

            cell.Initialize(e.Current.Value);
        }
    }

    private UiStatusUpgradeCell MakeCell(Transform parent)
    {
        return Instantiate<UiStatusUpgradeCell>(statusCellPrefab, parent);
    }

    private void OnEnable()
    {
        this.transform.SetAsLastSibling();
        UpdatePlayerView();
    }

    private void UpdatePlayerView()
    {
        int costumeId = DatabaseManager.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value;
        int petId = DatabaseManager.equipmentTable.TableDatas[EquipmentTable.Pet].Value;
        int weaponId = DatabaseManager.equipmentTable.TableDatas[EquipmentTable.Weapon].Value;
        int magicBookId = DatabaseManager.equipmentTable.TableDatas[EquipmentTable.MagicBook].Value;
        topRankerCell.Initialize(string.Empty, string.Empty, costumeId, petId, weaponId, magicBookId, 0);
    }

    public void OnClickStatResetButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "스텟 능력치를 초기화 합니까?", () =>
        {
            DatabaseManager.statusTable.GetTableData(StatusTable.IntLevelAddPer_StatPoint).Value = 1;
            DatabaseManager.statusTable.GetTableData(StatusTable.CriticalLevel_StatPoint).Value = 1;
            DatabaseManager.statusTable.GetTableData(StatusTable.CriticalDamLevel_StatPoint).Value = 1;
            DatabaseManager.statusTable.GetTableData(StatusTable.GoldGain_StatPoint).Value = 1;
            DatabaseManager.statusTable.GetTableData(StatusTable.ExpGain_StatPoint).Value = 1;
            DatabaseManager.statusTable.GetTableData(StatusTable.HpPer_StatPoint).Value = 1;
            DatabaseManager.statusTable.GetTableData(StatusTable.MpPer_StatPoint).Value = 1;
            DatabaseManager.statusTable.GetTableData(StatusTable.StatPoint).Value = (DatabaseManager.statusTable.GetTableData(StatusTable.Level).Value - 1) * GameBalance.StatPoint;

            DatabaseManager.statusTable.SyncAllData();
        }, null);


    }

    public void OnClickMemoryResetButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "기억 능력치를 초기화 합니까?", () =>
        {
            string log = $"보유 {DatabaseManager.statusTable.GetTableData(StatusTable.Memory).Value}";

            var e = TableManager.Instance.StatusDatas.GetEnumerator();

            int usedPoint = 0;

            while (e.MoveNext())
            {
                if (e.Current.Value.STATUSWHERE != StatusWhere.memory) continue;
                usedPoint += (DatabaseManager.statusTable.GetTableData(e.Current.Value.Key).Value - 1);
                DatabaseManager.statusTable.GetTableData(e.Current.Value.Key).Value = 1;
            }

            log += $"획득수량 {usedPoint}";

            DatabaseManager.statusTable.GetTableData(StatusTable.Memory).Value += usedPoint;
            log += $"최종 {DatabaseManager.statusTable.GetTableData(StatusTable.Memory).Value}";

            DatabaseManager.statusTable.SyncAllData();

            LogManager.Instance.SendLog("기억 능력치 초기화", log);
        }, null);
    }
}
