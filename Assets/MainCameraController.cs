using Cinemachine;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class MainCameraController : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    [SerializeField]
    private ObscuredFloat viewMin;

    [SerializeField]
    private ObscuredFloat viewMax;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        SettingData.view.AsObservable().Subscribe(WhenViewChanged).AddTo(this);
    }

    private void WhenViewChanged(float view)
    {
        cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(viewMin, viewMax, view);
    }
}
