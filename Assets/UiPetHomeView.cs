using BackEnd;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiPetHomeView : MonoBehaviour
{
    [SerializeField]
    private SkeletonGraphic skeletonGraphic;

    [SerializeField]
    private TextMeshProUGUI petName;

    [SerializeField]
    private TextMeshProUGUI hasDescription;

    private PetTableData petData;

    private PetServerData petServerData;

    [SerializeField]
    private GameObject notHasObject;

    [SerializeField]
    private Image rewardIcon;

    [SerializeField]
    private TextMeshProUGUI rewardValue;

    [SerializeField]
    private Button rewardButton;

    [SerializeField]
    private TextMeshProUGUI rewardDescription;


    public void Initialize(PetTableData petData)
    {
        SetPetSpine(petData.Id);

        this.petData = petData;

        this.petServerData = ServerData.petTable.TableDatas[petData.Stringid];

        petName.SetText($"{petData.Name}");

        rewardIcon.sprite = CommonUiContainer.Instance.GetItemIcon((Item_Type)petData.Getrewardtype);

        rewardValue.SetText(Utils.ConvertBigNum(petData.Getrewardvalue));

        Subscribe();
    }

    private void Subscribe()
    {
        petServerData.hasItem.AsObservable().Subscribe(e =>
        {
            notHasObject.SetActive(e == 0);

            if (e == 0)
            {
                hasDescription.SetText($"<color=yellow>미보유</color>");
            }
            else
            {
                hasDescription.SetText($"<color=yellow>보유중</color>");
            }
        }).AddTo(this);

        ServerData.etcServerTable.TableDatas[EtcServerTable.PetHomeReward].AsObservable().Subscribe(e =>
        {

            bool hasReward = ServerData.etcServerTable.HasPetHomeReward(petData.Id);

            rewardButton.interactable = !hasReward;

            rewardDescription.SetText(!hasReward ? "보상수령" : "수령완료");

        }).AddTo(this);

    }

    private void SetPetSpine(int idx)
    {

        skeletonGraphic.Clear();
        skeletonGraphic.skeletonDataAsset = CommonUiContainer.Instance.petCostumeList[idx];

        if (idx != 15)
        {
            skeletonGraphic.startingAnimation = "walk";
        }
        else
        {
            skeletonGraphic.startingAnimation = "idel";
        }

        skeletonGraphic.Initialize(true);
        skeletonGraphic.SetMaterialDirty();

        if (idx >= 15)
        {
            skeletonGraphic.transform.localScale = new Vector3(0.4f, 0.4f, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(-8f, -86.5f, 1f);
        }

        if (idx >= 24 && idx <= 27)
        {
            skeletonGraphic.transform.localScale = new Vector3(1f, 1f, 1f);
            skeletonGraphic.transform.localPosition = new Vector3(-8f, -145f, 1f);
        }
    }

    public void OnClickGetRewardButton()
    {
        if (ServerData.etcServerTable.HasPetHomeReward(petData.Id))
        {
            PopupManager.Instance.ShowAlarmMessage("이미 보상을 받았습니다!");
            return;
        }

        List<TransactionValue> transactions = new List<TransactionValue>();

        Item_Type rewardType = (Item_Type)petData.Getrewardtype;

        float rewardValue = petData.Getrewardvalue;

        transactions.Add(ServerData.GetItemTypeTransactionValueForAttendance(rewardType, rewardValue));

        ServerData.etcServerTable.TableDatas[EtcServerTable.PetHomeReward].Value += $"{BossServerTable.rewardSplit}{petData.Id}";

        Param etcParam = new Param();
        etcParam.Add(EtcServerTable.PetHomeReward, ServerData.etcServerTable.TableDatas[EtcServerTable.PetHomeReward].Value);

        transactions.Add(TransactionValue.SetUpdate(EtcServerTable.tableName, EtcServerTable.Indate, etcParam));

        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            PopupManager.Instance.ShowAlarmMessage("보상을 획득했습니다!");
        });

    }
}
