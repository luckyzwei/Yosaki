using Cinemachine;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ContentsState
{
    Fight, Dead, TimerEnd, Clear
}

public class ContentsManagerBase : SingletonMono<ContentsManagerBase>
{
    [SerializeField]
    private PolygonCollider2D cameracollider;

    [SerializeField]
    protected ObscuredInt playTime = 60;

    protected Coroutine timerRoutine;

    [SerializeField]
    protected TextMeshProUGUI timerText;

    protected ObscuredInt remainSec = 0;
    public ObscuredInt RemainSec => remainSec;
    public ObscuredInt PlayTime => playTime;

    public virtual Transform GetMainEnemyObjectTransform()
    {
        return null;
    }

    public virtual float GetBossRemainHpRatio()
    {
        return 0f;
    }

    protected virtual IEnumerator ModeTimer()
    {
        WaitForSeconds ws = new WaitForSeconds(1.0f);

        remainSec = playTime;

        while (remainSec >= 0)
        {
            timerText.SetText($"남은시간 : {remainSec}");
            yield return ws;
            remainSec--;
        }

        TimerEnd();
    }


    protected virtual void TimerEnd() { }

    protected void Start()
    {
        SetCameraCollider();

        timerRoutine = StartCoroutine(ModeTimer());

        UiSubMenues.Instance.gameObject.SetActive(false);
    }

    protected void StopTimer()
    {
        StopCoroutine(timerRoutine);
    }

    private void SetCameraCollider()
    {
        var cameraConfiner = GameObject.FindObjectOfType<CinemachineConfiner>();
        cameraConfiner.m_BoundingShape2D = cameracollider;
    }
}
