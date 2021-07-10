using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class UiMarbleForceIndicator : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> circleObject;

    [SerializeField]
    private List<Image> circleActibeObject;

    [SerializeField]
    private bool isInventoryBoard = false;

    [SerializeField]
    private float radius;

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

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        if (isInventoryBoard == false)
        {
            circleObject.ForEach(element => element.gameObject.SetActive(false));
        }

        var tableDatas = TableManager.Instance.MarbleTable.dataArray;

        for (int i = 0; i < tableDatas.Length; i++)
        {
            int currentIdx = i;

            DatabaseManager.marbleServerTable.TableDatas[tableDatas[i].Stringid].hasItem.AsObservable().Subscribe(e =>
            {
                if (isInventoryBoard == true)
                {
                    circleActibeObject[currentIdx].color = new Color(circleActibeObject[currentIdx].color.r, circleActibeObject[currentIdx].color.g, circleActibeObject[currentIdx].color.b, (e == 1 ? 1f : 0.25f));
                }
                else
                {
                    if (e == 1)
                    {
                        //하나라도 있을때 켜줌
                        circleObject.ForEach(element => element.gameObject.SetActive(true));
                    }

                    circleActibeObject[currentIdx].gameObject.SetActive(e == 1);
                }

            }).AddTo(this);
        }
    }
}
