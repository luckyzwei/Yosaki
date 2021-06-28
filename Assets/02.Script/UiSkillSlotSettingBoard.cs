using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiSkillSlotSettingBoard : MonoBehaviour
{
    [SerializeField]
    private List<Image> skillSlots;
    private List<int> selectedSkillIdx = new List<int>();

    private int currentSelectedSkillIdx;

    [SerializeField]
    private Sprite emptyIcon;

    private void Awake()
    {
        Initialize();

        UpdateSkillIcon();
    }

    private void Initialize()
    {
        LoadPrefSavedData();
    }

    private void LoadPrefSavedData()
    {
        selectedSkillIdx.Clear();
        var savedSlotData = DatabaseManager.skillServerTable.SelectedSkillIdx;
        for (int i = 0; i < savedSlotData.Count; i++)
        {
            selectedSkillIdx.Add(savedSlotData[i].Value);
        }
    }

    public void SetSkillIdx(int currentSelectedSkillIdx)
    {
        this.currentSelectedSkillIdx = currentSelectedSkillIdx;
    }

    private void OnEnable()
    {
        LoadPrefSavedData();
        UpdateSkillIcon();
    }


    //새로운 스킬이 등록될때
    public void OnClickSkillSlot(int idx)
    {
        var skillTableData = TableManager.Instance.SkillData[currentSelectedSkillIdx];

        for (int i = 0; i < selectedSkillIdx.Count; i++)
        {
            var prefSetSkillIdx = selectedSkillIdx[i];

            if (prefSetSkillIdx != -1)
            {
                var prefSkillData = TableManager.Instance.SkillData[prefSetSkillIdx];

                if (skillTableData.Id != prefSkillData.Id && skillTableData.Skilltype == prefSkillData.Skilltype)
                {
                    PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "같은 타입의 스킬은 등록이 불가능 합니다.\n 등록된 스킬을 제거하고 등록 합니까?",
                                    () =>
                                    {
                                        selectedSkillIdx[i] = -1;
                                        OnClickSkillSlot(idx);
                                    },
                                    () =>
                                    {

                                    });

                    return;
                }
            }
        }




        if (AutoManager.Instance.IsAutoMode)
        {
            AutoManager.Instance.ResetSkillQueue();
        }

        for (int i = 0; i < selectedSkillIdx.Count; i++)
        {
            if (i == idx)
            {
                selectedSkillIdx[i] = currentSelectedSkillIdx;
            }
            else if (selectedSkillIdx[i] == currentSelectedSkillIdx)
            {
                selectedSkillIdx[i] = -1;
            }
        }

        DatabaseManager.skillServerTable.UpdateSelectedSkillIdx(selectedSkillIdx);

        UpdateSkillIcon();
    }

    public void UpdateSkillIcon()
    {
        for (int i = 0; i < skillSlots.Count; i++)
        {
            if (selectedSkillIdx[i] == -1)
            {
                skillSlots[i].color = new Color(0f, 0f, 0f, 0f);
            }
            else
            {
                skillSlots[i].color = Color.white;
                skillSlots[i].sprite = CommonResourceContainer.GetSkillIconSprite(selectedSkillIdx[i]);
            }
        }
    }
}
