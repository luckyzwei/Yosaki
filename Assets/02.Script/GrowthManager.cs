using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GrowthManager : SingletonMono<GrowthManager>
{
    public ReactiveProperty<float> maxExp = new ReactiveProperty<float>();
    public ReactiveCommand WhenPlayerLevelUp = new ReactiveCommand();

    private new void Awake()
    {
        base.Awake();
        Initialize();
    }

    private void Initialize()
    {
        maxExp.Value = GameDataCalculator.GetMaxExp(DatabaseManager.statusTable.GetTableData(StatusTable.Level).Value);
    }
    public void GetExp(float exp)
    {
        exp += exp * PlayerStats.GetExpPlusValue();

        SystemMessage.Instance.SetMessage($"경험치 획득 ({(int)exp})");

        DatabaseManager.growthTable.GetTableData(GrowthTable.Exp).Value += exp;

        if (CanLevelUp())
        {
            DatabaseManager.growthTable.GetTableData(GrowthTable.Exp).Value -= maxExp.Value;

            levelUp();

            //추가레벨업 가능?
            if (CanLevelUp())
            {
                GetExp(0);
            }
            else
            {

                SyncLevelUpDatas();
            }
        }
    }

    private bool CanLevelUp()
    {
        return DatabaseManager.growthTable.GetTableData(GrowthTable.Exp).Value >= maxExp.Value;
    }

    public void levelUp()
    {
        UpdateLocalData();

        //최대경험치 갱신
        maxExp.Value = GameDataCalculator.GetMaxExp(DatabaseManager.statusTable.GetTableData(StatusTable.Level).Value);

        //레벨업 이벤트
        WhenPlayerLevelUp.Execute();

        EffectManager.SpawnEffect("LevelUp", PlayerMoveController.Instance.transform.position, PlayerMoveController.Instance.transform);

        //일일미션
        DailyMissionManager.UpdateDailyMission(DailyMissionKey.LevelUp, 1);
    }

    private void UpdateLocalData()
    {
        //레벨 증가
        DatabaseManager.statusTable.GetTableData(StatusTable.Level).Value++;

        //스킬포인트 증가
        DatabaseManager.statusTable.GetTableData(StatusTable.SkillPoint).Value += GameBalance.SkillPointGet;

        //스탯포인트 증가
        DatabaseManager.statusTable.GetTableData(StatusTable.StatPoint).Value += GameBalance.StatPoint;

        //스핀포인트 증가
        DatabaseManager.goodsTable.GetTableData(GoodsTable.BonusSpinKey).Value += GameBalance.levelUpSpinGet;

        ShowContentsUnlockAlarm();
    }

    private void ShowContentsUnlockAlarm()
    {
        int currentLevel = DatabaseManager.statusTable.GetTableData(StatusTable.Level).Value;

        if (currentLevel == GameBalance.bonusDungeonUnlockLevel)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"({CommonString.ContentsName_FireFly}) 해금됐습니다!\n전투 탭에서 도전 가능 합니다.", null);
        }
        else if (currentLevel == GameBalance.InfinityDungeonUnlockLevel)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"({CommonString.ContentsName_InfinityTower}) 해금됐습니다!\n전투 탭에서 도전 가능 합니다.", null);
        }
        else if (currentLevel == GameBalance.bossUnlockLevel)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"({CommonString.ContentsName_Boss}) 해금됐습니다!\n전투 탭에서 도전 가능 합니다.", null);
        }
    }

    public void SyncLevelUpDatas()
    {
        List<TransactionValue> transactionList = new List<TransactionValue>();

        Param statusParam = new Param();
        //레벨
        statusParam.Add(StatusTable.Level, DatabaseManager.statusTable.GetTableData(StatusTable.Level).Value);

        //스킬포인트
        statusParam.Add(StatusTable.SkillPoint, DatabaseManager.statusTable.GetTableData(StatusTable.SkillPoint).Value);

        //스탯포인트
        statusParam.Add(StatusTable.StatPoint, DatabaseManager.statusTable.GetTableData(StatusTable.StatPoint).Value);

        Param growthParam = new Param();
        growthParam.Add(GrowthTable.Exp, DatabaseManager.growthTable.GetTableData(GrowthTable.Exp).Value);

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.BonusSpinKey, DatabaseManager.goodsTable.GetTableData(GoodsTable.BonusSpinKey).Value);

        transactionList.Add(TransactionValue.SetUpdate(StatusTable.tableName, StatusTable.Indate, statusParam));
        transactionList.Add(TransactionValue.SetUpdate(GrowthTable.tableName, GrowthTable.Indate, growthParam));
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        DatabaseManager.SendTransaction(transactionList);
    }

    public void WhenPlayerDeadInNormalField()
    {
        //경험치 절반 감소 안시킴
        return;
        DatabaseManager.growthTable.GetTableData(GrowthTable.Exp).Value = DatabaseManager.growthTable.GetTableData(GrowthTable.Exp).Value * 0.5f;
        DatabaseManager.growthTable.UpData(GrowthTable.Exp,false);
    }
}
