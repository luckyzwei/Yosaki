using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UiPlayerHornView : MonoBehaviour
{
    [SerializeField]
    private Image hornView;


    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.equipmentTable.TableDatas[EquipmentTable.DokebiHornView].AsObservable().Subscribe(e =>
        {
            hornView.gameObject.SetActive(e != -1);

            if (e != -1)
            {
                hornView.sprite = CommonResourceContainer.GetHornSprite(e);
            }
        }).AddTo(this);
    }
}
