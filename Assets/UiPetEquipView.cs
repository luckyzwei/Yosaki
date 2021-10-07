using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class UiPetEquipView : MonoBehaviour
{
    [SerializeField]
    private string equipKey;

    [SerializeField]
    private GameObject rootObject;

    void Start()
    {
        CheckActive();
    }
    private void CheckActive()
    {
        ServerData.petEquipmentServerTable.TableDatas[equipKey].hasAbil.AsObservable().Subscribe(e =>
        {
            rootObject.SetActive(e == 1);
        }).AddTo(this);
    }
}
