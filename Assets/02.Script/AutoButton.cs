using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Coffee.UIEffects;

public class AutoButton : MonoBehaviour
{
    [SerializeField]
    private GameObject autoEffect;

    private static bool prefAuto = false;

    private void Start()
    {
        Subscribe();
        StartCoroutine(AutoSetRoutine());
    }

    private IEnumerator AutoSetRoutine()
    {
        yield return new WaitForSeconds(2.0f);

        if (prefAuto == true)
        {
            if (AutoManager.Instance.IsAutoMode == false)
            {
                AutoManager.Instance.SetAuto(true);
            }
        }
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
            prefAuto = false;
        }
        else
        {
            AutoManager.Instance.SetAuto(true);
            prefAuto = true;
        }

    }
}
