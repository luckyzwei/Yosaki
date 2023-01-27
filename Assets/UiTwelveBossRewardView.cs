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

    public TwelveBossRewardInfo RewardInfo => rewardInfo;

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
#if UNITY_EDITOR
        lockDescription.SetText($"{Utils.ConvertBigNum(rewardInfo.damageCut)}에 해금");
#else
        lockDescription.SetText($"{rewardInfo.rewardCutString}에 해금");
#endif

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
        else if (type == Item_Type.ChunPet0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet20"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet20", ServerData.petTable.TableDatas["pet20"].ConvertToString());
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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

        }
        else if (type == Item_Type.ChunPet1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet21"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet21", ServerData.petTable.TableDatas["pet21"].ConvertToString());
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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "고양이 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

        }
        else if (type == Item_Type.ChunPet2)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet22"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet22", ServerData.petTable.TableDatas["pet22"].ConvertToString());
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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "천둥오리 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

        }
        else if (type == Item_Type.ChunPet3)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.petTable.TableDatas["pet23"].hasItem.Value = 1;

            Param petParam = new Param();

            petParam.Add("pet23", ServerData.petTable.TableDatas["pet23"].ConvertToString());
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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "근두운 획득!!", null);
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
        else if (type == Item_Type.costume59)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume59"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume59", costumeServerData.ConvertToString());

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
        else if (type == Item_Type.costume60)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume60"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume60", costumeServerData.ConvertToString());

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
        else if (type == Item_Type.costume61)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume61"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume61", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "단풍 호순 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

        }
        else if (type == Item_Type.costume62)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume62"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume62", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "강시 호연 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

        }
        else if (type == Item_Type.costume63)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume63"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume63", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "강시 호순 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

        }
        else if (type == Item_Type.costume64)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume64"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume64", costumeServerData.ConvertToString());

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
        else if (type == Item_Type.costume65)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume65"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume65", costumeServerData.ConvertToString());

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
        else if (type == Item_Type.costume66)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume66"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume66", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "혈량 호연 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

        }
        else if (type == Item_Type.costume67)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume67"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume67", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "뇌량 호연 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

        }
        else if (type == Item_Type.costume68)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume68"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume68", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "암량 호연 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

        }
        //
        else if (type == Item_Type.costume69)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume69"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume69", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "12 월간 호연 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

        }
        //도깨비
        else if (type == Item_Type.costume70)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume70"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume70", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "화량 호순 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

        }
        else if (type == Item_Type.costume71)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume71"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume71", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "설량 호순 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

        }
        else if (type == Item_Type.costume72)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume72"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume72", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "크리스마스 호순 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

        }
        else if (type == Item_Type.costume73)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume73"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume73", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "핑크 호순 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

        }
        //
        else if (type == Item_Type.costume74)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume74"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume74", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "미량 호순 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

        }
        else if (type == Item_Type.costume75)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume75"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume75", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "흑량 호순 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

        }
        
        else if (type == Item_Type.costume78)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume78"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume78", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "남량 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

        }     
        else if (type == Item_Type.costume79)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume79"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume79", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "여량 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

        }     
        else if (type == Item_Type.costume80)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume80"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume80", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "우량 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

        }
        else if (type == Item_Type.costume81)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume81"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume81", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "지국천왕 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

        }
                else if (type == Item_Type.costume82)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            //
            var costumeServerData = ServerData.costumeServerTable.TableDatas["costume82"];

            costumeServerData.hasCostume.Value = true;

            Param costumeParam = new Param();

            costumeParam.Add("costume82", costumeServerData.ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "광목천왕 획득!!", null);
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
        else if (type == Item_Type.ChunNorigae3)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook38"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook38"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook38", ServerData.magicBookTable.TableDatas["magicBook38"].ConvertToString());

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
        else if (type == Item_Type.ChunNorigae4)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook39"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook39"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook39", ServerData.magicBookTable.TableDatas["magicBook39"].ConvertToString());

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
        //////
        else if (type == Item_Type.ChunNorigae5)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook40"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook40"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook40", ServerData.magicBookTable.TableDatas["magicBook40"].ConvertToString());

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
        else if (type == Item_Type.ChunNorigae6)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook41"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook41"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook41", ServerData.magicBookTable.TableDatas["magicBook41"].ConvertToString());

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
        /////
        else if (type == Item_Type.DokebiNorigae0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook42"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook42"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook42", ServerData.magicBookTable.TableDatas["magicBook42"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "혈량 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.DokebiNorigae1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook43"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook43"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook43", ServerData.magicBookTable.TableDatas["magicBook43"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "뇌량 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }

        else if (type == Item_Type.DokebiNorigae2)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook44"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook44"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook44", ServerData.magicBookTable.TableDatas["magicBook44"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "암량 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.DokebiNorigae3)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook46"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook46"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook46", ServerData.magicBookTable.TableDatas["magicBook46"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "화량 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.DokebiNorigae4)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook47"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook47"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook47", ServerData.magicBookTable.TableDatas["magicBook47"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "설량 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.DokebiNorigae5)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook48"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook48"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook48", ServerData.magicBookTable.TableDatas["magicBook48"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "미량 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.DokebiNorigae6)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook49"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook49"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook49", ServerData.magicBookTable.TableDatas["magicBook49"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "흑량 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
                else if (type == Item_Type.DokebiNorigae7)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook51"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook51"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook51", ServerData.magicBookTable.TableDatas["magicBook51"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "남량 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
                else if (type == Item_Type.DokebiNorigae8)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook52"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook52"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook52", ServerData.magicBookTable.TableDatas["magicBook52"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "여량 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
                else if (type == Item_Type.DokebiNorigae9)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook53"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook53"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook53", ServerData.magicBookTable.TableDatas["magicBook53"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "우량 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
            
        //
        else if (type == Item_Type.SumisanNorigae0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook54"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook54"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook54", ServerData.magicBookTable.TableDatas["magicBook54"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "지국천왕 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        
        else if (type == Item_Type.SumisanNorigae1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.magicBookTable.TableDatas["magicBook55"].amount.Value += 1;
            ServerData.magicBookTable.TableDatas["magicBook55"].hasItem.Value = 1;

            Param magicBookParam = new Param();

            magicBookParam.Add("magicBook55", ServerData.magicBookTable.TableDatas["magicBook55"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "광목천왕 노리개 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }

        //////
        //
        else if (type == Item_Type.DokebiHorn0)
        {
            var tableData = TableManager.Instance.DokebiHorn.dataArray[0];
         
            List<TransactionValue> transactions = new List<TransactionValue>();

            Param rewardParam = new Param();

            ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value += $"{BossServerTable.rewardSplit}{tableData.Id}";

            rewardParam.Add(EtcServerTable.DokebiHornReward, ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value);

            transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, rewardParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "도깨비 뿔 획득!!", null);
            });

        }
        else if (type == Item_Type.DokebiHorn1)
        {
            var tableData = TableManager.Instance.DokebiHorn.dataArray[1];

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param rewardParam = new Param();

            ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value += $"{BossServerTable.rewardSplit}{tableData.Id}";

            rewardParam.Add(EtcServerTable.DokebiHornReward, ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value);

            transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, rewardParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "도깨비 뿔 획득!!", null);
            });
        }
        else if (type == Item_Type.DokebiHorn2)
        {
            var tableData = TableManager.Instance.DokebiHorn.dataArray[2];

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param rewardParam = new Param();

            ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value += $"{BossServerTable.rewardSplit}{tableData.Id}";

            rewardParam.Add(EtcServerTable.DokebiHornReward, ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value);

            transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, rewardParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "도깨비 뿔 획득!!", null);
            });
        }
          else if (type == Item_Type.DokebiHorn3)
        {
            var tableData = TableManager.Instance.DokebiHorn.dataArray[3];

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param rewardParam = new Param();

            ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value += $"{BossServerTable.rewardSplit}{tableData.Id}";

            rewardParam.Add(EtcServerTable.DokebiHornReward, ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value);

            transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, rewardParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "도깨비 뿔 획득!!", null);
            });
        }
          else if (type == Item_Type.DokebiHorn4)
        {
            var tableData = TableManager.Instance.DokebiHorn.dataArray[4];

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param rewardParam = new Param();

            ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value += $"{BossServerTable.rewardSplit}{tableData.Id}";

            rewardParam.Add(EtcServerTable.DokebiHornReward, ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value);

            transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, rewardParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "도깨비 뿔 획득!!", null);
            });
        }
          else if (type == Item_Type.DokebiHorn5)
        {
            var tableData = TableManager.Instance.DokebiHorn.dataArray[5];

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param rewardParam = new Param();

            ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value += $"{BossServerTable.rewardSplit}{tableData.Id}";

            rewardParam.Add(EtcServerTable.DokebiHornReward, ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value);

            transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, rewardParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "도깨비 뿔 획득!!", null);
            });
        }
          else if (type == Item_Type.DokebiHorn6)
        {
            var tableData = TableManager.Instance.DokebiHorn.dataArray[6];

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param rewardParam = new Param();

            ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value += $"{BossServerTable.rewardSplit}{tableData.Id}";

            rewardParam.Add(EtcServerTable.DokebiHornReward, ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value);

            transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, rewardParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "도깨비 뿔 획득!!", null);
            });
        }


        //

             else if (type == Item_Type.DokebiHorn7)
        {
            var tableData = TableManager.Instance.DokebiHorn.dataArray[7];

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param rewardParam = new Param();

            ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value += $"{BossServerTable.rewardSplit}{tableData.Id}";

            rewardParam.Add(EtcServerTable.DokebiHornReward, ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value);

            transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, rewardParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "도깨비 뿔 획득!!", null);
            });
        }
             else if (type == Item_Type.DokebiHorn8)
        {
            var tableData = TableManager.Instance.DokebiHorn.dataArray[8];

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param rewardParam = new Param();

            ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value += $"{BossServerTable.rewardSplit}{tableData.Id}";

            rewardParam.Add(EtcServerTable.DokebiHornReward, ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value);

            transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, rewardParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "도깨비 뿔 획득!!", null);
            });
        }
             else if (type == Item_Type.DokebiHorn9)
        {
            var tableData = TableManager.Instance.DokebiHorn.dataArray[9];

            List<TransactionValue> transactions = new List<TransactionValue>();

            Param rewardParam = new Param();

            ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value += $"{BossServerTable.rewardSplit}{tableData.Id}";

            rewardParam.Add(EtcServerTable.DokebiHornReward, ServerData.etcServerTable.TableDatas[EtcServerTable.DokebiHornReward].Value);

            transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, rewardParam));

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "도깨비 뿔 획득!!", null);
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
        else if (type == Item_Type.ChunWeapon0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon43"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon43"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon43", ServerData.weaponTable.TableDatas["weapon43"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "태양검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.ChunWeapon1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon44"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon44"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon44", ServerData.weaponTable.TableDatas["weapon44"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "달의검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
         else if (type == Item_Type.ChunWeapon2)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon50"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon50"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon50", ServerData.weaponTable.TableDatas["weapon50"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "번개검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
         else if (type == Item_Type.ChunWeapon3)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon51"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon51"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon51", ServerData.weaponTable.TableDatas["weapon51"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "구름검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        //--------------------------------------
        else if (type == Item_Type.DokebiWeapon0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon57"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon57"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon57", ServerData.weaponTable.TableDatas["weapon57"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "혈량검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.DokebiWeapon1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon58"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon58"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon58", ServerData.weaponTable.TableDatas["weapon58"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "뇌량검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.DokebiWeapon2)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon59"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon59"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon59", ServerData.weaponTable.TableDatas["weapon59"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "암량검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }  else if (type == Item_Type.DokebiWeapon3)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon63"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon63"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon63", ServerData.weaponTable.TableDatas["weapon63"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "화량검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.DokebiWeapon4)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon64"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon64"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon64", ServerData.weaponTable.TableDatas["weapon64"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "설량검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        } else if (type == Item_Type.DokebiWeapon5)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon65"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon65"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon65", ServerData.weaponTable.TableDatas["weapon65"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "미량검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.DokebiWeapon6)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon66"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon66"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon66", ServerData.weaponTable.TableDatas["weapon66"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "흑량검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });

          
        }
        ///
        else if (type == Item_Type.DokebiWeapon7)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon77"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon77"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon77", ServerData.weaponTable.TableDatas["weapon77"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "남량검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        } 
        else if (type == Item_Type.DokebiWeapon8)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon78"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon78"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon78", ServerData.weaponTable.TableDatas["weapon78"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "여량검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        } 
        else if (type == Item_Type.DokebiWeapon9)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon79"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon79"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon79", ServerData.weaponTable.TableDatas["weapon79"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "우량검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        /////
        else if (type == Item_Type.SumisanWeapon0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon80"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon80"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon80", ServerData.weaponTable.TableDatas["weapon80"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "지국천왕 검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.SumisanWeapon1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon84"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon84"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon84", ServerData.weaponTable.TableDatas["weapon84"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "광목천왕 검 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        //--------------------------------------
        else if (type == Item_Type.RecommendWeapon0)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon45"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon45"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon45", ServerData.weaponTable.TableDatas["weapon45"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon1)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon46"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon46"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon46", ServerData.weaponTable.TableDatas["weapon46"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon2)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon47"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon47"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon47", ServerData.weaponTable.TableDatas["weapon47"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon3)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon48"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon48"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon48", ServerData.weaponTable.TableDatas["weapon48"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon4)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon49"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon49"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon49", ServerData.weaponTable.TableDatas["weapon49"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon5)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon52"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon52"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon52", ServerData.weaponTable.TableDatas["weapon52"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon6)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon53"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon53"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon53", ServerData.weaponTable.TableDatas["weapon53"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon7)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon54"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon54"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon54", ServerData.weaponTable.TableDatas["weapon54"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon8)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon55"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon55"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon55", ServerData.weaponTable.TableDatas["weapon55"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon9)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon56"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon56"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon56", ServerData.weaponTable.TableDatas["weapon56"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon10)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon60"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon60"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon60", ServerData.weaponTable.TableDatas["weapon60"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon11)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon61"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon61"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon61", ServerData.weaponTable.TableDatas["weapon61"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon12)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon62"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon62"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon62", ServerData.weaponTable.TableDatas["weapon62"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        //=======================================================================================
        else if (type == Item_Type.RecommendWeapon13)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon71"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon71"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon71", ServerData.weaponTable.TableDatas["weapon71"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon14)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon72"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon72"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon72", ServerData.weaponTable.TableDatas["weapon72"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon15)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon73"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon73"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon73", ServerData.weaponTable.TableDatas["weapon73"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon16)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon74"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon74"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon74", ServerData.weaponTable.TableDatas["weapon74"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon17)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon75"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon75"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon75", ServerData.weaponTable.TableDatas["weapon75"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon18)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon76"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon76"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon76", ServerData.weaponTable.TableDatas["weapon76"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        //
        else if (type == Item_Type.RecommendWeapon19)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon82"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon82"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon82", ServerData.weaponTable.TableDatas["weapon82"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        else if (type == Item_Type.RecommendWeapon20)
        {
            List<TransactionValue> transactions = new List<TransactionValue>();

            ServerData.weaponTable.TableDatas["weapon83"].amount.Value += 1;
            ServerData.weaponTable.TableDatas["weapon83"].hasItem.Value = 1;

            Param weaponParam = new Param();

            weaponParam.Add("weapon83", ServerData.weaponTable.TableDatas["weapon83"].ConvertToString());

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
                PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "십만대산 명예무기 획득!!", null);
                // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
            });
        }
        //=======================================================================================

        //----------------------------------------
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
