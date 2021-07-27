using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiGoodsView : MonoBehaviour
{
    void Start()
    {
        this.gameObject.SetActive(GameManager.Instance.IsNormalField);
    }
}
