using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiGachaResultView : SingletonMono<UiGachaResultView>
{
    public class GachaResultCellInfo
    {
        public WeaponData weaponData;
        public MagicBookData magicBookData;
        public SkillTableData skillData;
        public int amount;
    }

    [SerializeField]
    private GameObject rootObject;

    [SerializeField]
    private GachaResultViewCell GachaResultViewCellPrefab;

    [SerializeField]
    private Transform cellParent;

    private List<GachaResultViewCell> GachaResultViewCellContainer = new List<GachaResultViewCell>();

    [SerializeField]
    private GameObject closeButton;

    [SerializeField]
    private GameObject retryButton;

    private Action retryCallback;

    private Coroutine directionRoutine;

    private List<GachaResultCellInfo> results;

    [HideInInspector]
    public bool isAuto = false;

    [SerializeField]
    private Button gachaButton;

    [SerializeField]
    private Button closeButon;

    public Toggle autoToggle;

    private WaitForSeconds autoDelay = new WaitForSeconds(0.35f);

    private void Start()
    {
        SetWhiteEffectToggle();
    }

    private void SetWhiteEffectToggle()
    {
        whiteEffectToggle.isOn = SettingData.GachaWhiteEffect.Value == 1 ? true : false;
    }

    private void OnEnable()
    {
        autoToggle.isOn = false;
        OnAutoStateChanged(false);
    }

    public void OnAutoStateChanged(bool auto)
    {
        isAuto = auto;

        if (auto)
        {
            gachaButton.interactable = false;
            closeButon.interactable = false;

            if (directionRoutine == null)
            {
                retryCallback?.Invoke();
            }
        }
        else
        {
            gachaButton.interactable = true;
            closeButon.interactable = true;
        }
    }

    private enum State
    {
        playing, end
    }

    private State state;

    public void Initialize(List<GachaResultCellInfo> results, Action retryCallback)
    {
        state = State.playing;

        this.retryCallback = retryCallback;

        closeButton.SetActive(false);

        retryButton.SetActive(false);

        GachaResultViewCellContainer.ForEach(e => e.gameObject.SetActive(false));


        rootObject.SetActive(true);

        int interval = results.Count - GachaResultViewCellContainer.Count;

        for (int i = 0; i < interval; i++)
        {
            var cell = Instantiate<GachaResultViewCell>(GachaResultViewCellPrefab, cellParent);
            GachaResultViewCellContainer.Add(cell);
        }

        this.results = results;

        directionRoutine = StartCoroutine(ActiveRoutine());

    }

    private string GachaCompleteKey = "GachaComplete";
    private IEnumerator ActiveRoutine()
    {
        SoundManager.Instance.PlaySound(GachaCompleteKey);

        if (isAuto == false)
        {
            WaitForSeconds delay = new WaitForSeconds(0.01f);
            for (int i = 0; i < GachaResultViewCellContainer.Count; i++)
            {
                GachaResultViewCellContainer[i].gameObject.SetActive(i < results.Count);

                if (i < results.Count)
                {
                    GachaResultViewCellContainer[i].Initialzie(results[i].weaponData, results[i].magicBookData, results[i].skillData, results[i].amount);
                }

                if (isAuto == false)
                {
                    yield return delay;
                }
            }

        }

        if (isAuto)
        {
            SkipDirection();
            yield return autoDelay;
            retryCallback?.Invoke();
            yield break;
        }
        else
        {
            WhenDirectionEnd();
        }
    }

    private void WhenDirectionEnd()
    {

        directionRoutine = null;

        closeButton.SetActive(true);

        retryButton.SetActive(true);

        state = State.end;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && isAuto == false)
        {
            if (state == State.playing)
            {
                SkipDirection();
            }
        }
    }

    private void SkipDirection()
    {
        for (int i = 0; i < GachaResultViewCellContainer.Count; i++)
        {
            GachaResultViewCellContainer[i].gameObject.SetActive(i < results.Count);

            if (i < results.Count)
            {
                GachaResultViewCellContainer[i].Initialzie(results[i].weaponData, results[i].magicBookData, results[i].skillData, results[i].amount);
            }
        }

        if (directionRoutine != null)
        {
            StopCoroutine(directionRoutine);
        }

        WhenDirectionEnd();
    }

    public void OnClickRetryButton()
    {
        retryCallback?.Invoke();
    }

    [SerializeField]
    private Toggle whiteEffectToggle;

    public static string whiteEffectKey = "whiteEffectKey";
    public void OnGachaWhiteEffectStateChanged(bool state)
    {
        SettingData.GachaWhiteEffect.Value = state == true ? 1 : 0;
    }

}
