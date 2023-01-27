using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DayOfWeekResultPopup : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI enemyKillCountTxt;

    public void Initialize(int killCount)
    {
        enemyKillCountTxt.SetText($"{killCount} 처치 완료"!);
    }
}
