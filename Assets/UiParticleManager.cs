using Coffee.UIExtensions;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class UiParticleManager : MonoBehaviour
{
    [SerializeField]
    private List<UIParticle> particles;
    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        SettingData.ShowEffect.AsObservable().Subscribe(e =>
        {

            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].enabled = e == 1;
            }

        }).AddTo(this);
    }
}
