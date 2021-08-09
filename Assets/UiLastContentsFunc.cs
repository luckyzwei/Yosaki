using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static GameManager;

public class UiLastContentsFunc : MonoBehaviour
{
    [SerializeField]
    private ContentsType contentsType;

    public UnityEvent myUnityEvent;

    void Start()
    {
        if (GameManager.Instance.IsNormalField && contentsType == GameManager.Instance.lastContentsType)
        {
            myUnityEvent?.Invoke();
        }
    }
}
