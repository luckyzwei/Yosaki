using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiWeaponEnhanceApply : MonoBehaviour
{
    private Image image;
    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        image = GetComponent<Image>();

        if (image != null)
        {
            ServerData.equipmentTable.TableDatas[EquipmentTable.WeaponE_View].AsObservable().Subscribe(e =>
            {
                if (e < CommonUiContainer.Instance.weaponEnhnaceMats.Count)
                {
                    image.material = CommonUiContainer.Instance.weaponEnhnaceMats[e];
                }
            }).AddTo(this);
        }
    }
}
