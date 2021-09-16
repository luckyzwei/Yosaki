using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class UiTitleIndicator : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro text;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.equipmentTable.TableDatas[EquipmentTable.TitleSelectId].AsObservable().Subscribe(e =>
        {
            if (e != -1)
            {
                var tableData = TableManager.Instance.TitleTable.dataArray[e];

                text.SetText(tableData.Title);

                text.color = CommonUiContainer.Instance.itemGradeColor[tableData.Grade];
            }
            else
            {
                text.SetText(string.Empty);
            }

        }).AddTo(this);
    }
}
