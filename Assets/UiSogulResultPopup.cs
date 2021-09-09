using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiSogulResultPopup : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI lastClearStageDesc;

    [SerializeField]
    private GameObject deadGameObject;

    [SerializeField]
    private GameObject clearObject;

    public void Initialize(int lastClearStage, bool clearLastStage, bool isPlayerDead)
    {
        this.gameObject.SetActive(true);

        lastClearStageDesc.SetText($"{lastClearStage}단계 돌파!!");

        deadGameObject.SetActive(isPlayerDead);

        clearObject.SetActive(clearLastStage);
    }
}
