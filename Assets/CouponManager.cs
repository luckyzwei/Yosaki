using BackEnd;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CouponManager : SingletonMono<CouponManager>
{
    private Action requestEndCallBack;

    public void RequestCoupon(string coupon, Action requestEndCallBack)
    {
        this.requestEndCallBack = requestEndCallBack;

        Backend.Coupon.UseCoupon(coupon, (bro) =>
        {
            // 이후 처리
            requestEndCallBack.Invoke();

            if (bro.IsSuccess())
            {
                SoundManager.Instance.PlaySound("GoldUse");

                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "쿠폰 보상을 수령했습니다!", null);

                int itemType = 0;

                int itemCount = 0;

                var data = bro.GetReturnValuetoJSON();

                if (data.ContainsKey("items"))
                {
                    itemType = int.Parse(data["items"]["ItemType"].ToString());
                }

                if (data.ContainsKey("itemsCount"))
                {
                    itemCount = int.Parse(data["itemsCount"].ToString());
                }

                ServerData.GetPostItem((Item_Type)itemType, itemCount);

            }
            else
            {
                ShowErrorPopup(bro.GetStatusCode());
            }
        });
    }

    public void ShowErrorPopup(string statusCode)
    {
        switch (statusCode)
        {
            case "404":
                {
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "이미 사용되었거나 잘못된 번호 입니다.", null);
                }
                break;
        }
    }

}
