using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSusanoView : MonoBehaviour
{
    [SerializeField]
    private Image icon;

    private void Start()
    {
        Initialize();
        Subscribe();
    }

    private void Subscribe()
    {
        UiSusanoBuff.isImmune.AsObservable().Subscribe(e =>
        {
            if (e)
            {
                PopupManager.Instance.ShowAlarmMessage("악귀 무적 효과가 적용 됩니다");
            }

            this.transform.localScale = e ? Vector3.one * 2.5f : Vector3.one;

        }).AddTo(this);
    }

    private void Initialize()
    {
        int idx = PlayerStats.GetSusanoGrade();

        icon.gameObject.SetActive(idx != -1);

        if (idx != -1)
        {
            icon.sprite = CommonResourceContainer.GetSusanoIcon();

            icon.material = CommonUiContainer.Instance.weaponEnhnaceMats[idx / 3];
        }

    }
}
