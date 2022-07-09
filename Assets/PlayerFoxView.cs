using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerFoxView : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> foxTails;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.goodsTable.GetTableData(GoodsTable.gumiho0).AsObservable().Subscribe(e =>
        {

            foxTails[0].SetActive(e == 1);

        }).AddTo(this);

        ServerData.goodsTable.GetTableData(GoodsTable.gumiho1).AsObservable().Subscribe(e =>
        {

            foxTails[1].SetActive(e == 1);

        }).AddTo(this);

        ServerData.goodsTable.GetTableData(GoodsTable.gumiho2).AsObservable().Subscribe(e =>
        {

            foxTails[2].SetActive(e == 1);

        }).AddTo(this);

        ServerData.goodsTable.GetTableData(GoodsTable.gumiho3).AsObservable().Subscribe(e =>
        {

            foxTails[3].SetActive(e == 1);

        }).AddTo(this);

        ServerData.goodsTable.GetTableData(GoodsTable.gumiho4).AsObservable().Subscribe(e =>
        {

            foxTails[4].SetActive(e == 1);

        }).AddTo(this);

        ServerData.goodsTable.GetTableData(GoodsTable.gumiho5).AsObservable().Subscribe(e =>
        {

            foxTails[5].SetActive(e == 1);

        }).AddTo(this);

        ServerData.goodsTable.GetTableData(GoodsTable.gumiho6).AsObservable().Subscribe(e =>
        {

            foxTails[6].SetActive(e == 1);

        }).AddTo(this);

        ServerData.goodsTable.GetTableData(GoodsTable.gumiho7).AsObservable().Subscribe(e =>
        {

            foxTails[7].SetActive(e == 1);

        }).AddTo(this);

        ServerData.goodsTable.GetTableData(GoodsTable.gumiho8).AsObservable().Subscribe(e =>
        {

            foxTails[8].SetActive(e == 1);

        }).AddTo(this);
    }
}
