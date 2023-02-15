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

public class OnlineTowerManager : ContentsManagerBase
{
    [Header("BossInfo")]
    [SerializeField]
    private BossEnemyBase singleRaidEnemy;

    [SerializeField]
    private AgentHpController bossHpController;

    private BossTableData bossTableData;
    private ReactiveProperty<ObscuredDouble> damageAmount = new ReactiveProperty<ObscuredDouble>();
    private ReactiveProperty<ObscuredDouble> bossRemainHp = new ReactiveProperty<ObscuredDouble>();

    private double bossMaxHp = double.MaxValue;

    private double currentTotalDamamge = 0;

    public override Transform GetMainEnemyObjectTransform()
    {
        return null;
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
    private GameObject statusUi;

    [SerializeField]
    private GameObject directionUi;

    [SerializeField]
    private GameObject portalObject;

    [SerializeField]
    private Transform bossSpawnParent;

    private Coroutine sendScoreRoutine;

    public bool allPlayerEnd { get; private set; } = false;


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
            PartyRaidManager.Instance.NetworkManager.playerState.Value = NetworkManager.PlayerState.End;
        }

        if (state == (int)ContentsState.TimerEnd)
        {
            SendLastScore();
            StopTimer();
            DisableBoss();
            ShowResultPopup();

            PartyRaidResultPopup.Instance.UpdateScoreBoard();
        }
        else if (state == (int)ContentsState.Dead)
        {
            SendLastScore();
            DisableBoss();
            ShowResultPopup();

            PartyRaidResultPopup.Instance.UpdateScoreBoard();
        }
        else if (state == (int)ContentsState.AllPlayerEnd)
        {
            SendLastScore();
            StopTimer();
            DisableBoss();
            ShowResultPopup();

            allPlayerEnd = true;

            if (PartyRaidResultPopup.Instance != null)
            {
                PartyRaidResultPopup.Instance.ExitButtonActive();
            }

            PartyRaidResultPopup.Instance.UpdateScoreBoard();
        }
        else if (contentsState.Value == (int)ContentsState.Clear)
        {
            SendLastScore();
            StopTimer();
            DisableBoss();
            ShowResultPopup();

            if (SendAutoRecommend == false)
            {
                SendAutoRecommend = true;
                PartyRaidManager.Instance.NetworkManager.SendAutoRecommend();
            }

            allPlayerEnd = true;

            if (PartyRaidResultPopup.Instance != null)
            {
                PartyRaidResultPopup.Instance.ExitButtonActive();
            }

            PartyRaidResultPopup.Instance.UpdateScoreBoard();
        }
    }

    private bool SendAutoRecommend = false;

    private void DisableBoss()
    {
        singleRaidEnemy.gameObject.SetActive(false);
    }



    private void SetBossHp()
    {
        bossRemainHp.Value = float.MaxValue;

        bossHpController.SetRaidEnemy();
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

    private void AllPlayerEnd()
    {
        if (contentsState.Value == (int)ContentsState.AllPlayerEnd || contentsState.Value == (int)ContentsState.Clear) return;


        contentsState.Value = (int)ContentsState.AllPlayerEnd;
    }

    private void SendLastScore()
    {
        if (sendScoreRoutine != null)
        {
            StopCoroutine(sendScoreRoutine);
        }

        Debug.LogError("SendLastScore");

        //end
        PartyRaidManager.Instance.NetworkManager.SendScoreInfo(damageAmount.Value, true);
    }

    private void ShowResultPopup()
    {

        PartyRaidManager.Instance.NetworkManager.ShowPartyRaidResultPopup();

        // PartyRaidManager.Instance.NetworkManager.scoreBoardPanel.SetActive(false);

    }

    private IEnumerator EndCheckRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(1.0f);
        while (true)
        {
            CheckEndGame();
            yield return delay;
        }
    }

    protected override IEnumerator ModeTimer()
    {
        while (direciontEnd == false)
        {
            yield return null;
        }

        StartCoroutine(EndCheckRoutine());

        PartyRaidManager.Instance.NetworkManager.playerState.Value = NetworkManager.PlayerState.Playing;

        directionUi.SetActive(false);

        //  PartyRaidManager.Instance.NetworkManager.scoreBoardPanel.SetActive(true);

        PartyRaidManager.Instance.NetworkManager.whenScoreInfoReceived.AsObservable().Subscribe(e =>
        {

            UpdateBossHp();

        }).AddTo(this);

        SpawnBoss();

        AutoManager.Instance.StartAutoWithDelay();

        sendScoreRoutine = StartCoroutine(SendScoreRoutine());

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

    //
    private IEnumerator BossRandomActiveRoutine()
    {
        singleRaidEnemy.GetComponent<HellWarModeEnemy>().StartAttackRoutine_PartyRaid();

        yield return null;
    }
    //

    private IEnumerator SendScoreRoutine()
    {
        var delay = new WaitForSeconds(0.03f);

        while (true)
        {
            yield return delay;
            PartyRaidManager.Instance.NetworkManager.SendScoreInfo(damageAmount.Value);
        }

    }
    private void SpawnBoss()
    {
        singleRaidEnemy.gameObject.SetActive(true);

        UpdateBossHp();

        StartCoroutine(BossRandomActiveRoutine());

    }

    private void UpdateBossHp()
    {
        if (bossMaxHp == double.MaxValue)
        {
            int stageId = PartyRaidManager.Instance.NetworkManager.partyRaidTargetFloor;

            var TowerTableData4 = TableManager.Instance.towerTableMulti.dataArray[stageId];

            bossMaxHp = TowerTableData4.Hp;
        }

        currentTotalDamamge = PartyRaidManager.Instance.NetworkManager.GetTotalScore();

        if (UiOnlineTowerHpBar.Instance != null)
        {
            UiOnlineTowerHpBar.Instance.UpdateGauge(bossMaxHp - currentTotalDamamge, bossMaxHp);
        }

        CheckBossClear();

        CheckEndGame();
    }

    private void CheckEndGame()
    {
        //보상팝업
        if (PartyRaidManager.Instance.NetworkManager.IsAllPlayerEnd())
        {
            AllPlayerEnd();

            ShowResultPopup();
        }
    }

    private void CheckBossClear()
    {
        if (currentTotalDamamge >= bossMaxHp &&
            contentsState.Value != (int)ContentsState.Clear &&
            contentsState.Value != (int)ContentsState.AllPlayerEnd &&
            contentsState.Value != (int)ContentsState.TimerEnd)
        {
            contentsState.Value = (int)ContentsState.Clear;

            if (ServerData.userInfoTable.TableDatas[UserInfoTable.partyTowerFloor].Value == PartyRaidManager.Instance.NetworkManager.partyRaidTargetFloor)
            {
                SetClear();
            }
            else
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"현재 단계에서는 보상을 받으실 수 없습니다.\n클리어 : {PartyRaidManager.Instance.NetworkManager.partyRaidTargetFloor + 1} 현재 내 층수 : {ServerData.userInfoTable.TableDatas[UserInfoTable.partyTowerFloor].Value + 1}\n같은 층수 일때만 보상을 받으실 수 있습니다.", null);
            }

            GuideMissionManager.UpdateGuideMissionClear(GuideMissionKey.ClearCave);
        }
    }
    //null 일때 클리어 못한거
    private List<RewardData> rewardDatas;

    private bool rewarded = false;

    private void SetClear()
    {
        //중복진입 방지
        if (rewarded) return;

       

        rewarded = true;

        //보상지급
        int currentFloor = (int)ServerData.userInfoTable.GetTableData(UserInfoTable.partyTowerFloor).Value;

        var TowerTableData4 = TableManager.Instance.towerTableMulti.dataArray[currentFloor];

        rewardDatas = new List<RewardData>();

        var rewardData = new RewardData((Item_Type)TowerTableData4.Rewardtype, TowerTableData4.Rewardvalue);

        rewardDatas.Add(rewardData);

        List<TransactionValue> transactionList = new List<TransactionValue>();

        HashSet<int> syncDataList = new HashSet<int>();

        //데이터 적용(로컬)
        for (int i = 0; i < rewardDatas.Count; i++)
        {
            if (syncDataList.Contains((int)rewardDatas[i].itemType) == true)
            {
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"Duplicated tower itemType : {(Item_Type)(int)rewardDatas[i].itemType}", null);
                return;
            }
            else
            {
                syncDataList.Add((int)rewardDatas[i].itemType);
            }

            ServerData.AddLocalValue(rewardDatas[i].itemType, rewardDatas[i].amount);

            //서버 트랙잭션
            var rewardTransactionValue = ServerData.GetItemTypeTransactionValue((Item_Type)(int)rewardDatas[i].itemType);
            transactionList.Add(rewardTransactionValue);
        }

        //단계상승
        ServerData.userInfoTable.GetTableData(UserInfoTable.partyTowerFloor).Value = PartyRaidManager.Instance.NetworkManager.partyRaidTargetFloor + 1;

        Param floorParam = new Param();

        floorParam.Add(UserInfoTable.partyTowerFloor, ServerData.userInfoTable.GetTableData(UserInfoTable.partyTowerFloor).Value);

        transactionList.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, floorParam));

        ServerData.SendTransaction(transactionList, successCallBack: () =>
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{ServerData.userInfoTable.GetTableData(UserInfoTable.partyTowerFloor).Value}층 클리어!\n{CommonString.GetItemName((Item_Type)TowerTableData4.Rewardtype)} {Utils.ConvertBigNum(TowerTableData4.Rewardvalue)}개 획득!", null);
        });

        //추천 1회

    }

    private ObscuredBool direciontEnd = false;

    public void WhenDirectionAnimEnd()
    {
        direciontEnd = true;
    }
}
