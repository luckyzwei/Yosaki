using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonUiContainer : SingletonMono<CommonUiContainer>
{
    public List<Sprite> skillGradeFrame;

    public List<Sprite> itemGradeFrame;

    private List<string> itemGradeName_Weapon = new List<string>() { CommonString.ItemGrade_0, CommonString.ItemGrade_1, CommonString.ItemGrade_2, CommonString.ItemGrade_3, CommonString.ItemGrade_4, CommonString.ItemGrade_5 };
    public List<string> ItemGradeName_Weapon => itemGradeName_Weapon;

    private List<string> itemGradeName_Norigae = new List<string>() { CommonString.ItemGrade_0, CommonString.ItemGrade_1, CommonString.ItemGrade_2, CommonString.ItemGrade_3, CommonString.ItemGrade_4, CommonString.ItemGrade_5_Norigae };
    public List<string> ItemGradeName_Norigae => itemGradeName_Norigae;

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

    public Sprite Songpyeon;

    public Sprite relic;

    public Sprite relicEnter;

    public List<SkeletonDataAsset> enemySpineAssets;

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

            case Item_Type.RankFrame1_relic:
            case Item_Type.RankFrame2_relic:
            case Item_Type.RankFrame3_relic:
            case Item_Type.RankFrame4_relic:
            case Item_Type.RankFrame5_relic:
            case Item_Type.RankFrame6_20_relic:
            case Item_Type.RankFrame21_100_relic:
            case Item_Type.RankFrame101_1000_relic:
                return relicEnter;
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

                break;
            case Item_Type.Relic:
                return relic;
                break;
            case Item_Type.RelicTicket:
                return relicEnter;
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

}
