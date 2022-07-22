using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiHellBoard : MonoBehaviour
{
    [SerializeField]
    private Transform cellParents;

    [SerializeField]
    private UiHellAbilCell cellPrefab;

    void Start()
    {
        Intialize();
    }

    private void Intialize()
    {
        var tableDatas = TableManager.Instance.hellAbil.dataArray;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            var cell = Instantiate<UiHellAbilCell>(cellPrefab, cellParents);
            cell.Initialize(tableDatas[i]);
        }
    }
}
