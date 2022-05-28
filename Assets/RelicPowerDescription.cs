using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RelicPowerDescription : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private TextMeshProUGUI abilValueDescription;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        description.SetText($"영혼의숲 최고점수 100마다\n특수무공 능력치가 증가 합니다.\n(공격력증가,크리티컬데미지)");
        abilValueDescription.SetText($"{GameBalance.forestValue}배");
    }
}
