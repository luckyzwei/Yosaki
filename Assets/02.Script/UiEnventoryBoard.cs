using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiEnventoryBoard : MonoBehaviour
{
    [SerializeField]
    private UiInventoryWeaponView uiInventoryWeaponViewPrefab;

    [SerializeField]
    private Transform viewParentWeapon;
    [SerializeField]
    private Transform viewParentMagicBook;


    [SerializeField]
    private UiWeaponDetailView uiWeaponDetailView;

    private List<UiInventoryWeaponView> weaponViewContainer = new List<UiInventoryWeaponView>();
    private List<UiInventoryWeaponView> magicBookViewContainer = new List<UiInventoryWeaponView>();

    public void Start()
    {
        MakeWeaponBoard();
        MakeMagicBookBoard();
        MakePetBoard();
    }

    private void MakeWeaponBoard()
    {
        var e = TableManager.Instance.WeaponData.GetEnumerator();

        while (e.MoveNext())
        {
            UiInventoryWeaponView view = Instantiate<UiInventoryWeaponView>(uiInventoryWeaponViewPrefab, viewParentWeapon);

            view.Initialize(e.Current.Value, null, OnClickWeaponView);

            weaponViewContainer.Add(view);
        }
    }

    private void MakeMagicBookBoard()
    {
        var e = TableManager.Instance.MagicBoocDatas.GetEnumerator();

        while (e.MoveNext())
        {
            UiInventoryWeaponView view = Instantiate<UiInventoryWeaponView>(uiInventoryWeaponViewPrefab, viewParentMagicBook);

            view.Initialize(null, e.Current.Value, OnClickWeaponView);

            magicBookViewContainer.Add(view);
        }
    }

    private void OnClickWeaponView(WeaponData weaponData, MagicBookData magicBookData)
    {
        uiWeaponDetailView.gameObject.SetActive(true);
        uiWeaponDetailView.Initialize(weaponData, magicBookData);
    }

    [SerializeField]
    private UiPetView uiPetViewPrefeab;
    [SerializeField]
    private Transform petViewParent;

    private List<UiPetView> petViewContainer = new List<UiPetView>();

    private void MakePetBoard() 
    {
        var e = TableManager.Instance.PetDatas.GetEnumerator();

        while (e.MoveNext()) 
        {
            var petView = Instantiate<UiPetView>(uiPetViewPrefeab, petViewParent);

            petView.Initialize(e.Current.Value);

            petViewContainer.Add(petView);
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.U)) 
        //{
        //    var e = TableManager.Instance.WeaponData.GetEnumerator();
        //    while (e.MoveNext()) 
        //    {
        //        DatabaseManager.weaponTable.TableDatas[e.Current.Value.Stringid].hasItem.Value = 1;
        //        DatabaseManager.weaponTable.TableDatas[e.Current.Value.Stringid].amount.Value = 999;
        //        DatabaseManager.weaponTable.TableDatas[e.Current.Value.Stringid].level.Value = e.Current.Value.Maxlevel;
        //        DatabaseManager.weaponTable.SyncToServerAll();
        //    }

        //    var e2 = TableManager.Instance.MagicBoocDatas.GetEnumerator();
        //    while (e2.MoveNext())
        //    {
        //        DatabaseManager.magicBookTable.TableDatas[e2.Current.Value.Stringid].hasItem.Value = 1;
        //        DatabaseManager.magicBookTable.TableDatas[e2.Current.Value.Stringid].amount.Value = 999;
        //        DatabaseManager.magicBookTable.TableDatas[e2.Current.Value.Stringid].level.Value = e2.Current.Value.Maxlevel;
        //        DatabaseManager.magicBookTable.SyncToServerAll();
        //    }
        //}
        //else if (Input.GetKeyUp(KeyCode.Space)) 
        //{
        
        //}
    }

#endif
}
