using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiPetEquipment : MonoBehaviour
{
    [SerializeField]
    private UiPetEquipmentView equipViewPrefab;

    [SerializeField]
    private Transform cellParents;

    [SerializeField]
    private UiPetEquipmentView equipViewPrefab_Last;

    [SerializeField]
    private List<GameObject> emptyObjects;


    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var equipment = TableManager.Instance.PetEquipment.dataArray;

        for (int i = 0; i < 20; i++)
        {
            var cell = Instantiate<UiPetEquipmentView>(equipViewPrefab, cellParents);
            cell.Initialize(equipment[i]);
        }

        for (int i = 0; i < emptyObjects.Count; i++)
        {
            emptyObjects[i].transform.SetAsLastSibling();
        }

        equipViewPrefab_Last.Initialize(equipment[20]);
    }
}
