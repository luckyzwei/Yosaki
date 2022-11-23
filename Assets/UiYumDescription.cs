using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiYumDescription : MonoBehaviour
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
        currentIdx = PlayerStats.GetYumGrade();

        Initialize(currentIdx);
    }

    public void Initialize(int idx)
    {
        if (idx == -1) idx = 0;

        var tableData = TableManager.Instance.yumTable.dataArray[idx];

        unlockDesc.SetText($"{Utils.ConvertBigNum(tableData.Score)}");

        equipFrame.gameObject.SetActive(idx == PlayerStats.GetYumGrade());

        gradeText.SetText($"{idx + 1}단계");


      abilDescription.SetText($"{CommonString.GetStatusName(StatusType.SuperCritical3DamPer)}{Utils.ConvertBigNum(tableData.Abilvalue0 * 100f)}");
        


        image.sprite = Resources.Load<Sprite>($"Yum/{idx / 3}");
    }

    public void OnClickLeftButton()
    {
        currentIdx--;

        currentIdx = Mathf.Clamp(currentIdx, 0, TableManager.Instance.yumTable.dataArray.Length - 1);

        Initialize(currentIdx);

        if (currentIdx == -1)
        {
            PopupManager.Instance.ShowAlarmMessage("처음 단계 입니다!");
        }
    }

    public void OnClickRightButton()
    {
        if (currentIdx == TableManager.Instance.yumTable.dataArray.Length - 1)
        {
            PopupManager.Instance.ShowAlarmMessage("업데이트 예정 입니다!");
        }

        currentIdx++;

        currentIdx = Mathf.Clamp(currentIdx, 0, TableManager.Instance.yumTable.dataArray.Length - 1);

        Initialize(currentIdx);

    }
}
