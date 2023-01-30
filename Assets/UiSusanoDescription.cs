using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiSusanoDescription : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI abilDescription;

    [SerializeField]
    private TextMeshProUGUI immuneDescription;

    [SerializeField]
    private TextMeshProUGUI gradeText;

    [SerializeField]
    private TextMeshProUGUI unlockDesc;

    [SerializeField]
    private GameObject equipFrame;

    [SerializeField]
    private Image image;

    private int currentIdx;

    private void Start()
    {
        currentIdx = PlayerStats.GetSusanoGrade();

        Initialize(currentIdx);
    }

    public void Initialize(int idx)
    {
        if (idx == -1) idx = 0;

        var tableData = TableManager.Instance.susanoTable.dataArray[idx];
#if UNITY_EDITOR
        unlockDesc.SetText($"{Utils.ConvertBigNum(tableData.Score)}");
#else 
        unlockDesc.SetText($"{tableData.Scoredescription}");
#endif


        equipFrame.gameObject.SetActive(idx == PlayerStats.GetSusanoGrade());

        gradeText.SetText($"{idx + 1}단계");

        if (tableData.Abilvalue1 != 0)
        {
            abilDescription.SetText($"{CommonString.GetStatusName(StatusType.CriticalDam)}{Utils.ConvertBigNum(tableData.Abilvalue0 * 100f)}\n<color=yellow>{CommonString.GetStatusName(StatusType.PenetrateDefense)}{tableData.Abilvalue1 * 100f}</color>");
        }
        else
        {
            abilDescription.SetText($"{CommonString.GetStatusName(StatusType.CriticalDam)}{Utils.ConvertBigNum(tableData.Abilvalue0 * 100f)}");
        }

        immuneDescription.gameObject.SetActive(tableData.Buffsec != 0);

        if (tableData.Buffsec != 0)
        {
            immuneDescription.SetText($"피해면역 {tableData.Buffsec}초");
        }
        if (idx < 109)
        {
            image.sprite = Resources.Load<Sprite>($"Susano/{idx / 3}"); 
        }
        else
        {
            image.sprite = Resources.Load<Sprite>($"Susano/36");
        }
    }

    public void OnClickLeftButton()
    {
        currentIdx--;

        currentIdx = Mathf.Clamp(currentIdx, 0, TableManager.Instance.susanoTable.dataArray.Length - 1);

        Initialize(currentIdx);

        if (currentIdx == -1)
        {
            PopupManager.Instance.ShowAlarmMessage("처음 단계 입니다!");
        }
    }

    public void OnClickRightButton()
    {
        if (currentIdx == TableManager.Instance.susanoTable.dataArray.Length - 1)
        {
            PopupManager.Instance.ShowAlarmMessage("업데이트 예정 입니다!");
        }

        currentIdx++;

        currentIdx = Mathf.Clamp(currentIdx, 0, TableManager.Instance.susanoTable.dataArray.Length - 1);

        Initialize(currentIdx);

    }
}
