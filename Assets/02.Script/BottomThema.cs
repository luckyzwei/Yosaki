using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomThema : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer bottom;

    void Start()
    {
        SetThema();
    }

    private void SetThema()
    {
        var themaInfo = GameManager.Instance.MapThemaInfo;
        bottom.sprite = themaInfo.centerTile;
    }
}
