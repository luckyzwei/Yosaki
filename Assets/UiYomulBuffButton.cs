using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiYomulBuffButton : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return null;

        CheckActive();
    }

    private void CheckActive()
    {
        this.gameObject.SetActive(UiTutorialManager.Instance.isAllCleared);
    }
}
