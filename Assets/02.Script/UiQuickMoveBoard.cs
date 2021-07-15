using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class UiQuickMoveBoard : MonoBehaviour
{
    [SerializeField]
    private UiStageCell stageCellPrefab;

    [SerializeField]
    private Transform cellParent;

    private List<UiStageCell> stageCellList = new List<UiStageCell>();

    private ReactiveProperty<int> currentPresetId = new ReactiveProperty<int>(1);

    [SerializeField]
    private Button leftButton;

    [SerializeField]
    private Button rightButton;

    [SerializeField]
    private TextMeshProUGUI titleText;

    void Start()
    {
        Subscribe();

        SetMyStageInfo();
    }

    private void SetMyStageInfo()
    {
        int myLastStageId = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.topClearStageId].Value;

        if (myLastStageId == -1) 
        {
            myLastStageId = 0;
        }

        var stageTableData = TableManager.Instance.StageMapData[myLastStageId];
        currentPresetId.Value = stageTableData.Mappreset;

        RefreshStage(currentPresetId.Value);
    }

    private void Subscribe()
    {
        currentPresetId.AsObservable().Subscribe(e =>
        {
            titleText.SetText($"{e}단계");

            int lastPreset = TableManager.Instance.GetLastStagePreset();

            rightButton.interactable = e != lastPreset;
            leftButton.interactable = e != 1;
        }
        ).AddTo(this);
    }
    public void RefreshStage(int mapPreset)
    {
        var stageDatas = TableManager.Instance.StageMapTable.dataArray;

        var selectedPresets = stageDatas.ToList().Where(e => e.Mappreset == mapPreset).Select(cell => cell).ToList();

        int makeCount = selectedPresets.Count - stageCellList.Count;

        for (int i = 0; i < makeCount; i++)
        {
            stageCellList.Add(Instantiate<UiStageCell>(stageCellPrefab, cellParent));
        }

        for (int i = 0; i < stageCellList.Count; i++)
        {
            if (i < selectedPresets.Count)
            {
                stageCellList[i].gameObject.SetActive(true);
                stageCellList[i].Initialize(selectedPresets[i]);
            }
            else
            {
                stageCellList[i].gameObject.SetActive(false);
            }
        }

    }

    public void OnClickLeftButton()
    {
        if (currentPresetId.Value == 1) return;
        currentPresetId.Value--;
        RefreshStage(currentPresetId.Value);
    }

    public void OnClickRightButton()
    {
        int lastThema = TableManager.Instance.GetLastStagePreset();
        if (currentPresetId.Value == lastThema) return;
        currentPresetId.Value++;
        RefreshStage(currentPresetId.Value);
    }

}
