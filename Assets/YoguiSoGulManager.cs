using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoguiSoGulManager : SingletonMono<YoguiSoGulManager>
{
    [SerializeField]
    private PolygonCollider2D cameracollider;

    [SerializeField]
    private List<GameObject> spawnPoints;

    private void Start()
    {
        SetCameraCollider();
        DisableUi();
    }
    private void DisableUi()
    {
        UiSubMenues.Instance.gameObject.SetActive(false);
    }

    private void SetCameraCollider()
    {
        var cameraConfiner = GameObject.FindObjectOfType<CinemachineConfiner>();
        cameraConfiner.m_BoundingShape2D = cameracollider;
    }
}
