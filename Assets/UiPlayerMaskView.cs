using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UiPlayerMaskView : MonoBehaviour
{
    [SerializeField]
    private Image maskView;


    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.equipmentTable.TableDatas[EquipmentTable.FoxMaskView].AsObservable().Subscribe(e =>
        {
            maskView.gameObject.SetActive(e != -1);

            if (e != -1)
            {
                maskView.sprite = CommonResourceContainer.GetMaskSprite(e);
            }
        }).AddTo(this);
    }
}
