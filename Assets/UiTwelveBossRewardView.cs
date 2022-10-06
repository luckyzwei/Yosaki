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
                    if (UiGuildBossView.Instance != null && UiGuildBossView.Instance.rewardGrade_GangChul < rewardInfo.idx + 1)
                    {
                        UiGuildBossView.Instance.rewardGrade_GangChul = rewardInfo.idx + 1;
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

        if (type == Item_Type.DogPet)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet18"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet18", ServerData.petTable.TableDatas["pet18"].ConvertToString());
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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "삼목구 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

        }
        else if (type == Item_Type.ChunMaPet)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet19"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet19", ServerData.petTable.TableDatas["pet19"].ConvertToString());
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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "천마 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

        }
        else if (type == Item_Type.DogNorigae)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook27"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook27"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook27", ServerData.magicBookTable.TableDatas["magicBook27"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "삼목구 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.MihoNorigae)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook28"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook28"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook28", ServerData.magicBookTable.TableDatas["magicBook28"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "여우 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.ChunMaNorigae)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook29"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook29"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook29", ServerData.magicBookTable.TableDatas["magicBook29"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "천마 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.costume39)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume39"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume39", costumeServerData.ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(CostumeServerTable.tableName, CostumeServerTable.Indate, costumeParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "삼목구 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

        }
        else if (type == Item_Type.costume53)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume53"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume53", costumeServerData.ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(CostumeServerTable.tableName, CostumeServerTable.Indate, costumeParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "해원맥 호연 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

        }
        //
        else if (type == Item_Type.costume56)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume56"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume56", costumeServerData.ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(CostumeServerTable.tableName, CostumeServerTable.Indate, costumeParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "선녀 호순 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

        }
        else if (type == Item_Type.costume57)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume57"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume57", costumeServerData.ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(CostumeServerTable.tableName, CostumeServerTable.Indate, costumeParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "선녀 호순 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

        }
        else if (type == Item_Type.costume58)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume58"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume58", costumeServerData.ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(CostumeServerTable.tableName, CostumeServerTable.Indate, costumeParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "선녀 호순 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

        }
        //

        else if (type == Item_Type.costume51)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume51"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume51", costumeServerData.ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(CostumeServerTable.tableName, CostumeServerTable.Indate, costumeParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "월직 호순 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

        }
        else if (type == Item_Type.costume42)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume42"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume42", costumeServerData.ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(CostumeServerTable.tableName, CostumeServerTable.Indate, costumeParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "천마호연 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

        }
        ///////////////////
        else if (type == Item_Type.costume36)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume36"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume36", costumeServerData.ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(CostumeServerTable.tableName, CostumeServerTable.Indate, costumeParam));

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
        else if (type == Item_Type.costume47)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume47"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume47", costumeServerData.ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(CostumeServerTable.tableName, CostumeServerTable.Indate, costumeParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "여래 호연 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

        }
        else if (type == Item_Type.costume48)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume48"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume48", costumeServerData.ConvertToString());

            transactions.Add(TransactionValue.SetUpdate(CostumeServerTable.tableName, CostumeServerTable.Indate, costumeParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "강림 호연 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

        }
        else if (type == Item_Type.RabitPet)
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
        else if (type == Item_Type.YeaRaeNorigae)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook32"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook32"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook32", ServerData.magicBookTable.TableDatas["magicBook32"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "여래 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.GangrimNorigae)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook33"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook33"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook33", ServerData.magicBookTable.TableDatas["magicBook33"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "강림 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        //
        else if (type == Item_Type.ChunNorigae0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook35"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook35"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook35", ServerData.magicBookTable.TableDatas["magicBook35"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "선녀 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.ChunNorigae1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook36"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook36"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook36", ServerData.magicBookTable.TableDatas["magicBook36"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "선녀 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.ChunNorigae2)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook37"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook37"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook37", ServerData.magicBookTable.TableDatas["magicBook37"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "선녀 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        //
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
        else if (type == Item_Type.YeaRaeWeapon)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon33"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon33"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon33", ServerData.weaponTable.TableDatas["weapon33"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "여래의 검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.GangrimWeapon)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon34"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon34"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon34", ServerData.weaponTable.TableDatas["weapon34"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "강림검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        //
        else if (type == Item_Type.HaeWeapon)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon36"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon36"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon36", ServerData.weaponTable.TableDatas["weapon36"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "사인검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }

        else if (type == Item_Type.OrochiWeapon)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon28"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon28"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon28", ServerData.weaponTable.TableDatas["weapon28"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "오로치의 검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.MihoWeapon)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon30"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon30"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon30", ServerData.weaponTable.TableDatas["weapon30"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "여우검 획득!!", null);
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
        else if (type == Item_Type.Hae_Norigae)
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
                if (rewardButton != null)
                {
                    rewardButton.interactable = true;
                }
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
