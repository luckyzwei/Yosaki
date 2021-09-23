using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class UiHyeonmuArmor : MonoBehaviour
{
    [SerializeField]
    private GameObject rootObject;

    void Start()
    {
        CheckActive();
    }
    private void CheckActive()
    {
        ServerData.petEquipmentServerTable.TableDatas["petequip0"].hasAbil.AsObservable().Subscribe(e =>
        {
            rootObject.SetActive(e == 1);
        }).AddTo(this);
    }
}
