using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSusanoView : MonoBehaviour
{
    [SerializeField]
    private Image icon;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        int idx = PlayerStats.GetSusanoGrade();

        icon.gameObject.SetActive(idx != -1);

        if (idx != -1)
        {
            icon.sprite = CommonResourceContainer.GetSusanoIcon();

            icon.material = CommonUiContainer.Instance.weaponEnhnaceMats[idx / 3];
        }

    }
}
