using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonUiContainer : SingletonMono<CommonUiContainer>
{
    public List<Sprite> skillGradeFrame;

    public List<Sprite> itemGradeFrame;

    private List<string> itemGradeName = new List<string>() { CommonString.ItemGrade_0, CommonString.ItemGrade_1, CommonString.ItemGrade_2, CommonString.ItemGrade_3, CommonString.ItemGrade_4 };
    public List<string> ItemGradeName => itemGradeName;

    public List<Color> itemGradeColor;

    [SerializeField]
    private List<Sprite> costumeThumbnail;

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
}
