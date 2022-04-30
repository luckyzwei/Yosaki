using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventReceiver : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private string funcName;

    public void WhenAnimEnd_1()
    {
        if (string.IsNullOrEmpty(funcName) == false)
        {
            target.SendMessage(funcName);
        }
    }
}
