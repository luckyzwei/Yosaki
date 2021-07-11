using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class WeaponEquipView : MonoBehaviour
{
    [SerializeField]
    private Image weaponImage;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon].AsObservable().Subscribe(WhenEquipIdxChanged).AddTo(this);

    }

    private void WhenEquipIdxChanged(int idx)
    {
        weaponImage.sprite = CommonResourceContainer.GetWeaponSprite(idx);


        //var weaponGrade = TableManager.Instance.WeaponData[idx].Grade;

        //var emission = equipEffect.emission;
        //emission.rateOverTime = weaponGrade * 5;
    }

}
