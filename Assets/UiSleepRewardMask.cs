using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiSleepRewardMask : SingletonMono<UiSleepRewardMask>
{
    [SerializeField]
    private GameObject rootObject;

    [SerializeField]
    private TextMeshProUGUI description;

    private Coroutine textingRoutine;

    void Start()
    {
        ShowMaskObject(false);
    }

    public void ShowMaskObject(bool show)
    {
        rootObject.SetActive(show);

        if (show)
        {
            if (textingRoutine != null)
            {
                StopCoroutine(textingRoutine);
            }

            textingRoutine = StartCoroutine(TextingRoutine()); ;
        }
        else
        {
            if (textingRoutine != null)
            {
                StopCoroutine(textingRoutine);
            }
        }
    }

    private IEnumerator TextingRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(0.2f);

        while (true)
        {
            description.SetText("휴식보상 처리중입니다.");
            yield return delay;
            description.SetText("휴식보상 처리중입니다..");
            yield return delay;
            description.SetText("휴식보상 처리중입니다...");
            yield return delay;
            description.SetText("휴식보상 처리중입니다....");
            yield return delay;
        }
    }

}
