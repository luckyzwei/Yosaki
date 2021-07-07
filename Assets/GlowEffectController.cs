using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GlowEffectController : MonoBehaviour
{
    [SerializeField]
    private Slashes_MobileBloom mobileBloom;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        SettingData.GlowEffect.AsObservable().Subscribe(WhenEffectOptionChanged).AddTo(this);
    }

    private void WhenEffectOptionChanged(int option)
    {
        mobileBloom.enabled = option == 1;
    }
}
