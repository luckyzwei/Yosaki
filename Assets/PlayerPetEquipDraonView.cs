using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPetEquipDraonView : MonoBehaviour
{

    [SerializeField]
    private Image icon;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.statusTable.GetTableData(StatusTable.PetEquip_Level).AsObservable().Subscribe(e =>
        {
            int idx = PlayerStats.GetCurrentDragonIdx();

            if (idx == -1)
            {
                this.icon.gameObject.SetActive(false);
            }
            else
            {
                this.icon.gameObject.SetActive(true);
                icon.sprite = CommonResourceContainer.GetDragonBallSprite(idx);
            }
        }).AddTo(this);
    }
}
