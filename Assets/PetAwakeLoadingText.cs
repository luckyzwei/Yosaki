using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PetAwakeLoadingText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI loadingText;

    private void OnEnable()
    {
        StartCoroutine(TextRoutine());
    }

    private IEnumerator TextRoutine() 
    {
        WaitForSeconds delay = new WaitForSeconds(0.3f);
        while (true) 
        {
            loadingText.SetText("강화중.");
            yield return delay;
            loadingText.SetText("강화중..");
            yield return delay;
            loadingText.SetText("강화중...");
            yield return delay;
        }
    }
}
