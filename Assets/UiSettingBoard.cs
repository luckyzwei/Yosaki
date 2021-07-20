using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiSettingBoard : MonoBehaviour
{
    [SerializeField]
    private Slider bgmSlider;
    [SerializeField]
    private Slider efxSlider;
    [SerializeField]
    private Slider vierSlider;

    [SerializeField]
    private List<Toggle> graphicOptionToggle;

    [SerializeField]
    private List<Toggle> frameRateToggle;

    [SerializeField]
    private Toggle showDamageFontToggle;

    [SerializeField]
    private Toggle showEffectToggle;

    //Glow 효과임
    [SerializeField]
    private Toggle shakeCameraToggle;

    [SerializeField]
    private TextMeshProUGUI bgmDesc;

    [SerializeField]
    private TextMeshProUGUI sfxDesc;

    [SerializeField]
    private TextMeshProUGUI viewDesc;

    private void Awake()
    {
        Initialize();
    }

    private bool initialized = false;

    private void Initialize()
    {
        bgmSlider.value = PlayerPrefs.GetFloat(SettingKey.bgmVolume);
        efxSlider.value = PlayerPrefs.GetFloat(SettingKey.efxVolume);
        vierSlider.value = PlayerPrefs.GetFloat(SettingKey.view);

        graphicOptionToggle[PlayerPrefs.GetInt(SettingKey.GraphicOption)].isOn = true;

        frameRateToggle[PlayerPrefs.GetInt(SettingKey.FrameRateOption)].isOn = true;

        showDamageFontToggle.isOn = PlayerPrefs.GetInt(SettingKey.ShowDamageFont) == 1;

        showEffectToggle.isOn = PlayerPrefs.GetInt(SettingKey.ShowEffect) == 1;

        shakeCameraToggle.isOn = PlayerPrefs.GetInt(SettingKey.GlowEffect) == 1;

        initialized = true;

        SetSliderTexts();
    }

    private void SetSliderTexts() 
    {
        bgmDesc.SetText(((int)(bgmSlider.value * 100)).ToString());
        sfxDesc.SetText(((int)(efxSlider.value * 100)).ToString());
        viewDesc.SetText(((int)(vierSlider.value * 100)).ToString());
    }


    public void WhenBgmSliderChanged(float value)
    {
        if (initialized == false) return;
        SettingData.bgmVolume.Value = value;
        bgmDesc.SetText(((int)(value * 100)).ToString());
    }

    public void WhenEfxSliderChanged(float value)
    {
        if (initialized == false) return;
        SettingData.efxVolume.Value = value;
        sfxDesc.SetText(((int)(value * 100)).ToString());
    }

    public void WhenViewSliderChanged(float value)
    {
        if (initialized == false) return;
        SettingData.view.Value = value;
        viewDesc.SetText(((int)(value * 100)).ToString());
    }

    public void Graphic_Low_Select(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SettingData.GraphicOption.Value = 0;
            SoundManager.Instance.PlayButtonSound();
        }
    }

    public void Graphic_Middle_Select(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SettingData.GraphicOption.Value = 1;
            SoundManager.Instance.PlayButtonSound();
        }
    }

    public void Graphic_High_Select(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
            SettingData.GraphicOption.Value = 2;
        }
    }

    public void Graphic_Very_High_Select(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
            SettingData.GraphicOption.Value = 3;
        }
    }

    public void FrameRate_Low_Select(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
            SettingData.FrameRateOption.Value = 0;
        }
    }

    public void FrameRate_Middle_Select(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
            SettingData.FrameRateOption.Value = 1;
        }
    }

    public void FrameRate_High_Select(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
            SettingData.FrameRateOption.Value = 2;
        }
    }

    public void ShowDamageFont(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.ShowDamageFont.Value = on ? 1 : 0;
    }

    public void ShowEffect(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.ShowEffect.Value = on ? 1 : 0;
    }

    public void ShakeCamera(bool on)
    {
        if (initialized == false) return;

        if (on)
        {
            SoundManager.Instance.PlayButtonSound();
        }

        SettingData.GlowEffect.Value = on ? 1 : 0;
    }

    public void OnClickStory()
    {
        DialogManager.Instance.StartDialog();
    }

    public void OnClickCafeButton()
    {
        Application.OpenURL("https://cafe.naver.com/madaki");
    }

    private float AutoSaveDelay = 3600f;
    private Coroutine autoSaveRoutine = null;
    private IEnumerator AutoSaveRoutine()
    {
        yield return new WaitForSeconds(AutoSaveDelay);
        autoSaveRoutine = null;
    }

    public void OnClickForceSaveButton()
    {
        if (autoSaveRoutine != null)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "강제 저장은 1분에 한번만 가능합니다.", null);
            return;
        }

        SaveManager.Instance.SyncDatasInQueue();

        autoSaveRoutine = CoroutineExecuter.Instance.StartCoroutine(AutoSaveRoutine());
    }

    private void OnDestroy()
    {
        if (CoroutineExecuter.Instance != null && autoSaveRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(autoSaveRoutine);
        }
    }
}

public static class SettingKey
{
    public static string bgmVolume = "bgmVolume";
    public static string efxVolume = "efxVolume";
    public static string view = "view";
    public static string GraphicOption = "GraphicOption";
    public static string FrameRateOption = "FrameRateOption";
    public static string ShowDamageFont = "ShowDamageFont";
    public static string ShowEffect = "ShowEffect";
    public static string GlowEffect = "GlowEffect"; 
    public static string PotionUseHpOption = "PotionUseHpOption";
}

