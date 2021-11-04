using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UiRewardView;

public class UiSonBossResultPopup : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    public void Initialize(float damagedAmount)
    {
        scoreText.SetText(Utils.ConvertBigNum(damagedAmount));
    }
}
