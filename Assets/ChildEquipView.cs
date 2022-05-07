using BackEnd;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ChildEquipView : MonoBehaviour
{
    public void OnClickGetButton_Norigae()
    {
        if (ServerData.userInfoTable.TableDatas[UserInfoTable.killCountTotalChild].Value < 10000000f)
        {
            PopupManager.Instance.ShowAlarmMessage($"처치 1000만 이상일때 획득 가능 합니다!");
            return;
        }
        if (ServerData.magicBookTable.TableDatas["magicBook23"].hasItem.Value == 1)
        {
            PopupManager.Instance.ShowAlarmMessage($"이미 보유하고 있습니다.");
            return;
        }

        List<TransactionValue> transactions = new List<TransactionValue>();

        ServerData.magicBookTable.TableDatas["magicBook23"].amount.Value += 1;
        ServerData.magicBookTable.TableDatas["magicBook23"].hasItem.Value = 1;

        Param magicBookParam = new Param();

        magicBookParam.Add("magicBook23", ServerData.magicBookTable.TableDatas["magicBook23"].ConvertToString());

        transactions.Add(TransactionValue.SetUpdate(MagicBookTable.tableName, MagicBookTable.Indate, magicBookParam));


        ServerData.SendTransaction(transactions, successCallBack: () =>
        {
            SoundManager.Instance.PlaySound("Reward");
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "병아리 노리개 획득!!", null);
            // LogManager.Instance.SendLog("신수제작", $"신수제작 성공 {needPetId}");
        });
    }




}
