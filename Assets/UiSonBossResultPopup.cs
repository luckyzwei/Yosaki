using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UiRewardView;

public class UiSonBossResultPopup : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    public void Initialize(double damagedAmount)
    {
        scoreText.SetText(Utils.ConvertBigNum(damagedAmount));
    }
}
