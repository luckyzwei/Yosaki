using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class DokebiHornBoard : MonoBehaviour
{
    [SerializeField]
    private Transform cellParent;

    [SerializeField]
    private UiHornView uiHornView;

    [SerializeField]
    private TextMeshProUGUI currentFloor;

    public void Start()
    {

        Initialize();

        Subscribe();

    }

    private void Subscribe()
    {
        ServerData.userInfoTable.TableDatas[UserInfoTable.dokebiHorn].AsObservable().Subscribe(e =>
        {
            currentFloor.SetText($"{e + 1}단계 입장");
        }).AddTo(this);
    }

    private void Initialize()
    {
        var tableData = TableManager.Instance.DokebiHorn.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            var cell = Instantiate<UiHornView>(uiHornView, cellParent);

            cell.Initialize(tableData[i]);
        }
    }

    //public void OnClickEnterButton()
    //{
    //    int currentIdx = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.dokebiHorn].Value;

    //    if (currentIdx >= TableManager.Instance.DokebiHorn.dataArray.Length)
    //    {
    //        PopupManager.Instance.ShowAlarmMessage("업데이트 예정 입니다!");
    //        return;
    //    }

    //    PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{currentIdx + 1}단계\n도전 할까요?", () =>
    //      {

    //          GameManager.Instance.LoadContents(GameManager.ContentsType.DokebiFire);

    //      }, null);
    //}

    public void OnClickUpEquipButton()
    {
        ServerData.equipmentTable.ChangeEquip(EquipmentTable.DokebiHornView, -1);
    }
}
