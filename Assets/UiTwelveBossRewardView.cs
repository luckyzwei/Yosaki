using BackEnd;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using static UiTwelveRewardPopup;

public class UiTwelveBossRewardView : MonoBehaviour
{
    private TwelveBossRewardInfo rewardInfo;

    [SerializeField]
    private Image itemIcon;

    [SerializeField]
    private TextMeshProUGUI itemDescription;

    [SerializeField]
    private Button rewardButton;

    [SerializeField]
    private TextMeshProUGUI rewardButtonDescription;

    [SerializeField]
    private TextMeshProUGUI rewardAmount;

    [SerializeField]
    private GameObject rewardLockMask;

    [SerializeField]
    private TextMeshProUGUI lockDescription;

    [SerializeField]
    private TextMeshProUGUI gradeText;

    private BossServerData bossServerData;

    [SerializeField]
    private GameObject rewardedIcon;

    private CompositeDisposable disposable = new CompositeDisposable();

    private void OnDestroy()
    {
        disposable.Dispose();
    }

    public void Initialize(TwelveBossRewardInfo rewardInfo, BossServerData bossServerData)
    {
        this.rewardInfo = rewardInfo;

        this.bossServerData = bossServerData;

        rewardLockMask.SetActive(rewardInfo.currentDamage < rewardInfo.damageCut);

        itemIcon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)rewardInfo.rewardType);

        itemDescription.SetText($"{CommonString.GetItemName((Item_Type)rewardInfo.rewardType)}");

        rewardAmount.SetText($"{Utils.ConvertBigNum(rewardInfo.rewardAmount)}개");

        lockDescription.SetText($"{rewardInfo.rewardCutString}에 해금");

        if (gradeText != null)
        {
            gradeText.SetText($"{rewardInfo.idx + 1}단계\n({rewardInfo.idx + 1}점)");

            //문파만
            if (bossServerData.idx == 12)
            {
                if (rewardInfo.currentDamage >= rewardInfo.damageCut)
                {
                    if (UiGuildBossView.Instance != null && UiGuildBossView.Instance.rewardGrade < rewardInfo.idx + 1)
                    {
                        UiGuildBossView.Instance.rewardGrade = rewardInfo.idx + 1;
                    }
                }

                var bossTableData = TableManager.Instance.TwelveBossTable.dataArray[20];

                var bsd = ServerData.bossServerTable.TableDatas[bossTableData.Stringid];

                double currentDamage = 0f;

                if (string.IsNullOrEmpty(bsd.score.Value) == false)
                {
                    currentDamage = double.Parse(bsd.score.Value);
                }

                if (currentDamage >= rewardInfo.damageCut)
                {
                    //강철은 구미호와 같은 점수 계산 사용
                    if (UiGangChulView.Instance != null && UiGangChulView.Instance.rewardGrade < rewardInfo.idx + 1)
                    {
                        UiGangChulView.Instance.rewardGrade = rewardInfo.idx + 1;
                    }
                }
            }
        }

        Subscribe();
    }

    private void Subscribe()
    {

        disposable.Clear();

        bossServerData.rewardedId.AsObservable().Subscribe(e =>
        {
            var rewards = e.Split(BossServerTable.rewardSplit).ToList();

            bool rewarded = rewards.Contains(rewardInfo.idx.ToString());

            rewardButtonDescription.SetText(rewarded ? "완료" : "받기");

            rewardedIcon.SetActive(rewarded);

        }).AddTo(disposable);

    }

    public void OnClickGetButton()
    {
        if (rewardInfo.currentDamage < rewardInfo.damageCut)
        {
            PopupManager.Instance.ShowAlarmMessage("최대 피해량이 부족 합니다.");
            return;
        }

        var rewards = bossServerData.rewardedId.Value.Split(BossServerTable.rewardSplit).ToList();

        if (rewards.Contains(rewardInfo.idx.ToString()))
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다.");
            return;
        }

        rewardButton.interactable = false;

        Item_Type type = (Item_Type)rewardInfo.rewardType;

        if (type == Item_Type.RabitPet)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet17"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet17", ServerData.petTable.TableDatas["pet17"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

            //
            Param bossParam = new Param();

            bossServerData.rewardedId.Value += $"{BossServerTable.rewardSplit}{rewardInfo.idx}";

            var localTableData = TableManager.Instance.TwelveBossTable.dataArray[bossServerData.idx];

            bossParam.Add(localTableData.Stringid, bossServerData.ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(BossServerTable.tableName, BossServerTable.Indate, bossParam));
            //

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                SoundManager.Instance.PlaySound("Reward");
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "달토끼 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

        }
        else if (type == Item_Type.RabitNorigae)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook26"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook26"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook26", ServerData.magicBookTable.TableDatas["magicBook26"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

            //
            Param bossParam = new Param();

            bossServerData.rewardedId.Value += $"{BossServerTable.rewardSplit}{rewardInfo.idx}";

            var localTableData = TableManager.Instance.TwelveBossTable.dataArray[bossServerData.idx];

            bossParam.Add(localTableData.Stringid, bossServerData.ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(BossServerTable.tableName, BossServerTable.Indate, bossParam));
            //

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                SoundManager.Instance.PlaySound("Reward");
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "달토끼 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.NataWeapon)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon27"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon27"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon27", ServerData.weaponTable.TableDatas["weapon27"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

            //
            Param bossParam = new Param();

            bossServerData.rewardedId.Value += $"{BossServerTable.rewardSplit}{rewardInfo.idx}";

            var localTableData = TableManager.Instance.TwelveBossTable.dataArray[bossServerData.idx];

            bossParam.Add(localTableData.Stringid, bossServerData.ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(BossServerTable.tableName, BossServerTable.Indate, bossParam));
            //

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                SoundManager.Instance.PlaySound("Reward");
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "나타의 검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.Kirin_Pet)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet16"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet16", ServerData.petTable.TableDatas["pet16"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

            //
            Param bossParam = new Param();

            bossServerData.rewardedId.Value += $"{BossServerTable.rewardSplit}{rewardInfo.idx}";

            var localTableData = TableManager.Instance.TwelveBossTable.dataArray[bossServerData.idx];

            bossParam.Add(localTableData.Stringid, bossServerData.ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(BossServerTable.tableName, BossServerTable.Indate, bossParam));
            //

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                SoundManager.Instance.PlaySound("Reward");
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "기린 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

        }
        else if (type == Item_Type.KirinNorigae)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook25"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook25"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook25", ServerData.magicBookTable.TableDatas["magicBook25"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

            //
            Param bossParam = new Param();

            bossServerData.rewardedId.Value += $"{BossServerTable.rewardSplit}{rewardInfo.idx}";

            var localTableData = TableManager.Instance.TwelveBossTable.dataArray[bossServerData.idx];

            bossParam.Add(localTableData.Stringid, bossServerData.ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(BossServerTable.tableName, BossServerTable.Indate, bossParam));
            //

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                SoundManager.Instance.PlaySound("Reward");
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "기린 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.IndraWeapon) 
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon26"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon26"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon26", ServerData.weaponTable.TableDatas["weapon26"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

            //
            Param bossParam = new Param();

            bossServerData.rewardedId.Value += $"{BossServerTable.rewardSplit}{rewardInfo.idx}";

            var localTableData = TableManager.Instance.TwelveBossTable.dataArray[bossServerData.idx];

            bossParam.Add(localTableData.Stringid, bossServerData.ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(BossServerTable.tableName, BossServerTable.Indate, bossParam));
            //

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                SoundManager.Instance.PlaySound("Reward");
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "인드라의 검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.Sam_Pet)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet15"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet15", ServerData.petTable.TableDatas["pet15"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

            //
            Param bossParam = new Param();

            bossServerData.rewardedId.Value += $"{BossServerTable.rewardSplit}{rewardInfo.idx}";

            var localTableData = TableManager.Instance.TwelveBossTable.dataArray[bossServerData.idx];

            bossParam.Add(localTableData.Stringid, bossServerData.ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(BossServerTable.tableName, BossServerTable.Indate, bossParam));
            //

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                SoundManager.Instance.PlaySound("Reward");
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "아기 삼족오 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

        }
        else if (type == Item_Type.Sam_Norigae)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook24"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook24"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook24", ServerData.magicBookTable.TableDatas["magicBook24"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

            //
            Param bossParam = new Param();

            bossServerData.rewardedId.Value += $"{BossServerTable.rewardSplit}{rewardInfo.idx}";

            var localTableData = TableManager.Instance.TwelveBossTable.dataArray[bossServerData.idx];

            bossParam.Add(localTableData.Stringid, bossServerData.ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(BossServerTable.tableName, BossServerTable.Indate, bossParam));
            //

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                SoundManager.Instance.PlaySound("Reward");
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "삼족오 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
       else if (type == Item_Type.Hae_Pet) 
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet14"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet14", ServerData.petTable.TableDatas["pet14"].ConvertToString());
            transactions.Add(TransactionValue.SetUpdate(PetServerTable.tableName, PetServerTable.Indate, petParam));

            //
            Param bossParam = new Param();

            bossServerData.rewardedId.Value += $"{BossServerTable.rewardSplit}{rewardInfo.idx}";

            var localTableData = TableManager.Instance.TwelveBossTable.dataArray[bossServerData.idx];

            bossParam.Add(localTableData.Stringid, bossServerData.ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(BossServerTable.tableName, BossServerTable.Indate, bossParam));
            //

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                SoundManager.Instance.PlaySound("Reward");
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "아기 해태 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

        }
        else if(type == Item_Type.Hae_Norigae) 
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook22"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook22"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook22", ServerData.magicBookTable.TableDatas["magicBook22"].ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

            //
            Param bossParam = new Param();

            bossServerData.rewardedId.Value += $"{BossServerTable.rewardSplit}{rewardInfo.idx}";

            var localTableData = TableManager.Instance.TwelveBossTable.dataArray[bossServerData.idx];

            bossParam.Add(localTableData.Stringid, bossServerData.ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(BossServerTable.tableName, BossServerTable.Indate, bossParam));
            //

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                SoundManager.Instance.PlaySound("Reward");
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "해태 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else 
        {
            float amount = rewardInfo.rewardAmount;

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param bossParam = new Param();

            bossServerData.rewardedId.Value += $"{BossServerTable.rewardSplit}{rewardInfo.idx}";

            var localTableData = TableManager.Instance.TwelveBossTable.dataArray[bossServerData.idx];

            bossParam.Add(localTableData.Stringid, bossServerData.ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(BossServerTable.tableName, BossServerTable.Indate, bossParam));

            transactions.Add(ServerData.GetItemTypeTransactionValueForAttendance(type, (int)amount));

            ServerData.SendTransaction(transactions, successCallBack: () =>
            {
                PopupManager.Instance.ShowAlarmMessage("보상을 받았습니다!");
                SoundManager.Instance.PlaySound("Reward");
                rewardButton.interactable = true;
            });
        }
    }

    public bool GetRewardByScript()
    {
        if (rewardInfo.currentDamage < rewardInfo.damageCut)
        {
            return false;
        }

        var rewards = bossServerData.rewardedId.Value.Split(BossServerTable.rewardSplit).ToList();

        if (rewards.Contains(rewardInfo.idx.ToString()))
        {
            return false;
        }

        Item_Type type = (Item_Type)rewardInfo.rewardType;

        float amount = rewardInfo.rewardAmount;

        bossServerData.rewardedId.Value += $"{BossServerTable.rewardSplit}{rewardInfo.idx}";
        ServerData.goodsTable.GetTableData(GoodsTable.GuildReward).Value += amount;

        return true;
    }
}
