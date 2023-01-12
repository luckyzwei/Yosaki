using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiDokebiSpecialAbilBoard : MonoBehaviour
{
    [SerializeField]
    private WeaponView weaponView_0;

    [SerializeField]
    private WeaponView weaponView_1;

    [SerializeField]
    private WeaponView weaponView_2;


    void Start()
    {
        SetWeaponViews(); ;

    }

    private void SetWeaponViews() 
    {
        weaponView_0.Initialize(TableManager.Instance.WeaponData[77], null);
        weaponView_1.Initialize(TableManager.Instance.WeaponData[78], null);
        weaponView_2.Initialize(TableManager.Instance.WeaponData[79], null);
    }
    
}
