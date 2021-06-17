using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;

public class BatterySafeManager : SingletonMono<BatterySafeManager>
{
    [SerializeField]
    private GameObject battarySafeObjectRoot;

    [SerializeField]
    private Transform minPos;
    [SerializeField]
    private Transform maxPos;
    [SerializeField]
    private Transform descriptionObject;

    [SerializeField]
    private TextMeshProUGUI touchCountIndicator;

    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        clickCount.AsObservable().Subscribe(e =>
        {
            if (e != 0)
            {
                touchCountIndicator.SetText($"{e}/{unlockCountMax}");
            }
            else
            {
                touchCountIndicator.SetText(string.Empty);
            }
        }).AddTo(this);
    }

    public void SetBatterySafeMode(bool safeMode)
    {
        MainCamera.Instance.mainCam.enabled = !safeMode;
        InGameCanvas.Instance.canvas.enabled = !safeMode;
        battarySafeObjectRoot.SetActive(safeMode);

        if (safeMode)
        {
            this.transform.SetAsLastSibling();
            randomMoveRoutine = StartCoroutine(RandomMoveRoutine());
        }
        else
        {
            if (randomMoveRoutine != null)
            {
                StopCoroutine(randomMoveRoutine);
            }
        }

    }

    private Coroutine randomMoveRoutine;
    private IEnumerator RandomMoveRoutine()
    {
        WaitForSeconds randMoveDelay = new WaitForSeconds(5.0f);

        while (true)
        {
            yield return randMoveDelay;
            descriptionObject.transform.position = new Vector3(Random.Range(minPos.position.x, maxPos.position.x), Random.Range(minPos.position.y, maxPos.position.y), 0f);
        }
    }

    private Coroutine clickRoutine;
    private ReactiveProperty<int> clickCount = new ReactiveProperty<int>();
    private int unlockCountMax = 10;

    private IEnumerator ClickResetRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        clickCount.Value = 0;
    }
    public void OnClickRootObject()
    {
        SoundManager.Instance.PlayButtonSound();

        clickCount.Value++;

        if (clickCount.Value >= unlockCountMax)
        {
            SetBatterySafeMode(false);
            clickCount.Value = 0;
        }

        if (clickRoutine != null)
        {
            StopCoroutine(clickRoutine);
        }

        clickRoutine = StartCoroutine(ClickResetRoutine());


    }
}
