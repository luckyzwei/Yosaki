using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiBossWarning : MonoBehaviour
{
    [SerializeField]
    private Image bossIcon;
    void Start()
    {
        SetBossIcon();
    }

    private void SetBossIcon()
    {

        bossIcon.sprite = CommonUiContainer.Instance.bossIcon[GameManager.Instance.bossId];
    }
}
