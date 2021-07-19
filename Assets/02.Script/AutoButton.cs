using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Coffee.UIEffects;

public class AutoButton : MonoBehaviour
{
    [SerializeField]
    private GameObject autoEffect;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        AutoManager.Instance.AutoMode.AsObservable().Subscribe(WhenAutoModeChanged).AddTo(this);
    }

    private void WhenAutoModeChanged(bool auto)
    {
        autoEffect.gameObject.SetActive(auto);
    }

    public void OnClickAutoButton()
    {
//        if (GameManager.Instance.contentsType != GameManager.ContentsType.NormalField &&
//GameManager.Instance.contentsType != GameManager.ContentsType.FireFly)
//        {
//            PopupManager.Instance.ShowAlarmMessage("자동전투가 불가능한 던전 입니다.");
//            return;
//        }

        if (AutoManager.Instance.AutoMode.Value == true)
        {
            AutoManager.Instance.SetAuto(false);
        }
        else
        {
            AutoManager.Instance.SetAuto(true);
        }

    }
}
