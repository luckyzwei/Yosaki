using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine.Events;
using static ContinueOpenButton;
public class UiColdSeasonPassInsBuyButton : MonoBehaviour
{
    [SerializeField]
    private PointerDownEvent OnEvent;

    private ObscuredFloat killAddAmount = 30000000;

    public static readonly string monthInsPassKey = "coldseasonins";

    private CompositeDisposable disposable = new CompositeDisposable();

    private Button buyButton;

    [SerializeField]
    private TextMeshProUGUI buttonDesc;

    [SerializeField]
    private TextMeshProUGUI killCountDescription;

    void Start()
    {
        Subscribe();

        // buttonDesc.SetText($"처치 +{Utils.ConvertBigNum(killAddAmount)}");

        killCountDescription.SetText($"처치수 + {Utils.ConvertBigNum(killAddAmount)}");
    }

    private void OnDestroy()
    {
        disposable.Dispose();
    }

    private void Subscribe()
    {
        buyButton = GetComponent<Button>();

        disposable.Clear();

        IAPManager.Instance.WhenBuyComplete.AsObservable().Subscribe(e =>
        {
            SoundManager.Instance.PlaySound("GoldUse");
            GetPackageItem(e.purchasedProduct.definition.id);
        }).AddTo(disposable);

        IAPManager.Instance.disableBuyButton.AsObservable().Subscribe(e =>
        {
            buyButton.interactable = false;
        }).AddTo(disposable);

        IAPManager.Instance.activeBuyButton.AsObservable().Subscribe(e =>
        {
            buyButton.interactable = true;
        }).AddTo(disposable);
    }



    public void OnClickBuyButton()
    {

        if (CanBuyProduct() == false)
        {
            PopupManager.Instance.ShowAlarmMessage("9월 25일 부터 구매 가능합니다!");
            return;
        }

#if UNITY_EDITOR || TEST
        GetPackageItem(monthInsPassKey);
        return;
#endif

        IAPManager.Instance.BuyProduct(monthInsPassKey);
    }

    private bool CanBuyProduct()
    {
        var severTime = ServerData.userInfoTable.currentServerTime;

        if (severTime.Month == 9)
        {
            return severTime.Day >= 25;

        }
        else
        {
            return true;
        }

    }

    public void GetPackageItem(string productId)
    {
        if (TableManager.Instance.InAppPurchaseData.TryGetValue(productId, out var tableData) == false)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"등록되지 않은 상품 id {productId}", null);
            return;
        }
        else
        {
            // PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"{tableData.Title} 구매 성공!", null);
        }

        if (tableData.Productid != monthInsPassKey) return;


        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"구매 성공!", null);

        ServerData.userInfoTable.GetTableData(UserInfoTable.killCountTotalColdSeason).Value += killAddAmount;

        ServerData.userInfoTable.UpData(UserInfoTable.killCountTotalColdSeason, false);

        OnEvent?.Invoke();
    }
}
