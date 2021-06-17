using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiCouponPopup : MonoBehaviour
{
    [SerializeField]
    private Button requestButton;

    private string couponId;

    public void WhenTextChanged(string couponId)
    {
        this.couponId = couponId;
    }

    public void OnClickSendButton()
    {
        requestButton.interactable = false;

        CouponManager.Instance.RequestCoupon(couponId, WhenRequestEnd);
    }

    private void WhenRequestEnd()
    {
        requestButton.interactable = true;
    }
}
