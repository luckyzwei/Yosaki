using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiPlayerStatBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI descriptionBoard;

    [SerializeField]
    private RectTransform contentsTr;

    private void OnEnable()
    {
        Refresh();
    }

    private void Refresh()
    {
        string description = string.Empty;
        //전투력
        description += $"전투력 : {Utils.ConvertBigFloat(PlayerStats.GetTotalPower())}\n";
        //공격력
        description += "-------------------\n";
        description += $"{CommonString.GetStatusName(StatusType.IntAdd)} : {(int)PlayerStats.GetBaseAttackPower()}\n";
        //공격력증가(%)
        description += $"{CommonString.GetStatusName(StatusType.IntAddPer)} : {PlayerStats.GetBaseAttackAddPercentValue()*100f}\n";

        //크리티컬 확률(%)
        description += "-------------------\n";
        description += $"{CommonString.GetStatusName(StatusType.CriticalProb)} : {PlayerStats.GetCriticalProb() * 100f}\n";
        //크리티컬 데미지(%)
        description += $"{CommonString.GetStatusName(StatusType.CriticalDam)} : {PlayerStats.CriticalDam() * 100f}\n";
        //--
        //스킬 쿨타임 감소
        description += "-------------------\n";
        description += $"{CommonString.GetStatusName(StatusType.SkillCoolTime)} : {PlayerStats.GetSkillCoolTimeDecreaseValue() * 100f}\n";

        //스킬 데미지 증가
        description += $"{CommonString.GetStatusName(StatusType.SkillDamage)} : {PlayerStats.GetSkillDamagePercentValue() * 100f}\n";

        //최소데미지 보정
        description += "-------------------\n";
        description += $"{CommonString.GetStatusName(StatusType.DamBalance)} : +{PlayerStats.GetDamBalanceAddValue()*100f}%\n";
        description += $"데미지 범위(%) : {(DamageBalance.baseMinDamage+ PlayerStats.GetDamBalanceAddValue())*100f}%~{DamageBalance.baseMaxDamage * 100}%\n";

        //체력
        description += "-------------------\n";
        description += $"{CommonString.GetStatusName(StatusType.Hp)} : {PlayerStats.GetOriginHp()}\n";

        //체력 증가
        description += $"{CommonString.GetStatusName(StatusType.HpAddPer)} : {PlayerStats.GetMaxHpPercentAddValue() * 100f}\n";

        //초당체력회복
        description += $"{CommonString.GetStatusName(StatusType.HpRecover)} : {PlayerStats.GetHpRecover() * 100f}\n";

        //마력
        description += "-------------------\n";
        description += $"{CommonString.GetStatusName(StatusType.Mp)} : {PlayerStats.GetOriginMp()}\n";

        //마력 증가
        description += $"{CommonString.GetStatusName(StatusType.MpAddPer)} : {PlayerStats.GetMaxMpPercentAddValue() * 100f}\n";

        //초당마력회복
        description += $"{CommonString.GetStatusName(StatusType.MpRecover)} : {PlayerStats.GetMpRecover() * 100f}\n";

        //골드 추가 획득
        description += "-------------------\n";
        description += $"{CommonString.GetStatusName(StatusType.GoldGainPer)} : {PlayerStats.GetGoldPlusValue() * 100f}\n";

        //경험치 추가 획득
        description += $"{CommonString.GetStatusName(StatusType.ExpGainPer)} : {PlayerStats.GetExpPlusValue() * 100f}\n";

        //기억의파편 추가 획득
        description += $"{CommonString.GetStatusName(StatusType.MagicStoneAddPer)} : {PlayerStats.GetMagicStonePlusValue() * 100f}\n";
        description += "-------------------";

        descriptionBoard.SetText(description);
    }

}
