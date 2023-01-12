using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiMessageBoardAdjust : MonoBehaviour
{
    
    private void Start()
    {

#if UNITY_IOS
        RectTransform rect = GetComponent<RectTransform>();

        rect.localPosition = new Vector3(0,16f);
#endif
    }
}
