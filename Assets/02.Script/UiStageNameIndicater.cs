using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiStageNameIndicater : SingletonMono<UiStageNameIndicater>
{
    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private GameObject buttonRoot;

    [SerializeField]
    private GameObject leftButton;

    [SerializeField]
    private GameObject rightButtln;

    [SerializeField]
    private GameObject monsterCountView;

    [SerializeField]
    private GameObject fieldBossRemainObj;

    [SerializeField]
    private TextMeshProUGUI fieldBossRemainSec;

    public ReactiveCommand whenFieldBossTimerEnd = new ReactiveCommand();

    [SerializeField]
    private Image fieldBossTimer;

    private Coroutine bossTimerRoutine;

    public void StartFieldBossTimer(int timer)
    {
        StopFieldBossTimer();

        fieldBossRemainObj.SetActive(true);

        bossTimerRoutine = StartCoroutine(FieldBossRoutine(timer));
    }

    public void StopFieldBossTimer()
    {
        fieldBossRemainObj.SetActive(false);

        if (bossTimerRoutine != null)
        {
            StopCoroutine(bossTimerRoutine);
        }
    }

    private IEnumerator FieldBossRoutine(int timer)
    {
        float remainSec = timer;

        fieldBossTimer.fillAmount = 1f;

        while (remainSec > 0)
        {
            yield return null;
            remainSec -= Time.deltaTime;
            fieldBossTimer.fillAmount = (remainSec / (float)timer);
            fieldBossRemainSec.SetText($"남은시간 {(int)remainSec}");
        }

        fieldBossTimer.fillAmount = 0f;

        whenFieldBossTimerEnd.Execute();
    }


    void Start()
    {
        if (GameManager.Instance.contentsType != GameManager.ContentsType.NormalField)
        {
            this.gameObject.SetActive(false);
            return;
        }

        int stageId = GameManager.Instance.CurrentStageData.Id;


        int pref = (stageId - 1) / 6 + 1;
        int def = (stageId - 1) % 6 + 1;

        if (stageId != 0)
        {
            description.SetText($"{pref}-{def}");
        }
        else
        {
            description.SetText($"{CommonString.ThemaName[0]}");
        }

        monsterCountView.SetActive(GameManager.Instance.contentsType == GameManager.ContentsType.NormalField);

        SetArrowButtons();

        fieldBossRemainObj.SetActive(false);
    }

    private void SetArrowButtons()
    {
        if (GameManager.Instance.contentsType != GameManager.ContentsType.NormalField)
        {
            buttonRoot.SetActive(false);
        }
        else
        {
            leftButton.SetActive(!GameManager.Instance.IsFirstScene());
            rightButtln.SetActive(!GameManager.Instance.IsLastScene());
        }
    }

    public string GetStageName(int id)
    {
        if (GameManager.Instance.contentsType == GameManager.ContentsType.NormalField)
        {
            if (id == 0)
            {
                return CommonString.ThemaName[0];
            }
            else
            {
                return $"스테이지 : {id}";
            }
        }
        else
        {
            return CommonString.GetContentsName(GameManager.Instance.contentsType);
        }
    }

    public void OnClickLeftButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "이전 스테이지로 이동합니까?", () =>
        {
            SoundManager.Instance.PlayButtonSound();
            GameManager.Instance.LoadBackScene();
        }, null);
    }
    public void OnClickRightButton()
    {
        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "다음 스테이지로 이동합니까?", () =>
        {
            SoundManager.Instance.PlayButtonSound();
            GameManager.Instance.LoadNextScene();
        }, null);
    }
}
