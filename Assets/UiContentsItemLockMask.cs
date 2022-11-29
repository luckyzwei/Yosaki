using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UiContentsItemLockMask : MonoBehaviour
{

    [SerializeField]
    private List<string> itemObjectkeys;


    private void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        for (int i = 0; i < itemObjectkeys.Count; i++)
        {
            if (HasItem(itemObjectkeys[i]))
            {
                if (i == itemObjectkeys.Count - 1)
                {
                    this.gameObject.SetActive(false);
                }
                continue;
            }
            else
            {
                break;
            }
        }
    }

    private bool HasItem(string itemkey)
    {
        return ServerData.goodsTable.GetTableData(itemkey).Value > 0;
    }
}
