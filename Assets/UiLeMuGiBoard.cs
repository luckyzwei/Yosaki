using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiLeMuGiBoard : SingletonMono<UiLeMuGiBoard>
{
    [SerializeField]
    private GameObject rootObject;

    [SerializeField]
    private UiPetView uiPetViewPrefeab;
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
            if (e.Current.Value.Id != 12) continue;

            var petView = Instantiate<UiPetView>(uiPetViewPrefeab, petViewParent);

            petView.gameObject.SetActive(true);

            petView.transform.localPosition = Vector3.zero;

            petView.Initialize(e.Current.Value);
        }
    }
}
