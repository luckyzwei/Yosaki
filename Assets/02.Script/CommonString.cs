using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public static class CommonString
{
    public static string RunAnimKey = "Run";
    public static string WalkAnimKey = "Walk";
    public static string Attack = "Attack";
    public static string BottomBlock = "BottomBlock";
    public static string Platform = "Platform";


    public static string Notice = "알림";
    public static string DataLoadFailedRetry = "데이터 로드에 실패했습니다.\n 다시 로드하시겠습니까?";


    public static string NickNameError_400 = "잘못된 닉네임 입니다.";
    public static string NickNameError_409 = "이미 존재하는 닉네임 입니다.";

    public static string ItemGrade_0 = "하급";
    public static string ItemGrade_1 = "중급";
    public static string ItemGrade_2 = "상급";
    public static string ItemGrade_3 = "특급";
    public static string ItemGrade_4 = "전설";
    public static string ItemGrade_5 = "요물";
    public static string ItemGrade_5_Norigae = "신물";

    public static string GoldItemName = "골드";
    public static string BonusSpinCoin = "복주머니 뽑기권";

    public static string ContentsName_Boss = "고양이 요괴전";
    public static string ContentsName_FireFly = "반딧불 요괴전";
    public static string ContentsName_InfinityTower = "요괴 도장";
    public static string ContentsName_Dokebi = "도깨비 삼형재";

    public static string ChatConnectString = "채팅 채널에 입장했습니다.";

    public static char ChatSplitChar = '◙';

    public static string RankPrefix_Level = "레벨";
    public static string RankPrefix_Stage = "스테이지";
    public static string RankPrefix_Boss = "십이지신(묘)";
    public static string RankPrefix_Real_Boss = "십이지신(토끼)";
    public static string RankPrefix_YoguiSogul = "백귀야행";
    public static string RankPrefix_Relic = "영혼의숲";

    public static string[] ThemaName = { "마왕성 정원", "이상한 숲", "마법 동굴", "리퍼의 영역", "지옥 입구", "지옥 성곽", "지옥 안채", "지옥숲" };

    public static string CafeURL = "https://cafe.naver.com/yokiki";

    public static string GetContentsName(ContentsType contentsType)
    {
        switch (contentsType)
        {
            case ContentsType.NormalField:
                {
                }
                break;
            case ContentsType.FireFly:
                {
                    return ContentsName_FireFly;
                }
                break;
            case ContentsType.Boss:
                {
                    return ContentsName_Boss;
                }
                break;
            case ContentsType.InfiniteTower:
                {
                    return ContentsName_InfinityTower;
                }
                break;
        }

        return "미등록";
    }

    public static string GetItemName(Item_Type item_type)
    {
        switch (item_type)
        {
            case Item_Type.Gold: return "금화";
            case Item_Type.Jade: return "옥";
            case Item_Type.GrowthStone: return "수련의돌";
            case Item_Type.Memory: return "무공비급";
            case Item_Type.Ticket: return "소환서";
            case Item_Type.costume0: return TableManager.Instance.Costume.dataArray[0].Name;
            case Item_Type.costume1: return TableManager.Instance.Costume.dataArray[1].Name;
            case Item_Type.costume2: return TableManager.Instance.Costume.dataArray[2].Name;
            case Item_Type.costume3: return TableManager.Instance.Costume.dataArray[3].Name;
            case Item_Type.costume4: return TableManager.Instance.Costume.dataArray[4].Name;
            case Item_Type.costume5: return TableManager.Instance.Costume.dataArray[5].Name;
            case Item_Type.costume6: return TableManager.Instance.Costume.dataArray[6].Name;
            case Item_Type.costume7: return TableManager.Instance.Costume.dataArray[7].Name;
            case Item_Type.costume8: return TableManager.Instance.Costume.dataArray[8].Name;
            case Item_Type.pet0: return TableManager.Instance.PetDatas[0].Name;
            case Item_Type.pet1: return TableManager.Instance.PetDatas[1].Name;
            case Item_Type.pet2: return TableManager.Instance.PetDatas[2].Name;
            case Item_Type.pet3: return TableManager.Instance.PetDatas[3].Name;
            case Item_Type.Marble: return "여우구슬";
            case Item_Type.MagicStoneBuff: return "기억의파편 버프 +50%(드랍)";
            case Item_Type.weapon12: return "특급 4등급 무기";
            case Item_Type.weapon14: return "특급 2등급 무기";
            case Item_Type.magicBook11: return "특급 1등급 노리개";
            case Item_Type.skill3: return "전방베기4형 기술";
            case Item_Type.Dokebi: return "도깨비 뿔";
            case Item_Type.SkillPartion: return "기술 조각";
            case Item_Type.WeaponUpgradeStone: return "힘의 증표";
            case Item_Type.PetUpgradeSoul: return "요괴구슬";
            case Item_Type.YomulExchangeStone: return "탐욕의 증표";
            case Item_Type.Songpyeon: return "송편";
            case Item_Type.TigerBossStone: return "강함의 증표";

            case Item_Type.Relic: return "영혼 조각";
            case Item_Type.RelicTicket: return "영혼 열쇠";
            case Item_Type.RabitBossStone: return "영혼의 증표";
        }
        return "미등록"; 
    }
    
    public static string GetStatusName(StatusType type)
    {
        switch (type)
        {
            case StatusType.AttackAddPer:
                return "공격력 증가(%)";
                break;
            case StatusType.CriticalProb:
                return "크리티컬 확률(%)";
                break;
            case StatusType.CriticalDam:
                return "크리티컬 데미지(%)";
                break;
            case StatusType.SkillCoolTime:
                return "기술 시전 속도(%)";
                break;
            case StatusType.SkillDamage:
                return "추가 기술 데미지(%)";
                break;
            case StatusType.MoveSpeed:
                return $"이동 속도 증가";
                break;
            case StatusType.DamBalance:
                return "최소데미지 보정(%)";
                break;
            case StatusType.HpAddPer:
                return "체력 증가(%)";
                break;
            case StatusType.MpAddPer:
                return "마력 증가(%)";
                break;
            case StatusType.GoldGainPer:
                return "골드 획득 증가(%)";
                break;
            case StatusType.ExpGainPer:
                return "경험치 획득 증가(%)";
                break;
            case StatusType.AttackAdd:
                return "공격력";
                break;
            case StatusType.Hp:
                return "체력";
                break;
            case StatusType.Mp:
                return "마력";
                break;
            case StatusType.HpRecover:
                return "5초당 체력 회복(%)";
                break;
            case StatusType.MpRecover:
                return "5초당 마력 회복(%)";
                break;
            case StatusType.MagicStoneAddPer:
                return "수련의돌 추가 획득(%)";
                break;
            case StatusType.Damdecrease:
                return "피해 감소(%)";
                break;
            case StatusType.IgnoreDefense:
                return "방어력 무시";
                break;
            case StatusType.PenetrateDefense:
                return "초과 방어력당 추가 피해량(%)";
                break;
            case StatusType.DashCount:
                return "순보 횟수";
                break;
            case StatusType.DropAmountAddPer:
                return "몬스터 전리품 수량 증가(%)";
                break;
            case StatusType.BossDamAddPer:
                return "보스 데미지 증가(%)";
                break;
            case StatusType.SkillAttackCount:
                return "기술 타격 횟수 증가";
                break;
        }

        return "등록필요";
    }

    public static string GetDialogTextName(DialogCharcterType type)
    {
        return "미등록";
    }
}
