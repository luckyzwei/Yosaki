using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class IndraRotater : MonoBehaviour
{
    [SerializeField]
    private GameObject rotateObject;

    [SerializeField]
    private GameObject indra0;

    [SerializeField]
    private GameObject indra1;

    [SerializeField]
    private GameObject indra2;

    public float moveSpeed = 1f;
    public float radius = 1f;
    private float currentAngle;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.goodsTable.GetTableData(GoodsTable.Indra0).AsObservable().Subscribe(e =>
        {
            if (ServerData.goodsTable.GetTableData(GoodsTable.Indra1).Value == 0 && ServerData.goodsTable.GetTableData(GoodsTable.Indra2).Value == 0)
            {
                indra0.SetActive(e >= 1);
            }

        }).AddTo(this);

        ServerData.goodsTable.GetTableData(GoodsTable.Indra1).AsObservable().Subscribe(e =>
            {
                if (ServerData.goodsTable.GetTableData(GoodsTable.Indra2).Value == 0)
                {
                    indra0.SetActive(false);
                    indra1.SetActive(e >= 1);
                }
            }).AddTo(this);

        ServerData.goodsTable.GetTableData(GoodsTable.Indra2).AsObservable().Subscribe(e =>
            {
                if (e >= 1)
                {
                    indra0.SetActive(false);
                    indra1.SetActive(false);
                    indra2.SetActive(e == 1);
                }
                else
                {
                    indra2.SetActive(false);
                }


            }).AddTo(this);
    }

    void Update()
    {
        currentAngle += Time.deltaTime * moveSpeed;

        rotateObject.transform.localPosition = Quaternion.Euler(0f, 0f, currentAngle) * Vector3.right * radius;

        if (currentAngle >= 360f)
        {
            currentAngle = currentAngle - 360f;
        }
    }
}
