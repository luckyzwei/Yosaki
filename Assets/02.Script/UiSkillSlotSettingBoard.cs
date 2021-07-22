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

    public static int currentSelectedSkillGroup = 0;

    [SerializeField]
    private List<Toggle> skillGroupToggles;

    public void WhenSkillGroupChanged(int group)
    {
        currentSelectedSkillGroup = group;

        // ServerData.userInfoTable.UpData(UserInfoTable.selectedSkillGroupId, currentSelectedSkillGroup, false);

        RefreshUi();

        if (AutoManager.Instance.IsAutoMode)
        {
            AutoManager.Instance.ResetSkillQueue();
        }
    }

    private void Start()
    {
        SetSkillGroup();
    }
    private void SetSkillGroup()
    {
        int currentSelectedGroupId = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.selectedSkillGroupId].Value;

        skillGroupToggles[currentSelectedGroupId].isOn = true;

        WhenSkillGroupChanged(currentSelectedGroupId);
    }

    private void RefreshUi()
    {
        selectedSkillIdx.Clear();
        var savedSlotData = ServerData.skillServerTable.GetSelectedSkillIdx(currentSelectedSkillGroup);
        for (int i = 0; i < savedSlotData.Count; i++)
        {
            selectedSkillIdx.Add(savedSlotData[i].Value);
        }

        UpdateSkillIcon();
    }

    public void SetSkillIdx(int currentSelectedSkillIdx)
    {
        this.currentSelectedSkillIdx = currentSelectedSkillIdx;
    }

    //새로운 스킬이 등록될때
    public void OnClickSkillSlot(int idx)
    {
        //var skillTableData = TableManager.Instance.SkillData[currentSelectedSkillIdx];
        //#if !UNITY_EDITOR
        //        for (int i = 0; i < selectedSkillIdx.Count; i++)
        //        {
        //            var prefSetSkillIdx = selectedSkillIdx[i];

        //            if (prefSetSkillIdx != -1)
        //            {
        //                var prefSkillData = TableManager.Instance.SkillData[prefSetSkillIdx];

        //                if (skillTableData.Id != prefSkillData.Id && skillTableData.Skilltype == prefSkillData.Skilltype)
        //                {
        //                    PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "같은 타입의 스킬은 등록이 불가능 합니다.\n 등록된 스킬을 제거하고 등록 합니까?",
        //                                    () =>
        //                                    {
        //                                        selectedSkillIdx[i] = -1;
        //                                        OnClickSkillSlot(idx);
        //                                    },
        //                                    () =>
        //                                    {

        //                                    });

        //                    return;
        //                }
        //            }
        //        }
        //#endif

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

        ServerData.skillServerTable.UpdateSelectedSkillIdx(selectedSkillIdx, currentSelectedSkillGroup);

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

    public void RemoveSkillInSlot(int id)
    {
        //
        ServerData.skillServerTable.RemoveSkillInEquipList(selectedSkillIdx[id], currentSelectedSkillGroup);

        if (AutoManager.Instance.IsAutoMode)
        {
            AutoManager.Instance.ResetSkillQueue();
        }

        WhenSkillGroupChanged(currentSelectedSkillGroup);
    }

    private void OnDisable()
    {
        UiSkillBoard.Instance.WhenSkillRegistered();
    }
}
