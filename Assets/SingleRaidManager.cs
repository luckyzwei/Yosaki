using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;
using CodeStage.AntiCheat.ObscuredTypes;
using static UiRewardView;
using BackEnd;



public class SingleRaidManager : ContentsManagerBase
{
    [Header("BossInfo")]
    private BossEnemyBase singleRaidEnemy;
    private AgentHpController bossHpController;

    private BossTableData bossTableData;
    private ReactiveProperty<ObscuredFloat> damageAmount = new ReactiveProperty<ObscuredFloat>();
    private ReactiveProperty<ObscuredFloat> bossRemainHp = new ReactiveProperty<ObscuredFloat>();

    public override Transform GetMainEnemyObjectTransform()
    {
        return singleRaidEnemy.transform;
    }
    public override float GetBossRemainHpRatio()
    {
        return damageAmount.Value / bossRemainHp.Value;
    }
    public float BossRemainHp => bossRemainHp.Value;

    public override float GetDamagedAmount()
    {
        return damageAmount.Value;
    }

    [Header("Ui")]
    [SerializeField]
    private TextMeshProUGUI damageIndicator;
    [SerializeField]
    private Animator damagedAnim;
    private string DamageAnimName = "Play";

    [Header("State")]
    private ReactiveProperty<ObscuredInt> contentsState = new ReactiveProperty<ObscuredInt>((int)ContentsState.Fight);

    [SerializeField]
    private UiBossResultPopup uiBossResultPopup;

    [SerializeField]
    private GameObject statusUi;

    [SerializeField]
    private GameObject directionUi;

    [SerializeField]
    private GameObject portalObject;

    [SerializeField]
    private Transform bossSpawnParent;

