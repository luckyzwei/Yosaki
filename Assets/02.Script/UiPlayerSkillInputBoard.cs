using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class UiPlayerSkillInputBoard : SingletonMono<UiPlayerSkillInputBoard>
{
    [SerializeField]
    private List<Image> skillIcons;

    [SerializeField]
    private List<GameObject> maskObjects;

    [SerializeField]
    private Sprite emptySprite;

    private List<ReactiveProperty<int>> selectedSkillIdxList;

    private CompositeDisposable disposables = new CompositeDisposable();

    private new void Awake()
    {
        base.Awake();

        Subscribe();

        LoadSkillSlotData();
    }

    private void LoadSkillSlotData()
    {
        WhenSelectedSkillIdxChanged(ServerData.skillServerTable.SelectedSkillIdx);
    }

    private void Subscribe()
    {
        ServerData.skillServerTable.whenSelectedSkillIdxChanged.AsObservable().Subscribe(WhenSelectedSkillIdxChanged).AddTo(this);
    }

    private void WhenSelectedSkillIdxChanged(List<ReactiveProperty<int>> list)
    {
        disposables.Clear();

        selectedSkillIdxList = list;

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Value == -1)
            {
                skillIcons[i].sprite = emptySprite;

                skillIcons[i].fillAmount = 0f;

              //  maskObjects[i].SetActive(false);
            }
            else
            {
               // maskObjects[i].SetActive(true);

                skillIcons[i].sprite = CommonResourceContainer.GetSkillIconSprite(list[i].Value);

                //쿨타임 관리
                SubscribeCoolTimeMask(list[i], i);
            }
        }
    }

    private void SubscribeCoolTimeMask(ReactiveProperty<int> skillId, int idx)
    {
        if (SkillCoolTimeManager.remainCool.ContainsKey(skillId.Value) == false)
        {
            SkillCoolTimeManager.SetActiveSkillCool(skillId.Value, 0f);
        }

        SkillCoolTimeManager.remainCool[skillId.Value].AsObservable().Subscribe(remainTime =>
        {
            float coolTimeMax = SkillCoolTimeManager.GetSkillCoolTimeMax(skillId.Value);

            skillIcons[idx].fillAmount = 1f - (remainTime / coolTimeMax);

        }).AddTo(disposables);
    }

    public void OnClickSKillToggle(int idx) 
    {
        if (AutoManager.Instance.IsAutoMode)
        {
            //체크박스
            if (SkillCoolTimeManager.registeredSkillIdx[idx].Value == 0)
            {
                SkillCoolTimeManager.SetUseSkill(idx);
                PopupManager.Instance.ShowAlarmMessage($"자동 스킬 등록 완료!");
            }
            else
            {
                SkillCoolTimeManager.RemoveUseSkill(idx);
                PopupManager.Instance.ShowAlarmMessage($"자동 스킬 등록 해제!");
            }

            //if (selectedSkillIdxList[idx].Value == -1)
            //{
            //    PopupManager.Instance.ShowAlarmMessage("스킬이 없습니다.");
            //}

            return;
        }
    }

    public void OnClickSkillSlot(int idx)
    {

        if (selectedSkillIdxList[idx].Value == -1)
        {
#if UNITY_EDITOR
            Debug.LogError("Slot is empty");
#endif
            return;
        }

        PlayerSkillCaster.Instance.UseSkill(selectedSkillIdxList[idx].Value);

    }

    private new void OnDestroy()
    {
        base.OnDestroy();
        disposables.Dispose();
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnClickSkillSlot(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnClickSkillSlot(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            OnClickSkillSlot(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            OnClickSkillSlot(3);
        }
    }

#endif
}
