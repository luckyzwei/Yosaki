using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxMaskBossMask : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        int stageId = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.foxMask).Value;
        spriteRenderer.sprite = CommonResourceContainer.GetMaskSprite(stageId);
    }
}
