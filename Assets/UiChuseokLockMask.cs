using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiChuseokLockMask : MonoBehaviour
{
    [SerializeField]
    private GameObject rootObject;
    // Start is called before the first frame update
    private void OnEnable()
    {
        bool canBuy = ServerData.userInfoTable.CanBuyEventPackage();
        rootObject.gameObject.SetActive(!canBuy);
    }
}
