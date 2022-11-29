using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using TMPro;

public class UiBeltView : MonoBehaviour
{
    [SerializeField]
    private Image beltIcon;

    [SerializeField]
    private TextMeshProUGUI abilDescription;

    private CaveBeltData caveBeltData;

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

    public void Initialize(CaveBeltData caveBeltData)
    {
        this.caveBeltData = caveBeltData;

        lockDescription.SetText($"{caveBeltData.Id + 1}단계");

        nameDescription.SetText(caveBeltData.Name);

        abilDescription.SetText($"장착효과\n{CommonString.GetStatusName((StatusType)caveBeltData.Abiltype)} {Utils.ConvertBigNum(caveBeltData.Abilvalue)}");

        beltIcon.sprite = CommonResourceContainer.GetBeltSprite(caveBeltData.Id);

        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.TableDatas[UserInfoTable.caveBelt].AsObservable().Subscribe(e =>
        {
            lockObject.SetActive(e <= this.caveBeltData.Id);
        }).AddTo(this);

        ServerData.equipmentTable.TableDatas[EquipmentTable.CaveBelt].AsObservable().Subscribe(e =>
        {
            equipImage.sprite = e == caveBeltData.Id ? unEquipSprite : equipSprite;
        }).AddTo(this);

        ServerData.equipmentTable.TableDatas[EquipmentTable.CaveBeltView].AsObservable().Subscribe(e =>
        {
            equipViewImage.sprite = e == caveBeltData.Id ? unEquipSprite : equipSprite;
        }).AddTo(this);
    }

    private bool IsUnlock()
    {
        return ServerData.userInfoTable.TableDatas[UserInfoTable.caveBelt].Value < caveBeltData.Id;
    }

    public void OnClickEquipButton()
    {
        if (IsUnlock())
        {
            return;
        }

        ServerData.equipmentTable.ChangeEquip(EquipmentTable.CaveBelt, caveBeltData.Id);
        ServerData.equipmentTable.ChangeEquip(EquipmentTable.CaveBeltView, caveBeltData.Id);

        PopupManager.Instance.ShowAlarmMessage("변경 완료");
    }
    public void OnClickEquipViewButton()
    {
        if (IsUnlock())
        {
            return;
        }

        ServerData.equipmentTable.ChangeEquip(EquipmentTable.CaveBeltView, caveBeltData.Id);

        PopupManager.Instance.ShowAlarmMessage("외형 변경 완료");
    }
}
