using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiIosCouponBoard : MonoBehaviour
{
    [SerializeField]
    private UiIosCouponCell cellPrefab;

    [SerializeField]
    private Transform cellParent;


    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var tableData = TableManager.Instance.IosCoupon.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            var cell = Instantiate<UiIosCouponCell>(cellPrefab, cellParent);

            cell.Initialize(tableData[i]);
        }
    }
}
