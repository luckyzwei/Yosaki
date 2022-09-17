using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ParyRaidConnectingMask : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI connectingText;

    private void OnEnable()
    {
        StartCoroutine(TextingRoutine());
    }

    private IEnumerator TextingRoutine()
    {
        var ws = new WaitForSeconds(0.5f);

        while (true)
        {
            connectingText.SetText($"십만대산으로 이동중.");
            yield return ws;
            connectingText.SetText($"십만대산으로 이동중.");
            yield return ws;
            connectingText.SetText($"십만대산으로 이동중..");
            yield return ws;
            connectingText.SetText($"십만대산으로 이동중...");
            yield return ws;

        }
    }
}
