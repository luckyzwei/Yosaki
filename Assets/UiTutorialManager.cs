using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

//     settings |= (GoodsSettings)flag; 더할때
// 체크
[System.Flags]
public enum TutorialStep
{
    KillEnemy = 2, //★
    UseTelePort = 4,//★
    UpgradeGoldStat = 8,//★
    GetPet = 16,//★
    ClearBoss1 = 32,//★
    GoNextStage = 64,
    GetWeapon = 128,
    EquipWeapon = 256,
    ClearBoss2 = 512,//★
    GetSkill = 1024,//★
    LearnSkill = 2048,//★
    SetPotion = 4096,//★
    Level20 = 8192,//★
    PlayFireFly = 16384,//★
    BuyTicket = 32768,//★
    PlayCatContents = 65536,//★
    Clear = 131072
}

public class UiTutorialManager : SingletonMono<UiTutorialManager>
{
    private ObscuredInt rewardGemNum = 5000;
    private ObscuredInt lastRewardGemNum = 50000;

    [SerializeField]
    private GameObject rootObject;

    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private TextMeshProUGUI rewardText;

    [SerializeField]
    private Button rewardButton;

    [SerializeField]
    private Animator animator;
    private ReactiveProperty<float> tutorialStep => ServerData.userInfoTable.GetTableData(UserInfoTable.tutorialCurrentStep);
    private ReactiveProperty<float> tutorialClearFlags => ServerData.userInfoTable.GetTableData(UserInfoTable.tutorialClearFlags);

    public bool HasClearFlag(TutorialStep step)
    {
        return ((TutorialStep)tutorialClearFlags.Value).IsSet(step);
    }

    public bool isAllCleared { get; private set; } = false;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        rootObject.SetActive(true);

