using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbbObject : MonoBehaviour
{
    private void OnEnable()
    {
        if (ABBManager.Instance != null)
        {
            ABBManager.Instance.Register(this);
        }
    }
    private void OnDisable()
    {
        if (ABBManager.Instance != null)
        {
            ABBManager.Instance.WhenObjectDisable(this);
        }
    }
}
