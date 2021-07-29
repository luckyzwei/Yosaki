using System.Collections;
using UnityEngine;
using BackEnd;
using LitJson;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class UiBackEndNotice : MonoBehaviour
{
    [SerializeField]
    private Transform noticePop;

    [SerializeField]
    private TextMeshProUGUI titleTMP;

    [SerializeField]
    private TextMeshProUGUI descriptionTMP;

    private static bool isFirstAppStart = true;

    private void Start()
    {
        OnClickNoticeList();
    }

    public void OnClickNoticeList()
    {
        if (isFirstAppStart == false)
        {
            this.gameObject.SetActive(false);
            return;
        }

        BackendReturnObject BRO = Backend.Notice.NoticeList();

        if (BRO.IsSuccess())
        {
            // 전체 공지 리스트
            Debug.Log(BRO.GetReturnValue());


            // 전체 공지 중에 2번째 공지를 저장합니다.
            JsonData noticeData = BRO.GetReturnValuetoJSON()["rows"][0];

            var title = noticeData["title"][0].ToString();
            var contents = noticeData["content"][0].ToString();

            titleTMP.SetText(title);
            descriptionTMP.SetText(contents);

            isFirstAppStart = false;
        }

        else
        {
            Debug.Log("서버 공통 에러 발생: " + BRO.GetErrorCode());

            this.gameObject.SetActive(false);
        }
    }

    public void OnClickCloseButton()
    {
        this.gameObject.SetActive(false);
    }

    public void OnClickGoCafeButton() 
    {
        Application.OpenURL(CommonString.CafeURL);
    }

}