        if ((TutorialStep)tutorialStep.Value == TutorialStep.Clear)
        {
            rootObject.SetActive(false);
            isAllCleared = true;
        }
        else
        {
            tutorialStep.AsObservable().Subscribe(WhenTutorialStepChanged).AddTo(this);
            tutorialClearFlags.AsObservable().Subscribe(WhenTutorialClearFlag).AddTo(this);

            ServerData.userInfoTable.GetTableData(UserInfoTable.dailyEnemyKillCount).AsObservable().Pairwise((pre, cur) => cur > pre).Subscribe(e =>
            {
                SetClear(TutorialStep.KillEnemy);
            }).AddTo(this);

            ServerData.statusTable.GetTableData(StatusTable.Level).AsObservable().Subscribe(e =>
            {
                if (e >= 12)
                {
                    SetClear(TutorialStep.Level20);
                }
            }).AddTo(this);
        }
    }

    private void WhenTutorialStepChanged(float step)
    {
        var currentStep = (TutorialStep)step;

        if (currentStep == TutorialStep.Clear)
        {
            rootObject.SetActive(false);
            isAllCleared = true;
            return;
        }

        bool clear = ((TutorialStep)tutorialClearFlags.Value).HasFlag(currentStep);

        rewardButton.gameObject.SetActive(clear);

        int rewardAmount = currentStep != TutorialStep.Level20 ? (int)rewardGemNum : (int)lastRewardGemNum;
        rewardText.SetText($"보상 {rewardAmount}");

        description.SetText(GetDescription((TutorialStep)step));
    }
    private void WhenTutorialClearFlag(float clearState)
    {
        WhenTutorialStepChanged(tutorialStep.Value);
    }

    public void OnClickReward()
    {
        if ((TutorialStep)tutorialStep.Value == TutorialStep.PlayCatContents)
        {
            PopupManager.Instance.ShowReviewPopup();
        }

        animator.SetTrigger("Anim");

        SoundManager.Instance.PlaySound("Reward");

        int rewardAmount = (TutorialStep)tutorialStep.Value != TutorialStep.Level20 ? (int)rewardGemNum : (int)lastRewardGemNum;

        ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value += rewardAmount;

        TutorialStep nextStep = (TutorialStep)(tutorialStep.Value * 2f);

        ServerData.userInfoTable.GetTableData(UserInfoTable.tutorialCurrentStep).Value = (float)nextStep;

        List<TransactionValue> transactions = new List<TransactionValue>();
        //보상루틴
        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.tutorialCurrentStep, ServerData.userInfoTable.GetTableData(UserInfoTable.tutorialCurrentStep).Value);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        ServerData.SendTransaction(transactions);
    }

    public void SetClear(TutorialStep state)
    {
        if (isAllCleared == true || ((TutorialStep)tutorialClearFlags.Value).IsSet(state) == true) return;

        SoundManager.Instance.PlaySound("Reward");

        var prefValue = (TutorialStep)tutorialClearFlags.Value;

        prefValue |= state;

        tutorialClearFlags.Value = (float)prefValue;

        ServerData.userInfoTable.UpData(UserInfoTable.tutorialClearFlags, tutorialClearFlags.Value, false);
    }

    private string GetDescription(TutorialStep step)
    {
        switch (step)
        {
            case TutorialStep.KillEnemy:
                return "기본 기술을 사용해\n다람쥐 요괴를 처치해 보세요\n(오른쪽 하단 버튼)";
                break;
            case TutorialStep.UseTelePort:
                return "<color=red>위쪽 방향키</color>를 연속으로 <color=red>두번</color>\n눌러서 순보를 써보세요\n(모든 방향키 가능)";
                break;
            case TutorialStep.UpgradeGoldStat:
                return "오른쪽 상단 메뉴->수련에서\n능력치를 올려보세요";
                break;
            case TutorialStep.GetPet:
                return "오른쪽 상단 <color=yellow>메뉴->가방</color>의 환수탭에서 <color=yellow>\n아기현무</color>를 얻어보세요\n(무료)";
                break;
            case TutorialStep.ClearBoss1:
                return "화면 상단의 <color=#ff00ffff>보스도전</color> 버튼을\n클릭하여 필드 보스를 처치해 보세요";
                break;
            case TutorialStep.GoNextStage:
                return "화면 상단의 <color=#ff00ffff>다음 스테이지</color> 버튼을\n클릭해 다음 스테이지로 이동해 보세요.";
                break;
            case TutorialStep.GetWeapon:
                return "오른쪽 상단 <color=yellow>메뉴->상점의 소환탭</color>에서\n<color=yellow>무기</color>를 소환해 보세요.";
                break;
            case TutorialStep.ClearBoss2:
                return "화면 상단의  <color=#ff00ffff>보스도전</color> 버튼을 클릭하여 필드 보스를 처치해 보세요";
                break;
            case TutorialStep.GetSkill:
                return "오른쪽 상단 메뉴->상점의 소환 탭에서\n<color=red>기술</color>을 소환해 보세요.";
                break;
            case TutorialStep.LearnSkill:
                return "오른쪽 상단 <color=yellow>기술메뉴</color> 에서\n기술을 배워보세요.";
                break;
            case TutorialStep.Level20:
                return $"<color=yellow>레벨 12</color>을 달성해 해보세요\n보상:{CommonString.GetItemName(Item_Type.Jade)}{lastRewardGemNum}개";
                break;
            case TutorialStep.Clear:
                break;
            case TutorialStep.EquipWeapon:
                return "오른쪽 상단 <color=yellow>메뉴->가방</color>의 무기\n탭에서 무기를 장착 해보세요";
                break;
            case TutorialStep.SetPotion:
                return "오른쪽 하단 <color=yellow>뿌리식물</color> 메뉴에 들어가서\n뿌리식물을 구매해보세요";
                break;
            case TutorialStep.PlayFireFly:
                return "오른쪽 상단 <color=yellow>요괴 사냥</color> 메뉴에서\n반딧불 요괴전을 플레이 해보세요";
                break;
            case TutorialStep.BuyTicket:
                return "오른쪽 상단 <color=yellow>요괴 사냥</color> 메뉴에서\n소환권을 구매해 보세요";
                break;
            case TutorialStep.PlayCatContents:
                return "오른쪽 상단 <color=yellow>요괴 사냥</color> 메뉴에서\n고양이 요괴전을 플레이 해보세요";
                break;
                //case TutorialStep._1_MoveField:
                //    return "맵 오른쪽 끝의 <color=#00ffffff>파란색</color> 포탈을\n이용해 사냥터로 이동하세요\n<color=yellow>(포탈에서 왼쪽패드의</color>\n<color=yellow>위쪽 버튼 입력)</color>";
                //    break;
                //case TutorialStep._2_Jump:
                //    return "오른쪽의 점프 버튼을 두번 클릭해서 이단 점프를 해보세요";
                //case TutorialStep._3_Down:
                //    return "1층에 올라간 다음,\n아래 방향키를 입력한\n상태로 점프 버튼을 클릭하여 \n아래로 하강 해보세요\n";
                //    break;
                //case TutorialStep._3_KillEnemy:
                //    return "오른쪽의 스킬 버튼을 입력해 적을 처치해 보세요";
                //    break;
                //case TutorialStep._4_AbilityUp:
                //    return "오른쪽 상단 능력치 메뉴에서 <color=yellow>골드</color>를 이용해 능력치를 올려보세요";
                //    break;
                //case TutorialStep._5_SkillLevelUp:
                //    return "오른쪽 상단 스킬 메뉴에서 스킬의 레벨을 올려 보세요";
                //    break;
                //case TutorialStep._6_Level5:
                //    return "캐릭터 레벨 5을 달성해 보세요";
                //    break;
                //case TutorialStep._7_MissionReward:
                //    return "오른쪽 상단의 더보기 버튼을 클릭하고,미션 메뉴에서 보상을 수령해보세요";
                //    break;
                //case TutorialStep._8_GetPet:
                //    return "오른쪽 상단 가방메뉴의 펫 탭에서 검은용 펫을 얻어보세요\n(무료)";
                //    break;
                //case TutorialStep._9_GetBonusReward:
                //    return "오른쪽 상단 보너스메뉴에서 룰렛을 돌려보세요\n(자동뽑기 토글 클릭)";
                //    break;
                //case TutorialStep._10_GetWeaponInShop:
                //    return "오른쪽 상단 상점메뉴에서 무기를 소환해 보세요";
                //    break;
                //case TutorialStep._10_EquipWeapon:
                //    return "오른쪽 상단 가방메뉴의 무기 탭에서 무기를 장착해 보세요.";
                //    break;
                //case TutorialStep._10_GetSkill:
                //    return "오른쪽 상단 스킬메뉴에서 스킬을 배워보세요.";
                //    break;
                //case TutorialStep._11_Level20:
                //    return $"캐릭터 레벨 20을 달성해 보세요";
                //    break;
                //case TutorialStep._12_ClearGoblin:
                //    return $"레벨 {GameBalance.bonusDungeonUnlockLevel}달성 후에\n오른쪽 상단 전투 메뉴에서\n{CommonString.GetContentsName(GameManager.ContentsType.FireFly)}를 클리어 해보세요\n보상 : <color=yellow>{CommonString.GetItemName(Item_Type.Jade)} {lastRewardGemNum}개</color>";
                //    break;
                //case TutorialStep.Clear:
                //    break;
        }

        return "미등록";
    }
}
