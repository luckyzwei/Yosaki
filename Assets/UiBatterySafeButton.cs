using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiBatterySafeButton : MonoBehaviour
{
    public void OnClickButton()
    {
        BatterySafeManager.Instance.SetBatterySafeMode(true);
    }
}
