using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StairThema : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer left;

    [SerializeField]
    private SpriteRenderer center;

    [SerializeField]
    private SpriteRenderer right;

    private void Start()
    {
        SetThema();
    }

    private void SetThema()
    {
        var themaInfo = GameManager.Instance.MapThemaInfo;
        left.sprite = themaInfo.leftTile;
        center.sprite = themaInfo.centerTile;
        right.sprite = themaInfo.rightTile;
    }
}
