using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using TMPro;

public class UiHornView : MonoBehaviour
{
    [SerializeField]
    private Image hornIcon;

    [SerializeField]
    private TextMeshProUGUI abilDescription;

    private DokebiHornData dokebiHornData;

    [SerializeField]
    private TextMeshProUGUI lockDescription;

    [SerializeField]
    private GameObject lockObject;

    [SerializeField]
    private Image equipViewImage;

    [SerializeField]
    private Image equipImage;

    [SerializeField]
    private Sprite equipSprite;

    [SerializeField]
    private Sprite unEquipSprite;

    [SerializeField]
    private TextMeshProUGUI nameDescription;

    private bool isUnlock = false;

    public void Initialize(DokebiHornData dokebiHornData)
    {
        this.dokebiHornData = dokebiHornData;

        lockDescription.SetText($"{dokebiHornData.Id + 1}단계");

        nameDescription.SetText(dokebiHornData.Name);

        abilDescription.SetText($"장착효과\n{CommonString.GetStatusName((StatusType)dokebiHornData.Abiltype)} {Utils.ConvertBigNum(dokebiHornData.Abilvalue * 100f)}%");

        hornIcon.sprite = CommonResourceContainer.GetHornSprite(dokebiHornData.Id);

        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].AsObservable().Subscribe(e =>
        {
            var list = e.Split(BossServerTable.rewardSplit);
            if(list.Length <= 1)
            {
                return;
            }
            for (int i = 0; i <list.Length; i++)
            {
                if(string.IsNullOrEmpty(list[i]))
                {
                    continue;
                }    
                if (int.Parse(list[i]) == this.dokebiHornData.Id)
                {
                    lockObject.SetActive(false);
                    isUnlock = true;
                    return;
                }
            }
        }).AddTo(this);
        ServerData.equipmentTable.TableDatas[EquipmentTable.DokebiHorn].AsObservable().Subscribe(e =>
        {
            equipImage.sprite = e == dokebiHornData.Id ? unEquipSprite : equipSprite;
        }).AddTo(this);

        ServerData.equipmentTable.TableDatas[EquipmentTable.DokebiHornView].AsObservable().Subscribe(e =>
        {
            equipViewImage.sprite = e == dokebiHornData.Id ? unEquipSprite : equipSprite;
        }).AddTo(this);
    }

    public void OnClickEquipButton()
    {
        if (!isUnlock)
        {
            return;
        }

        ServerData.equipmentTable.ChangeEquip(EquipmentTable.DokebiHorn, dokebiHornData.Id);
        ServerData.equipmentTable.ChangeEquip(EquipmentTable.DokebiHornView, dokebiHornData.Id);

        PopupManager.Instance.ShowAlarmMessage("변경 완료");
    }
    public void OnClickEquipViewButton()
    {
        if (!isUnlock)
        {
            return;
        }

        ServerData.equipmentTable.ChangeEquip(EquipmentTable.DokebiHornView, dokebiHornData.Id);

        PopupManager.Instance.ShowAlarmMessage("외형 변경 완료");
    }
}
