using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiPotionUseToggle : MonoBehaviour
{
    [SerializeField]
    private List<Toggle> toggleList;

    private bool initialized = false;
    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        int savedKey = PlayerPrefs.GetInt(SettingKey.PotionUseHpOption);
        Debug.LogError($"Potion savedKey {savedKey}");
        toggleList[savedKey].isOn = true;

        initialized = true;
    }

    public void ToggleChanged_0(bool isOn)
    {
        if (initialized == false) return;

        if (isOn)
        {
            SettingData.PotionUseHpOption.Value = 0;
        }
    }
    public void ToggleChanged_1(bool isOn)
    {
        if (initialized == false) return;

        if (isOn)
        {
            SettingData.PotionUseHpOption.Value = 1;
        }
    }
    public void ToggleChanged_2(bool isOn)
    {
        if (initialized == false) return;

        if (isOn)
        {
            SettingData.PotionUseHpOption.Value = 2;
        }
    }

}
