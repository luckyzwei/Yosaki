using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiCostumePopup : MonoBehaviour
{
    private void OnDisable()
    {
        ServerData.costumeServerTable.ApplyAbilityByCurrentSelectedPreset();
    }
}
