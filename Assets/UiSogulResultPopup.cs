using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiSogulResultPopup : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI lastClearStageDesc;

    public void Initialize(int lastClearStage, bool clearLastStage)
    {
        this.gameObject.SetActive(true);

        lastClearStageDesc.SetText($"{lastClearStage}단계 돌파!!");
    }
}
