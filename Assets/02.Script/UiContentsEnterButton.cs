using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiContentsEnterButton : MonoBehaviour
{
    [SerializeField]
    private GameManager.ContentsType contentsType;

    [SerializeField]
    private ObscuredInt bossId;

    public void OnClickButton()
    {
        UiContentsEnterPopup.Instance.Initialize(contentsType, bossId);
    }

    private void OnEnable()
    {
        StartCoroutine(RandomizeRoutine());
    }

    private IEnumerator RandomizeRoutine()
    {
        var delay = new WaitForSeconds(1.0f);

        while (true)
        {
            RandomizeKey();
            yield return delay;
        }
    }

    private void OnApplicationPause(bool pause)
    {
        RandomizeKey();
    }

    private void RandomizeKey() 
    {
        bossId.RandomizeCryptoKey();
    }

}
