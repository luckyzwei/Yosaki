using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class MyEvent : UnityEvent<int> { }

public class UiSkillInputButton : MonoBehaviour
{
    private Coroutine autoUpRoutine;

    private WaitForSeconds autuUpDelay = new WaitForSeconds(0.01f);

    public MyEvent useSkillFunc;
    public int slotIdx;

    public void PointerDown()
    {
        if (AutoManager.Instance.IsAutoMode)
        {
            PopupManager.Instance.ShowAlarmMessage("자동 모드에서는 사용할수 없습니다.");
            return;
        }

        if (autoUpRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(autoUpRoutine);
        }

        autoUpRoutine = CoroutineExecuter.Instance.StartCoroutine(AutuUpgradeRoutine());

    }
    public void PointerUp()
    {
        if (autoUpRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(autoUpRoutine);
        }
    }

    private void OnDestroy()
    {
        if (autoUpRoutine != null)
        {
            StopCoroutine(autoUpRoutine);
        }
    }

    private IEnumerator AutuUpgradeRoutine()
    {
        while (true)
        {
            useSkillFunc.Invoke(slotIdx);
            yield return autuUpDelay;
        }
    }
}
