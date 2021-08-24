using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;
using System;

public class UiCostumeAbilityCell : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI description;

    private CostumeData costumeData;

    private CompositeDisposable disposable = new CompositeDisposable();

    [SerializeField]
    private GameObject lockMask;

    [SerializeField]
    private Image lockIcon;

    [SerializeField]
    private Sprite lockedSprite;

    [SerializeField]
    private Sprite unlockSprite;

    [SerializeField]
    private Image gradeBg;

    private ObscuredInt slotId;

    private CostumeAbilityData abilityData;

    [SerializeField]
    private List<Sprite> gradeSprite;

    private Action whenCostumeLocked;

    private void OnDestroy()
    {
        disposable.Dispose();
    }

    public void Initialize(CostumeData costumeData, int abilityIdx, int slotId, Action whenCostumeLocked)
    {
        this.whenCostumeLocked = whenCostumeLocked;

        this.costumeData = costumeData;

        this.abilityData = TableManager.Instance.CostumeAbilityData[abilityIdx];

        this.slotId = slotId;

        description.SetText($"{abilityData.Description}");

        Subscribe();

        SetGradeBg();
    }

    private void SetGradeBg()
    {
        int grade = abilityData.Grade - 1;

        grade = Mathf.Max(0, grade);
        //grade 1부터
        gradeBg.sprite = gradeSprite[grade];
    }

    private void Subscribe()
    {
        disposable.Clear();

        var serverData = ServerData.costumeServerTable.TableDatas[costumeData.Stringid];

        serverData.lockIdx[slotId].AsObservable().Subscribe(e =>
        {
            bool isLocked = e == 1;

            lockIcon.sprite = isLocked ? lockedSprite : unlockSprite;

            lockMask.SetActive(isLocked);


        }).AddTo(disposable);
    }

    public void OnClickLockButton()
    {
        var serverData = ServerData.costumeServerTable.TableDatas[costumeData.Stringid];

        if (serverData.hasCostume.Value == false)
        {
            PopupManager.Instance.ShowAlarmMessage("외형이 없습니다.");
            return;
        }

        serverData.lockIdx[slotId].Value = serverData.lockIdx[slotId].Value == 1 ? 0 : 1;

        //서버 싱크
        if (lockSyncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(lockSyncRoutine);
        }

        lockSyncRoutine = CoroutineExecuter.Instance.StartCoroutine(LockSyncRoutine(costumeData.Stringid));

        whenCostumeLocked.Invoke();
    }

    private Coroutine lockSyncRoutine;

    private WaitForSeconds syncDelay = new WaitForSeconds(2.0f);
    public IEnumerator LockSyncRoutine(string key)
    {
        yield return syncDelay;

        ServerData.costumeServerTable.SyncCostumeData(key);

        lockSyncRoutine = null;
    }
}
