using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiChuseokLockMask : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        bool canBuy = ServerData.userInfoTable.CanBuyEventPackage();
        this.gameObject.SetActive(!canBuy);
    }
}
