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
    _1_MoveField = 2, //★
    _2_Jump = 4, //★
    _3_Down = 8, //★
    _3_KillEnemy = 16,//★
    _4_AbilityUp = 32,//★
    _5_SkillLevelUp = 64,
    _6_Level5 = 128,//★
    _7_MissionReward = 256,//★
    _8_GetPet = 512,//★
    _9_GetBonusReward = 1024,//★
    _10_GetWeaponInShop = 2048,//★
    _10_EquipWeapon = 4096,
    _10_GetSkill = 8192,
    _11_Level20 = 16384,//★
    _12_ClearGoblin = 32768,//★
    Clear = 32768 * 2
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
    private ReactiveProperty<float> tutorialStep => DatabaseManager.userInfoTable.GetTableData(UserInfoTable.tutorialCurrentStep);
    private ReactiveProperty<float> tutorialClearFlags => DatabaseManager.userInfoTable.GetTableData(UserInfoTable.tutorialClearFlags);

    private bool isAllCleared = false;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        if ((TutorialStep)tutorialStep.Value == TutorialStep.Clear)
        {
            rootObject.SetActive(false);
            isAllCleared = true;
        }
        else
        {
            tutorialStep.AsObservable().Subscribe(WhenTutorialStepChanged).AddTo(this);
            tutorialClearFlags.AsObservable().Subscribe(WhenTutorialClearFlag).AddTo(this);

            DatabaseManager.userInfoTable.GetTableData(UserInfoTable.dailyEnemyKillCount).AsObservable().Pairwise((pre, cur) => cur > pre).Subscribe(e =>
            {
                SetClear(TutorialStep._3_KillEnemy);
            }).AddTo(this);

            DatabaseManager.statusTable.GetTableData(StatusTable.Level).AsObservable().Subscribe(e =>
            {
                if (e == 5)
                {
                    SetClear(TutorialStep._6_Level5);
                }
                else if (e == 20)
                {
                    SetClear(TutorialStep._11_Level20);
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

        int rewardAmount = currentStep != TutorialStep._12_ClearGoblin ? (int)rewardGemNum : (int)lastRewardGemNum;
        rewardText.SetText($"보상 {rewardAmount}");

        description.SetText(GetDescription((TutorialStep)step));
    }
    private void WhenTutorialClearFlag(float clearState)
    {
        WhenTutorialStepChanged(tutorialStep.Value);
    }

    public void OnClickReward()
    {
        if ((TutorialStep)tutorialStep.Value == TutorialStep._12_ClearGoblin) 
        {
            PopupManager.Instance.ShowReviewPopup();
        }

        animator.SetTrigger("Anim");

        SoundManager.Instance.PlaySound("TutorialButton");

        int rewardAmount = (TutorialStep)tutorialStep.Value != TutorialStep._12_ClearGoblin ? (int)rewardGemNum : (int)lastRewardGemNum;

        DatabaseManager.goodsTable.GetTableData(GoodsTable.BlueStone).Value += rewardAmount;

        TutorialStep nextStep = (TutorialStep)(tutorialStep.Value * 2f);

        DatabaseManager.userInfoTable.GetTableData(UserInfoTable.tutorialCurrentStep).Value = (float)nextStep;

        List<TransactionValue> transactions = new List<TransactionValue>();
        //보상루틴
        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.BlueStone, DatabaseManager.goodsTable.GetTableData(GoodsTable.BlueStone).Value);

        Param userInfoParam = new Param();
        userInfoParam.Add(UserInfoTable.tutorialCurrentStep, DatabaseManager.userInfoTable.GetTableData(UserInfoTable.tutorialCurrentStep).Value);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(UserInfoTable.tableName, UserInfoTable.Indate, userInfoParam));

        DatabaseManager.SendTransaction(transactions);
    }

    public void SetClear(TutorialStep state)
    {
        if (isAllCleared == true || ((TutorialStep)tutorialClearFlags.Value).IsSet(state) == true) return;

        //
        if (state == TutorialStep._6_Level5)
        {
            UiManagerDescription.Instance.SetManagerDescription(ManagerDescriptionType.statusDescription);
        }
        //

        var prefValue = (TutorialStep)tutorialClearFlags.Value;

        prefValue |= state;

        tutorialClearFlags.Value = (float)prefValue;

        DatabaseManager.userInfoTable.UpData(UserInfoTable.tutorialClearFlags, tutorialClearFlags.Value, false);
    }

    private string GetDescription(TutorialStep step)
    {
        switch (step)
        {
            case TutorialStep._1_MoveField:
                return "맵 오른쪽 끝의 <color=#00ffffff>파란색</color> 포탈을\n이용해 사냥터로 이동하세요\n<color=yellow>(포탈에서 왼쪽패드의</color>\n<color=yellow>위쪽 버튼 입력)</color>";
                break;
            case TutorialStep._2_Jump:
                return "오른쪽의 점프 버튼을 두번 클릭해서 이단 점프를 해보세요";
            case TutorialStep._3_Down:
                return "1층에 올라간 다음,\n아래 방향키를 입력한\n상태로 점프 버튼을 클릭하여 \n아래로 하강 해보세요\n";
                break;
            case TutorialStep._3_KillEnemy:
                return "오른쪽의 스킬 버튼을 입력해 적을 처치해 보세요";
                break;
            case TutorialStep._4_AbilityUp:
                return "오른쪽 상단 능력치 메뉴에서 <color=yellow>골드</color>를 이용해 능력치를 올려보세요";
                break;
            case TutorialStep._5_SkillLevelUp:
                return "오른쪽 상단 스킬 메뉴에서 스킬의 레벨을 올려 보세요";
                break;
            case TutorialStep._6_Level5:
                return "캐릭터 레벨 5을 달성해 보세요";
                break;
            case TutorialStep._7_MissionReward:
                return "오른쪽 상단의 더보기 버튼을 클릭하고,미션 메뉴에서 보상을 수령해보세요";
                break;
            case TutorialStep._8_GetPet:
                return "오른쪽 상단 가방메뉴의 펫 탭에서 검은용 펫을 얻어보세요\n(무료)";
                break;
            case TutorialStep._9_GetBonusReward:
                return "오른쪽 상단 보너스메뉴에서 룰렛을 돌려보세요\n(자동뽑기 토글 클릭)";
                break;
            case TutorialStep._10_GetWeaponInShop:
                return "오른쪽 상단 상점메뉴에서 무기를 소환해 보세요";
                break;
            case TutorialStep._10_EquipWeapon:
                return "오른쪽 상단 가방메뉴의 무기 탭에서 무기를 장착해 보세요.";
                break;
            case TutorialStep._10_GetSkill:
                return "오른쪽 상단 스킬메뉴에서 스킬을 배워보세요.";
                break;
            case TutorialStep._11_Level20:
                return $"캐릭터 레벨 20을 달성해 보세요";
                break;
            case TutorialStep._12_ClearGoblin:
                return $"레벨 {GameBalance.bonusDungeonUnlockLevel}달성 후에\n오른쪽 상단 전투 메뉴에서\n{CommonString.GetContentsName(GameManager.ContentsType.BonusDefense)}를 클리어 해보세요\n보상 : <color=yellow>{CommonString.GetItemName(Item_Type.BlueStone)} {lastRewardGemNum}개</color>";
                break;
            case TutorialStep.Clear:
                break;

        }

        return "미등록";
    }
}
