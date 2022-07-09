using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UiGumihoAbilCell : MonoBehaviour
{
    [SerializeField]
    private ObscuredString key;

    [SerializeField]
    private GameObject lockMask;

    [SerializeField]
    private TextMeshProUGUI description;

    void Start()
    {
        Subscribe();

        SetDescription();
    }

    private void SetDescription()
    {
        if (key == GoodsTable.gumiho0)
        {
            description.SetText($"<color=yellow>{CommonString.GetItemName(Item_Type.gumiho0)}</color>\n{CommonString.GetStatusName(StatusType.IgnoreDefense)}\n{PlayerStats.gumihoValue0}");
        }
        else if (key == GoodsTable.gumiho1)
        {
            description.SetText($"<color=yellow>{CommonString.GetItemName(Item_Type.gumiho1)}</color>\n{CommonString.GetStatusName(StatusType.IgnoreDefense)}\n{PlayerStats.gumihoValue1}");
        }
        else if (key == GoodsTable.gumiho2)
        {
            description.SetText($"<color=yellow>{CommonString.GetItemName(Item_Type.gumiho2)}</color>\n{CommonString.GetStatusName(StatusType.IgnoreDefense)}\n{PlayerStats.gumihoValue2  }");
        }
        else if (key == GoodsTable.gumiho3)
        {
            description.SetText($"<color=yellow>{CommonString.GetItemName(Item_Type.gumiho3)}</color>\n{CommonString.GetStatusName(StatusType.IgnoreDefense)}\n{PlayerStats.gumihoValue3  }");
        }
        else if (key == GoodsTable.gumiho4)
        {
            description.SetText($"<color=yellow>{CommonString.GetItemName(Item_Type.gumiho4)}</color>\n{CommonString.GetStatusName(StatusType.IgnoreDefense)}\n{PlayerStats.gumihoValue4  }");
        }
        else if (key == GoodsTable.gumiho5)
        {
            description.SetText($"<color=yellow>{CommonString.GetItemName(Item_Type.gumiho5)}</color>\n{CommonString.GetStatusName(StatusType.IgnoreDefense)}\n{PlayerStats.gumihoValue5  }");
        }
        else if (key == GoodsTable.gumiho6)
        {
            description.SetText($"<color=yellow>{CommonString.GetItemName(Item_Type.gumiho6)}</color>\n{CommonString.GetStatusName(StatusType.IgnoreDefense)}\n{PlayerStats.gumihoValue6  }");
        }
        else if (key == GoodsTable.gumiho7)
        {
            description.SetText($"<color=yellow>{CommonString.GetItemName(Item_Type.gumiho7)}</color>\n{CommonString.GetStatusName(StatusType.IgnoreDefense)}\n{PlayerStats.gumihoValue7  }");
        }
        else if (key == GoodsTable.gumiho8)
        {
            description.SetText($"<color=yellow>{CommonString.GetItemName(Item_Type.gumiho8)}</color>\n{CommonString.GetStatusName(StatusType.IgnoreDefense)}\n{PlayerStats.gumihoValue8  }");
        }
    }

    private void Subscribe()
    {
        ServerData.goodsTable.GetTableData(key).AsObservable().Subscribe(e =>
        {
            lockMask.SetActive(e == 0);
        }).AddTo(this);
    }
}
