using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class FoxMaskBoard : MonoBehaviour
{
    [SerializeField]
    private Transform cellParent;

    [SerializeField]
    private UiMaskView uiMaskView;

    [SerializeField]
    private TextMeshProUGUI currentFloor;

    public void Start()
    {

        Initialize();

        Subscribe();

    }

    private void Subscribe()
    {
        ServerData.userInfoTable.TableDatas[UserInfoTable.foxMask].AsObservable().Subscribe(e =>
        {
            currentFloor.SetText($"{e + 1}단계 입장");
        }).AddTo(this);
    }

    private void Initialize()
    {
        var tableData = TableManager.Instance.FoxMask.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            var cell = Instantiate<UiMaskView>(uiMaskView, cellParent);

            cell.Initialize(tableData[i]);
        }
    }

    public void OnClickEnterButton()
    {
        int currentIdx = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.foxMask].Value;

        if (currentIdx >= TableManager.Instance.FoxMask.dataArray.Length)
        {
            PopupManager.Instance.ShowAlarmMessage("업데이트 예정 입니다!");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, $"{currentIdx + 1}단계\n도전 할까요?", () =>
          {

              GameManager.Instance.LoadContents(GameManager.ContentsType.FoxMask);

          }, null);
    }

    public void OnClickUpEquipButton()
    {
        ServerData.equipmentTable.ChangeEquip(EquipmentTable.FoxMaskView, -1);
    }
}
