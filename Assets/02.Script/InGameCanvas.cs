using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class InGameCanvas : SingletonMono<InGameCanvas>
{
    public Canvas canvas;

    [SerializeField]
    private Camera mainCam;

    public Camera MainCam => mainCam;

    [SerializeField]
    private CanvasScaler canvasScaler;

    private void Start()
    {
        Subsribe();
    }

    private void Subsribe()
    {
        SettingData.uiView.AsObservable().Subscribe(e =>
        {
            canvasScaler.matchWidthOrHeight = e;
        }).AddTo(this);
    }
}
