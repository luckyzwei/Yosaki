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
        int costumeId = ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].Value;
        int petId = ServerData.equipmentTable.TableDatas[EquipmentTable.Pet].Value;
        int weaponId = ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon].Value;
        int magicBookId = ServerData.equipmentTable.TableDatas[EquipmentTable.MagicBook].Value;
        topRankerCell.Initialize(string.Empty, string.Empty, costumeId, petId, weaponId, magicBookId, 0, string.Empty);
    }

    public void OnClickStatResetButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "스텟 능력치를 초기화 합니까?", () =>
        {
            ServerData.statusTable.GetTableData(StatusTable.IntLevelAddPer_StatPoint).Value = 1;
            ServerData.statusTable.GetTableData(StatusTable.CriticalLevel_StatPoint).Value = 1;
            ServerData.statusTable.GetTableData(StatusTable.CriticalDamLevel_StatPoint).Value = 1;
            ServerData.statusTable.GetTableData(StatusTable.HpPer_StatPoint).Value = 1;
            ServerData.statusTable.GetTableData(StatusTable.MpPer_StatPoint).Value = 1;
            ServerData.statusTable.GetTableData(StatusTable.StatPoint).Value = (ServerData.statusTable.GetTableData(StatusTable.Level).Value - 1) * GameBalance.StatPoint;

            ServerData.statusTable.SyncAllData();
        }, null);


    }

    public void OnClickMemoryResetButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "비급 능력치를 초기화 합니까?", () =>
        {
            string log = $"보유 {ServerData.statusTable.GetTableData(StatusTable.Memory).Value}";

            var e = TableManager.Instance.StatusDatas.GetEnumerator();

            int usedPoint = 0;

            while (e.MoveNext())
            {
                if (e.Current.Value.STATUSWHERE != StatusWhere.memory) continue;
                usedPoint += (ServerData.statusTable.GetTableData(e.Current.Value.Key).Value);
                ServerData.statusTable.GetTableData(e.Current.Value.Key).Value = 0;
            }

            if (usedPoint == 0)
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "초기화에 실패했습니다", null);
                return;
            }

            log += $"획득수량 {usedPoint}";

            ServerData.statusTable.GetTableData(StatusTable.Memory).Value += usedPoint;
            log += $"최종 {ServerData.statusTable.GetTableData(StatusTable.Memory).Value}";

            ServerData.statusTable.SyncAllData();

            LogManager.Instance.SendLog("기억 능력치 초기화", log);
        }, null);
    }
}
