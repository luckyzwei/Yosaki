using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using BackEnd;
using Spine.Unity;

public class UiNorigaeCraftBoard : SingletonMono<UiNorigaeCraftBoard>
{
    [SerializeField]
    private GameObject rootObject;

    [SerializeField]
    private TextMeshProUGUI magidBookAmount;

    [SerializeField]
    private SkeletonGraphic petGraphic;


    [SerializeField]
    private WeaponView sinSuNorigaeView;

    [SerializeField]
    private WeaponView legend1View;

    private MagicBookData sinsuData;

    private MagicBookData legendNorigaeData;

    private CompositeDisposable disposable = new CompositeDisposable();

    private int needPetId = 0;

    [SerializeField]
    private TextMeshProUGUI petHasDescription;

    private new void OnDestroy()
    {
        base.OnDestroy();
        disposable.Dispose();
    }

    public void Initialize(int norigaeId)
    {
        sinsuData = TableManager.Instance.MagicBookTable.dataArray[norigaeId];

        var sinsuServerData = ServerData.magicBookTable.TableDatas[sinsuData.Stringid];

        if (sinsuServerData.hasItem.Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보유하고 있습니다.");
            return;
        }

        disposable.Clear();

        rootObject.SetActive(true);

        sinSuNorigaeView.Initialize(null, sinsuData);

        legendNorigaeData = TableManager.Instance.MagicBookTable.dataArray[15];
        legend1View.Initialize(null, legendNorigaeData);

        ServerData.magicBookTable.TableDatas[legendNorigaeData.Stringid].amount.AsObservable().Subscribe(e =>
        {
            magidBookAmount.SetText($"{e}/{1}");
        }).AddTo(disposable);

        needPetId = sinsuData.Needpetid;

        petGraphic.Clear();
        petGraphic.skeletonDataAsset = CommonUiContainer.Instance.petCostumeList[sinsuData.Needpetid];
        petGraphic.startingAnimation = "walk";
        petGraphic.Initialize(true);
        petGraphic.SetMaterialDirty();


        var petTableData = TableManager.Instance.PetTable.dataArray[needPetId];

        bool hasPet = ServerData.petTable.TableDatas[petTableData.Stringid].hasItem.Value == 1;

        petHasDescription.SetText(hasPet ? "<color=green>보유</color>" : "<color=red>미보유</color>");
    }

    public void OnClickCraftButton()
    {
        int legendMagicBookAmount = ServerData.magicBookTable.TableDatas[legendNorigaeData.Stringid].amount.Value;

        var petTableData = TableManager.Instance.PetTable.dataArray[needPetId];

        bool hasPet = ServerData.petTable.TableDatas[petTableData.Stringid].hasItem.Value == 1;

        if (legendMagicBookAmount < 1 || hasPet == false)
        {
            PopupManager.Instance.ShowAlarmMessage("재료가 부족 합니다.");
            return;
        }

        List<TransactionValue> transactions = new List<TransactionValue>();

        ServerData.magicBookTable.TableDatas[legendNorigaeData.Stringid].amount.Value -= 1;

        ServerData.magicBookTable.TableDatas[sinsuData.Stringid].amount.Value += 1;
        ServerData.magicBookTable.TableDatas[sinsuData.Stringid].hasItem.Value = 1;

        Param magicBookParam = new Param();

        magicBookParam.Add(legendNorigaeData.Stringid, ServerData.magicBookTable.TableDatas[legendNorigaeData.Stringid].ConvertToString());
        magicBookParam.Add(sinsuData.Stringid, ServerData.magicBookTable.TableDatas[sinsuData.Stringid].ConvertToString());

        transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            SoundManager.Instance.PlaySound("Reward");
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "신수 제작 완료!", null);
            LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
        });
    }
}
