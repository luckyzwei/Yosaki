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
        AutoMode.Value = on;
    }
}
