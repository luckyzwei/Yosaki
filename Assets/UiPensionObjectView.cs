using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using Coffee.UIEffects;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiPensionObjectView : MonoBehaviour
{
    public ObscuredString pensionKey;

    public ObscuredInt instantReceiveValue;

    public ObscuredInt dailyReceiveValue;

    public ObscuredInt dayCountMax;

    [SerializeField]
    private UiPensionItemCell cellPrefab;

    [SerializeField]
    private Transform cellParents;

    [SerializeField]
    private UIShiny uiShiny;

    [SerializeField]
    private TextMeshProUGUI buyButtonDesc;

    [SerializeField]
    private GameObject buyButton;

    [SerializeField]
    private TextMeshProUGUI instantRewardDesc;

    [SerializeField]
    private TextMeshProUGUI dailyRewardDesc;

    [SerializeField]
    private TextMeshProUGUI attendanceCount;

    void Start()
    {
        Initialize();
        Subscribe();
    }

    private void Initialize()
    {
        for (int i = 0; i < dayCountMax; i++)
        {
            var cell = Instantiate<UiPensionItemCell>(cellPrefab, cellParents);
            cell.gameObject.SetActive(true);
            cell.Initialize(pensionKey, i, dailyReceiveValue, dayCountMax);
        }

        instantRewardDesc.SetText(Utils.ConvertBigNum(instantReceiveValue));
        dailyRewardDesc.SetText(Utils.ConvertBigNum(dailyReceiveValue));
    }

    private void Subscribe()
    {
        ServerData.iapServerTable.TableDatas[pensionKey].buyCount.AsObservable().Subscribe(e =>
        {
            uiShiny.enabled = e > 0;

            string price = IAPManager.m_StoreController.products.WithID(pensionKey).metadata.localizedPrice.ToString("N0");

            buyButton.SetActive(e == 0);

            buyButtonDesc.SetText(e > 0 ? "구매함" : $"{price}");

            attendanceCount.gameObject.SetActive(true);
        }).AddTo(this);

        IAPManager.Instance.WhenBuyComplete.AsObservable().Subscribe(e =>
        {
            SoundManager.Instance.PlaySound("Reward");
            GetPackageItem(e.purchasedProduct.definition.id);
        }).AddTo(this);

        ServerData.userInfoTable.TableDatas[pensionKey].AsObservable().Subscribe(e =>
        {
            attendanceCount.gameObject.SetActive(ServerData.iapServerTable.TableDatas[pensionKey].buyCount.Value > 0);
            attendanceCount.SetText($"{e + 1}일차");
        }).AddTo(this);
    }

    public void GetPackageItem(string productId)
    {
        if (productId.Equals("removeadios"))
        {
            productId = "removead";
        }

        if (TableManager.Instance.InAppPurchaseData.TryGetValue(productId, out var tableData) == false)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"등록되지 않은 상품 id {productId}", null);
            return;
        }

        if (tableData.Productid != pensionKey) return;

        ServerData.iapServerTable.TableDatas[tableData.Productid].buyCount.Value++;

        Item_Type itemType = Item_Type.Gold;

        if (pensionKey == "oakpension")
        {
            itemType = Item_Type.Jade;
        }
        else if (pensionKey == "marblepension")
        {
            itemType = Item_Type.Marble;
        }
        else if (pensionKey == "relicpension")
        {
            itemType = Item_Type.RelicTicket;
        }
        else if (pensionKey == "peachpension")
        {
            itemType = Item_Type.PeachReal;
        }
        else if (pensionKey == "smithpension")
        {
            itemType = Item_Type.SmithFire;
        }   
        else if (pensionKey == "weaponpension")
        {
            itemType = Item_Type.SP;
        }

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param iapParam = new Param();
        iapParam.Add(tableData.Productid, ServerData.iapServerTable.TableDatas[tableData.Productid].ConvertToString());

        transactions.Add(ServerData.GetItemTypeTransactionValueForAttendance(itemType, instantReceiveValue));
        transactions.Add(TransactionValue.SetUpdate(IAPServerTable.tableName, IAPServerTable.Indate, iapParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"구매 성공!\n{CommonString.GetItemName(itemType)} {Utils.ConvertBigNum(instantReceiveValue)}개 획득!", null);
        });
    }

    public void OnClickBuyButton()
    {
        if (ServerData.iapServerTable.TableDatas[pensionKey].buyCount.Value > 0f)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 구입함");
            return;
        }

#if UNITY_EDITOR || TEST
        GetPackageItem(pensionKey);
        return;
#endif

        IAPManager.Instance.BuyProduct(pensionKey);
    }



}
