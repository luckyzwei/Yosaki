using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiPetEquipment : MonoBehaviour
{
    [SerializeField]
    private UiPetEquipmentView equipViewPrefab;

    [SerializeField]
    private Transform cellParents;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var equipment = TableManager.Instance.PetEquipment.dataArray;

        for (int i = 0; i < equipment.Length; i++)
        {
            var cell = Instantiate<UiPetEquipmentView>(equipViewPrefab, cellParents);
            cell.Initialize(equipment[i]);
        }

    }
}
