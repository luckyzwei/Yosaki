using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;
using CodeStage.AntiCheat.ObscuredTypes;
using static UiRewardView;
using BackEnd;
using System.Linq;

public class HellWarModeManager : ContentsManagerBase
{
    [Header("BossInfo")]
    [SerializeField]
    private List<BossEnemyBase> singleRaidEnemy;

    [SerializeField]
    private List<AgentHpController> bossHpController;

    private BossTableData bossTableData;
    private ReactiveProperty<ObscuredDouble> damageAmount = new ReactiveProperty<ObscuredDouble>();
    private ReactiveProperty<ObscuredDouble> bossRemainHp = new ReactiveProperty<ObscuredDouble>();

    public override Transform GetMainEnemyObjectTransform()
    {
        return singleRaidEnemy[0].transform;
    }
    public override double GetBossRemainHpRatio()
    {
        return damageAmount.Value / bossRemainHp.Value;
    }
    public double BossRemainHp => bossRemainHp.Value;

    public override double GetDamagedAmount()
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
    private UiSonBossResultPopup uiBossResultPopup;

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
        for (int i = 0; i < bossHpController.Count; i++)
        {
            bossHpController[i].whenEnemyDamaged.AsObservable().Subscribe(WhenBossDamaged).AddTo(this);
        }

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
        bossRemainHp.Value = float.MaxValue;

        for (int i = 0; i < bossHpController.Count; i++)
        {
            bossHpController[i].SetRaidEnemy();
        }

    }

    private void whenDamageAmountChanged(ObscuredDouble hp)
    {
        damageIndicator.SetText(Utils.ConvertBigNum(hp));
        damagedAnim.SetTrigger(DamageAnimName);
    }

    private void WhenBossDamaged(ObscuredDouble hp)
    {
        //  bossHpBar.UpdateHpBar(hp, bossTableData.Hp);

        if (hp <= 0f && contentsState.Value == (int)ContentsState.Fight)
        {
            // WhenBossDead();
        }
    }

    private void WhenBossDamaged(double damage)
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
        singleRaidEnemy.ForEach(e => e.gameObject.SetActive(false));

        //타이머 종료
        if (contentsState.Value != (int)ContentsState.TimerEnd)
        {
            StopTimer();
        }

        //점수 전송
        SendScore();

        //보상팝업
        ShowResultPopup();
    }

    private void SendScore()
    {
        double reqValue = damageAmount.Value * GameBalance.BossScoreSmallizeValue;

        if (reqValue > ServerData.userInfoTable.TableDatas[UserInfoTable.hellWarScore].Value)
        {
            ServerData.userInfoTable.TableDatas[UserInfoTable.hellWarScore].Value = reqValue;

            ServerData.userInfoTable.UpData(UserInfoTable.hellWarScore, false);

            RankManager.Instance.UpdateBoss_Score(reqValue);
        }


    }

    private void ShowResultPopup()
    {
        //결과 UI
        uiBossResultPopup.gameObject.SetActive(true);
        statusUi.SetActive(false);
        uiBossResultPopup.Initialize(damageAmount.Value);
    }

    protected override IEnumerator ModeTimer()
    {
        while (direciontEnd == false)
        {
            yield return null;
        }
        directionUi.SetActive(false);

        StartCoroutine(BossRandomActiveRoutine());

        AutoManager.Instance.StartAutoWithDelay();

        portalObject.gameObject.SetActive(false);

        float remainSec = playTime;

        while (remainSec >= 0)
        {
            timerText.SetText($"남은시간 : {(int)remainSec}");
            yield return null;
            remainSec -= Time.deltaTime;
            this.remainSec = remainSec;
        }

        TimerEnd();
    }
    private IEnumerator BossRandomActiveRoutine()
    {
        WaitForSeconds spawnDelay = new WaitForSeconds(9.0f);
        WaitForSeconds spawnDelay_short = new WaitForSeconds(6.5f);

        int idx = 0;

        int prefIdx = 0;

        List<int> randIdx = new List<int>() { 0, 1, 2 };

        randIdx = randIdx.OrderBy(a => System.Guid.NewGuid()).ToList();

        while (true)
        {
            prefIdx = randIdx[idx];

            if (contentsState.Value == (int)ContentsState.Fight)
            {
                for (int i = 0; i < singleRaidEnemy.Count; i++)
                {
                    singleRaidEnemy[i].gameObject.SetActive(i == randIdx[idx]);

                    if (i == randIdx[idx])
                    {
                        singleRaidEnemy[i].GetComponent<HellWarModeEnemy>().StartAttackRoutine();
                    }
                }
            }

            if (remainSec >= 60)
            {
                yield return spawnDelay;
            }
            else
            {
                yield return spawnDelay_short;
            }

            idx++;

            if (idx >= singleRaidEnemy.Count)
            {
                idx = 0;

                randIdx = randIdx.OrderBy(a => System.Guid.NewGuid()).ToList();

                if (randIdx[idx] == prefIdx)
                {
                    idx++;
                }
            }
        }
    }

    private ObscuredBool direciontEnd = false;

    public void WhenDirectionAnimEnd()
    {
        direciontEnd = true;
    }


}
