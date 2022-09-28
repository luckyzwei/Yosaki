using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Coffee.UIEffects;

public class AutoButton : MonoBehaviour
{
    [SerializeField]
    private GameObject autoEffect;

    private static bool prefAuto = true;

    private void Start()
    {
        Subscribe();
        StartCoroutine(AutoSetRoutine());
    }

    private IEnumerator AutoSetRoutine()
    {
        yield return new WaitForSeconds(1.5f);

        //보스전은 다른데서 자동전투 켜줌
        if (prefAuto == true && GameManager.contentsType.IsBossContents() == false)
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
