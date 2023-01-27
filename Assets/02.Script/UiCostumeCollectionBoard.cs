using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using BackEnd;

public class UiCostumeCollectionBoard : MonoBehaviour
{
    [SerializeField]
    private CostumeCollectionCell costumeCollectionCellPrefab;

    [SerializeField]
    private Transform cellParent;

    private List<CostumeCollectionCell> costumeCollectionCellList = new List<CostumeCollectionCell>();

    [SerializeField]
    private TextMeshProUGUI titleText;

    [SerializeField]
    private TextMeshProUGUI costumeAmount;

    private void OnEnable()
    {
        ActiveCheck();
    }

    private void ActiveCheck()
    {
        int costumeAmount = ServerData.costumeServerTable.GetCostumeHasAmount();

        if (costumeAmount < GameBalance.costumeCollectionUnlockNum)
        {
            PopupManager.Instance.ShowAlarmMessage($"외형 {GameBalance.costumeCollectionUnlockNum}개 이상일때 해금 됩니다!");
            this.gameObject.SetActive(false);
        }
    }


    void Start()
    {
        Initialize();

    }

    public void Initialize()
    {
        var stageDatas = TableManager.Instance.costumeCollection.dataArray;

        for (int i = 0; i < stageDatas.Length; i++)
        {
            costumeCollectionCellList.Add(Instantiate<CostumeCollectionCell>(costumeCollectionCellPrefab, cellParent));

        }
        for (int i = 0; i < costumeCollectionCellList.Count; i++)
        {
            costumeCollectionCellList[i].Initialize(stageDatas[i]);
        }

        costumeAmount.SetText($"외형 {ServerData.costumeServerTable.GetCostumeHasAmount()}개 보유중\n" +
            $"외형 강화 효과 {PlayerStats.IsCostumeCollectionEnhance()}배 적용됨");
    }


}
