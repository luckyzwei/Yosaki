using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiPlayerStatBoard : SingletonMono<UiPlayerStatBoard>
{
    [SerializeField]
    private TextMeshProUGUI descriptionBoard1;
    [SerializeField]
    private TextMeshProUGUI descriptionBoard2;

    [SerializeField]
    private TextMeshProUGUI fightPoint;

    [SerializeField]
    private RectTransform contentsTr;

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        string description1 = string.Empty;
        string description2 = string.Empty;
        //전투력
        fightPoint.SetText($"무력 : {Utils.ConvertBigNum(PlayerStats.GetTotalPower())}");

        //공격력
        description1 += $"{CommonString.GetStatusName(StatusType.AttackAdd)} : {Utils.ConvertBigNum(PlayerStats.GetBaseAttackPower())}\n";
        //공격력증가(%)
        description2 += $"{CommonString.GetStatusName(StatusType.AttackAddPer)} : {Utils.ConvertBigNum(PlayerStats.GetBaseAttackAddPercentValue() * 100f)}\n";

        //크리티컬 확률(%)
        description1 += $"{CommonString.GetStatusName(StatusType.CriticalProb)} : {Utils.ConvertBigNum(PlayerStats.GetCriticalProb() * 100f)}\n";
        //크리티컬 데미지(%)
        description2 += $"{CommonString.GetStatusName(StatusType.CriticalDam)} : {Utils.ConvertBigNum(PlayerStats.CriticalDam() * 100f)}\n";
        //--
        //스킬 쿨타임 감소
        description1 += $"{CommonString.GetStatusName(StatusType.SkillCoolTime)} : {Utils.ConvertBigNum(PlayerStats.GetSkillCoolTimeDecreaseValue() * 100f)}\n";

        //스킬 데미지 증가
        description2 += $"{CommonString.GetStatusName(StatusType.SkillDamage)} : {Utils.ConvertBigNum(PlayerStats.GetSkillDamagePercentValue() * 100f)}\n";

        ////최소데미지 보정
        //description1 += $"{CommonString.GetStatusName(StatusType.DamBalance)} : +{PlayerStats.GetDamBalanceAddValue() * 100f}%\n";
        //description1 += $"데미지 범위(%) : {(DamageBalance.baseMinDamage + PlayerStats.GetDamBalanceAddValue()) * 100f}%~{DamageBalance.baseMaxDamage * 100}%\n";

        //체력
        description1 += $"{CommonString.GetStatusName(StatusType.Hp)} : {Utils.ConvertBigNum(PlayerStats.GetOriginHp())}\n";
        //체력 증가
        description2 += $"{CommonString.GetStatusName(StatusType.HpAddPer)} : {Utils.ConvertBigNum(PlayerStats.GetMaxHpPercentAddValue() * 100f)}\n";

        //초당체력회복
        description1 += $"{CommonString.GetStatusName(StatusType.HpRecover)} : {PlayerStats.GetHpRecover() * 100f}\n";
        description2 += $"{CommonString.GetStatusName(StatusType.MagicStoneAddPer)} : {PlayerStats.GetMagicStonePlusValue() * 100f}\n";

        ////마력
        //description1 += $"{CommonString.GetStatusName(StatusType.Mp)} : {PlayerStats.GetOriginMp()}\n";

        ////마력 증가
        //description1 += $"{CommonString.GetStatusName(StatusType.MpAddPer)} : {PlayerStats.GetMaxMpPercentAddValue() * 100f}\n";

        ////초당마력회복
        //description1 += $"{CommonString.GetStatusName(StatusType.MpRecover)} : {PlayerStats.GetMpRecover() * 100f}\n";

        //골드 추가 획득
        description1 += $"{CommonString.GetStatusName(StatusType.GoldGainPer)} : {Utils.ConvertBigNum(PlayerStats.GetGoldPlusValue() * 100f) }\n";

        //경험치 추가 획득
        description2 += $"{CommonString.GetStatusName(StatusType.ExpGainPer)} : {PlayerStats.GetExpPlusValue() * 100f}\n";

        //아이템 획득량
        description1 += $"{CommonString.GetStatusName(StatusType.DropAmountAddPer)} : {PlayerStats.GetDropAmountAddValue()}\n";

        //이동속도
        description2 += $"{CommonString.GetStatusName(StatusType.MoveSpeed)} : {PlayerStats.GetMoveSpeedValue()}\n";

        //피해 감소
        description2 += $"{CommonString.GetStatusName(StatusType.Damdecrease)} : {PlayerStats.GetDamDecreaseValue() * 100f}\n";

        //보스피해
        description1 += $"{CommonString.GetStatusName(StatusType.BossDamAddPer)} : {PlayerStats.GetBossDamAddValue() * 100f}\n";

        //방어도 무시
        description1 += $"{CommonString.GetStatusName(StatusType.IgnoreDefense)} : {PlayerStats.GetIgnoreDefenseValue()}\n";

        //관통
        description2 += $"{CommonString.GetStatusName(StatusType.PenetrateDefense)} : {(PlayerStats.GetPenetrateDefense() * 100f).ToString("F3")}\n";

        //타격수
        description1 += $"{CommonString.GetStatusName(StatusType.SkillAttackCount)} : {PlayerStats.GetSkillHitAddValue()}\n";
        //방무 GetIgnoreDefenseValue
        //천공베기
        description2 += $"{CommonString.GetStatusName(StatusType.SuperCritical1Prob)} : {Utils.ConvertBigNum(PlayerStats.GetSuperCriticalProb() * 100f)}\n";

        //타격수
        description1 += $"{CommonString.GetStatusName(StatusType.SuperCritical1DamPer)} : {Utils.ConvertBigNum(PlayerStats.GetSuperCriticalDamPer() * 100f)}\n";

        description2 += $"{CommonString.GetStatusName(StatusType.MarbleAddPer)} : {PlayerStats.GetMarblePlusValue() * 100f}\n";

        description1 += $"{CommonString.GetStatusName(StatusType.SuperCritical2DamPer)} : {Utils.ConvertBigNum(PlayerStats.GetSuperCritical2DamPer() * 100f)}\n";
        description2 += $"{CommonString.GetStatusName(StatusType.DecreaseBossHp)} : {PlayerStats.DecreaseBossHp() * 100f}\n";
        ////기억의파편 추가 획득
        //description1 += $"{CommonString.GetStatusName(StatusType.MagicStoneAddPer)} : {PlayerStats.GetMagicStonePlusValue() * 100f}\n";

        int hellPlusSpawnNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.du).Value;

        int chunPlusSpawnNum = 0;

        if (PlayerStats.IsChunMonsterSpawnAdd())
        {
            chunPlusSpawnNum = 5;
        }

        int plusSpawnNum = GuildManager.Instance.GetGuildSpawnEnemyNum(GuildManager.Instance.guildLevelExp.Value) + hellPlusSpawnNum + chunPlusSpawnNum;

        //지옥베기
        description1 += $"{CommonString.GetStatusName(StatusType.SuperCritical3DamPer)} : {Utils.ConvertBigNum(PlayerStats.GetSuperCritical3DamPer() * 100f)}\n";

        description2 += $"요괴 추가소환 : {plusSpawnNum}\n";
        //천상베기
        description1 += $"{CommonString.GetStatusName(StatusType.SuperCritical4DamPer)} : {Utils.ConvertBigNum(PlayerStats.GetSuperCritical4DamPer() * 100f)}\n";

        

        descriptionBoard1.SetText(description1);
        descriptionBoard2.SetText(description2);
    }

}
