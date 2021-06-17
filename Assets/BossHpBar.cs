using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI gaugeDescription;

    [SerializeField]
    private Image gauge;

    public void UpdateHpBar(float currentHp, float maxHp)
    {
        currentHp = Mathf.Max(0f, currentHp);

        gauge.fillAmount = currentHp / maxHp;

        gaugeDescription.SetText($"{Utils.ConvertBigFloat(currentHp)}/{Utils.ConvertBigFloat(maxHp)}");
    }
}
