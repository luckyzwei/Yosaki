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

            this.transform.localScale = e ? Vector3.one * 3 : Vector3.one;

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
