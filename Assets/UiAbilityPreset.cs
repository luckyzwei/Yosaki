using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiAbilityPreset : MonoBehaviour
{
    [SerializeField]
    private List<Image> toggles;

    [SerializeField]
    private Color selectedColor;

    [SerializeField]
    private Color defaultColor;
    private void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.equipmentTable.TableDatas[EquipmentTable.CostumePresetId].AsObservable().Subscribe(e =>
        {
            for (int i = 0; i < toggles.Count; i++)
            {
                toggles[i].color = i == e ? selectedColor : defaultColor;
            }
        }).AddTo(this);
    }

    public void PresetSelected(int preset)
    {
        //저장
        ServerData.equipmentTable.ChangeEquip(EquipmentTable.CostumePresetId, preset);

        ServerData.costumeServerTable.ApplyAbilityByCurrentSelectedPreset();

        UiCostumeAbilityBoard.Instance.RefreshAllData();
    }
}