public static class SettingData
{
    //볼룸
    public static ReactiveProperty<float> bgmVolume = new ReactiveProperty<float>();
    public static ReactiveProperty<float> efxVolume = new ReactiveProperty<float>();
    public static ReactiveProperty<float> view = new ReactiveProperty<float>();
    public static ReactiveProperty<int> GraphicOption = new ReactiveProperty<int>(); //하중상최상
    public static ReactiveProperty<int> FrameRateOption = new ReactiveProperty<int>(); //30 45 60
    public static ReactiveProperty<int> ShowDamageFont = new ReactiveProperty<int>();
    public static ReactiveProperty<int> ShowEffect = new ReactiveProperty<int>();
    public static ReactiveProperty<int> GlowEffect = new ReactiveProperty<int>();
    public static ReactiveProperty<int> PotionUseHpOption = new ReactiveProperty<int>();//x이하일떄 (3개옵션)

    public static int screenWidth = Screen.width;
    public static int screenHeight = Screen.height;

    public static void InitFirst()
    {
        FirstInit();
        Initialize();
    }
    static void FirstInit()
    {
        if (PlayerPrefs.HasKey(SettingKey.bgmVolume) == false)
            PlayerPrefs.SetFloat(SettingKey.bgmVolume, 0.5f);

        if (PlayerPrefs.HasKey(SettingKey.efxVolume) == false)
            PlayerPrefs.SetFloat(SettingKey.efxVolume, 0.5f);

        if (PlayerPrefs.HasKey(SettingKey.view) == false)
            PlayerPrefs.SetFloat(SettingKey.view, 1f);

        if (PlayerPrefs.HasKey(SettingKey.GraphicOption) == false)
            PlayerPrefs.SetInt(SettingKey.GraphicOption, 2);

        if (PlayerPrefs.HasKey(SettingKey.FrameRateOption) == false)
            PlayerPrefs.SetInt(SettingKey.FrameRateOption, 2);

        if (PlayerPrefs.HasKey(SettingKey.ShowDamageFont) == false)
            PlayerPrefs.SetInt(SettingKey.ShowDamageFont, 1);

        if (PlayerPrefs.HasKey(SettingKey.ShowEffect) == false)
            PlayerPrefs.SetInt(SettingKey.ShowEffect, 1);

        if (PlayerPrefs.HasKey(SettingKey.GlowEffect) == false)
            PlayerPrefs.SetInt(SettingKey.GlowEffect, 1);

        if (PlayerPrefs.HasKey(SettingKey.PotionUseHpOption) == false)
            PlayerPrefs.SetInt(SettingKey.PotionUseHpOption, 1);
    }

    static void Initialize()
    {
        bgmVolume.Value = PlayerPrefs.GetFloat(SettingKey.bgmVolume, 1f);
        efxVolume.Value = PlayerPrefs.GetFloat(SettingKey.efxVolume, 1f);
        view.Value = PlayerPrefs.GetFloat(SettingKey.view, 0.5f);
        GraphicOption.Value = PlayerPrefs.GetInt(SettingKey.GraphicOption, 2);
        FrameRateOption.Value = PlayerPrefs.GetInt(SettingKey.FrameRateOption, 2);
        ShowDamageFont.Value = PlayerPrefs.GetInt(SettingKey.ShowDamageFont, 1);
        ShowEffect.Value = PlayerPrefs.GetInt(SettingKey.ShowEffect, 1);
        GlowEffect.Value = PlayerPrefs.GetInt(SettingKey.GlowEffect, 1);
        PotionUseHpOption.Value = PlayerPrefs.GetInt(SettingKey.PotionUseHpOption, 1);
        Subscribe();
    }

    static void Subscribe()
    {
        bgmVolume.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetFloat(SettingKey.bgmVolume, e);
        });

        efxVolume.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetFloat(SettingKey.efxVolume, e);
        });

        view.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetFloat(SettingKey.view, e);
        });

        GraphicOption.AsObservable().Subscribe(e => { PlayerPrefs.SetInt(SettingKey.GraphicOption, e); });
        FrameRateOption.AsObservable().Subscribe(e => { PlayerPrefs.SetInt(SettingKey.FrameRateOption, e); });

        ShowDamageFont.AsObservable().Subscribe(e => { PlayerPrefs.SetInt(SettingKey.ShowDamageFont, e); });
        ShowEffect.AsObservable().Subscribe(e => { PlayerPrefs.SetInt(SettingKey.ShowEffect, e); });
        GlowEffect.AsObservable().Subscribe(e => { PlayerPrefs.SetInt(SettingKey.GlowEffect, e); });
        PotionUseHpOption.AsObservable().Subscribe(e =>
        {
            Debug.LogError($"Potion optionChanged {e}");
            PlayerPrefs.SetInt(SettingKey.PotionUseHpOption, e);
        });

        GraphicOption.AsObservable().Subscribe(e =>
        {
            PlayerPrefs.SetInt(SettingKey.GraphicOption, e);
            SetGraphicOption(e);
        });
    }

    public static void SetGraphicOption(int option)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (option == 0)
            {
                Screen.SetResolution(640, 640 * screenHeight / screenWidth, true);
            }
            else if (option == 1)
            {
                Screen.SetResolution(1280, 1280 * screenHeight / screenWidth, true);
            }
            else if (option == 2)
            {
                Screen.SetResolution(1500, 1500 * screenHeight / screenWidth, true);
            }
            else
            {
                Screen.SetResolution(screenWidth, screenHeight, true);
            }
        }
    }
}


