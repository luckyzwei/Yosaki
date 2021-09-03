﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiTwelveBossIcon : MonoBehaviour
{
    [SerializeField]
    private Image icon;

    [SerializeField]
    private List<Sprite> sprites;

    // Start is called before the first frame update
    void Start()
    {
        icon.sprite = sprites[GameManager.Instance.bossId];
    }
}
