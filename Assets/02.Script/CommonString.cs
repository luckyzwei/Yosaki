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

    public static string WeaponGrade_0 = "저급";
    public static string WeaponGrade_1 = "일반";
    public static string WeaponGrade_2 = "레어";
    public static string WeaponGrade_3 = "유니크";
    public static string WeaponGrade_4 = "유물";

    public static string GoldItemName = "골드";
    public static string BonusSpinCoin = "마계코인";

    public static string ContentsName_Boss = "보스전";
    public static string ContentsName_GoblinDefense = "고블린 도적단 소탕";
    public static string ContentsName_InfinityTower = "지하던전";

    public static string ChatConnectString = "채팅 채널에 입장했습니다.";

    public static char ChatSplitChar = '◙';

    public static string RankPrefix_Level = "레벨";
    public static string RankPrefix_Boss0 = "세이튼";
    public static string RankPrefix_Boss1 = "바엘";
    public static string RankPrefix_Infinity = ContentsName_InfinityTower;

    public static string[] ThemaName = { "마왕성 정원", "이상한 숲", "마법 동굴", "리퍼의 영역", "지옥 입구", "지옥 성곽", "지옥 안채","지옥숲"};

    public static string GetContentsName(ContentsType contentsType)
    {
        switch (contentsType)
        {
            case ContentsType.NormalField:
                {
                }
                break;
            case ContentsType.BonusDefense:
                {
                    return ContentsName_GoblinDefense;
                }
                break;
            case ContentsType.SingleRaid:
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
            case Item_Type.Gold: return "골드";
            case Item_Type.BlueStone: return "마나스톤";
            case Item_Type.MagicStone: return "기억의파편";
            case Item_Type.Memory: return "기억의조각";
            case Item_Type.Ticket: return "마계티켓";
            case Item_Type.costume0: return TableManager.Instance.CostumeData[0].Name;
            case Item_Type.costume1: return TableManager.Instance.CostumeData[1].Name;
            case Item_Type.costume2: return TableManager.Instance.CostumeData[2].Name;
            case Item_Type.costume3: return TableManager.Instance.CostumeData[3].Name;
            case Item_Type.costume4: return TableManager.Instance.CostumeData[4].Name;
            case Item_Type.pet0: return TableManager.Instance.PetDatas[0].Name;
            case Item_Type.pet1: return TableManager.Instance.PetDatas[1].Name;
            case Item_Type.pet2: return TableManager.Instance.PetDatas[2].Name;
            case Item_Type.pet3: return TableManager.Instance.PetDatas[3].Name;
            case Item_Type.Feather: return "악마깃털";
            case Item_Type.MagicStoneBuff: return "기억의파편 버프 +50%(드랍)";
        }
        return "미등록";
    }

    public static string GetStatusName(StatusType type)
    {
        string ret = "등록필요";

        switch (type)
        {
            case StatusType.IntAddPer:
                ret = "마법공격력 증가(%)";
                break;
            case StatusType.CriticalProb:
                ret = "크리티컬 확률(%)";
                break;
            case StatusType.CriticalDam:
                ret = "크리티컬 데미지(%)";
                break;
            case StatusType.SkillCoolTime:
                ret = "스킬 쿨타임 감소(%)";
                break;
            case StatusType.SkillDamage:
                ret = "추가 스킬 데미지(%)";
                break;
            case StatusType.MoveSpeed:
                ret = "이동 속도";
                break;
            case StatusType.DamBalance:
                ret = "최소데미지 보정(%)";
                break;
            case StatusType.HpAddPer:
                ret = "체력 증가(%)";
                break;
            case StatusType.MpAddPer:
                ret = "마력 증가(%)";
                break;
            case StatusType.GoldGainPer:
                ret = "골드 획득 증가(%)";
                break;
            case StatusType.ExpGainPer:
                ret = "경험치 획득 증가(%)";
                break;
            case StatusType.IntAdd:
                ret = "마법공격력";
                break;
            case StatusType.Hp:
                ret = "체력";
                break;
            case StatusType.Mp:
                ret = "마력";
                break;
            case StatusType.HpRecover:
                ret = "5초당 체력 회복(%)";
                break;
            case StatusType.MpRecover:
                ret = "5초당 마력 회복(%)";
                break;
            case StatusType.MagicStoneAddPer:
                ret = "기억의 파편 획득 증가(%)";
                break;
        }

        return ret;
    }

    public static string GetDialogTextName(DialogCharcterType type)
    {
        switch (type)
        {
            case DialogCharcterType.BeforeLuccy:
                return "마왕 루시";
                break;
            case DialogCharcterType.CurrentLuccy:
                return "약해진 루시";
                break;
            case DialogCharcterType.Manager:
                return "관리인 도비";
                break;
        }

        return "미등록";
    }
}
