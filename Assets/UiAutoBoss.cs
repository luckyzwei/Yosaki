using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiAutoBoss : SingletonMono<UiAutoBoss>
{
    [SerializeField]
    private Toggle toggle;

    public static ReactiveProperty<bool> AutoMode = new ReactiveProperty<bool>(false);

    private new void Awake()
    {
        base.Awake();
    }

    public void WhenToggleChanged(bool on)
    {
        if (on)
        {
            PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "자동으로 진행 합니까?", () =>
            {
                AutoMode.Value = on;

                if (autoRoutine != null)
                {
                    StopCoroutine(autoRoutine);
                }

                autoRoutine = StartCoroutine(AutoRoutine());

            }, () =>
            {
                StopAutoBoss();
            });
        }

    }

    public void StopAutoBoss()
    {
        toggle.isOn = false;

        if (autoRoutine != null)
        {
            StopCoroutine(autoRoutine);
        }
    }

    private Coroutine autoRoutine;

    private IEnumerator AutoRoutine()
    {
        yield return null;
    }
}
