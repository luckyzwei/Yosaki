using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UiBuffIndicator : MonoBehaviour
{
    [SerializeField]
    private Image buffIconPrefab;

    [SerializeField]
    private Transform cellParent;

    private List<Image> buffIconList;

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        buffIconList = new List<Image>();

        var tableData = TableManager.Instance.BuffTable.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            var buffImage = Instantiate<Image>(buffIconPrefab, cellParent);

            buffImage.sprite = CommonUiContainer.Instance.buffIconList[i];

            buffIconList.Add(buffImage);

            buffImage.gameObject.SetActive(!tableData[i].Isyomulabil);
        }

        for (int i = 0; i < tableData.Length; i++)
        {
            Image buffImage = buffIconList[i];

            var buffTableData = tableData[i];

            ServerData.buffServerTable.TableDatas[tableData[i].Stringid].remainSec.AsObservable().Subscribe(e =>
            {
                buffImage.gameObject.SetActive((e == -1f || e > 0) && buffTableData.Isyomulabil == false);
            }).AddTo(this);
        }
    }
}
