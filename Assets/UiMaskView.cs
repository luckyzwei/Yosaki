using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using TMPro;

public class UiMaskView : MonoBehaviour
{
    [SerializeField]
    private Image maskIcon;

    [SerializeField]
    private TextMeshProUGUI abilDescription;

    private FoxMaskData foxMaskData;

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

    public void Initialize(FoxMaskData foxMaskData)
    {
        this.foxMaskData = foxMaskData;

        lockDescription.SetText($"{foxMaskData.Id + 1}단계");

        nameDescription.SetText(foxMaskData.Name);

        abilDescription.SetText($"장착효과\n{CommonString.GetStatusName((StatusType)foxMaskData.Abiltype)} {Utils.ConvertBigNum(foxMaskData.Abilvalue * 100f)}%");

        maskIcon.sprite = CommonResourceContainer.GetMaskSprite(foxMaskData.Id);

        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.TableDatas[UserInfoTable.foxMask].AsObservable().Subscribe(e =>
        {
            lockObject.SetActive(e <= this.foxMaskData.Id);
        }).AddTo(this);

        ServerData.equipmentTable.TableDatas[EquipmentTable.FoxMask].AsObservable().Subscribe(e =>
        {
            equipImage.sprite = e == foxMaskData.Id ? unEquipSprite : equipSprite;
        }).AddTo(this);

        ServerData.equipmentTable.TableDatas[EquipmentTable.FoxMaskView].AsObservable().Subscribe(e =>
        {
            equipViewImage.sprite = e == foxMaskData.Id ? unEquipSprite : equipSprite;
        }).AddTo(this);
    }

    private bool IsUnlock()
    {
        return ServerData.userInfoTable.TableDatas[UserInfoTable.foxMask].Value < foxMaskData.Id;
    }

    public void OnClickEquipButton()
    {
        if (IsUnlock())
        {
            return;
        }

        ServerData.equipmentTable.ChangeEquip(EquipmentTable.FoxMask, foxMaskData.Id);
        ServerData.equipmentTable.ChangeEquip(EquipmentTable.FoxMaskView, foxMaskData.Id);

        PopupManager.Instance.ShowAlarmMessage("변경 완료");
    }
    public void OnClickEquipViewButton()
    {
        if (IsUnlock())
        {
            return;
        }

        ServerData.equipmentTable.ChangeEquip(EquipmentTable.FoxMaskView, foxMaskData.Id);

        PopupManager.Instance.ShowAlarmMessage("외형 변경 완료");
    }
}
