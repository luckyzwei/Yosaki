using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiInfinityHardText : MonoBehaviour
{
    void Start()
    {
        this.gameObject.SetActive(ServerData.userInfoTable.IsLastFloor());
    }
}
