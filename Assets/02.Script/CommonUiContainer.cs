using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonUiContainer : SingletonMono<CommonUiContainer>
{
    public List<Sprite> skillGradeFrame;

    public List<Sprite> itemGradeFrame;

    private List<string> itemGradeName_Weapon = new List<string>() { CommonString.ItemGrade_0, 
        CommonString.ItemGrade_1, CommonString.ItemGrade_2, CommonString.ItemGrade_3, CommonString.ItemGrade_4, CommonString.ItemGrade_5,
        CommonString.ItemGrade_6, CommonString.ItemGrade_7, CommonString.ItemGrade_8, CommonString.ItemGrade_9, CommonString.ItemGrade_10,
        CommonString.ItemGrade_11, CommonString.ItemGrade_12, CommonString.ItemGrade_13, CommonString.ItemGrade_14, CommonString.ItemGrade_15, 
        CommonString.ItemGrade_16, CommonString.ItemGrade_17, CommonString.ItemGrade_18, CommonString.ItemGrade_19, CommonString.ItemGrade_20 ,
        CommonString.ItemGrade_21 , CommonString.ItemGrade_22 , CommonString.ItemGrade_23 , CommonString.ItemGrade_24 };
    public List<string> ItemGradeName_Weapon => itemGradeName_Weapon;

    private List<string> itemGradeName_Norigae = new List<string>() { CommonString.ItemGrade_0, 
        CommonString.ItemGrade_1, CommonString.ItemGrade_2, CommonString.ItemGrade_3, CommonString.ItemGrade_4, CommonString.ItemGrade_5_Norigae, //1~5
        CommonString.ItemGrade_6_Norigae, CommonString.ItemGrade_7_Norigae, CommonString.ItemGrade_8_Norigae, CommonString.ItemGrade_9_Norigae, CommonString.ItemGrade_10_Norigae,  //6~10
        string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,  //11~15
        CommonString.ItemGrade_11_Norigae, CommonString.ItemGrade_12_Norigae, CommonString.ItemGrade_13_Norigae, string.Empty, string.Empty,//16~20
        string.Empty, CommonString.ItemGrade_22_Norigae, string.Empty,CommonString.ItemGrade_24_Norigae ,string.Empty //21~25
    }; 
    public List<string> ItemGradeName_Norigae => itemGradeName_Norigae;

    private List<string> itemGradeName_Skill = new List<string>() { 
        CommonString.ItemGrade_0,
        CommonString.ItemGrade_1, CommonString.ItemGrade_2, CommonString.ItemGrade_3, CommonString.ItemGrade_4_Skill, CommonString.ItemGrade_5_Skill,
        CommonString.ItemGrade_6_Skill, CommonString.ItemGrade_7_Skill, CommonString.ItemGrade_8_Skill, CommonString.ItemGrade_9_Skill, CommonString.ItemGrade_10_Skill,
        CommonString.ItemGrade_11_Skill };
    public List<string> ItemGradeName_Skill => itemGradeName_Skill;

    private List<string> itemGradeName_NewGacha= new List<string>() { 
        CommonString.ItemGrade_0,
        CommonString.ItemGrade_1, CommonString.ItemGrade_2, CommonString.ItemGrade_3, CommonString.ItemGrade_4, CommonString.ItemGrade_5};
    public List<string> ItemGradeName_NewGacha => itemGradeName_NewGacha;

    public List<Color> itemGradeColor;

    [SerializeField]
    private List<Sprite> costumeThumbnail;

    [SerializeField]
    private List<Sprite> rankFrame;

    [SerializeField]
    public List<Sprite> petEquipment;

    [SerializeField]
    public List<Color> petEquipColor;

    public Sprite GetCostumeThumbnail(int idx)
    {
        if (idx >= costumeThumbnail.Count) return null;

        return costumeThumbnail[idx];
    }

    public Sprite magicStone;

    public Sprite blueStone;

    public Sprite gold;

    public Sprite memory;

    public Sprite ticket;

    public Sprite marble;

    public Sprite dokebi;

    public Sprite WeaponUpgradeStone;

    public Sprite YomulExchangeStone;

    public Sprite TigerBossStone;

    public Sprite RabitBossStone;

    public Sprite DragonBossStone;
    public Sprite SnakeStone;
    public Sprite HorseStone;
    public Sprite SheepStone;
    public Sprite CockStone;
    public Sprite MonkeyStone;
    public Sprite DogStone;
    public Sprite PigStone;
    public Sprite MiniGameTicket;
    public Sprite MiniGameTicket2;

    public Sprite Songpyeon;

    public Sprite EventCollection;
    public Sprite EventCollection2;

    public Sprite StageRelic;

    public Sprite Peach;

    public Sprite relic;

    public Sprite relicEnter;
    public Sprite SwordPartial;
    public Sprite HaeNorigae;
    public Sprite HaePet;
    public Sprite SamNorigae;
    public Sprite SamPet;
    public Sprite KirinNorigae;
    public Sprite DogNorigae;
    public Sprite Kirin_Pet;
    public Sprite RabitNorigae;
    public Sprite YeaRaeNorigae;
    public Sprite GangrimNorigae;

    public Sprite ChunNorigae0;
    public Sprite ChunNorigae1;
    public Sprite ChunNorigae2;
    public Sprite ChunNorigae3;
    public Sprite ChunNorigae4;
    public Sprite ChunNorigae5;
    public Sprite ChunNorigae6;

    public Sprite DokebiNorigae0;
    public Sprite DokebiNorigae1;
    public Sprite DokebiNorigae2;
    public Sprite DokebiNorigae3;
    public Sprite DokebiNorigae4;
    public Sprite DokebiNorigae5;
    public Sprite DokebiNorigae6;

    public Sprite DokebiNorigae7;
    public Sprite DokebiNorigae8;
    public Sprite DokebiNorigae9;

    public Sprite SumisanNorigae0;
    public Sprite SumisanNorigae1;
    public Sprite SumisanNorigae2;
    public Sprite SumisanNorigae3;

    public Sprite MonthNorigae0;
    public Sprite MonthNorigae1;
    public Sprite MonthNorigae2;
    
    public Sprite DokebiHorn0;
    public Sprite DokebiHorn1;
    public Sprite DokebiHorn2;
    public Sprite DokebiHorn3;
    public Sprite DokebiHorn4;
    public Sprite DokebiHorn5;
    public Sprite DokebiHorn6;

    public Sprite DokebiHorn7;
    public Sprite DokebiHorn8;
    public Sprite DokebiHorn9;

    public Sprite ChunSun0;
    public Sprite ChunSun1;
    public Sprite ChunSun2;

    public Sprite YeaRaeWeapon;
    public Sprite GangrimWeapon;
    public Sprite HaeWeapon;
    public Sprite RabitPet;
    public Sprite DogPet;
    public Sprite ChunMaPet;
    public Sprite ChunPet0;
    public Sprite ChunPet1;
    public Sprite ChunPet2;
    public Sprite ChunPet3;

    public Sprite SasinsuPet0;
    public Sprite SasinsuPet1;
    public Sprite SasinsuPet2;
    public Sprite SasinsuPet3;

    public Sprite Hel;
    public Sprite YeoMarble;
    public Sprite du;
    public Sprite Fw;
    public Sprite Cw;
    public Sprite Event_Fall;
    public Sprite Event_Fall_Gold;
    public Sprite Event_XMas;
    public Sprite FoxMaskPartial;
    public Sprite DokebiFire;
    public Sprite Mileage;
    public Sprite HellPower;
    public Sprite DokebiFireKey;
    public Sprite DokebiTreasure;
    public Sprite DokebiFireEnhance;
    public Sprite SusanoTreasure;
    public Sprite SumiFire;
    public Sprite SumiFireKey;
    public Sprite NewGachaEnergy;
    public Sprite DokebiBundle;

    public List<SkeletonDataAsset> enemySpineAssets;

    public Sprite GuildReward;
    public Sprite SulItem;
    public Sprite FeelMulStone;
    public Sprite SmithFire;
    public Sprite AsuraHand0;
    public Sprite AsuraHand1;
    public Sprite AsuraHand2;
    public Sprite AsuraHand3;
    public Sprite AsuraHand4;
    public Sprite AsuraHand5;

    public Sprite Indra0;
    public Sprite Indra1;
    public Sprite Indra2;
    public Sprite IndraPower;

    public Sprite OrochiTooth0;
    public Sprite OrochiTooth1;

    public Sprite springIcon;
    public Sprite Aduk;

    public Sprite SinSkill0;
    public Sprite SinSkill1;
    public Sprite SinSkill2;
    public Sprite SinSkill3;
    public Sprite LeeMuGiStone;
    public Sprite IndraWeapon;
    public Sprite Event_Item_SnowMan;
    public Sprite NataWeapon;
    public Sprite OrochiWeapon;
    public Sprite ChunWeapon0;
    public Sprite ChunWeapon1;
    public Sprite ChunWeapon2;
    public Sprite ChunWeapon3;

    public Sprite DokebiWeapon0;
    public Sprite DokebiWeapon1;
    public Sprite DokebiWeapon2;
    public Sprite DokebiWeapon3;
    public Sprite DokebiWeapon4;
    public Sprite DokebiWeapon5;
    public Sprite DokebiWeapon6;

    public Sprite DokebiWeapon7;
    public Sprite DokebiWeapon8;
    public Sprite DokebiWeapon9;

    public Sprite SumisanWeapon0;
    public Sprite SumisanWeapon1;
    public Sprite SumisanWeapon2;
    public Sprite SumisanWeapon3;
    

    public Sprite SasinsuWeapon0;
    public Sprite SasinsuWeapon1;
    public Sprite SasinsuWeapon2;
    public Sprite SasinsuWeapon3;
    

    public Sprite NataSkill;
    public Sprite OrochiSkill;
    public Sprite GangrimSkill;
    public Sprite MihoWeapon;
    public Sprite MihoNorigae;
    public Sprite MihoTail;
    public Sprite ChunMaNorigae;

    public Sprite RecommendWeapon0;
    public Sprite RecommendWeapon1;
    public Sprite RecommendWeapon2;
    public Sprite RecommendWeapon3;
    public Sprite RecommendWeapon4;

    public Sprite RecommendWeapon5;
    public Sprite RecommendWeapon6;
    public Sprite RecommendWeapon7;
    public Sprite RecommendWeapon8;
    public Sprite RecommendWeapon9;

    public Sprite RecommendWeapon10;
    public Sprite RecommendWeapon11;
    public Sprite RecommendWeapon12;

    public Sprite RecommendWeapon13;
    public Sprite RecommendWeapon14;
    public Sprite RecommendWeapon15;
    public Sprite RecommendWeapon16;
    public Sprite RecommendWeapon17;
    public Sprite RecommendWeapon18;
    public Sprite RecommendWeapon19;
    public Sprite RecommendWeapon20;

    public Sprite weapon81;

    public Sprite Sun0;
    public Sprite Sun1;
    public Sprite Sun2;
    public Sprite Sun3;
    public Sprite Sun4;

    public Sprite Chun0;
    public Sprite Chun1;
    public Sprite Chun2;
    public Sprite Chun3;
    public Sprite Chun4;

    public Sprite DokebiSkill0;
    public Sprite DokebiSkill1;
    public Sprite DokebiSkill2;
    public Sprite DokebiSkill3;
    public Sprite DokebiSkill4;


    public Sprite FourSkill0;
    public Sprite FourSkill1;
    public Sprite FourSkill2;
    public Sprite FourSkill3;

    public Sprite HellMark0;
    public Sprite HellMark1;
    public Sprite HellMark2;
    public Sprite HellMark3;
    public Sprite HellMark4;
    public Sprite HellMark5;
    public Sprite HellMark6;
    public Sprite HellMark7;


    public Sprite GetItemIcon(Item_Type type)
    {
        switch (type)
        {
            case Item_Type.Gold:
                return gold;
                break;
            case Item_Type.Jade:
                return blueStone;
                break;
            case Item_Type.GrowthStone:
                return magicStone;
                break;
            case Item_Type.Memory:
                return memory;
                break;
            case Item_Type.Ticket:
                return ticket;
            case Item_Type.Marble:
                return marble;
            case Item_Type.Dokebi:
                return dokebi;
            case Item_Type.costume1:
                return costumeThumbnail[1];
            case Item_Type.costume2:
                return costumeThumbnail[2];
            case Item_Type.costume3:
                return costumeThumbnail[3];
            case Item_Type.costume4:
                return costumeThumbnail[4];
                break;
            case Item_Type.costume8:
                return costumeThumbnail[8];
                break;
            case Item_Type.costume11:
                return costumeThumbnail[11];
                break;
            case Item_Type.costume12:
                return costumeThumbnail[12];
                break;
            case Item_Type.costume13:
                return costumeThumbnail[13];
                break;
            case Item_Type.costume14:
                return costumeThumbnail[14];
                break;
            case Item_Type.costume15:
                return costumeThumbnail[15];
                break;
            case Item_Type.costume16:
                return costumeThumbnail[16];
                break;
            case Item_Type.costume17:
                return costumeThumbnail[17];
                break;
            case Item_Type.costume18:
                return costumeThumbnail[18];
                break;
            case Item_Type.costume19:
                return costumeThumbnail[19];
                break;
            case Item_Type.costume20:
                return costumeThumbnail[20];
                break;
            case Item_Type.costume21:
                return costumeThumbnail[21];
                break;
            case Item_Type.costume22:
                return costumeThumbnail[22];
                break;

            case Item_Type.costume23:
                return costumeThumbnail[23];
                break;

            case Item_Type.costume24:
                return costumeThumbnail[24];
                break;

            case Item_Type.costume25:
                return costumeThumbnail[25];
                break;

            case Item_Type.costume26:
                return costumeThumbnail[26];
                break;

            case Item_Type.costume27:
                return costumeThumbnail[27];
                break;

            case Item_Type.costume28:
                return costumeThumbnail[28];
                break;


            case Item_Type.costume29:
                return costumeThumbnail[29];
                break;

            case Item_Type.costume30:
                return costumeThumbnail[30];
                break;
            case Item_Type.costume31:
                return costumeThumbnail[31];
                break;

            case Item_Type.costume32:
                return costumeThumbnail[32];
                break;

            case Item_Type.costume33:
                return costumeThumbnail[33];
                break;

            case Item_Type.costume34:
                return costumeThumbnail[34];
                break;

            case Item_Type.costume35:
                return costumeThumbnail[35];
                break;
            case Item_Type.costume36:
                return costumeThumbnail[36];
                break;

            case Item_Type.costume37:
                return costumeThumbnail[37];
                break;

            case Item_Type.costume38:
                return costumeThumbnail[38];
                break;
            case Item_Type.costume39:
                return costumeThumbnail[39];
                break;
            case Item_Type.costume40:
                return costumeThumbnail[40];
                break;
            case Item_Type.costume41:
                return costumeThumbnail[41];
                break;
            case Item_Type.costume42:
                return costumeThumbnail[42];
                break;
            case Item_Type.costume43:
                return costumeThumbnail[43];
                break;
            case Item_Type.costume44:
                return costumeThumbnail[44];
                break;
            case Item_Type.costume45:
                return costumeThumbnail[45];
                break;
            case Item_Type.costume46:
                return costumeThumbnail[46];
                break;
            case Item_Type.costume47:
                return costumeThumbnail[47];
                break;
            case Item_Type.costume48:
                return costumeThumbnail[48];
                break;
            case Item_Type.costume49:
                return costumeThumbnail[49];
                break;
            case Item_Type.costume50:
                return costumeThumbnail[50];
                break;

            case Item_Type.costume51:
                return costumeThumbnail[51];
                break;

            case Item_Type.costume52:
                return costumeThumbnail[52];
                break;
            case Item_Type.costume53:
                return costumeThumbnail[53];
                break;
            case Item_Type.costume54:
                return costumeThumbnail[54];
                break;
            case Item_Type.costume55:
                return costumeThumbnail[55];
                break;

            //
            case Item_Type.costume56:
                return costumeThumbnail[56];
                break;
            case Item_Type.costume57:
                return costumeThumbnail[57];
                break;
            case Item_Type.costume58:
                return costumeThumbnail[58];
                break;

            case Item_Type.costume59:
                return costumeThumbnail[59];
                break;
            case Item_Type.costume60:
                return costumeThumbnail[60];
                break;
            case Item_Type.costume61:
                return costumeThumbnail[61];
                break;
            //
            case Item_Type.costume62:
                return costumeThumbnail[62];
                break;
            case Item_Type.costume63:
                return costumeThumbnail[63];
                break;
            case Item_Type.costume64:
                return costumeThumbnail[64];
                break;
            case Item_Type.costume65:
                return costumeThumbnail[65];
                break;
            case Item_Type.costume66:
                return costumeThumbnail[66];
                break;
            case Item_Type.costume67:
                return costumeThumbnail[67];
                break;
            case Item_Type.costume68:
                return costumeThumbnail[68];
                break;
            case Item_Type.costume69:
                return costumeThumbnail[69];
                break;
            case Item_Type.costume70:
                return costumeThumbnail[70];
                break;
            case Item_Type.costume71:
                return costumeThumbnail[71];
                break;
            case Item_Type.costume72:
                return costumeThumbnail[72];
                break;
            case Item_Type.costume73:
                return costumeThumbnail[73];
                break;
            case Item_Type.costume74:
                return costumeThumbnail[74];
                break;
            case Item_Type.costume75:
                return costumeThumbnail[75];
                break;  
            case Item_Type.costume76:
                return costumeThumbnail[76];
                break;   
            case Item_Type.costume77:
                return costumeThumbnail[77];
                break;
            case Item_Type.costume78:
                return costumeThumbnail[78];
                break;  
            case Item_Type.costume79:
                return costumeThumbnail[79];
                break;   
            case Item_Type.costume80:
                return costumeThumbnail[80];
                break;
            case Item_Type.costume81:
                return costumeThumbnail[81];
                break;
            case Item_Type.costume82:
                return costumeThumbnail[82];
                break;
            case Item_Type.costume83:
                return costumeThumbnail[83];
                break;
            case Item_Type.costume84:
                return costumeThumbnail[84];
                break;
            case Item_Type.costume85:
                return costumeThumbnail[85];
                break;
            case Item_Type.costume86:
                return costumeThumbnail[86];
                break;


            case Item_Type.RankFrame1:
                return rankFrame[8];
                break;
            case Item_Type.RankFrame2:
                return rankFrame[7];
                break;
            case Item_Type.RankFrame3:
                return rankFrame[6];
                break;
            case Item_Type.RankFrame4:
                return rankFrame[5];
                break;
            case Item_Type.RankFrame5:
                return rankFrame[4];
                break;
            case Item_Type.RankFrame6_20:
                return rankFrame[3];
                break;
            case Item_Type.RankFrame21_100:
                return rankFrame[2];
                break;
            case Item_Type.RankFrame101_1000:
                return rankFrame[1];
                break;
            case Item_Type.RankFrame1001_10000:
                return rankFrame[9];
                break;

            case Item_Type.PartyRaidRankFrame1:
                return rankFrame[8];
                break;
            case Item_Type.PartyRaidRankFrame2:
                return rankFrame[7];
                break;
            case Item_Type.PartyRaidRankFrame3:
                return rankFrame[6];
                break;
            case Item_Type.PartyRaidRankFrame4:
                return rankFrame[5];
                break;
            case Item_Type.PartyRaidRankFrame5:
                return rankFrame[4];
                break;
            case Item_Type.PartyRaidRankFrame6_20:
                return rankFrame[3];
                break;
            case Item_Type.PartyRaidRankFrame21_100:
                return rankFrame[2];
                break;
            case Item_Type.PartyRaidRankFrame101_1000:
                return rankFrame[1];
                break;
            case Item_Type.PartyRaidRankFrame1001_10000:
                return rankFrame[9];
                break;


            case Item_Type.RankFrame1_relic:
            case Item_Type.RankFrame2_relic:
            case Item_Type.RankFrame3_relic:
            case Item_Type.RankFrame4_relic:
            case Item_Type.RankFrame5_relic:
            case Item_Type.RankFrame6_20_relic:
            case Item_Type.RankFrame21_100_relic:
            case Item_Type.RankFrame101_1000_relic:
            case Item_Type.RankFrame1001_10000_relic:
                return relicEnter;
                break;

            case Item_Type.RankFrame1_relic_hell:
            case Item_Type.RankFrame2_relic_hell:
            case Item_Type.RankFrame3_relic_hell:
            case Item_Type.RankFrame4_relic_hell:
            case Item_Type.RankFrame5_relic_hell:
            case Item_Type.RankFrame6_20_relic_hell:
            case Item_Type.RankFrame21_100_relic_hell:
            case Item_Type.RankFrame101_1000_relic_hell:
            case Item_Type.RankFrame1001_10000_relic_hell:
                return Hel;
                break;
            case Item_Type.RankFrame1_2_war_hell: { return HellMark7; }break;
            case Item_Type.RankFrame3_5_war_hell: { return HellMark6; } break;
            case Item_Type.RankFrame6_20_war_hell: { return HellMark5; } break;
            case Item_Type.RankFrame21_50_war_hell: { return HellMark4; } break;
            case Item_Type.RankFrame51_100_war_hell: { return HellMark3; } break;
            case Item_Type.RankFrame101_1000_war_hell: { return HellMark2; } break;
            case Item_Type.RankFrame1001_10000_war_hell: { return HellMark1; } break;


            case Item_Type.RankFrame1_miniGame:
            case Item_Type.RankFrame2_miniGame:
            case Item_Type.RankFrame3_miniGame:
            case Item_Type.RankFrame4_miniGame:
            case Item_Type.RankFrame5_miniGame:
            case Item_Type.RankFrame6_20_miniGame:
            case Item_Type.RankFrame21_100_miniGame:
            case Item_Type.RankFrame101_1000_miniGame:
            case Item_Type.RankFrame1001_10000_miniGame:
                return MiniGameTicket;
                break;

            case Item_Type.RankFrame1_new_miniGame:
            case Item_Type.RankFrame2_new_miniGame:
            case Item_Type.RankFrame3_new_miniGame:
            case Item_Type.RankFrame4_new_miniGame:
            case Item_Type.RankFrame5_new_miniGame:
            case Item_Type.RankFrame6_20_new_miniGame:
            case Item_Type.RankFrame21_100_new_miniGame:
            case Item_Type.RankFrame101_1000_new_miniGame:
            case Item_Type.RankFrame1001_10000_new_miniGame:
                return MiniGameTicket2;
                break;

            case Item_Type.RankFrame1_guild:
            case Item_Type.RankFrame2_guild:
            case Item_Type.RankFrame3_guild:
            case Item_Type.RankFrame4_guild:
            case Item_Type.RankFrame5_guild:
            case Item_Type.RankFrame6_20_guild:
            case Item_Type.RankFrame21_100_guild:
            case Item_Type.RankFrame101_1000_guild:
                return GuildReward;
                break;

            case Item_Type.RankFrame1guild_new:
            case Item_Type.RankFrame2guild_new:
            case Item_Type.RankFrame3guild_new:
            case Item_Type.RankFrame4guild_new:
            case Item_Type.RankFrame5guild_new:
            case Item_Type.RankFrame6_20_guild_new:
            case Item_Type.RankFrame21_50_guild_new:
            case Item_Type.RankFrame51_100_guild_new:
                return GuildReward;
                break;

            case Item_Type.RankFrameParty1guild_new:
            case Item_Type.RankFrameParty2guild_new:
            case Item_Type.RankFrameParty3guild_new:
            case Item_Type.RankFrameParty4guild_new:
            case Item_Type.RankFrameParty5guild_new:
            case Item_Type.RankFrameParty6_20_guild_new:
            case Item_Type.RankFrameParty21_50_guild_new:
            case Item_Type.RankFrameParty51_100_guild_new:
                return GuildReward;
                break;

            case Item_Type.RankFrame1_boss_new:
            case Item_Type.RankFrame2_boss_new:
            case Item_Type.RankFrame3_boss_new:
            case Item_Type.RankFrame4_boss_new:
            case Item_Type.RankFrame5_boss_new:
            case Item_Type.RankFrame6_10_boss_new:
            case Item_Type.RankFrame10_30_boss_new:
            case Item_Type.RankFrame30_50boss_new:
            case Item_Type.RankFrame50_70_boss_new:
            case Item_Type.RankFrame70_100_boss_new:
            case Item_Type.RankFrame100_200_boss_new:
            case Item_Type.RankFrame200_500_boss_new:
            case Item_Type.RankFrame500_1000_boss_new:
            case Item_Type.RankFrame1000_3000_boss_new:

                return Peach;
                break;
            case Item_Type.RankFrame1_boss_GangChul:
            case Item_Type.RankFrame2_boss_GangChul:
            case Item_Type.RankFrame3_boss_GangChul:
            case Item_Type.RankFrame4_boss_GangChul:
            case Item_Type.RankFrame5_boss_GangChul:
            case Item_Type.RankFrame6_10_boss_GangChul:
            case Item_Type.RankFrame10_30_boss_GangChul:
            case Item_Type.RankFrame30_50_boss_GangChul:
            case Item_Type.RankFrame50_70_boss_GangChul:
            case Item_Type.RankFrame70_100_boss_GangChul:
            case Item_Type.RankFrame100_200_boss_GangChul:
            case Item_Type.RankFrame200_500_boss_GangChul:
            case Item_Type.RankFrame500_1000_boss_GangChul:
            case Item_Type.RankFrame1000_3000_boss_GangChul:

                return Cw;
                break;

            case Item_Type.WeaponUpgradeStone:
                return WeaponUpgradeStone;
                break;
            case Item_Type.YomulExchangeStone:
                return YomulExchangeStone;
                break;
            case Item_Type.Songpyeon:
                return Songpyeon;
                break;
            case Item_Type.TigerBossStone:
                return TigerBossStone;
            case Item_Type.RabitBossStone:
                return RabitBossStone;
            case Item_Type.DragonBossStone:
                return DragonBossStone;
                break;
            case Item_Type.SnakeStone:
                return SnakeStone;
                break;
            case Item_Type.HorseStone:
                return HorseStone;
                break;
            case Item_Type.SheepStone:
                return SheepStone;
                break;
            case Item_Type.CockStone:
                return CockStone;
                break;
            case Item_Type.DogStone:
                return DogStone;
                break;
            case Item_Type.PigStone:
                return PigStone;
                break;
            case Item_Type.MonkeyStone:
                return MonkeyStone;
                break;
            case Item_Type.MiniGameReward:
                return MiniGameTicket;
                break;
            case Item_Type.MiniGameReward2:
                return MiniGameTicket2;
                break;
            case Item_Type.Relic:
                return relic;
                break;
            case Item_Type.RelicTicket:
                return relicEnter;
                break;
            case Item_Type.Event_Item_0:
                return EventCollection;
                break;
            case Item_Type.StageRelic:
                return StageRelic;
                break;

            case Item_Type.PeachReal:
                return Peach;
                break;

            case Item_Type.SP:
                return SwordPartial;
                break;

            case Item_Type.Hel:
                return Hel;
                break;

            case Item_Type.Ym:
                return YeoMarble;
                break;

            case Item_Type.Fw:
                return Fw;
                break;
            case Item_Type.Cw:
                return Cw;
                break; 
            case Item_Type.FoxMaskPartial:
                return FoxMaskPartial;
                break;
            case Item_Type.DokebiFire:
                return DokebiFire;
                break;   
            case Item_Type.SumiFire:
                return SumiFire;
                break;
            case Item_Type.NewGachaEnergy:
                return NewGachaEnergy;
                break;   
            case Item_Type.DokebiBundle:
                return DokebiBundle;
                break;   
            case Item_Type.HellPower:
                return HellPower;
                break;  
            case Item_Type.DokebiTreasure:
                return DokebiTreasure;
                break;
            case Item_Type.DokebiFireEnhance:
                return DokebiFireEnhance;
                break; 
            case Item_Type.SusanoTreasure:
                return SusanoTreasure;
                break; 
            case Item_Type.DokebiFireKey:
                return DokebiFireKey;
                break; 
            case Item_Type.SumiFireKey:
                return SumiFireKey;
                break;
            case Item_Type.Mileage:
                return Mileage;
                break;
            case Item_Type.Event_Fall:
                return Event_Fall;
                break;
            case Item_Type.Event_Fall_Gold:
                return Event_Fall_Gold;
                break;
            case Item_Type.Event_NewYear:
                return Event_XMas;
                break;

            case Item_Type.du:
                return du;
                break;

            case Item_Type.Hae_Norigae:
                return HaeNorigae;
                break;

            case Item_Type.Hae_Pet:
                return HaePet;
                break;

            case Item_Type.Sam_Norigae:
                return SamNorigae;
                break;

            case Item_Type.KirinNorigae:
                return KirinNorigae;
                break;
            case Item_Type.DogNorigae:
                return DogNorigae;
                break;
            case Item_Type.RabitNorigae:
                return RabitNorigae;
                break;
            case Item_Type.YeaRaeNorigae:
                return YeaRaeNorigae;
                break;
            case Item_Type.GangrimNorigae:
                return GangrimNorigae;
                break;

            case Item_Type.ChunNorigae0:
                return ChunNorigae0;
                break;

            case Item_Type.ChunNorigae1:
                return ChunNorigae1;
                break;

            case Item_Type.ChunNorigae2:
                return ChunNorigae2;
                break;

            case Item_Type.ChunNorigae3:
                return ChunNorigae3;
                break;

            case Item_Type.ChunNorigae4:
                return ChunNorigae4;
                break;

            case Item_Type.ChunNorigae5:
                return ChunNorigae5;
                break;

            case Item_Type.ChunNorigae6:
                return ChunNorigae6;
                break;
            //
            case Item_Type.DokebiNorigae0:
                return DokebiNorigae0;
                break;
            case Item_Type.DokebiNorigae1:
                return DokebiNorigae1;
                break;
            case Item_Type.DokebiNorigae2:
                return DokebiNorigae2;
                break;
            case Item_Type.DokebiNorigae3:
                return DokebiNorigae3;
                break;
            case Item_Type.DokebiNorigae4:
                return DokebiNorigae4;
                break;
            case Item_Type.DokebiNorigae5:
                return DokebiNorigae5;
                break;
            case Item_Type.DokebiNorigae6:
                return DokebiNorigae6;
                break;
            //
            case Item_Type.DokebiNorigae7:
                return DokebiNorigae7;
                break;
            case Item_Type.DokebiNorigae8:
                return DokebiNorigae8;
                break;
            case Item_Type.DokebiNorigae9:
                return DokebiNorigae9;
                break;
            //
            
            case Item_Type.SumisanNorigae0:
                return SumisanNorigae0;
                break;
            case Item_Type.SumisanNorigae1:
                return SumisanNorigae1;
                break;
            case Item_Type.SumisanNorigae2:
                return SumisanNorigae2;
                break;
            case Item_Type.SumisanNorigae3:
                return SumisanNorigae3;
                break;
            //
            case Item_Type.MonthNorigae0:
                return MonthNorigae0;
                break;
            case Item_Type.MonthNorigae1:
                return MonthNorigae1;
                break;
            case Item_Type.MonthNorigae2:
                return MonthNorigae2;
                break;
            //
            //
            case Item_Type.DokebiHorn0:
                return DokebiHorn0;
                break;
            case Item_Type.DokebiHorn1:
                return DokebiHorn1;
                break;
            case Item_Type.DokebiHorn2:
                return DokebiHorn2;
                break;
            case Item_Type.DokebiHorn3:
                return DokebiHorn3;
                break;
            case Item_Type.DokebiHorn4:
                return DokebiHorn4;
                break;
            case Item_Type.DokebiHorn5:
                return DokebiHorn5;
                break;
            case Item_Type.DokebiHorn6:
                return DokebiHorn6;

            case Item_Type.DokebiHorn7:
                return DokebiHorn7;
                break;
            case Item_Type.DokebiHorn8:
                return DokebiHorn8;
                break;
            case Item_Type.DokebiHorn9:
                return DokebiHorn9;
                break;
            //
            case Item_Type.ChunSun0:
                return ChunSun0;
                break;

            case Item_Type.ChunSun1:
                return ChunSun1;
                break;

            case Item_Type.ChunSun2:
                return ChunSun2;
                break;


            //


            case Item_Type.GangrimWeapon:
                return GangrimWeapon;
                break;   
            
            case Item_Type.YeaRaeWeapon:
                return YeaRaeWeapon;
                break;

            case Item_Type.HaeWeapon:
                return HaeWeapon;
                break;

            case Item_Type.Sam_Pet:
                return SamPet;
                break;
            case Item_Type.Kirin_Pet:
                return Kirin_Pet;
                break;
            case Item_Type.RabitPet:
                return RabitPet;
                break;
            case Item_Type.DogPet:
                return DogPet;
                break;

            case Item_Type.ChunMaPet:
                return ChunMaPet;
                break;
            case Item_Type.ChunPet0:
                return ChunPet0;
                break;
            case Item_Type.ChunPet1:
                return ChunPet1;
                break;
            case Item_Type.ChunPet2:
                return ChunPet2;
                break;
            case Item_Type.ChunPet3:
                return ChunPet3;
                break;
            case Item_Type.SasinsuPet0:
                return SasinsuPet0;
                break;
            case Item_Type.SasinsuPet1:
                return SasinsuPet1;
                break;
            case Item_Type.SasinsuPet2:
                return SasinsuPet2;
                break;
            case Item_Type.SasinsuPet3:
                return SasinsuPet3;
                break;

            case Item_Type.GuildReward:
                return GuildReward;
                break;
            case Item_Type.SulItem:
                return SulItem;
                break;
            case Item_Type.SmithFire:
                return SmithFire;
                break;
            case Item_Type.FeelMulStone:
                return FeelMulStone;
                break;

            case Item_Type.Asura0:
                return AsuraHand0;
                break;
            case Item_Type.Asura1:
                return AsuraHand1;
                break;
            case Item_Type.Asura2:
                return AsuraHand2;
                break;
            case Item_Type.Asura3:
                return AsuraHand3;
                break;
            case Item_Type.Asura4:
                return AsuraHand4;
                break;
            case Item_Type.Asura5:
                return AsuraHand5;
                break;
            //
            case Item_Type.Indra0:
                return Indra0;
                break;
            case Item_Type.Indra1:
                return Indra1;
                break;
            case Item_Type.Indra2:
                return Indra2;
                break;
            case Item_Type.IndraPower:
                return IndraPower;
                break;

            case Item_Type.OrochiTooth0:
                return OrochiTooth0;
                break;
            case Item_Type.OrochiTooth1:
                return OrochiTooth1;
                break;

            //
            case Item_Type.Aduk:
                return Aduk;
                break;
            case Item_Type.Event_Item_1:
                return springIcon;
                break;
            case Item_Type.Event_Item_SnowMan:
                return Event_Item_SnowMan;
                break;
            //
            case Item_Type.SinSkill0:
                return SinSkill0;
                break;
            case Item_Type.SinSkill1:
                return SinSkill1;
                break;
            case Item_Type.SinSkill2:
                return SinSkill2;
                break;
            case Item_Type.NataSkill:
                return NataSkill;
                break;
            case Item_Type.OrochiSkill:
                return OrochiSkill;
                break;
            //
            case Item_Type.Sun0:
                return Sun0;
                break;
            case Item_Type.Sun1:
                return Sun1;
                break;
            case Item_Type.Sun2:
                return Sun2;
                break;
            case Item_Type.Sun3:
                return Sun3;
                break;
            case Item_Type.Sun4:
                return Sun4;
                break;
            //
            case Item_Type.Chun0:
                return Chun0;
                break;
            case Item_Type.Chun1:
                return Chun1;
                break;
            case Item_Type.Chun2:
                return Chun2;
                break;
            case Item_Type.Chun3:
                return Chun3;
                break;
            case Item_Type.Chun4:
                return Chun4;
                break;
            //
            //
            case Item_Type.DokebiSkill0:
                return DokebiSkill0;
                break;
            case Item_Type.DokebiSkill1:
                return DokebiSkill1;
                break;
            case Item_Type.DokebiSkill2:
                return DokebiSkill2;
                break;
            case Item_Type.DokebiSkill3:
                return DokebiSkill3;
                break;
            case Item_Type.DokebiSkill4:
                return DokebiSkill4;
                break;
            //
            //
            case Item_Type.FourSkill0:
                return FourSkill0;
                break;
            case Item_Type.FourSkill1:
                return FourSkill1;
                break;
            case Item_Type.FourSkill2:
                return FourSkill2;
                break;
            case Item_Type.FourSkill3:
                return FourSkill3;
                break;
            //
            case Item_Type.GangrimSkill:
                return GangrimSkill;
                break;
            case Item_Type.SinSkill3:
                return SinSkill3;
                break;
            case Item_Type.LeeMuGiStone:
                return LeeMuGiStone;
                break;
            case Item_Type.IndraWeapon:
                return IndraWeapon;
                break;
            case Item_Type.NataWeapon:
                return NataWeapon;
                break;
            case Item_Type.OrochiWeapon:
                return OrochiWeapon;
                break;

            case Item_Type.MihoWeapon:
                return MihoWeapon;
                break;
            case Item_Type.ChunWeapon0:
                return ChunWeapon0;
                break;
            case Item_Type.ChunWeapon1:
                return ChunWeapon1;
                break;
            case Item_Type.ChunWeapon2:
                return ChunWeapon2;
                break;
            case Item_Type.ChunWeapon3:
                return ChunWeapon3;
                break;
            case Item_Type.DokebiWeapon0:
                return DokebiWeapon0;
                break;
            case Item_Type.DokebiWeapon1:
                return DokebiWeapon1;
                break;
            case Item_Type.DokebiWeapon2:
                return DokebiWeapon2;
                break;
            case Item_Type.DokebiWeapon3:
                return DokebiWeapon3;
                break;
            case Item_Type.DokebiWeapon4:
                return DokebiWeapon4;
                break;
            case Item_Type.DokebiWeapon5:
                return DokebiWeapon5;
                break;
            case Item_Type.DokebiWeapon6:
                return DokebiWeapon6;
                break;
            case Item_Type.DokebiWeapon7:
                return DokebiWeapon7;
                break;
            case Item_Type.DokebiWeapon8:
                return DokebiWeapon8;
                break;
            case Item_Type.DokebiWeapon9:
                return DokebiWeapon9;
                break;
            case Item_Type.SumisanWeapon0:
                return SumisanWeapon0;
                break;
            case Item_Type.SumisanWeapon1:
                return SumisanWeapon1;
                break;
            case Item_Type.SumisanWeapon2:
                return SumisanWeapon2;
                break;
            case Item_Type.SumisanWeapon3:
                return SumisanWeapon3;
                break;

            case Item_Type.SasinsuWeapon0:
                return SasinsuWeapon0;
                break;
            case Item_Type.SasinsuWeapon1:
                return SasinsuWeapon1;
                break;
            case Item_Type.SasinsuWeapon2:
                return SasinsuWeapon2;
                break;
            case Item_Type.SasinsuWeapon3:
                return SasinsuWeapon3;
                break;

            case Item_Type.MihoNorigae:
                return MihoNorigae;
                break;

            case Item_Type.ChunMaNorigae:
                return ChunMaNorigae;
                break;

            case Item_Type.RecommendWeapon0:
                return RecommendWeapon0;
                break;
            case Item_Type.RecommendWeapon1:
                return RecommendWeapon1;
                break;
            case Item_Type.RecommendWeapon2:
                return RecommendWeapon2;
                break;
            case Item_Type.RecommendWeapon3:
                return RecommendWeapon3;
                break;
            case Item_Type.RecommendWeapon4:
                return RecommendWeapon4;
                break;
            case Item_Type.RecommendWeapon5:
                return RecommendWeapon5;
                break;
            case Item_Type.RecommendWeapon6:
                return RecommendWeapon6;
                break;
            case Item_Type.RecommendWeapon7:
                return RecommendWeapon7;
                break;
            case Item_Type.RecommendWeapon8:
                return RecommendWeapon8;
                break;
            case Item_Type.RecommendWeapon9:
                return RecommendWeapon9;
                break;
            case Item_Type.RecommendWeapon10:
                return RecommendWeapon10;
                break;
            case Item_Type.RecommendWeapon11:
                return RecommendWeapon11;
                break;
            case Item_Type.RecommendWeapon12:
                return RecommendWeapon12;
                break;   
            case Item_Type.RecommendWeapon13:
                return RecommendWeapon13;
                break;
            case Item_Type.RecommendWeapon14:
                return RecommendWeapon14;
                break;
            case Item_Type.RecommendWeapon15:
                return RecommendWeapon15;
                break;
            case Item_Type.RecommendWeapon16:
                return RecommendWeapon16;
                break;
            case Item_Type.RecommendWeapon17:
                return RecommendWeapon17;
                break;
            case Item_Type.RecommendWeapon18:
                return RecommendWeapon18;
                break;
            case Item_Type.RecommendWeapon19:
                return RecommendWeapon19;
                break;
            case Item_Type.RecommendWeapon20:
                return RecommendWeapon20;
                break;
            case Item_Type.weapon81:
                return weapon81;
                break;

            case Item_Type.gumiho0:
            case Item_Type.gumiho1:
            case Item_Type.gumiho2:
            case Item_Type.gumiho3:
            case Item_Type.gumiho4:
            case Item_Type.gumiho5:
            case Item_Type.gumiho6:
            case Item_Type.gumiho7:
            case Item_Type.gumiho8:
                return MihoTail;
                break;

            case Item_Type.h0: return CommonResourceContainer.GetHellIconSprite(0);
            case Item_Type.h1: return CommonResourceContainer.GetHellIconSprite(1);
            case Item_Type.h2: return CommonResourceContainer.GetHellIconSprite(2);
            case Item_Type.h3: return CommonResourceContainer.GetHellIconSprite(3);
            case Item_Type.h4: return CommonResourceContainer.GetHellIconSprite(4);
            case Item_Type.h5: return CommonResourceContainer.GetHellIconSprite(5);
            case Item_Type.h6: return CommonResourceContainer.GetHellIconSprite(6);
            case Item_Type.h7: return CommonResourceContainer.GetHellIconSprite(7);
            case Item_Type.h8: return CommonResourceContainer.GetHellIconSprite(8);
            case Item_Type.h9:
                return CommonResourceContainer.GetHellIconSprite(9);
                break;

            case Item_Type.c0: return CommonResourceContainer.GetChunIconSprite(0);
            case Item_Type.c1: return CommonResourceContainer.GetChunIconSprite(1);
            case Item_Type.c2: return CommonResourceContainer.GetChunIconSprite(2);
            case Item_Type.c3: return CommonResourceContainer.GetChunIconSprite(3);
            case Item_Type.c4: return CommonResourceContainer.GetChunIconSprite(4);
            case Item_Type.c5: return CommonResourceContainer.GetChunIconSprite(5);
            case Item_Type.c6:
                return CommonResourceContainer.GetChunIconSprite(6);
                break;

        }

        return null;
    }

    public List<Sprite> statusIcon;

    public List<Sprite> loadingTipIcon;

    public List<Sprite> bossIcon;

    public List<SkeletonDataAsset> costumeList;

    public List<SkeletonDataAsset> petCostumeList;

    public List<GameObject> wingList;

    public List<Sprite> buffIconList;

    public List<Sprite> relicIconList;
    public List<Sprite> stageRelicIconList;

    public List<RuntimeAnimatorController> sonAnimators;
    public List<Sprite> sonThumbNail;

    public List<Sprite> guildIcon;
    public List<int> guildIconGrade;

    public List<Material> weaponEnhnaceMats;
}
