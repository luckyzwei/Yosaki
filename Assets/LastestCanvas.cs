using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class LastestCanvas : SingletonMono<LastestCanvas>
{
    [SerializeField]
    private GameObject buyProcessObject;

    private void Start()
    {
        Subscribe();
    }
    private void Subscribe()
    {
        IAPManager.Instance.disableBuyButton.AsObservable().Subscribe(e =>
        {
            buyProcessObject.SetActive(true);
        }).AddTo(this);

        IAPManager.Instance.activeBuyButton.AsObservable().Subscribe(e =>
        {
            buyProcessObject.SetActive(false);
        }).AddTo(this);
    }
}
