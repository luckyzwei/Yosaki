using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;
using UnityEngine.UI;

public class UiIapItemCell : MonoBehaviour
{
    [SerializeField]
    private bool isInspectorItem = false;
    [SerializeField]
    private string productId;

    private InAppPurchaseData productData;

    [SerializeField]
    private TextMeshProUGUI titleText;

    [SerializeField]
    private TextMeshProUGUI descriptionText;

    [SerializeField]
    private TextMeshProUGUI itemDetailText;

    [SerializeField]
    private TextMeshProUGUI priceText;
    [SerializeField]
    private TextMeshProUGUI buyCountText;
    [SerializeField]
    private TextMeshProUGUI itemAmountText;
    [SerializeField]
    private Image packageIcon;

    [SerializeField]
    private WeaponView weaponView;

    private CompositeDisposable disposable = new CompositeDisposable();

    [SerializeField]
    private Button buyButton;


    [SerializeField]
    private GameObject levelPackageComplete;

    private void Start()
    {
        if (isInspectorItem == true)
        {
            InitByInspector();
        }
    }

    private void InitByInspector()
    {
        if (TableManager.Instance.InAppPurchaseData.TryGetValue(productId, out var tableData) == false)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"등록되지 않은 productId {productId}", null);
            return;
        }

        Initialize(tableData);
    }

    public void Initialize(InAppPurchaseData productData)
    {
        this.productData = productData;

        if (productData.Active == false)
        {
            this.gameObject.SetActive(false);
        }

        SetTexts();

        Subscribe();

        SetPackageIcon();
    }

    private void SetPackageIcon()
    {
        if (packageIcon != null)
        {
            packageIcon.sprite = Resources.Load<Sprite>($"Package/{productData.Productid}");
        }
    }

    private void SetTexts()
    {
        if (titleText != null)
        {
            titleText.SetText(productData.Title);
            titleText.color = CommonUiContainer.Instance.itemGradeColor[productData.Grade];
        }

        if (descriptionText != null)
            descriptionText.SetText(productData.Description);

        if (productData.Needlevel == 0)
        {
#if UNITY_ANDROID
            string price = IAPManager.m_StoreController.products.WithID(productData.Productid).metadata.localizedPrice.ToString("N0");
#endif
#if UNITY_IOS
        string price = IAPManager.m_StoreController.products.WithID(productData.Productidios).metadata.localizedPrice.ToString("N0");
#endif

            if (priceText != null)
            {
                if (Application.systemLanguage == SystemLanguage.Korean)
                {
                    priceText.SetText($"{price}원");
                }
                else
                {
                    priceText.SetText($"{price}$");
                }

            }
        }

        string itemDetailDesc = null;
        string itemAmount = null;
        weaponView.gameObject.SetActive(false);

        for (int i = 0; i < productData.Rewardtypes.Length; i++)
        {
            //골드,파편버프 표시 X
            if ((Item_Type)productData.Rewardtypes[i] != 0 && (Item_Type)productData.Rewardtypes[i] != 0)
            {
                itemDetailDesc += $"{CommonString.GetItemName((Item_Type)productData.Rewardtypes[i])} {Utils.ConvertBigNum(productData.Rewardvalues[i])}개";
                itemAmount += $"{Utils.ConvertBigNum(productData.Rewardvalues[i])}개";

                if (i != productData.Rewardtypes.Length - 1)
                {
                    itemDetailDesc += "\n";
                    itemAmount += "\n";
                }
            }

            Item_Type itemType = (Item_Type)productData.Rewardtypes[i];

            if (itemType.IsWeaponItem())
            {
                weaponView.gameObject.SetActive(true);
                int itemIdx = productData.Rewardtypes[i] % 1000;

                weaponView.Initialize(TableManager.Instance.WeaponData[itemIdx], null, null);
            }
            else if (itemType.IsNorigaeItem())
            {
                weaponView.gameObject.SetActive(true);
                int itemIdx = productData.Rewardtypes[i] % 2000;

                weaponView.Initialize(null, TableManager.Instance.MagicBoocDatas[itemIdx], null);
            }
            else if (itemType.IsSkillItem())
            {
                weaponView.gameObject.SetActive(true);
                int itemIdx = productData.Rewardtypes[i] % 3000;
                weaponView.Initialize(null, null, TableManager.Instance.SkillData[itemIdx]);
            }
        }

        if (itemDetailText != null)
            itemDetailText.SetText(itemDetailDesc);

        if (itemAmountText != null)
            itemAmountText.SetText(itemAmount);
    }

    public void OnClickBuyButton()
    {
        if (productData.Needlevel != 0)
        {
            int currentLevel = (int)ServerData.userInfoTable.TableDatas[UserInfoTable.topClearStageId].Value + 1;

            if (currentLevel < productData.Needlevel)
            {
                PopupManager.Instance.ShowAlarmMessage($"스테이지 {productData.Needlevel}이상일때 보상을 받으실 수 있습니다!");
                return;
            }
        }

        if (CanBuyProduct() == false)
        {
            if (productData.Needlevel == 0)
            {
                PopupManager.Instance.ShowAlarmMessage("더이상 구매 불가");
            }
            else
            {
                PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다!");
            }

            return;
        }

        if (IsRewardCollect() == false)
        {
            PopupManager.Instance.ShowAlarmMessage("보상 데이터 오류");
            return;
        }

        if (productData.Needlevel == 0)
        {
#if TEST
        UiShop.Instance.BuyProduct(productData.Productid);
        return;
#endif

#if UNITY_ANDROID
            IAPManager.Instance.BuyProduct(productData.Productid);
#endif

#if UNITY_IOS
        IAPManager.Instance.BuyProduct(productData.Productidios);
#endif
        }
        else
        {
            UiLevelUpEventShop.Instance.GetPackageItem(productData.Productid);
        }
    }

    private bool CanBuyProduct()
    {
        if (productData.BUYTYPE != BuyType.Fixed)
        {
            int buyCount = ServerData.iapServerTable.TableDatas[productData.Productid].buyCount.Value;

            return buyCount < GetBuyCount();
        }
        else
        {
            int buyCount = ServerData.iAPServerTableTotal.TableDatas[productData.Productid].buyCount.Value;

            return buyCount < GetBuyCount();
        }

    }

    private bool IsRewardCollect()
    {
        if (productData.Rewardvalues.Length == 0 || productData.Rewardvalues.Length == 0) return false;

        return productData.Rewardtypes.Length == productData.Rewardvalues.Length;
    }

    private int GetBuyCount()
    {
        switch (productData.BUYTYPE)
        {
            case BuyType.NoLimit:
                {
                    return int.MaxValue;
                }
                break;
            case BuyType.DayOfOne:
            case BuyType.MonthOfOne:
            case BuyType.AllTimeOne:
                {
                    return 1;
                }
            case BuyType.WeekOfTwo:
                {
                    return 2;
                }
                break;
            case BuyType.MonthOfFive:
                return 5;
                break;
            case BuyType.WeekOfFive:
                return 5;
                break;
            case BuyType.DayOfFive:
                return 5;
            case BuyType.MonthOfTen:
                return 10;
            case BuyType.Fixed:
                return productData.Fixedbuycount;
                break;
        }

        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"미등록된 타입 {productData.BUYTYPE}", null);

        return int.MaxValue;
    }
    private string GetBuyPrefix()
    {
        switch (productData.BUYTYPE)
        {
            case BuyType.NoLimit:
                return "구매제한 없음";
                break;
            case BuyType.DayOfOne:
                return "일 1회 구매가능";
                break;
            case BuyType.WeekOfTwo:
                return "주 2회 구매가능";
                break;
            case BuyType.MonthOfOne:
                return "월 1회 구매가능";
                break;
            case BuyType.AllTimeOne:
                return "1회만 구매가능";
                break;
            case BuyType.Fixed:
                return $"{productData.Fixedbuycount}회만 구매가능";
                break;


            case BuyType.MonthOfFive:
                return "월 5회 구매가능";
                break;
            case BuyType.WeekOfFive:
                return "주 5회 구매가능";
                break;
            case BuyType.DayOfFive:
                return "일 5회 구매가능";
            case BuyType.MonthOfTen:
                return "월 10회 구매가능";
                break;
        }

        return $"미등록 {productData.BUYTYPE}";
    }

    private void Subscribe()
    {
        disposable.Clear();

        if (productData.BUYTYPE != BuyType.Fixed)
        {
            ServerData.iapServerTable.TableDatas[productData.Productid].buyCount.AsObservable().Subscribe(e =>
            {
                string text = null;

                text += GetBuyPrefix();

                int canBuyCount = GetBuyCount();

                if (canBuyCount != int.MaxValue)
                {
                    text += "\n";
                    text += $"({e}/{canBuyCount})";
                }

                if (buyCountText != null)
                    buyCountText.SetText(text);

            }).AddTo(disposable);
        }
        else
        {
            ServerData.iAPServerTableTotal.TableDatas[productData.Productid].buyCount.AsObservable().Subscribe(e =>
            {
                string text = null;

                text += GetBuyPrefix();

                int canBuyCount = GetBuyCount();

                if (canBuyCount != int.MaxValue)
                {
                    text += "\n";
                    text += $"({e}/{canBuyCount})";
                }

                if (buyCountText != null)
                    buyCountText.SetText(text);

            }).AddTo(disposable);
        }



        IAPManager.Instance.disableBuyButton.AsObservable().Subscribe(e =>
        {
            buyButton.interactable = false;
        }).AddTo(disposable);

        IAPManager.Instance.activeBuyButton.AsObservable().Subscribe(e =>
        {
            buyButton.interactable = true;
        }).AddTo(disposable);

        if (productData.Needlevel != 0)
        {
            ServerData.iapServerTable.TableDatas[productData.Productid].buyCount.AsObservable().Subscribe(e =>
            {
                if (levelPackageComplete != null)
                {
                    levelPackageComplete.SetActive(e >= 1);
                }

                if (buyButton != null)
                {
                    buyButton.gameObject.SetActive(e == 0);
                }
            }).AddTo(disposable);
        }
    }

    private void OnDestroy()
    {
        disposable.Dispose();
    }
}
