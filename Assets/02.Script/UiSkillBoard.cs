using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using BackEnd;

public class UiSkillBoard : SingletonMono<UiSkillBoard>
{
    [SerializeField]
    private UiSkillCell skillCellPrefab;

    private List<UiSkillCell> skillCells = new List<UiSkillCell>();

    [SerializeField]
    private Transform skillCellParent;

    [SerializeField]
    private UiPassiveSkillCell passiveSkillCellPrefab;

    [SerializeField]
    private Transform passiveSkillCellParent;

    [SerializeField]
    private UiSkillSlotSettingBoard uiSkillSlotSettingBoard;

    [SerializeField]
    private UiSkillDescriptionPopup uiSkillDescriptionPopup;

    private bool initialized = false;

    private void Start()
    {
        UpdateView();
    }

    private void UpdateSkillDescriptionPopup(SkillTableData data)
    {
        uiSkillDescriptionPopup.gameObject.SetActive(true);
        uiSkillDescriptionPopup.Initialize(data);
    }

    private void UpdateView()
    {
        var skillList = TableManager.Instance.SkillTable.dataArray.ToList();

        skillList.Sort((a, b) =>
        {
            if (a.Displayorder < b.Displayorder)
                return -1;

            return 1;
        });

        for (int i = 0; i < skillList.Count; i++)
        {
            var cell = Instantiate<UiSkillCell>(skillCellPrefab, skillCellParent);

            cell.Initialize(skillList[i], OnCliCkSlotSettingButton, UpdateSkillDescriptionPopup);

            skillCells.Add(cell);
        }

        var passiveSkillList = TableManager.Instance.PassiveSkill.dataArray.ToList();

        for (int i = 0; i < passiveSkillList.Count; i++)
        {
            var cell = Instantiate<UiPassiveSkillCell>(passiveSkillCellPrefab, passiveSkillCellParent);

            cell.Refresh(passiveSkillList[i]);
        }
    }

    public void WhenSkillRegistered()
    {
        for (int i = 0; i < skillCells.Count; i++)
        {
            skillCells[i].CheckUnlock(0);
        }
    }

    private void OnCliCkSlotSettingButton(int idx)
    {
        uiSkillSlotSettingBoard.gameObject.SetActive(true);
        uiSkillSlotSettingBoard.SetSkillIdx(idx);
    }
}
