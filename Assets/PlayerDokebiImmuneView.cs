using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDokebiImmuneView : MonoBehaviour
{
    [SerializeField]
    private GameObject icon;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        //도깨비버프
        UiDokebiBuff.isImmune.AsObservable().Subscribe(e =>
        {
            icon.SetActive(e);

            if (e)
            {
                PopupManager.Instance.ShowAlarmMessage("도깨비 무적 효과로 1회 생존 합니다.");
            }


        }).AddTo(this);
    }
}
