using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;
using static UiGuildMemberCell;

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
    public static string ItemGrade_6 = "야차";
    public static string ItemGrade_7 = "필멸";
    public static string ItemGrade_8 = "필멸(암)";
    public static string ItemGrade_9 = "필멸(천)";
    public static string ItemGrade_10 = "필멸(극)";
    public static string ItemGrade_11 = "인드라";
    public static string ItemGrade_12 = "나타";
    public static string ItemGrade_13 = "오로치";
    public static string ItemGrade_14 = "필멸(패)";
    public static string ItemGrade_15 = "여우검";
    public static string ItemGrade_16 = "지옥";
    public static string ItemGrade_17 = "여래";
    public static string ItemGrade_5_Norigae = "신물";
    public static string ItemGrade_6_Norigae = "영물";
    public static string ItemGrade_7_Norigae = "영물";
    public static string ItemGrade_8_Norigae = "영물";
    public static string ItemGrade_9_Norigae = "병아리";
    public static string ItemGrade_10_Norigae = "영물";
    public static string ItemGrade_11_Norigae = "지옥";
    public static string ItemGrade_12_Norigae = "여래";

    public static string ItemGrade_4_Skill = "주작";
    public static string ItemGrade_5_Skill = "청룡";
    public static string ItemGrade_6_Skill = "흑룡";
    public static string ItemGrade_7_Skill = "나타";
    public static string ItemGrade_8_Skill = "오로치";
    public static string ItemGrade_9_Skill = "강림";

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
    public static string RankPrefix_Boss = "고양이요괴전";
    public static string RankPrefix_Real_Boss = "십이지신(해)";
    public static string RankPrefix_Relic = "영혼의숲(지옥)";
    public static string RankPrefix_MiniGame = "미니게임";
    public static string RankPrefix_GangChul = "강철이";

    public static string[] ThemaName = { "마왕성 정원", "이상한 숲", "마법 동굴", "리퍼의 영역", "지옥 입구", "지옥 성곽", "지옥 안채", "지옥숲" };

    public static string CafeURL = "https://cafe.naver.com/yokiki";

    public static string IOS_nick = "_IOS";
    public static string IOS_loginType = "IOS_LoginType";
    public static string SavedLoginTypeKey = "SavedLoginTypeKey";
    public static string SavedLoginPassWordKey = "SavedLoginPassWordKey";

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
            case Item_Type.costume9: return TableManager.Instance.Costume.dataArray[9].Name;
            case Item_Type.costume10: return TableManager.Instance.Costume.dataArray[10].Name;
            case Item_Type.costume11: return TableManager.Instance.Costume.dataArray[11].Name;
            case Item_Type.costume12: return TableManager.Instance.Costume.dataArray[12].Name;
            case Item_Type.costume13: return TableManager.Instance.Costume.dataArray[13].Name;
            case Item_Type.costume14: return TableManager.Instance.Costume.dataArray[14].Name;
            case Item_Type.costume15: return TableManager.Instance.Costume.dataArray[15].Name;
            case Item_Type.costume16: return TableManager.Instance.Costume.dataArray[16].Name;
            case Item_Type.costume17: return TableManager.Instance.Costume.dataArray[17].Name;
            case Item_Type.costume18: return TableManager.Instance.Costume.dataArray[18].Name;
            case Item_Type.costume19: return TableManager.Instance.Costume.dataArray[19].Name;
            case Item_Type.costume20: return TableManager.Instance.Costume.dataArray[20].Name;
            case Item_Type.costume21: return TableManager.Instance.Costume.dataArray[21].Name;
            case Item_Type.costume22: return TableManager.Instance.Costume.dataArray[22].Name;

            case Item_Type.costume23: return TableManager.Instance.Costume.dataArray[23].Name;
            case Item_Type.costume24: return TableManager.Instance.Costume.dataArray[24].Name;
            case Item_Type.costume25: return TableManager.Instance.Costume.dataArray[25].Name;
            case Item_Type.costume26: return TableManager.Instance.Costume.dataArray[26].Name;
            case Item_Type.costume27: return TableManager.Instance.Costume.dataArray[27].Name;
            case Item_Type.costume28: return TableManager.Instance.Costume.dataArray[28].Name;
            case Item_Type.costume29: return TableManager.Instance.Costume.dataArray[29].Name;
            case Item_Type.costume30: return TableManager.Instance.Costume.dataArray[30].Name;
            case Item_Type.costume31: return TableManager.Instance.Costume.dataArray[31].Name;
            case Item_Type.costume32: return TableManager.Instance.Costume.dataArray[32].Name;
            case Item_Type.costume33: return TableManager.Instance.Costume.dataArray[33].Name;
            case Item_Type.costume34: return TableManager.Instance.Costume.dataArray[34].Name;
            case Item_Type.costume35: return TableManager.Instance.Costume.dataArray[35].Name;
            case Item_Type.costume36: return TableManager.Instance.Costume.dataArray[36].Name;
            case Item_Type.costume37: return TableManager.Instance.Costume.dataArray[37].Name;
            case Item_Type.costume38: return TableManager.Instance.Costume.dataArray[38].Name;
            case Item_Type.costume39: return TableManager.Instance.Costume.dataArray[39].Name;
            case Item_Type.costume40: return TableManager.Instance.Costume.dataArray[40].Name;
            case Item_Type.costume41: return TableManager.Instance.Costume.dataArray[41].Name;
            case Item_Type.costume42: return TableManager.Instance.Costume.dataArray[42].Name;
            case Item_Type.costume43: return TableManager.Instance.Costume.dataArray[43].Name;
            case Item_Type.costume44: return TableManager.Instance.Costume.dataArray[44].Name;
            case Item_Type.costume45: return TableManager.Instance.Costume.dataArray[45].Name;
            case Item_Type.costume46: return TableManager.Instance.Costume.dataArray[46].Name;
            case Item_Type.costume47: return TableManager.Instance.Costume.dataArray[47].Name;
            case Item_Type.costume48: return TableManager.Instance.Costume.dataArray[48].Name;
            case Item_Type.costume49: return TableManager.Instance.Costume.dataArray[49].Name;
            case Item_Type.costume50: return TableManager.Instance.Costume.dataArray[50].Name;
            case Item_Type.costume51: return TableManager.Instance.Costume.dataArray[51].Name;
            case Item_Type.costume52: return TableManager.Instance.Costume.dataArray[52].Name;

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
            case Item_Type.Event_Item_0: return "눈송이";
            case Item_Type.Event_Item_1: return "벚꽃";
            case Item_Type.StageRelic: return "유물 파편";
            case Item_Type.DragonBossStone: return "천공의 증표";
            case Item_Type.SnakeStone: return "치명의 증표";
            case Item_Type.PeachReal: return "천도 복숭아";
            case Item_Type.HorseStone: return "하늘의 증표";
            case Item_Type.SheepStone: return "폭주석";
            case Item_Type.MonkeyStone: return "지배석";
            case Item_Type.MiniGameReward: return "뽑기권";
            case Item_Type.GuildReward: return "요괴석";
            case Item_Type.CockStone: return "태양석";
            case Item_Type.DogStone: return "천공석";
            case Item_Type.SulItem: return "설날 복주머니";
            case Item_Type.PigStone: return "십이지석";
            case Item_Type.SmithFire: return "요괴 불꽃";
            case Item_Type.FeelMulStone: return "필멸석";


            case Item_Type.Asura0: return "첫번째팔";
            case Item_Type.Asura1: return "두번째팔";
            case Item_Type.Asura2: return "세번째팔";
            case Item_Type.Asura3: return "네번째팔";
            case Item_Type.Asura4: return "다섯번째팔";
            case Item_Type.Asura5: return "여섯번째팔";

            case Item_Type.Indra0: return "인드라의 힘1";
            case Item_Type.Indra1: return "인드라의 힘2";
            case Item_Type.Indra2: return "인드라의 힘3";
            case Item_Type.IndraPower: return "인드라의 번개";


            case Item_Type.Aduk: return "어둑시니의 뿔";
            case Item_Type.SinSkill0: return "등껍질 부수기";
            case Item_Type.SinSkill1: return "백호 발톱";
            case Item_Type.SinSkill2: return "주작 베기";
            case Item_Type.SinSkill3: return "청룡 베기";
            case Item_Type.LeeMuGiStone: return "여의주";
            case Item_Type.SP: return "검조각";
            case Item_Type.Hae_Norigae: return "해태 노리개 조각";
            case Item_Type.Hae_Pet: return "아기 해태 구슬";
            case Item_Type.Event_Item_Summer: return "수박";
            case Item_Type.NataSkill: return "나타 베기";
            case Item_Type.OrochiSkill: return "오로치 베기";
            case Item_Type.GangrimSkill: return "강림 베기";

            case Item_Type.OrochiTooth0: return "오로치 이빨1";
            case Item_Type.OrochiTooth1: return "오로치 이빨2";

            case Item_Type.gumiho0: return "구미호 꼬리1";
            case Item_Type.gumiho1: return "구미호 꼬리2";
            case Item_Type.gumiho2: return "구미호 꼬리3";
            case Item_Type.gumiho3: return "구미호 꼬리4";
            case Item_Type.gumiho4: return "구미호 꼬리5";
            case Item_Type.gumiho5: return "구미호 꼬리6";
            case Item_Type.gumiho6: return "구미호 꼬리7";
            case Item_Type.gumiho7: return "구미호 꼬리8";
            case Item_Type.gumiho8: return "구미호 꼬리9";

            case Item_Type.h0: return TableManager.Instance.hellAbil.dataArray[0].Name;
            case Item_Type.h1: return TableManager.Instance.hellAbil.dataArray[1].Name;
            case Item_Type.h2: return TableManager.Instance.hellAbil.dataArray[2].Name;
            case Item_Type.h3: return TableManager.Instance.hellAbil.dataArray[3].Name;
            case Item_Type.h4: return TableManager.Instance.hellAbil.dataArray[4].Name;
            case Item_Type.h5: return TableManager.Instance.hellAbil.dataArray[5].Name;
            case Item_Type.h6: return TableManager.Instance.hellAbil.dataArray[6].Name;
            case Item_Type.h7: return TableManager.Instance.hellAbil.dataArray[7].Name;
            case Item_Type.h8: return TableManager.Instance.hellAbil.dataArray[8].Name;
            case Item_Type.h9: return TableManager.Instance.hellAbil.dataArray[9].Name;
            case Item_Type.Hel: return "불멸석";
            case Item_Type.Ym: return "염주";
            case Item_Type.du: return "저승 명부";
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
            case StatusType.SuperCritical1Prob:
                return "천공베기 확률(%)";
                break;
            case StatusType.SuperCritical1DamPer:
                return "천공베기 피해(%)";
                break;
            case StatusType.MarbleAddPer:
                return "여우구슬 추가 획득(%)";
                break;
            case StatusType.SuperCritical2DamPer:
                return "필멸 피해(%)";
                break;
            case StatusType.growthStoneUp:
                return "수련의돌 추가 획득";
                break;
            case StatusType.WeaponHasUp:
                return "무기 보유효과 강화";
                break;
            case StatusType.NorigaeHasUp:
                return "노리개 보유효과 강화";
                break;
            case StatusType.PetEquipHasUp:
                return "환수장비 보유효과 강화";
                break;
            case StatusType.PetEquipProbUp:
                return "환수장비 강화확률 증가";
                break;
            case StatusType.DecreaseBossHp:
                return "스테이지 보스 체력 감소(%)";
                break;   
            case StatusType.SuperCritical3DamPer:
                return "지옥베기 피해(%)";
                break;
        }

        return "등록필요";
    }

    public static string GetDialogTextName(DialogCharcterType type)
    {
        return "미등록";
    }

    public static string GetGuildGradeName(GuildGrade grade)
    {
        switch (grade)
        {
            case GuildGrade.Member:
                return "문파원";
                break;
            case GuildGrade.ViceMaster:
                return "부문주";
                break;
            case GuildGrade.Master:
                return "문주";
                break;
        }

        return "미등록";
    }
}
