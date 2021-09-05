using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiCostumeAbilityBoard : SingletonMono<UiCostumeAbilityBoard>
{
    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private UiCostumeAbilityCell costumeAbilityCellPrefab;

    [SerializeField]
    private Transform cellParent;

    private List<UiCostumeAbilityCell> abilityCells = new List<UiCostumeAbilityCell>();

    private CostumeData costumeData;

    [SerializeField]
    private GameObject notExistText;

    [SerializeField]
    private GameObject abilityGachaButton;

    [SerializeField]
    private GameObject costumeEquipButton;

    private CompositeDisposable disposable = new CompositeDisposable();

    private CostumeServerData CurrentServerData => ServerData.costumeServerTable.TableDatas[costumeData.Stringid];

    [SerializeField]
    private TextMeshProUGUI priceText;

    [SerializeField]
    private ContinueOpenButton continueOpenButton;

    [SerializeField]
    private TextMeshProUGUI currentSelectedSlotDesc;

    private void SetCurrentSelectedSlotDesc()
    {
        currentSelectedSlotDesc.SetText($"{this.costumeData.Id}번 슬롯");
    }

    private ObscuredInt price;

    private bool showYellowPop = false;
    public void WhenYellowAbilPopupChanged(bool ison)
    {
        showYellowPop = ison;
    }


    #region Security
    private void OnEnable()
    {
        continueOpenButton.canExecute = true;
        StartCoroutine(RandomizeRoutine());
    }

    private IEnumerator RandomizeRoutine()
    {
        var delay = new WaitForSeconds(1.0f);

        while (true)
        {
            RandomizeKey();
            yield return delay;
        }
    }

    private void OnApplicationPause(bool pause)
    {
        RandomizeKey();
    }

    private void RandomizeKey()
    {
        price.RandomizeCryptoKey();
    }
    #endregion

    private void UpdatePrice()
    {
        var serverData = CurrentServerData;

        int lockIdxCount = 0;

        for (int i = 0; i < serverData.lockIdx.Count; i++)
        {
            if (serverData.lockIdx[i].Value == 1) lockIdxCount++;
        }

        price = 200 + lockIdxCount * 200;
        priceText.SetText(price.ToString());
    }

    private new void OnDestroy()
    {
        base.OnDestroy();
        disposable.Dispose();
    }

    public void Initialize(CostumeData costumeData)
    {
        this.costumeData = costumeData;

        RefreshAllData();
    }

    public void RefreshAllData() 
    {
        SetCurrentSelectedSlotDesc();

        nameText.SetText(costumeData.Name);

        MakeAbilityCell();

        SetExist();

        Subscribe();

        RefreshUi();
    }


    private void OnCostumeLocked()
    {
        UpdatePrice();
    }

    private void Start()
    {
    }

    private void Subscribe()
    {
        disposable.Clear();

        CurrentServerData.hasCostume.AsObservable().Subscribe(WhenCostumeHasValueChanged).AddTo(disposable);

        UpdatePrice();
    }

    private void WhenCostumeHasValueChanged(bool hasCostume)
    {
        abilityGachaButton.SetActive(hasCostume);
        // costumeEquipButton.SetActive(hasCostume);
    }


    public void OnClickEquipButton()
    {
        if (CurrentServerData.hasCostume.Value == false)
        {
            PopupManager.Instance.ShowAlarmMessage("외형이 없습니다.");
            return;
        }

        if (ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeSlot].Value == costumeData.Id)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 장착중입니다.");
            return;
        }

        ServerData.equipmentTable.TableDatas[EquipmentTable.CostumeSlot].Value = costumeData.Id;

        ServerData.equipmentTable.SyncData(EquipmentTable.CostumeSlot);
    }

    public void OnClickGachaButton()
    {
        if (CanGacha() == false)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.Jade)}이 부족합니다.");
            return;
        }

        if (CurrentServerData.hasCostume.Value == false)
        {
            PopupManager.Instance.ShowAlarmMessage("외형이 없습니다.");
            return;
        }

        //최고 옵션 있을때 처리
        bool hasMaxOption = false;
        bool hasYellowOption = false;
        int lockAbilityCount = 0;

        for (int i = 0; i < CurrentServerData.abilityIdx.Count; i++)
        {
            bool isLock = CurrentServerData.lockIdx[i].Value == 1;

            if (isLock)
            {
                lockAbilityCount++;
            }

            var abilityData = TableManager.Instance.CostumeAbilityData[CurrentServerData.abilityIdx[i].Value];

            if (isLock == false && abilityData.Grade == GameBalance.costumeMaxGrade)
            {
                hasMaxOption = true;
                break;
            }

            if (isLock == false && abilityData.Grade == GameBalance.costumeMaxGrade - 1 && showYellowPop)
            {
                hasYellowOption = true;
                break;
            }
        }

        //전부잠겼을떄 처리
        if (lockAbilityCount == CurrentServerData.abilityIdx.Count)
        {
            PopupManager.Instance.ShowAlarmMessage("모든 능력치가 잠겨있습니다.");
            return;
        }

        if (hasMaxOption)
        {
            continueOpenButton.canExecute = false;

            PopupManager.Instance.ShowYesNoPopup("알림", "최고등급 옵션이 있습니다. 그래도 다시 뽑습니까?", GachaSLots,
                () =>
                {
                    continueOpenButton.canExecute = true;
                });
        }
        else if (hasYellowOption && showYellowPop)
        {
            continueOpenButton.canExecute = false;

            PopupManager.Instance.ShowYesNoPopup("알림", "노란색등급 옵션이 있습니다. 그래도 다시 뽑습니까?", GachaSLots,
                () =>
                {
                    continueOpenButton.canExecute = true;
                });
        }
        else
        {
            GachaSLots();
        }
    }

    public void OnClickGetButton_Test()
    {
        if (CurrentServerData.hasCostume.Value == true)
        {
            PopupManager.Instance.ShowAlarmMessage("이미 갖고 있습니다.");
            return;
        }

        CurrentServerData.hasCostume.Value = true;
        ServerData.costumeServerTable.SyncCostumeData(costumeData.Stringid);

    }

    private void GachaSLots()
    {
        SoundManager.Instance.PlayButtonSound();

        continueOpenButton.canExecute = true;

        var costumeAbilities = TableManager.Instance.GetRandomCostumeOptions(costumeData.Slotnum);

        for (int i = 0; i < CurrentServerData.abilityIdx.Count; i++)
        {
            if (CurrentServerData.lockIdx[i].Value == 1) continue;
            CurrentServerData.abilityIdx[i].Value = costumeAbilities[i];
        }
        //
        ServerData.costumePreset.TableDatas[ServerData.equipmentTable.GetCurrentCostumePresetKey()] =ServerData.costumeServerTable.ConvertAllCostumeDataToString();
        //

        //재화 차감
        ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value -= price;


        if (syncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(syncRoutine);
        }

        syncRoutine = CoroutineExecuter.Instance.StartCoroutine(SyncServerData());

        //UI 업데이트
        RefreshUi();
    }

    private Coroutine syncRoutine;

    private IEnumerator SyncServerData()
    {
        yield return new WaitForSeconds(0.5f);

        List<TransactionValue> transactionList = new List<TransactionValue>();

        //서버저장
        //ServerData.costumeServerTable.SyncCostumeData(key);

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.Jade, ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value);
        transactionList.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));

        Param presetParam = new Param();

        string presetKey =ServerData.equipmentTable.GetCurrentCostumePresetKey();

        presetParam.Add(presetKey, ServerData.costumeServerTable.ConvertAllCostumeDataToString());

        transactionList.Add(TransactionValue.SetUpdate(CostumePreset.tableName, CostumePreset.Indate, presetParam));

        ServerData.SendTransaction(transactionList);
    }

    private bool CanGacha()
    {
        int currentBlueStoneNum = (int)ServerData.goodsTable.GetTableData(GoodsTable.Jade).Value;

        return currentBlueStoneNum >= price;
    }

    private void SetExist()
    {
        notExistText.SetActive(!CurrentServerData.hasCostume.Value);
    }

    private void MakeAbilityCell()
    {
        int slotNum = costumeData.Slotnum;
        int interval = slotNum - abilityCells.Count;

        for (int i = 0; i < interval; i++)
        {
            var cell = Instantiate<UiCostumeAbilityCell>(costumeAbilityCellPrefab, cellParent);
            abilityCells.Add(cell);
        }

        for (int i = 0; i < abilityCells.Count; i++)
        {
            if (i < slotNum)
            {
                abilityCells[i].gameObject.SetActive(true);
            }
            else
            {
                abilityCells[i].gameObject.SetActive(false);
            }
        }
    }

    public void RefreshUi()
    {
        var costumeServerData = ServerData.costumeServerTable.TableDatas[costumeData.Stringid];

        for (int i = 0; i < costumeServerData.abilityIdx.Count; i++)
        {
            abilityCells[i].Initialize(costumeData, costumeServerData.abilityIdx[i].Value, i, OnCostumeLocked);
        }
    }
}
