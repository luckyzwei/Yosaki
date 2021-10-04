using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiCouponButton : MonoBehaviour
{
    void Start()
    {
        CheckIos();
    }

    private void CheckIos()
    {
#if UNITY_IOS
        this.gameObject.SetActive(false);
#endif
    }
}
