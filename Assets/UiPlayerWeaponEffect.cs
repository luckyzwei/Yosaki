using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class UiPlayerWeaponEffect : MonoBehaviour
{
    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon_View].AsObservable().Subscribe(e =>
        {
            this.gameObject.SetActive(e == 20);
        }).AddTo(this);
    }
}
