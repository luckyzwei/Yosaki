using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class CaveBeltBoard : MonoBehaviour
{
    [SerializeField]
    private Transform cellParent;

    [SerializeField]
    private UiBeltView uiBeltView;

    [SerializeField]
    private TextMeshProUGUI currentFloor;

    public void Start()
    {

        Initialize();

       // Subscribe();

    }

    //private void Subscribe()
    //{
    //    ServerData.userInfoTable.TableDatas[UserInfoTable.foxMask].AsObservable().Subscribe(e =>
    //    {
    //        currentFloor.SetText($"{e + 1}단계 입장");
    //    }).AddTo(this);
    //}

    private void Initialize()
    {
        var tableData = TableManager.Instance.CaveBelt.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            var cell = Instantiate<UiBeltView>(uiBeltView, cellParent);

            cell.Initialize(tableData[i]);
        }
    }


    public void OnClickUpEquipButton()
    {
        ServerData.equipmentTable.ChangeEquip(EquipmentTable.CaveBeltView, -1);
    }
}
