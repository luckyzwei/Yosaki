using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiBossIconCell : MonoBehaviour
{
    [SerializeField]
    private Image bossIcon;

    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private GameObject notExistObject;

    private BossTableData bossTableData;

    [SerializeField]
    private GameObject selectedFrame;

    public void Initialize(BossTableData bossTableData)
    {
        this.bossTableData = bossTableData;

        bossIcon.sprite = CommonUiContainer.Instance.bossIcon[bossTableData.Id];

        Subscribe();
    }

    private void Subscribe()
    {
        DatabaseManager.bossServerTable.TableDatas[bossTableData.Stringid].artifactLevel.AsObservable().Subscribe(e =>
        {
            description.SetText($"LV{e} {bossTableData.Name}");
        }).AddTo(this);

        DatabaseManager.bossServerTable.TableDatas[bossTableData.Stringid].clear.AsObservable().Subscribe(e =>
        {
            notExistObject.SetActive(e == 0);
        }).AddTo(this);
    }

    public void SetSelectedFrame(bool onOff)
    {
        selectedFrame.SetActive(onOff);
    }

    public void OnClickCell()
    {
        if (notExistObject.activeInHierarchy == true)
        {
            PopupManager.Instance.ShowAlarmMessage("아직 클리어 하지 못했습니다.");
            return;
        }
        UiMemory.Instance.OnClickCell(bossTableData.Id);
    }

}
