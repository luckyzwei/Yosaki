using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : SingletonMono<SaveManager>
{
#if UNITY_EDITOR
    private WaitForSeconds updateDelay = new WaitForSeconds(10f);
#else
    private WaitForSeconds updateDelay = new WaitForSeconds(180.0f);
#endif

    private WaitForSeconds versionCheckDelay = new WaitForSeconds(300.0f);

    //12시간
    private WaitForSeconds tockenRefreshDelay = new WaitForSeconds(43200f);

    public void StartAutoSave()
    {
        StartCoroutine(AutoSaveRoutine());
        StartCoroutine(TockenRefreshRoutine());
        StartCoroutine(VersionCheckRoutine());
    }

    private IEnumerator VersionCheckRoutine()
    {
        while (true)
        {
            yield return versionCheckDelay;

            CheckClientVersion();
        }
    }

    private void CheckClientVersion()
    {
        SendQueue.Enqueue(Backend.Utils.GetLatestVersion, bro =>
        {
            if (bro.IsSuccess())
            {
                int clientVersion = int.Parse(Application.version);

                var jsonData = bro.GetReturnValuetoJSON();
                string serverVersion = jsonData["version"].ToString();

                //버전이 높거나 같음
                if (clientVersion >= int.Parse(serverVersion))
                {

                }
                else
                {
                    SyncDatasForce();

                    PopupManager.Instance.ShowVersionUpPopup(CommonString.Notice, "업데이트 버전이 있습니다. 스토어로 이동합니다.\n업데이트 버튼이 활성화 되지 않은 경우\n구글 플레이 스토어를 닫았다가 다시 열어 보세요!", () =>
                    {
                        Application.OpenURL("https://play.google.com/store/apps/details?id=com.DragonGames.yoyo&hl=ko");
                    }, false);
                }
            }
        });
    }

    private IEnumerator TockenRefreshRoutine()
    {
        while (true)
        {
            yield return tockenRefreshDelay;
            BackEnd.Backend.BMember.LoginWithTheBackendToken(e =>
            {
                if (e.IsSuccess())
                {
                    Debug.Log("토큰 갱신 성공");
                }
                else
                {
                    Debug.Log("토큰 갱신 실패");
                }
            });
        }
    }

    private IEnumerator AutoSaveRoutine()
    {
        while (true)
        {
            SyncDatasInQueue();
            yield return updateDelay;
        }
    }

    //SendQueue에서 저장
    public void SyncDatasInQueue()
    {
       
        ServerData.goodsTable.SyncAllData();

        ServerData.userInfoTable.AutoUpdateRoutine();

        ServerData.growthTable.UpData(GrowthTable.Exp, false);

        //CollectionManager.Instance.SyncToServer();

        if (BuffManager.Instance != null)
        {
            BuffManager.Instance.UpdateBuffTime();
            ServerData.buffServerTable.SyncAllData();
        }


    }
    private void OnApplicationQuit()
    {
        SyncDatasForce();
    }

    //동기로 저장
    public void SyncDatasForce()
    {
        ServerData.goodsTable.SyncAllDataForce();

        //CollectionManager.Instance.SyncToServerForce();

        ServerData.growthTable.SyncDataForce();

        if (BuffManager.Instance != null)
        {
            BuffManager.Instance.UpdateBuffTime();
            ServerData.buffServerTable.SyncAllDataForce();
        }
    }
}
