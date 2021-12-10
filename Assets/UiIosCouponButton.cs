using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiIosCouponButton : MonoBehaviour
{
    void Start()
    {
        CheckPlatform();
    }

    private void CheckPlatform()
    {
#if UNITY_EDITOR || UNITY_IOS
        this.gameObject.SetActive(true);
#else 
        this.gameObject.SetActive(false);
#endif
    }
}
