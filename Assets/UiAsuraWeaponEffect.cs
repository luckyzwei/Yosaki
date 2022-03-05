using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class UiAsuraWeaponEffect : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> effectObject;

    void Start()
    {
        ServerData.goodsTable.GetTableData("a0").AsObservable().Subscribe(e =>
        {
            effectObject[0].SetActive(e == 1);
        }).AddTo(this);

        ServerData.goodsTable.GetTableData("a1").AsObservable().Subscribe(e =>
        {
            effectObject[1].SetActive(e == 1);
        }).AddTo(this);

        ServerData.goodsTable.GetTableData("a2").AsObservable().Subscribe(e =>
        {
            effectObject[2].SetActive(e == 1);
        }).AddTo(this);

        ServerData.goodsTable.GetTableData("a3").AsObservable().Subscribe(e =>
        {
            effectObject[3].SetActive(e == 1);
        }).AddTo(this);

        ServerData.goodsTable.GetTableData("a4").AsObservable().Subscribe(e =>
        {
            effectObject[4].SetActive(e == 1);
        }).AddTo(this);

        ServerData.goodsTable.GetTableData("a5").AsObservable().Subscribe(e =>
        {
            effectObject[5].SetActive(e == 1);
        }).AddTo(this);

    }
 
}
