using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiPackageShop : MonoBehaviour
{
    [SerializeField]
    private UiIapItemCell iapCellPrefab;

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

            if (e.Current.Value.SHOPCATEGORY == ShopCategory.Limit1)
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
            else if (e.Current.Value.SHOPCATEGORY == ShopCategory.Event)
            {
                var cell = Instantiate<UiIapItemCell>(iapCellPrefab, eventParent);
                cell.Initialize(e.Current.Value);
            }
        }
    }
}
