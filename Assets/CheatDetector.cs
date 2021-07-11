using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatDetector : SingletonMono<CheatDetector>
{
    public void WhenCheatDetected()
    {
        ServerData.userInfoTable.GetTableData(UserInfoTable.hackingCount).Value++;
        ServerData.userInfoTable.UpData(UserInfoTable.hackingCount, false);
        
        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "불법 프로그램이 감지되었습니다.\n 검토후 계정이 정지됩니다.\n 계속 사용시 민 형사상의 책임을 지게될 수 있습니다.", Restart);
    }

    public void Restart()
    {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaObject pm = currentActivity.Call<AndroidJavaObject>("getPackageManager");
            AndroidJavaObject intent = pm.Call<AndroidJavaObject>("getLaunchIntentForPackage", Application.identifier);
            intent.Call<AndroidJavaObject>("setFlags", 0x20000000);//Intent.FLAG_ACTIVITY_SINGLE_TOP

            AndroidJavaClass pendingIntent = new AndroidJavaClass("android.app.PendingIntent");
            AndroidJavaObject contentIntent = pendingIntent.CallStatic<AndroidJavaObject>("getActivity", currentActivity, 0, intent, 0x8000000); //PendingIntent.FLAG_UPDATE_CURRENT = 134217728 [0x8000000]
            AndroidJavaObject alarmManager = currentActivity.Call<AndroidJavaObject>("getSystemService", "alarm");
            AndroidJavaClass system = new AndroidJavaClass("java.lang.System");
            long currentTime = system.CallStatic<long>("currentTimeMillis");
            alarmManager.Call("set", 1, currentTime + 1000, contentIntent); // android.app.AlarmManager.RTC = 1 [0x1]

            Debug.LogError("alarm_manager set time " + currentTime + 1000);
            currentActivity.Call("finish");

            AndroidJavaClass process = new AndroidJavaClass("android.os.Process");
            int pid = process.CallStatic<int>("myPid");
            process.CallStatic("killProcess", pid);
        }
    }
}
