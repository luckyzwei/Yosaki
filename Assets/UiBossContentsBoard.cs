using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiBossContentsBoard : MonoBehaviour
{
    [SerializeField]
    private UiBossContentsView uiBossContentsViewPrefab;

    [SerializeField]
    private Transform cellParent;
    
    void Start()
    {
        Initialize();
    }

    private void Initialize() 
    {
        var tableDatas = TableManager.Instance.BossTable.dataArray;

        for (int i=0;i< tableDatas.Length; i++) 
        {
            var cell = Instantiate<UiBossContentsView>(uiBossContentsViewPrefab, cellParent);

            cell.Initialize(tableDatas[i]);
        }

    }
}
