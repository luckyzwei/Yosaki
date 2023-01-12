using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class UiSmithWoodBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private TextMeshProUGUI clearAmount;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.TableDatas[UserInfoTable.smithTreeClear].AsObservable().Subscribe(e =>
        {
            int addAmount = (int)(e * GameBalance.smithTreeAddValue);

            description.SetText($"{CommonString.GetItemName(Item_Type.SmithFire)} + {addAmount}개 추가 적용됨");

            clearAmount.SetText($"{Utils.ConvertBigNum(e)}");

        }).AddTo(this);

    }

}
