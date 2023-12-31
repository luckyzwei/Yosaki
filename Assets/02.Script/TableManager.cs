using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;

public class TableManager : SingletonMono<TableManager>
{
    [SerializeField]
    private EnemyTable enemyTable;
    public EnemyTable EnemyTable => enemyTable;

    private Dictionary<int, EnemyTableData> enemyData = null;
    public Dictionary<int, EnemyTableData> EnemyData
    {
        get
        {
            LoadEnemyData();
            return enemyData;
        }
    }

    private void LoadEnemyData()
    {
        if (enemyData != null) return;

        enemyData = new Dictionary<int, EnemyTableData>();

        for (int i = 0; i < enemyTable.dataArray.Length; i++)
        {
            enemyData.Add(enemyTable.dataArray[i].Id, enemyTable.dataArray[i]);
        }
    }


    [SerializeField]
    private SkillTable skillTable;
    public SkillTable SkillTable => skillTable;
    private Dictionary<int, SkillTableData> skillData = null;
    public Dictionary<int, SkillTableData> SkillData
    {
        get
        {
            LoadSkillData();
            return skillData;
        }
    }

    private void LoadSkillData()
    {
        if (skillData != null) return;

        skillData = new Dictionary<int, SkillTableData>();

        for (int i = 0; i < skillTable.dataArray.Length; i++)
        {
            skillData.Add(skillTable.dataArray[i].Id, skillTable.dataArray[i]);
        }
    }

    [SerializeField]
    private NewGachaTable newGachaTable;
    public NewGachaTable NewGachaTable => newGachaTable;
    private Dictionary<int, NewGachaTableData> newGachaData = null;
    public Dictionary<int, NewGachaTableData> NewGachaData
    {
        get
        {
            LoadNewGachaData();
            return newGachaData;
        }
    }

    private void LoadNewGachaData()
    {
        if (newGachaData != null) return;

        newGachaData = new Dictionary<int, NewGachaTableData>();

        for (int i = 0; i < newGachaTable.dataArray.Length; i++)
        {
            newGachaData.Add(newGachaTable.dataArray[i].Id, newGachaTable.dataArray[i]);
        }
    }

    [SerializeField]
    private Weapon weaponTable;
    public Weapon WeaponTable => weaponTable;

    private Dictionary<int, WeaponData> weaponData = null;
    public Dictionary<int, WeaponData> WeaponData
    {
        get
        {
            LoadWeaponData();
            return weaponData;
        }
    }