    #region Security
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
        damageAmount.Value.RandomizeCryptoKey();
        bossRemainHp.Value.RandomizeCryptoKey();
        contentsState.Value.RandomizeCryptoKey();
    }
    #endregion

    protected new void Start()
    {
        base.Start();
        Initialize();
        Subscribe();

   
    }
    private void Initialize()
    {
        SoundManager.Instance.PlaySound("BossAppear");

        SetBossHp();
    }

    private void Subscribe()
    {
        bossHpController.whenEnemyDamaged.AsObservable().Subscribe(WhenBossDamaged).AddTo(this);
        PlayerStatusController.Instance.whenPlayerDead.Subscribe(e => { WhenPlayerDead(); }).AddTo(this);

        damageAmount.AsObservable().Subscribe(whenDamageAmountChanged).AddTo(this);
        bossRemainHp.AsObservable().Subscribe(WhenBossDamaged).AddTo(this);

        contentsState.AsObservable().Subscribe(WhenBossModeStateChanged).AddTo(this);
    }

    private void WhenBossModeStateChanged(ObscuredInt state)
    {
        if (state != (int)ContentsState.Fight)
        {
            EndBossMode();
        }
    }

    private void SetBossHp()
    {
        bossTableData = TableManager.Instance.BossTableData[GameManager.Instance.bossId];
        bossRemainHp.Value = float.MaxValue;

        var prefab = Resources.Load<BossEnemyBase>($"Boss/{GameManager.Instance.bossId}");

        singleRaidEnemy = Instantiate<BossEnemyBase>(prefab, bossSpawnParent);
        singleRaidEnemy.transform.localPosition = Vector3.zero;
        singleRaidEnemy.gameObject.SetActive(false);
        bossHpController = singleRaidEnemy.GetComponent<AgentHpController>();
    }

    private void whenDamageAmountChanged(ObscuredFloat hp)
    {
        damageIndicator.SetText(Utils.ConvertBigNum(hp));
        damagedAnim.SetTrigger(DamageAnimName);
    }

    private void WhenBossDamaged(ObscuredFloat hp)
    {
        //  bossHpBar.UpdateHpBar(hp, bossTableData.Hp);

        if (hp <= 0f && contentsState.Value == (int)ContentsState.Fight)
        {
            WhenBossDead();
        }
    }

    private void WhenBossDamaged(float damage)
    {
        damageAmount.Value -= damage;
        bossRemainHp.Value += damage;
    }
    #region EndConditions
    //클리어조건1 플레이어 사망
    private void WhenPlayerDead()
    {
        if (contentsState.Value != (int)ContentsState.Fight) return;

        contentsState.Value = (int)ContentsState.Dead;
    }

    //클리어조건1 보스 처치 성공
    private void WhenBossDead()
    {
        //클리어 체크
        contentsState.Value = (int)ContentsState.Clear;

        //SendClearInfo();
    }

    //private void SendClearInfo()
    //{
    //    var serverData = ServerData.bossServerTable.TableDatas[bossTableData.Stringid];

    //    if (serverData.clear.Value != 1)
    //    {
    //        serverData.clear.Value = 1;

    //        ServerData.bossServerTable.UpdateData(bossTableData.Stringid);
    //    }
    //}

    //클리어조건1 타이머 종료
    protected override void TimerEnd()
    {
        base.TimerEnd();
        contentsState.Value = (int)ContentsState.TimerEnd;
    }
    #endregion

    private void EndBossMode()
    {
        //공격루틴 제거 + 클리어면 이펙트 켜주던지.?
        singleRaidEnemy.gameObject.SetActive(false);

        //타이머 종료
        if (contentsState.Value != (int)ContentsState.TimerEnd)
        {
            StopTimer();
        }

        //점수 전송
        SendScore();

        //보상팝업
        RewardItem();

        UiTutorialManager.Instance.SetClear(TutorialStep.PlayCatContents);
    }

    private void SendScore()
    {
        RankManager.Instance.UpdateBoss_Score(damageAmount.Value);

        //var serverData = ServerData.bossServerTable.TableDatas[bossTableData.Stringid];

        //serverData.score.Value = damageAmount.Value.ToString();

        //ServerData.bossServerTable.UpdateData(bossTableData.Stringid);
    }

    public static List<RewardData> GetRewawrdData(BossTableData bossTableData, float damagedHp, int clearCount = 1)
    {
        List<RewardData> rewardDatas = new List<RewardData>();
        float rewardPer = Mathf.Clamp(damagedHp, bossTableData.Rewardminhp, bossTableData.Rewardmaxhp) / bossTableData.Rewardmaxhp;
        //보상 산정
        for (int i = 0; i < bossTableData.Rewardtypes.Length; i++)
        {
            Item_Type rewardType = (Item_Type)bossTableData.Rewardtypes[i];
            int rewardAmount = (int)(Mathf.Ceil(rewardPer * bossTableData.Rewardmaxvalues[i])) * clearCount;

            RewardData viewData = new RewardData(rewardType, rewardAmount);
            rewardDatas.Add(viewData);
        }

        //데이터 적용(로컬)
        for (int i = 0; i < rewardDatas.Count; i++)
        {
            ServerData.AddLocalValue(rewardDatas[i].itemType, rewardDatas[i].amount);
        }

        return rewardDatas;
    }

    private void RewardItem()
    {
        DailyMissionManager.UpdateDailyMission(DailyMissionKey.RewardedBossContents, 1);

        float damagedHp = damageAmount.Value;

        List<RewardData> rewardDatas = GetRewawrdData(bossTableData, damagedHp);

        //데이터 적용(서버)
        ServerData.SendTransaction(rewardDatas);

        //결과 UI
        uiBossResultPopup.gameObject.SetActive(true);
        statusUi.SetActive(false);
        uiBossResultPopup.Initialize(damageAmount.Value, damageAmount.Value / bossTableData.Hp, rewardDatas);
    }

    protected override IEnumerator ModeTimer()
    {
        while (direciontEnd == false)
        {
            yield return null;
        }
        directionUi.SetActive(false);
        singleRaidEnemy.gameObject.SetActive(true);

        WaitForSeconds ws = new WaitForSeconds(1.0f);

        portalObject.gameObject.SetActive(false);

        int remainSec = playTime;

        while (remainSec >= 0)
        {
            timerText.SetText($"남은시간 : {remainSec}");
            yield return ws;
            remainSec--;
            this.remainSec = remainSec;
        }

        TimerEnd();
    }

    private ObscuredBool direciontEnd = false;

    public void WhenDirectionAnimEnd()
    {
        direciontEnd = true;
    }
}
