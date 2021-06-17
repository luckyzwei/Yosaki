using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapThema : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer backGround;

    void Start()
    {
        SetThema();
    }

    private void SetThema()
    {
        var themaInfo = GameManager.Instance.MapThemaInfo;
        backGround.sprite = themaInfo.backGround;
    }
}
