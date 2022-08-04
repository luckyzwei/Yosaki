using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class YeoRaeMarbleView : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> circleObject;

    [SerializeField]
    private float radius;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.goodsTable.GetTableData(GoodsTable.Ym).AsObservable().Subscribe(count =>
        {
            circleObject.ForEach(e => e.SetActive(false));

            for (int i = 0; i < count; i++)
            {
                if (i < circleObject.Count)
                {
                    circleObject[i].SetActive(true);
                }
            }

        }).AddTo(this);
    }


#if UNITY_EDITOR
    private void OnValidate()
    {
        if (circleObject.Count == 0) return;
        if (GetActiveCount() == 0) return;

        float angle = 360f / GetActiveCount();

        for (int i = 0; i < circleObject.Count; i++)
        {
            circleObject[i].transform.position = this.transform.position + Quaternion.Euler(0f, 0f, angle * i) * Vector3.up * radius;
        }
    }

    private int GetActiveCount()
    {
        int ret = 0;

        for (int i = 0; i < circleObject.Count; i++)
        {
            if (circleObject[i].activeInHierarchy == true) ret++;
        }

        return ret;
    }

#endif


}
