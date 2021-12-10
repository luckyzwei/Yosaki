using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiMiniGamePlayer : MonoBehaviour
{
    [SerializeField]
    private Image thunbnailIcon;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeLook].AsObservable().Subscribe(e =>
        {
            thunbnailIcon.sprite = CommonUiContainer.Instance.GetCostumeThumbnail((int)e);
        }).AddTo(this);
    }


    private string bulletTag = "Bullet";
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals(bulletTag))
        {
            UiMinigameBoard.Instance.PlayerDamaged();
            collision.gameObject.SetActive(false);
        }
    }
}
