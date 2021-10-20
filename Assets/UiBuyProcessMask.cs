using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiBuyProcessMask : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI buyProcessText;

    private void OnEnable()
    {
        StartCoroutine(buyProcessRotuine());
    }

    private IEnumerator buyProcessRotuine()
    {
        WaitForSeconds delay = new WaitForSeconds(0.5f);
        while (true)
        {
            buyProcessText.SetText("구매 진행중.");
            yield return delay;
            buyProcessText.SetText("구매 진행중..");
            yield return delay;
            buyProcessText.SetText("구매 진행중...");
            yield return delay;
        }
    }
}
