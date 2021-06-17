using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UiMagicBookCollectCell : MonoBehaviour
{
    [SerializeField]
    private WeaponView weaponView;

    private MagicBookData magicBookData;

    private MagicBookServerData magicBookServerData;

    [SerializeField]
    private Image upgradeButton;

    [SerializeField]
    private Sprite normalSprite;

    [SerializeField]
    private Sprite maxLevelSprite;

    [SerializeField]
    private TextMeshProUGUI levelText;

    [SerializeField]
    private TextMeshProUGUI abilName;

    [SerializeField]
    private TextMeshProUGUI abilDescription;

    [SerializeField]
    private TextMeshProUGUI buttonText;


    public void Initialize(MagicBookData magicBookData)
    {
        this.magicBookData = magicBookData;
        this.magicBookServerData = DatabaseManager.magicBookTable.TableDatas[magicBookData.Stringid];

        weaponView.Initialize(null, this.magicBookData);

        Subscribe();
    }

    private void Subscribe()
    {
        magicBookServerData.collectLevel.AsObservable().Subscribe(WhenCollectionLevelChanged).AddTo(this);
    }

    private void WhenCollectionLevelChanged(int level)
    {
        if (level >= magicBookData.Collectionabiltmaxlevel)
        {
            upgradeButton.sprite = maxLevelSprite;

            levelText.SetText($"LV : {level}(MAX)");
            buttonText.SetText("최고레벨");

            float collectionAbilValue = magicBookData.Collectionvalue * magicBookServerData.collectLevel.Value;

            StatusType statusType = (StatusType)magicBookData.Collectionabiltype;
            abilName.SetText(CommonString.GetStatusName(statusType));
            if (statusType.IsPercentStat() == false)
            {
                abilDescription.SetText($"+{collectionAbilValue}");
            }
            else
            {
                abilDescription.SetText($"+{(collectionAbilValue * 100f).ToString("F2")}%");
            }
        }
        else
        {
            upgradeButton.sprite = normalSprite;

            levelText.SetText($"LV : {level}");
            buttonText.SetText("레벨업");

            float collectionAbilValue = magicBookData.Collectionvalue * magicBookServerData.collectLevel.Value;
            float collectionAbilNextValue = magicBookData.Collectionvalue * (magicBookServerData.collectLevel.Value + 1);

            StatusType statusType = (StatusType)magicBookData.Collectionabiltype;
            abilName.SetText(CommonString.GetStatusName(statusType));
            if (statusType.IsPercentStat() == false)
            {
                abilDescription.SetText($"+{collectionAbilValue}->{collectionAbilNextValue}");
            }
            else
            {
                abilDescription.SetText($"+{(collectionAbilValue * 100f).ToString("F2")}%->+{(collectionAbilNextValue * 100f).ToString("F2")}%");
            }
        }


    }

    public void OnClickUpgradeButton()
    {
        if (magicBookServerData.collectLevel.Value >= this.magicBookData.Collectionabiltmaxlevel)
        {
            PopupManager.Instance.ShowAlarmMessage("최대레벨 입니다.");
            return;
        }

        if (magicBookServerData.amount.Value <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage("마도서가 부족 합니다.");
            return;
        }

        SoundManager.Instance.PlayButtonSound();

        magicBookServerData.amount.Value--;
        magicBookServerData.collectLevel.Value++;

        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncRoutine());
    }

    private Coroutine syncRoutine;
    private WaitForSeconds syncDelay = new WaitForSeconds(2.0f);

    private IEnumerator SyncRoutine()
    {
        yield return syncDelay;
        DatabaseManager.magicBookTable.SyncToServerEach(magicBookData.Stringid);
        syncRoutine = null;
    }
}
