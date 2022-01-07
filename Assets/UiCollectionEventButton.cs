using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiCollectionEventButton : MonoBehaviour
{
    private void OnEnable()
    {
        this.gameObject.SetActive(ServerData.userInfoTable.CanMakeEventItem());
    }
}
