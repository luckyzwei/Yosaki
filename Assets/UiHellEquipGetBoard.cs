using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UiHellEquipGetBoard : MonoBehaviour
{
    [SerializeField]
    private double reqCostume0;
    [SerializeField]
    private double reqCostume1;
    [SerializeField]
    private double norigaeReq;
    [SerializeField]
    private double weaponReq;
    [SerializeField]
    private double norigaeReq2;
    [SerializeField]
    private double weaponReq2;

    [SerializeField]
    private double norigaeReq3;
    [SerializeField]
    private double weaponReq3;

    [SerializeField]
    private WeaponView norigaeView;
    [SerializeField]
    private WeaponView weaponView;
    [SerializeField]
    private WeaponView norigaeView2;
    [SerializeField]
    private WeaponView weaponView2;
    [SerializeField]
    private WeaponView norigaeView3;
    [SerializeField]
    private WeaponView weaponView3;

    [SerializeField]
    private TextMeshProUGUI scoreDescription;

    //
    [SerializeField]
    private TextMeshProUGUI buttonDesc0;

    [SerializeField]
    private TextMeshProUGUI buttonDesc1;

    [SerializeField]
    private TextMeshProUGUI buttonDesc2;

    [SerializeField]
    private TextMeshProUGUI buttonDesc3;

    [SerializeField]
    private TextMeshProUGUI buttonDesc4;

    [SerializeField]
    private TextMeshProUGUI buttonDesc5;

    [SerializeField]
    private TextMeshProUGUI buttonDesc6;

    [SerializeField]
    private TextMeshProUGUI buttonDesc7;

    //
    [SerializeField]
    private TextMeshProUGUI scoreDesc0;

    [SerializeField]
    private TextMeshProUGUI scoreDesc1;

    [SerializeField]
    private TextMeshProUGUI scoreDesc2;

    [SerializeField]
    private TextMeshProUGUI scoreDesc3;

    [SerializeField]
    private TextMeshProUGUI scoreDesc4;

    [SerializeField]
    private TextMeshProUGUI scoreDesc5;

    [SerializeField]
    private TextMeshProUGUI scoreDesc6;

    [SerializeField]
    private TextMeshProUGUI scoreDesc7;

    private void Start()
    {
        Initialize();
    }


    private void Initialize()
    {
        scoreDescription.SetText($"최고 점수 : {Utils.ConvertBigNum(ServerData.userInfoTable.TableDatas[UserInfoTable.hellScore].Value * GameBalance.BossScoreConvertToOrigin)}");

        norigaeView.Initialize(null, TableManager.Instance.MagicBoocDatas[30]);
        norigaeView2.Initialize(null, TableManager.Instance.MagicBoocDatas[31]);
        norigaeView3.Initialize(null, TableManager.Instance.MagicBoocDatas[34]);

        weaponView.Initialize(TableManager.Instance.WeaponData[31], null);
        weaponView2.Initialize(TableManager.Instance.WeaponData[32], null);
        weaponView3.Initialize(TableManager.Instance.WeaponData[35], null);


        scoreDesc0.SetText($"{Utils.ConvertBigNum(reqCostume0)} 이상");
        scoreDesc1.SetText($"{Utils.ConvertBigNum(reqCostume1)} 이상");
        scoreDesc2.SetText($"{Utils.ConvertBigNum(norigaeReq)} 이상");
        scoreDesc3.SetText($"{Utils.ConvertBigNum(weaponReq)} 이상");

        scoreDesc4.SetText($"{Utils.ConvertBigNum(norigaeReq2)} 이상");
        scoreDesc5.SetText($"{Utils.ConvertBigNum(weaponReq2)} 이상");

        scoreDesc6.SetText($"{Utils.ConvertBigNum(norigaeReq3)} 이상");
        scoreDesc7.SetText($"{Utils.ConvertBigNum(weaponReq3)} 이상");

        Subscribe();
    }

    private void Subscribe()
    {
        //costume43 저승사자
        ServerData.costumeServerTable.TableDatas["costume43"].hasCostume.AsObservable().Subscribe(e =>
        {
            buttonDesc0.SetText(e == false ? "획득" : "보유중");
        }).AddTo(this);

        //costume44 염라대왕
        ServerData.costumeServerTable.TableDatas["costume44"].hasCostume.AsObservable().Subscribe(e =>
        {
            buttonDesc1.SetText(e == false ? "획득" : "보유중");
        }).AddTo(this);

        //magicBook30
        ServerData.magicBookTable.TableDatas["magicBook30"].hasItem.AsObservable().Subscribe(e =>
        {
            buttonDesc2.SetText(e == 0 ? "획득" : "보유중");
        }).AddTo(this);

        //weapon31
        ServerData.weaponTable.TableDatas["weapon31"].hasItem.AsObservable().Subscribe(e =>
        {
            buttonDesc3.SetText(e == 0 ? "획득" : "보유중");
        }).AddTo(this);

        //

        //magicBook30
        ServerData.magicBookTable.TableDatas["magicBook31"].hasItem.AsObservable().Subscribe(e =>
        {
            buttonDesc4.SetText(e == 0 ? "획득" : "보유중");
        }).AddTo(this);

        //weapon31
        ServerData.weaponTable.TableDatas["weapon32"].hasItem.AsObservable().Subscribe(e =>
        {
            buttonDesc5.SetText(e == 0 ? "획득" : "보유중");
        }).AddTo(this);

        //

        //magicBook30
        ServerData.magicBookTable.TableDatas["magicBook34"].hasItem.AsObservable().Subscribe(e =>
        {
            buttonDesc6.SetText(e == 0 ? "획득" : "보유중");
        }).AddTo(this);

        //weapon31
        ServerData.weaponTable.TableDatas["weapon35"].hasItem.AsObservable().Subscribe(e =>
        {
            buttonDesc7.SetText(e == 0 ? "획득" : "보유중");
        }).AddTo(this);
    }


    public void OnClickGetButton(int idx)
    {
        double currentScore = ServerData.userInfoTable.TableDatas[UserInfoTable.hellScore].Value * GameBalance.BossScoreConvertToOrigin;

        //외형1
        if (idx == 0)
        {
            if (currentScore >= reqCostume0)
            {
                var costumeServerData = ServerData.costumeServerTable.TableDatas["costume43"];

                if (costumeServerData.hasCostume.Value == true)
                {
                    PopupManager.Instance.ShowAlarmMessage("이미 외형이 있습니다.");
                    return;
                }

                costumeServerData.hasCostume.Value = true;

                Param param = new Param();

                param.Add("costume43", costumeServerData.ConvertToString());

                SendQueue.Enqueue(Backend.GameData.Update, CostumeServerTable.tableName, CostumeServerTable.Indate, param, e =>
                {
                    if (e.IsSuccess())
                    {
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "외형 획득!", null);
                    }
                });

                ServerData.costumeServerTable.SyncCostumeData("costume43");
            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage("점수가 부족 합니다!");
            }
        }

        //외형2
        if (idx == 1)
        {
            if (currentScore >= reqCostume1)
            {

                var costumeServerData = ServerData.costumeServerTable.TableDatas["costume44"];

                if (costumeServerData.hasCostume.Value == true)
                {
                    PopupManager.Instance.ShowAlarmMessage("이미 외형이 있습니다.");
                    return;
                }

                costumeServerData.hasCostume.Value = true;

                Param param = new Param();

                param.Add("costume44", costumeServerData.ConvertToString());

                SendQueue.Enqueue(Backend.GameData.Update, CostumeServerTable.tableName, CostumeServerTable.Indate, param, e =>
                {
                    if (e.IsSuccess())
                    {
                        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "외형 획득!", null);
                    }
                });

                ServerData.costumeServerTable.SyncCostumeData("costume44");

            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage("점수가 부족 합니다!");
            }
        }

        //노리개
        if (idx == 2)
        {
            if (currentScore >= norigaeReq)
            {
                if (ServerData.magicBookTable.TableDatas["magicBook30"].hasItem.Value == 1)
                {
                    PopupManager.Instance.ShowAlarmMessage("이미 노리개가 있습니다.");
                    return;
                }

                List<TransactionValue> transactions = new List<TransactionValue>();

                ServerData.magicBookTable.TableDatas["magicBook30"].amount.Value += 1;
                ServerData.magicBookTable.TableDatas["magicBook30"].hasItem.Value = 1;

                Param magicBookParam = new Param();

                magicBookParam.Add("magicBook30", ServerData.magicBookTable.TableDatas["magicBook30"].ConvertToString());

                transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

                ServerData.SendTransaction(transactions, successCallBack: () =>
                {
                    SoundManager.Instance.PlaySound("Reward");
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "노리개 획득!!", null);
                    // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
                });
            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage("점수가 부족 합니다!");
            }
        }

        //무기
        if (idx == 3)
        {
            if (currentScore >= weaponReq)
            {
                if (ServerData.weaponTable.TableDatas["weapon31"].hasItem.Value == 1)
                {
                    PopupManager.Instance.ShowAlarmMessage("이미 무기가 있습니다.");
                    return;
                }

                List<TransactionValue> transactions = new List<TransactionValue>();

                ServerData.weaponTable.TableDatas["weapon31"].amount.Value += 1;
                ServerData.weaponTable.TableDatas["weapon31"].hasItem.Value = 1;

                Param weaponParam = new Param();

                weaponParam.Add("weapon31", ServerData.weaponTable.TableDatas["weapon31"].ConvertToString());

                transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

                ServerData.SendTransaction(transactions, successCallBack: () =>
                {
                    SoundManager.Instance.PlaySound("Reward");
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "무기 획득!!", null);
                    // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
                });
            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage("점수가 부족 합니다!");
            }
        }

        ////
        /// //
        //노리개
        if (idx == 4)
        {
            if (currentScore >= norigaeReq2)
            {
                if (ServerData.magicBookTable.TableDatas["magicBook31"].hasItem.Value == 1)
                {
                    PopupManager.Instance.ShowAlarmMessage("이미 노리개가 있습니다.");
                    return;
                }

                List<TransactionValue> transactions = new List<TransactionValue>();

                ServerData.magicBookTable.TableDatas["magicBook31"].amount.Value += 1;
                ServerData.magicBookTable.TableDatas["magicBook31"].hasItem.Value = 1;

                Param magicBookParam = new Param();

                magicBookParam.Add("magicBook31", ServerData.magicBookTable.TableDatas["magicBook31"].ConvertToString());

                transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

                ServerData.SendTransaction(transactions, successCallBack: () =>
                {
                    SoundManager.Instance.PlaySound("Reward");
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "노리개 획득!!", null);
                    // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
                });
            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage("점수가 부족 합니다!");
            }
        }

        //무기
        if (idx == 5)
        {
            if (currentScore >= weaponReq2)
            {
                if (ServerData.weaponTable.TableDatas["weapon32"].hasItem.Value == 1)
                {
                    PopupManager.Instance.ShowAlarmMessage("이미 무기가 있습니다.");
                    return;
                }

                List<TransactionValue> transactions = new List<TransactionValue>();

                ServerData.weaponTable.TableDatas["weapon32"].amount.Value += 1;
                ServerData.weaponTable.TableDatas["weapon32"].hasItem.Value = 1;

                Param weaponParam = new Param();

                weaponParam.Add("weapon32", ServerData.weaponTable.TableDatas["weapon32"].ConvertToString());

                transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

                ServerData.SendTransaction(transactions, successCallBack: () =>
                {
                    SoundManager.Instance.PlaySound("Reward");
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "무기 획득!!", null);
                    // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
                });
            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage("점수가 부족 합니다!");
            }
        }
        //

        //노리개
        if (idx == 6)
        {
            if (currentScore >= norigaeReq3)
            {
                if (ServerData.magicBookTable.TableDatas["magicBook34"].hasItem.Value == 1)
                {
                    PopupManager.Instance.ShowAlarmMessage("이미 노리개가 있습니다.");
                    return;
                }

                List<TransactionValue> transactions = new List<TransactionValue>();

                ServerData.magicBookTable.TableDatas["magicBook34"].amount.Value += 1;
                ServerData.magicBookTable.TableDatas["magicBook34"].hasItem.Value = 1;

                Param magicBookParam = new Param();

                magicBookParam.Add("magicBook34", ServerData.magicBookTable.TableDatas["magicBook34"].ConvertToString());

                transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

                ServerData.SendTransaction(transactions, successCallBack: () =>
                {
                    SoundManager.Instance.PlaySound("Reward");
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "노리개 획득!!", null);
                    // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
                });
            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage("점수가 부족 합니다!");
            }
        }

        //무기
        if (idx == 7)
        {
            if (currentScore >= weaponReq3)
            {
                if (ServerData.weaponTable.TableDatas["weapon35"].hasItem.Value == 1)
                {
                    PopupManager.Instance.ShowAlarmMessage("이미 무기가 있습니다.");
                    return;
                }

                List<TransactionValue> transactions = new List<TransactionValue>();

                ServerData.weaponTable.TableDatas["weapon35"].amount.Value += 1;
                ServerData.weaponTable.TableDatas["weapon35"].hasItem.Value = 1;

                Param weaponParam = new Param();

                weaponParam.Add("weapon35", ServerData.weaponTable.TableDatas["weapon35"].ConvertToString());

                transactions.Add(TransactionValue.SetUpdate(WeaponTable.tableName, WeaponTable.Indate, weaponParam));

                ServerData.SendTransaction(transactions, successCallBack: () =>
                {
                    SoundManager.Instance.PlaySound("Reward");
                    PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "무기 획득!!", null);
                    // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
                });
            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage("점수가 부족 합니다!");
            }
        }

    }
}
