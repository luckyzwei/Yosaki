using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using static GameManager;
using UnityEngine.UI;

public class UiSonExtraLevelDescription : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI description;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.TableDatas[UserInfoTable.sonCloneClear].AsObservable().Subscribe(e =>
        {
            int addAmount = (int)(e * GameBalance.sonCloneAddValue);

            description.SetText($"손오공분신 + {addAmount}개 추가 적용됨");


        }).AddTo(this);

    }

}
