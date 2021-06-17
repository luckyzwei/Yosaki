using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Firebase SDK 참조
using Firebase;
using BackEnd;

public class PushManager : SingletonMono<PushManager>
{
    FirebaseApp app;
    private bool canInit = false;


    // Start is called before the first frame update
    public void Initialize()
    {
        // Firebase 클라우드 메시징 초기화
        // https://firebase.google.com/docs/cloud-messaging/unity/client?hl=ko#initialize
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
        Firebase.Messaging.FirebaseMessaging.TokenRegistrationOnInitEnabled = true;

        // GooglePlay 서비스 버전 요구사항 확인
        // https://firebase.google.com/docs/cloud-messaging/unity/client?hl=ko#confirm_google_play_version
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;

                UnityEngine.Debug.LogError("등록 성공");

                canInit = true;

                // Backend.Android.PutDeviceToken();
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));

                // Firebase Unity SDK is not safe to use here.
            }
        });
    }
#if !UNITY_EDITOR
    private void Update()
    {
        if (canInit == true) 
        {
            Backend.Android.PutDeviceToken(Backend.Android.GetDeviceToken(), (callback) =>
            {
                // 이후 처리
                if (callback.IsSuccess())
                {
                    Debug.LogError("뒤끝 등록 성공");
                }
                else
                {
                    Debug.LogError($"뒤끝 등록 실패 {callback.GetStatusCode()}");
                }
            });
            canInit = false;
        }
    }
#endif

    // 토큰을 수신하며 차후 토큰을 사용하도록 캐시한다.
    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
    }

    // 메시지를 수신한다.
    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        var notification = e.Message.Notification;

        UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
    }
}