    public WeaponData GetWeaponDataByStringId(string id)
    {
        var e = WeaponData.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Current.Value.Stringid == id) return e.Current.Value;
        }

        return null;
    }

    private void LoadWeaponData()
    {
        if (weaponData != null) return;

        weaponData = new Dictionary<int, WeaponData>();

        for (int i = 0; i < weaponTable.dataArray.Length; i++)
        {
            weaponData.Add(weaponTable.dataArray[i].Id, weaponTable.dataArray[i]);
        }
    }

    [SerializeField]
    private StageMap stageMapTable;
    public StageMap StageMapTable => stageMapTable;

    private Dictionary<int, StageMapData> stageMapData = null;
    public Dictionary<int, StageMapData> StageMapData
    {
        get
        {
            LoadStageMapData();
            return stageMapData;
        }
    }
    public int GetLastStageIdx()
    {
        return stageMapTable.dataArray[stageMapTable.dataArray.Length - 1].Id;
    }

    public int GetLastStagePreset()
    {
        return stageMapTable.dataArray[stageMapTable.dataArray.Length - 1].Mappreset;
    }

    private void LoadStageMapData()
    {
        if (stageMapData != null) return;

        stageMapData = new Dictionary<int, StageMapData>();

        for (int i = 0; i < stageMapTable.dataArray.Length; i++)
        {
            stageMapData.Add(stageMapTable.dataArray[i].Id, stageMapTable.dataArray[i]);
        }
    }

    [SerializeField]
    private DailyMission dailyMission;

    public DailyMission DailyMission => dailyMission;

    private Dictionary<int, DailyMissionData> dailyMissionDatas;
    public Dictionary<int, DailyMissionData> DailyMissionDatas
    {
        get
        {
            if (dailyMissionDatas == null)
            {
                dailyMissionDatas = new Dictionary<int, DailyMissionData>();

                for (int i = 0; i < dailyMission.dataArray.Length; i++)
                {
                    dailyMissionDatas.Add(dailyMission.dataArray[i].Id, dailyMission.dataArray[i]);
                }
            }

            return dailyMissionDatas;
        }
    }
    [SerializeField]
    private EventMission eventMission;

    public EventMission EventMission => eventMission;

    private Dictionary<int, EventMissionData> eventMissionDatas;
    public Dictionary<int, EventMissionData> EventMissionDatas
    {
        get
        {
            if (eventMissionDatas == null)
            {
                eventMissionDatas = new Dictionary<int, EventMissionData>();

                for (int i = 0; i < eventMission.dataArray.Length; i++)
                {
                    eventMissionDatas.Add(eventMission.dataArray[i].Id, eventMission.dataArray[i]);
                }
            }

            return eventMissionDatas;
        }
    }
    [SerializeField]
    private GuideMission guideMission;

    public GuideMission GuideMission => guideMission;

    private Dictionary<int, GuideMissionData> guideMissionDatas;
    public Dictionary<int, GuideMissionData> GuideMissionDatas
    {
        get
        {
            if (guideMissionDatas == null)
            {
                guideMissionDatas = new Dictionary<int, GuideMissionData>();

                for (int i = 0; i < eventMission.dataArray.Length; i++)
                {
                    guideMissionDatas.Add(eventMission.dataArray[i].Id, guideMission.dataArray[i]);
                }
            }

            return guideMissionDatas;
        }
    }
    //



    [SerializeField]
    private StatusSetting statusTable;
    public StatusSetting StatusTable => statusTable;

    private Dictionary<string, StatusSettingData> statusDatas;
    public Dictionary<string, StatusSettingData> StatusDatas
    {
        get
        {
            if (statusDatas == null)
            {
                statusDatas = new Dictionary<string, StatusSettingData>();

                for (int i = 0; i < statusTable.dataArray.Length; i++)
                {
                    statusDatas.Add(statusTable.dataArray[i].Key, statusTable.dataArray[i]);
                }
            }

            return statusDatas;
        }
    }

    [SerializeField]
    private WeaponEffect weaponEffectTable;

    private Dictionary<int, WeaponEffectData> weaponEffectDatas;
    public Dictionary<int, WeaponEffectData> WeaponEffectDatas
    {
        get
        {
            if (weaponEffectDatas == null)
            {
                weaponEffectDatas = new Dictionary<int, WeaponEffectData>();

                for (int i = 0; i < weaponEffectTable.dataArray.Length; i++)
                {
                    weaponEffectDatas.Add(weaponEffectTable.dataArray[i].Id, weaponEffectTable.dataArray[i]);
                }
            }

            return weaponEffectDatas;
        }
    }

    [SerializeField]
    private MagicBook magicBookTable;
    public MagicBook MagicBookTable => magicBookTable;
    private Dictionary<int, MagicBookData> magicBoocDatas;
    public Dictionary<int, MagicBookData> MagicBoocDatas
    {
        get
        {
            if (magicBoocDatas == null)
            {
                magicBoocDatas = new Dictionary<int, MagicBookData>();

                for (int i = 0; i < magicBookTable.dataArray.Length; i++)
                {
                    magicBoocDatas.Add(magicBookTable.dataArray[i].Id, magicBookTable.dataArray[i]);
                }
            }

            return magicBoocDatas;
        }
    }

    public MagicBookData GetMagicBookDataByStringId(string id)
    {
        var e = MagicBoocDatas.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Current.Value.Stringid == id) return e.Current.Value;
        }

        return null;
    }

    [SerializeField]
    private PetTable petTable;
    public PetTable PetTable => petTable;

    private Dictionary<int, PetTableData> petDatas;
    public Dictionary<int, PetTableData> PetDatas
    {
        get
        {
            if (petDatas == null)
            {
                petDatas = new Dictionary<int, PetTableData>();
                for (int i = 0; i < petTable.dataArray.Length; i++)
                {
                    petDatas.Add(petTable.dataArray[i].Id, petTable.dataArray[i]);
                }
            }

            return petDatas;
        }

    }

    [SerializeField]
    private LevelPass levelPass;

    public LevelPass LevelPass => levelPass;

    [SerializeField]
    private Costume costume;

    public Costume Costume => costume;

    private Dictionary<int, CostumeData> costumeData;
    public Dictionary<int, CostumeData> CostumeData
    {
        get
        {
            if (costumeData == null)
            {
                costumeData = new Dictionary<int, CostumeData>();
                for (int i = 0; i < costume.dataArray.Length; i++)
                {
                    costumeData.Add(costume.dataArray[i].Id, costume.dataArray[i]);
                }
            }

            return costumeData;
        }
    }


    [SerializeField]
    private CostumeAbility costumeAbility;

    public CostumeAbility CostumeAbility => costumeAbility;

    private Dictionary<int, CostumeAbilityData> costumeAbilityData;
    public Dictionary<int, CostumeAbilityData> CostumeAbilityData
    {
        get
        {
            if (costumeAbilityData == null)
            {
                costumeAbilityData = new Dictionary<int, CostumeAbilityData>();

                int costumeMaxGrade = 0;

                for (int i = 0; i < costumeAbility.dataArray.Length; i++)
                {
                    if (costumeMaxGrade < costumeAbility.dataArray[i].Grade)
                    {
                        costumeMaxGrade = costumeAbility.dataArray[i].Grade;
                    }

                    costumeAbilityData.Add(costumeAbility.dataArray[i].Id, costumeAbility.dataArray[i]);
                }

                GameBalance.costumeMaxGrade = costumeMaxGrade;
            }

            return costumeAbilityData;
        }
    }

    private List<ObscuredFloat> costumeProbs;
    private List<ObscuredInt> costumeList = new List<ObscuredInt>();

    public List<ObscuredInt> GetRandomCostumeOptions(int amount)
    {
        if (costumeProbs == null)
        {
            costumeProbs = new List<ObscuredFloat>();

            for (int i = 0; i < costumeAbility.dataArray.Length; i++)
            {
                costumeProbs.Add(costumeAbility.dataArray[i].Prob);
            }
        }

        costumeList.Clear();

        for (int i = 0; i < amount; i++)
        {
            int randomIdx = Utils.GetRandomIdx(costumeProbs.Select(e => (float)e).ToList());
            //0번꺼가 -1라서 얘는 제외
            costumeList.Add(randomIdx - 1);
        }

        return costumeList;
    }

    [SerializeField]
    private BossTable bossTable;
    public BossTable BossTable => bossTable;


    private Dictionary<int, BossTableData> bossTableData;
    public Dictionary<int, BossTableData> BossTableData
    {
        get
        {
            if (bossTableData == null)
            {
                bossTableData = new Dictionary<int, BossTableData>();
                for (int i = 0; i < bossTable.dataArray.Length; i++)
                {
                    bossTableData.Add(bossTable.dataArray[i].Id, bossTable.dataArray[i]);
                }
            }

            return bossTableData;
        }
    }

    [SerializeField]
    private TowerTable towerTable;
    public TowerTable TowerTable => towerTable;

    private Dictionary<int, TowerTableData> towerTableData;
    public Dictionary<int, TowerTableData> TowerTableData
    {
        get
        {
            if (towerTableData == null)
            {
                towerTableData = new Dictionary<int, TowerTableData>();

                for (int i = 0; i < towerTable.dataArray.Length; i++)
                {
                    towerTableData.Add(towerTable.dataArray[i].Id, towerTable.dataArray[i]);
                }
            }

            return towerTableData;
        }
    }
    //

    [SerializeField]
    private TowerTable2 towerTable2;
    public TowerTable2 TowerTable2 => towerTable2;

    private Dictionary<int, TowerTable2Data> towerTableData2;
    public Dictionary<int, TowerTable2Data> TowerTableData2
    {
        get
        {
            if (towerTableData2 == null)
            {
                towerTableData2 = new Dictionary<int, TowerTable2Data>();

                for (int i = 0; i < towerTable2.dataArray.Length; i++)
                {
                    towerTableData2.Add(towerTable2.dataArray[i].Id, towerTable2.dataArray[i]);
                }
            }

            return towerTableData2;
        }
    }
    //



    [SerializeField]
    private DailyPass dailyPass;
    public DailyPass DailyPass => dailyPass;

    private Dictionary<int, DailyPassData> dailyPassData;
    public Dictionary<int, DailyPassData> DailyPassData
    {
        get
        {
            if (dailyPassData == null)
            {
                dailyPassData = new Dictionary<int, DailyPassData>();

                for (int i = 0; i < dailyPass.dataArray.Length; i++)
                {
                    dailyPassData.Add(dailyPass.dataArray[i].Id, dailyPass.dataArray[i]);
                }
            }

            return dailyPassData;
        }

    }

    [SerializeField]
    private BonusRoulette bonusRoulette;
    public BonusRoulette BonusRoulette => bonusRoulette;

    [SerializeField]
    private OldDokebiBonusRoulette oldDokebiBonusRoulette;
    public OldDokebiBonusRoulette OldDokebiBonusRoulette => oldDokebiBonusRoulette;

    [SerializeField]
    private InAppPurchase inAppPurchase;
    public InAppPurchase InAppPurchase => inAppPurchase;

    private Dictionary<string, InAppPurchaseData> inAppPurchaseData;
    public Dictionary<string, InAppPurchaseData> InAppPurchaseData
    {
        get
        {
            if (inAppPurchaseData == null)
            {
                inAppPurchaseData = new Dictionary<string, InAppPurchaseData>();

                var datas = inAppPurchase.dataArray;

                for (int i = 0; i < datas.Length; i++)
                {
                    inAppPurchaseData.Add(datas[i].Productid, datas[i]);
                }
            }

            return inAppPurchaseData;
        }
    }

    [SerializeField]
    private LoadingTip loadingTip;
    public LoadingTip LoadingTip => loadingTip;

    [SerializeField]
    private BadWord badWord;
    public BadWord BadWord => badWord;

    [SerializeField]
    private AttendanceReward attendanceReward;
    public AttendanceReward AttendanceReward => attendanceReward;

    [SerializeField]
    private WingTable wingTable;

    public WingTable WingTable => wingTable;

    public int WingMaxValue => wingTable.dataArray[wingTable.dataArray.Length - 1].Requirejump;

    [SerializeField]
    private BuffTable buffTable;

    public BuffTable BuffTable => buffTable;

    [SerializeField]
    private PassiveSkill passiveSkill;

    public PassiveSkill PassiveSkill => passiveSkill;

    [SerializeField]
    private PassiveSkill2 passiveSkill2;

    public PassiveSkill2 PassiveSkill2 => passiveSkill2;

    [SerializeField]
    private MarbleTable marbleTable;

    public MarbleTable MarbleTable => marbleTable;

    [SerializeField]
    private Dokebi dokebiTable;

    public Dokebi DokebiTable => dokebiTable;

    [SerializeField]
    private DokebiRewardTable dokebiRewardTable;
    public DokebiRewardTable DokebiRewardTable => dokebiRewardTable;

    [SerializeField]
    private TwelveBossTable twelveBossTable;

    public TwelveBossTable TwelveBossTable => twelveBossTable;

    [SerializeField]
    private TitleTable titleTable;

    public TitleTable TitleTable => titleTable;

    private Dictionary<int, List<TitleTableData>> titleAbils;
    public Dictionary<int, List<TitleTableData>> TitleAbils
    {
        get
        {
            if (titleAbils == null)
            {
                titleAbils = new Dictionary<int, List<TitleTableData>>();

                for (int i = 0; i < titleTable.dataArray.Length; i++)
                {
                    if (titleAbils.ContainsKey(titleTable.dataArray[i].Abiltype1) == false)
                    {
                        titleAbils.Add(titleTable.dataArray[i].Abiltype1, new List<TitleTableData>());
                    }

                    titleAbils[titleTable.dataArray[i].Abiltype1].Add(titleTable.dataArray[i]);
                }
            }

            return titleAbils;
        }
    }

    [SerializeField]
    private YomulAbil yomulAbilTable;
    public YomulAbil YomulAbilTable => yomulAbilTable;

    [SerializeField]
    private ChuseokEvent chuseokEventTable;

    public ChuseokEvent ChuseokEventTable => chuseokEventTable;

    [SerializeField]
    private YoguiSogulTable yoguisogulTable;

    public YoguiSogulTable YoguisogulTable => yoguisogulTable;

    [SerializeField]
    private OldDokebi2Table oldDokebiTable;

    public OldDokebi2Table OldDokebiTable => oldDokebiTable;

    [SerializeField]
    private PetEquipment petEquipment;
    public PetEquipment PetEquipment => petEquipment;


    [SerializeField]
    private MonthlyPass monthlyPass;
    public MonthlyPass MonthlyPass => monthlyPass;

    [SerializeField]
    private RelicTable rericTable;

    public RelicTable RelicTable => rericTable;


    [SerializeField]
    private StageRelic stageRelic;

    public StageRelic StageRelic => stageRelic;

    [SerializeField]
    private monthpass2 monthlyPass2;
    public monthpass2 MonthlyPass2 => monthlyPass2;

    [SerializeField]
    private SonReward sonReward;

    public SonReward SonReward => sonReward;

    [SerializeField]
    private SonAbil sonAbil;

    public SonAbil SonAbil => sonAbil;

    [SerializeField]
    private Attendance100 attendanceReward_100;
    public Attendance100 AttendanceReward_100 => attendanceReward_100;

    [SerializeField]
    private IosCoupon iosCoupon;

    public IosCoupon IosCoupon => iosCoupon;

    [SerializeField]
    private GuildRewardTable guildRewardTable;

    public GuildRewardTable GuildRewardTable => guildRewardTable;

    [SerializeField]
    private GuildLevel guildLevel;

    public GuildLevel GuildLevel => guildLevel;

    [SerializeField]
    private SeolPass seolPass;
    public SeolPass SeolPass => seolPass;

    [SerializeField]
    private SulPass sulPass;
    public SulPass SulPass => sulPass;

    public SmithTable smithTable;

    public SmithEnemy smithEnemy;

    public GumGiTable gumGiTable;

    public WinterPass winterPass;
    public ChildPass childPass;
    public ColdSeasonPass coldSeasonPass;

    public FoxMask FoxMask;
    public DokebiHorn DokebiHorn;
    public CaveBelt CaveBelt;

    [SerializeField]
    private SummerCollection summerCollection;

    public SummerCollection SummerCollection => summerCollection;

    public SusanoTable susanoTable;
    public OkTable okTable;
    public YumTable yumTable;
    public DoTable doTable;
    //사신수
    public SasinsuTable sasinsuTable;


    public BokPass bokPass;
    public GuildBook guildBook;
    public OneYearAtten oneYearAtten;
    public HellAbil hellAbil;
    public HellAbilBase hellAbilBase;
    public HellReward hellReward;
    public DragonBall dragonBall;
    public ChuSeokAtten chuSeokAtten;
    public ColdSeasonAtten coldSeasonAtten;

    public ChunAbilBase chunAbilBase;
    public DokebiAbilBase dokebiAbilBase;
    public ChunMarkAbil chunMarkAbil;
    public FallCollection fallCollection;
    public XMasCollection xMasCollection;
    public MileageReward mileageReward;
    public PetHome petHome;
    public TowerTable3 towerTable3;
    public TowerTable4 towerTableMulti;
    public TowerTable5 sumisanTowerTable;
    public GradeTestTable gradeTestTable;
    public CommonCollectionEvent commoncollectionEvent;
    public SnowManAtten snowManAtten;
    public SumiAbilBase sumiAbilBase;
    public FoxCup foxCup;
    public DayOfWeekDungeon dayOfWeekDungeon;
    public CostumeCollection costumeCollection;
    public TwoCave twoCave;
    public SpringAtten springAtten;

}


public enum CommonTableEventType
{
    SnowMan, Namul, Wind , DdukGuk
}
