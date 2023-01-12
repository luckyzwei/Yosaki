using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiPackageShop : MonoBehaviour
{
    [SerializeField]
    private UiIapItemCell iapCellPrefab;

    [SerializeField]
    private Transform gemCategoryParent;

    [SerializeField]
    private Transform package1Parent;

    [SerializeField]
    private Transform package2Parent;

    [SerializeField]
    private Transform petCostumeParent;

    [SerializeField]
    private Transform relicParent;

    [SerializeField]
    private Transform eventParent;

    [SerializeField]
    private Transform springEventParent;

    [SerializeField]
    private Transform chunFlower;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var e = TableManager.Instance.InAppPurchaseData.GetEnumerator();

        while (e.MoveNext())
        {
            if (e.Current.Value.Active == false) continue;

            if (e.Current.Value.SHOPCATEGORY == ShopCategory.Gem)
            {
                var cell = Instantiate<UiIapItemCell>(iapCellPrefab, gemCategoryParent);
                cell.Initialize(e.Current.Value);

            }
            else if (e.Current.Value.SHOPCATEGORY == ShopCategory.Limit1)
            {
                var cell = Instantiate<UiIapItemCell>(iapCellPrefab, package1Parent);
                cell.Initialize(e.Current.Value);
            }

            else if (e.Current.Value.SHOPCATEGORY == ShopCategory.Limit2)
            {
                var cell = Instantiate<UiIapItemCell>(iapCellPrefab, package2Parent);
                cell.Initialize(e.Current.Value);
            }
            else if (e.Current.Value.SHOPCATEGORY == ShopCategory.Pet || e.Current.Value.SHOPCATEGORY == ShopCategory.Costume)
            {
                var cell = Instantiate<UiIapItemCell>(iapCellPrefab, petCostumeParent);
                cell.Initialize(e.Current.Value);
            }
            else if (e.Current.Value.SHOPCATEGORY == ShopCategory.Limit3)
            {
                var cell = Instantiate<UiIapItemCell>(iapCellPrefab, relicParent);
                cell.Initialize(e.Current.Value);
            }
            else if (e.Current.Value.SHOPCATEGORY == ShopCategory.Event2)
            {
                var cell = Instantiate<UiIapItemCell>(iapCellPrefab, eventParent);
                cell.Initialize(e.Current.Value);
            }
            else if (e.Current.Value.SHOPCATEGORY == ShopCategory.Event3)
            {
                if (e.Current.Value.Productid == "chris0" || e.Current.Value.Productid == "chris1")
                {
                    if (ServerData.userInfoTable.currentServerTime.Month == 1 &&
                        ServerData.userInfoTable.currentServerTime.Day >= 6)
                    {
                        continue;
                    }
                }
#if UNITY_EDITOR
#else

                if (e.Current.Value.Productid == "newyearset0" || e.Current.Value.Productid == "newyearset1" )
                {
                    //1월 20일 전에는 생성 x
                    if (ServerData.userInfoTable.currentServerTime.Month == 1 &&
                        ServerData.userInfoTable.currentServerTime.Day < 20)
                    {
                        continue;
                    }
                }
#endif

                var cell = Instantiate<UiIapItemCell>(iapCellPrefab, springEventParent);
                cell.Initialize(e.Current.Value);
            }
            else if (e.Current.Value.SHOPCATEGORY == ShopCategory.ChunFlower)
            {
                var cell = Instantiate<UiIapItemCell>(iapCellPrefab, chunFlower);
                cell.Initialize(e.Current.Value);//
            }
        }
    }
}
