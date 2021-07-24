using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UiCostumeSlotView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI slotNum;

    private CostumeData costumeData;

    [SerializeField]
    private GameObject equpObject;

    [SerializeField]
    private GameObject currentSelectedObject;

    private Action<int> selectCallBack;

    [SerializeField]
    private TextMeshProUGUI costumeHasText;

    public void SetCurrentSelect(bool show)
    {
        currentSelectedObject.SetActive(show);
    }

    public void Initialize(CostumeData costumeData, Action<int> selectCallBack)
    {
        this.selectCallBack = selectCallBack;

        this.costumeData = costumeData;
        slotNum.SetText(costumeData.Id.ToString());
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeSlot].AsObservable().Subscribe(e =>
        {
            equpObject.SetActive(e == this.costumeData.Id);
        }).AddTo(this);

        ServerData.costumeServerTable.TableDatas[costumeData.Stringid].hasCostume.AsObservable().Subscribe(e =>
        {
            costumeHasText.SetText(e ? "적용됨" : "");
        }).AddTo(this);
    }

    public void OnClickSlotButton()
    {
        selectCallBack?.Invoke(this.costumeData.Id);

        CostumeData costumeData = TableManager.Instance.CostumeData[this.costumeData.Id];

        UiCostumeAbilityBoard.Instance.Initialize(costumeData);
    }
}
