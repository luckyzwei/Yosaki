using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UiContentsItemLockMask : MonoBehaviour
{

    [SerializeField]
    private List<string> itemObjectkeys;
    void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        this.gameObject.SetActive(false);
        for (int i = 0; i < itemObjectkeys.Count; i++)
        {
            if (!HasItem(itemObjectkeys[i]))
            {
                this.gameObject.SetActive(true);
                break;
            }
        }
    }

    private bool HasItem(string itemkey)
    {
        return ServerData.goodsTable.GetTableData(itemkey).Value > 0;
    }
}
