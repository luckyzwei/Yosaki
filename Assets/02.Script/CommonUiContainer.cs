using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonUiContainer : SingletonMono<CommonUiContainer>
{
    public List<Sprite> skillGradeFrame;

    public List<Sprite> itemGradeFrame;

    private List<string> itemGradeName_Weapon = new List<string>() { CommonString.ItemGrade_0, CommonString.ItemGrade_1, CommonString.ItemGrade_2, CommonString.ItemGrade_3, CommonString.ItemGrade_4, CommonString.ItemGrade_5, CommonString.ItemGrade_6, CommonString.ItemGrade_7, CommonString.ItemGrade_8, CommonString.ItemGrade_9, CommonString.ItemGrade_10, CommonString.ItemGrade_11, CommonString.ItemGrade_12, CommonString.ItemGrade_13, CommonString.ItemGrade_14, CommonString.ItemGrade_15, CommonString.ItemGrade_16, CommonString.ItemGrade_17, CommonString.ItemGrade_18, CommonString.ItemGrade_19 };
    public List<string> ItemGradeName_Weapon => itemGradeName_Weapon;

    private List<string> itemGradeName_Norigae = new List<string>() { CommonString.ItemGrade_0, CommonString.ItemGrade_1, CommonString.ItemGrade_2, CommonString.ItemGrade_3, CommonString.ItemGrade_4, CommonString.ItemGrade_5_Norigae, CommonString.ItemGrade_6_Norigae, CommonString.ItemGrade_7_Norigae, CommonString.ItemGrade_8_Norigae, CommonString.ItemGrade_9_Norigae, CommonString.ItemGrade_10_Norigae, CommonString.ItemGrade_11_Norigae, CommonString.ItemGrade_11_Norigae, CommonString.ItemGrade_11_Norigae, CommonString.ItemGrade_11_Norigae, CommonString.ItemGrade_11_Norigae, CommonString.ItemGrade_11_Norigae, CommonString.ItemGrade_12_Norigae, CommonString.ItemGrade_13_Norigae };
    public List<string> ItemGradeName_Norigae => itemGradeName_Norigae;

    private List<string> itemGradeName_Skill = new List<string>() { CommonString.ItemGrade_0, CommonString.ItemGrade_1, CommonString.ItemGrade_2, CommonString.ItemGrade_3, CommonString.ItemGrade_4_Skill, CommonString.ItemGrade_5_Skill, CommonString.ItemGrade_6_Skill, CommonString.ItemGrade_7_Skill, CommonString.ItemGrade_8_Skill, CommonString.ItemGrade_9_Skill };
    public List<string> ItemGradeName_Skill => itemGradeName_Skill;

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
    public Sprite Hel;
    public Sprite YeoMarble;
    public Sprite du;
    public Sprite Fw;
    public Sprite Cw;
    public Sprite Event_Fall;

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
    public Sprite Event_Item_Summer;
    public Sprite NataWeapon;
    public Sprite OrochiWeapon;
    public Sprite NataSkill;
    public Sprite OrochiSkill;
    public Sprite GangrimSkill;
    public Sprite MihoWeapon;
    public Sprite MihoNorigae;
    public Sprite MihoTail;
    public Sprite ChunMaNorigae;


    public Sprite Sun0;
    public Sprite Sun1;
    public Sprite Sun2;
    public Sprite Sun3;
    public Sprite Sun4;

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
            //


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

            case Item_Type.RankFrame1_2_war_hell:
            case Item_Type.RankFrame3_5_war_hell:
            case Item_Type.RankFrame6_20_war_hell:
            case Item_Type.RankFrame21_50_war_hell:
            case Item_Type.RankFrame51_100_war_hell:
            case Item_Type.RankFrame101_1000_war_hell:
            case Item_Type.RankFrame1001_10000_war_hell:

                return Hel;
                break;

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
            case Item_Type.Event_Fall:
                return Event_Fall;
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
            case Item_Type.Event_Item_Summer:
                return Event_Item_Summer;
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

            case Item_Type.MihoNorigae:
                return MihoNorigae;
                break;

            case Item_Type.ChunMaNorigae:
                return ChunMaNorigae;
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
