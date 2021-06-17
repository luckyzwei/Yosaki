using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiWhiteEffect : MonoBehaviour
{
    public void WhenAnimEnd()
    {
        this.gameObject.SetActive(false);
    }
}
