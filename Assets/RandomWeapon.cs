using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWeapon : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private void OnEnable()
    {
        spriteRenderer.sprite = CommonResourceContainer.GetRandomWeaponSprite();
    }
}
