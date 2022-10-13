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
        if (GameManager.Instance.bossId < CommonUiContainer.Instance.bossIcon.Count)
        {
            bossIcon.sprite = CommonUiContainer.Instance.bossIcon[GameManager.Instance.bossId];
        }
        else
        {
            bossIcon.gameObject.SetActive(false);
        }                    
    }
}
