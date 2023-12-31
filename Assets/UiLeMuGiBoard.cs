﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiLeMuGiBoard : SingletonMono<UiLeMuGiBoard>
{
    [SerializeField]
    private GameObject rootObject;

    [SerializeField]
    private UiPetView uiPetViewPrefeab_LeeMuGi;

    [SerializeField]
    private UiPetView uiPetViewPrefeab_GoldDragon;

    [SerializeField]
    private UiPetView uiPetViewPrefeab_Haetae;

    [SerializeField]
    private UiPetView uiPetViewPrefeab_Sam;

    [SerializeField]
    private UiPetView uiPetViewPrefeab_Kirin;

    [SerializeField]
    private UiPetView uiPetViewPrefeab_Rabit;

    [SerializeField]
    private UiPetView uiPetViewPrefeab_Dog; 
    
    [SerializeField]
    private UiPetView uiPetViewPrefeab_Horse;   
    
    [SerializeField]
    private UiPetView uiPetViewPrefeab_ChunDog; 
    
    [SerializeField]
    private UiPetView uiPetViewPrefeab_ChunCat;

    [SerializeField]
    private UiPetView uiPetViewPrefeab_Chungdung;

    [SerializeField]
    private UiPetView uiPetViewPrefeab_Cloud;

    [SerializeField]
    private Transform petViewParent;
    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var e = TableManager.Instance.PetDatas.GetEnumerator();

        while (e.MoveNext())
        {
            //이무기는 생성X
            if (e.Current.Value.Id == 12) 
            {
                var petView = Instantiate<UiPetView>(uiPetViewPrefeab_LeeMuGi, petViewParent);

                petView.gameObject.SetActive(true);

                petView.transform.localPosition = Vector3.zero;

                petView.Initialize(e.Current.Value);
            }
            else if(e.Current.Value.Id == 13) 
            {
                var petView = Instantiate<UiPetView>(uiPetViewPrefeab_GoldDragon, petViewParent);

                petView.gameObject.SetActive(true);

                petView.transform.localPosition = Vector3.zero;

                petView.Initialize(e.Current.Value);

            }
            else if (e.Current.Value.Id == 14) 
            {
                var petView = Instantiate<UiPetView>(uiPetViewPrefeab_Haetae, petViewParent);

                petView.gameObject.SetActive(true);

                petView.transform.localPosition = Vector3.zero;

                petView.Initialize(e.Current.Value);
            }
            else if (e.Current.Value.Id == 15)
            {
                var petView = Instantiate<UiPetView>(uiPetViewPrefeab_Sam, petViewParent);

                petView.gameObject.SetActive(true);

                petView.transform.localPosition = Vector3.zero;

                petView.Initialize(e.Current.Value);
            }
            else if (e.Current.Value.Id == 16)
            {
                var petView = Instantiate<UiPetView>(uiPetViewPrefeab_Kirin, petViewParent);

                petView.gameObject.SetActive(true);

                petView.transform.localPosition = Vector3.zero;

                petView.Initialize(e.Current.Value);
            }
            else if (e.Current.Value.Id == 17)
            {
                var petView = Instantiate<UiPetView>(uiPetViewPrefeab_Rabit, petViewParent);

                petView.gameObject.SetActive(true);

                petView.transform.localPosition = Vector3.zero;

                petView.Initialize(e.Current.Value);
            }  
            else if (e.Current.Value.Id == 18)
            {
                var petView = Instantiate<UiPetView>(uiPetViewPrefeab_Dog, petViewParent);

                petView.gameObject.SetActive(true);

                petView.transform.localPosition = Vector3.zero;

                petView.Initialize(e.Current.Value);
            }  
            else if (e.Current.Value.Id == 19)
            {
                var petView = Instantiate<UiPetView>(uiPetViewPrefeab_Horse, petViewParent);

                petView.gameObject.SetActive(true);

                petView.transform.localPosition = Vector3.zero;

                petView.Initialize(e.Current.Value);
            }
            else if (e.Current.Value.Id == 20)
            {
                var petView = Instantiate<UiPetView>(uiPetViewPrefeab_ChunDog, petViewParent);

                petView.gameObject.SetActive(true);

                petView.transform.localPosition = Vector3.zero;

                petView.Initialize(e.Current.Value);
            }
            else if (e.Current.Value.Id == 21)
            {
                var petView = Instantiate<UiPetView>(uiPetViewPrefeab_ChunCat, petViewParent);

                petView.gameObject.SetActive(true);

                petView.transform.localPosition = Vector3.zero;

                petView.Initialize(e.Current.Value);
            }
            //
            else if (e.Current.Value.Id == 22)
            {
                var petView = Instantiate<UiPetView>(uiPetViewPrefeab_Chungdung, petViewParent);

                petView.gameObject.SetActive(true);

                petView.transform.localPosition = Vector3.zero;

                petView.Initialize(e.Current.Value);
            }
            else if (e.Current.Value.Id == 23)
            {
                var petView = Instantiate<UiPetView>(uiPetViewPrefeab_Cloud, petViewParent);

                petView.gameObject.SetActive(true);

                petView.transform.localPosition = Vector3.zero;

                petView.Initialize(e.Current.Value);
            }
        }
    }
}
