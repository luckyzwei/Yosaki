using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiMagicbookCraftCell : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI description;

    private SkillTableData skillData;

    private bool subscribed = false;

    private Coroutine makeSyncRoutine;
    private Coroutine destroySyncRoutine;

    private readonly WaitForSeconds syncDelay = new WaitForSeconds(0.5f);

    [SerializeField]
    private WeaponView weaponView;

    public void Initialize(SkillTableData skillData)
    {
        this.skillData = skillData;

        weaponView.Initialize(null, null, skillData);

        description.SetText($"{CommonString.GetItemName(Item_Type.SkillPartion)}\n{skillData.Makerequirevalue}개");

        if (subscribed == false)
        {
            subscribed = true;
            Subscribe();
        }
    }

    private void Subscribe()
    {

    }

    public void OnClickMakeButton()
    {
        var currentPartionAmount = ServerData.goodsTable.GetTableData(GoodsTable.SkillPartion);

        if ((int)currentPartionAmount.Value < skillData.Makerequirevalue)
        {
            PopupManager.Instance.ShowAlarmMessage($"{CommonString.GetItemName(Item_Type.SkillPartion)}이 부족합니다.");
            return;
        }



        //로컬
        currentPartionAmount.Value -= skillData.Makerequirevalue;

        ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][this.skillData.Id].Value++;
        ServerData.skillServerTable.TableDatas[SkillServerTable.SkillAlreadyHas][this.skillData.Id].Value = 1;

        if (makeSyncRoutine != null)
        {
            CoroutineExecuter.Instance.StopCoroutine(makeSyncRoutine);
        }

        makeSyncRoutine = CoroutineExecuter.Instance.StartCoroutine(MakeSyncRoutine());
    }

    private IEnumerator MakeSyncRoutine()
    {
        yield return syncDelay;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.SkillPartion, ServerData.goodsTable.GetTableData(GoodsTable.SkillPartion).Value);

        Param skillParam = new Param();

        List<int> skillAmountSyncData = new List<int>();
        List<int> skillAlreadyHasSyncData = new List<int>();

        for (int i = 0; i < ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount].Count; i++)
        {
            skillAmountSyncData.Add(ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][i].Value);
            skillAlreadyHasSyncData.Add(ServerData.skillServerTable.TableDatas[SkillServerTable.SkillAlreadyHas][i].Value);
        }

        skillParam.Add(SkillServerTable.SkillHasAmount, skillAmountSyncData);
        skillParam.Add(SkillServerTable.SkillAlreadyHas, skillAlreadyHasSyncData);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(SkillServerTable.tableName, SkillServerTable.Indate, skillParam));

        ServerData.SendTransaction(transactions);
    }


    public void OnClickDestroyButton()
    {
        var currentPartionAmount = ServerData.goodsTable.GetTableData(GoodsTable.SkillPartion);

        var skillAmountData = ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][this.skillData.Id];

        if (skillAmountData.Value <= 0)
        {
            PopupManager.Instance.ShowAlarmMessage($"기술이 부족합니다.");
            return;
        }

        PopupManager.Instance.ShowYesNoPopup(CommonString.Notice, "기술을 전부 분해 합니까?", () =>
        {
            //로컬
            int destroyNum = skillAmountData.Value;
            int getNum = skillData.Destroyvalue * skillAmountData.Value;

            currentPartionAmount.Value += getNum;

            skillAmountData.Value = 0;

            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, $"기술{destroyNum}개 분해\n{CommonString.GetItemName(Item_Type.SkillPartion)} {getNum}개 획득!", null);

            if (destroySyncRoutine != null)
            {
                CoroutineExecuter.Instance.StopCoroutine(destroySyncRoutine);
            }

            destroySyncRoutine = CoroutineExecuter.Instance.StartCoroutine(DestroySyncRoutine());

            SoundManager.Instance.PlaySound("Reward");
        }, null);
    }

    private IEnumerator DestroySyncRoutine()
    {
        yield return syncDelay;

        List<TransactionValue> transactions = new List<TransactionValue>();

        Param goodsParam = new Param();
        goodsParam.Add(GoodsTable.SkillPartion, ServerData.goodsTable.GetTableData(GoodsTable.SkillPartion).Value);

        Param skillParam = new Param();

        List<int> skillAmountSyncData = new List<int>();
        List<int> skillAlreadyHasSyncData = new List<int>();

        for (int i = 0; i < ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount].Count; i++)
        {
            skillAmountSyncData.Add(ServerData.skillServerTable.TableDatas[SkillServerTable.SkillHasAmount][i].Value);
        }

        skillParam.Add(SkillServerTable.SkillHasAmount, skillAmountSyncData);

        transactions.Add(TransactionValue.SetUpdate(GoodsTable.tableName, GoodsTable.Indate, goodsParam));
        transactions.Add(TransactionValue.SetUpdate(SkillServerTable.tableName, SkillServerTable.Indate, skillParam));

        ServerData.SendTransaction(transactions);
    }

}
