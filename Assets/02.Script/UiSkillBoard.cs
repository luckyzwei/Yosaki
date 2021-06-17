using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using BackEnd;

public class UiSkillBoard : MonoBehaviour
{
    [SerializeField]
    private UiSkillCell skillCellPrefab;

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

    private void Start()
    {
        UpdateView();
        UiManagerDescription.Instance.SetManagerDescription(ManagerDescriptionType.skillBoardDescription);
    }

    private void ShowSkillDescriptionPopup(SkillTableData data)
    {
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

            cell.Initialize(skillList[i], OnCliCkSlotSettingButton, ShowSkillDescriptionPopup);
        }

        var passiveSkillList = TableManager.Instance.PassiveSkill.dataArray.ToList();

        for(int i = 0; i < passiveSkillList.Count; i++) 
        {
            var cell = Instantiate<UiPassiveSkillCell>(passiveSkillCellPrefab, passiveSkillCellParent);

            cell.Refresh(passiveSkillList[i]);
        }
    }

    private void OnCliCkSlotSettingButton(int idx)
    {
        uiSkillSlotSettingBoard.gameObject.SetActive(true);
        uiSkillSlotSettingBoard.SetSkillIdx(idx);
    }
}
