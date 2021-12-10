using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Globalization;

public static class Utils
{
    public static bool IsSet(this TutorialStep self, TutorialStep flag)
    {
        return (self & flag) == flag;
    }
    public static bool IsSet(this ManagerDescriptionType self, ManagerDescriptionType flag)
    {
        return (self & flag) == flag;
    }

    public static bool IsCostumeItem(this Item_Type type)
    {
        return type == Item_Type.costume0 ||
                  type == Item_Type.costume1 ||
                  type == Item_Type.costume2 ||
                  type == Item_Type.costume3 ||
                  type == Item_Type.costume4 ||
                  type == Item_Type.costume5 ||
                  type == Item_Type.costume6 ||
                  type == Item_Type.costume7 ||
                  type == Item_Type.costume8 ||
                  type == Item_Type.costume9 ||
                  type == Item_Type.costume10 ||
                  type == Item_Type.costume11 ||
                  type == Item_Type.costume12||
                  type == Item_Type.costume13||
                  type == Item_Type.costume14||
                  type == Item_Type.costume15;
    }
    public static bool IsPetItem(this Item_Type type)
    {
        return type == Item_Type.pet0 ||
                  type == Item_Type.pet1 ||
                  type == Item_Type.pet2 ||
                  type == Item_Type.pet3;
    }
    public static bool IsGoodsItem(this Item_Type type)
    {
        return type == Item_Type.Gold ||
                type == Item_Type.Jade ||
                type == Item_Type.GrowthStone ||
                type == Item_Type.Marble ||
                type == Item_Type.Ticket ||
                type == Item_Type.Songpyeon ||
                type == Item_Type.Event_Item_0 ||
                type == Item_Type.RelicTicket;
    }

    public static bool IsStatusItem(this Item_Type type)
    {
        return type == Item_Type.Memory;
    }

    public static bool IsWeaponItem(this Item_Type type)
    {
        return type >= Item_Type.weapon0 && type <= Item_Type.weapon16;
    }

    public static bool IsNorigaeItem(this Item_Type type)
    {
        return type >= Item_Type.magicBook0 && type <= Item_Type.magicBook11;
    }

    public static bool IsSkillItem(this Item_Type type)
    {
        return type >= Item_Type.skill0 && type <= Item_Type.skill8;
    }

    public static bool IsPercentStat(this StatusType type)
    {
        return
            type != StatusType.MoveSpeed &&
            type != StatusType.DamBalance &&
            type != StatusType.AttackAdd &&
            type != StatusType.Hp &&
            type != StatusType.Mp &&
            type != StatusType.IgnoreDefense &&
            type != StatusType.DashCount &&
            type != StatusType.SkillAttackCount;
    }

    public static bool IsRankFrameItem(this Item_Type type)
    {
        return type >= Item_Type.RankFrame1 && type <= Item_Type.RankFrame1001_10000;
    }

    public static bool IsRelicRewardItem(this Item_Type type)
    {
        return type >= Item_Type.RankFrame1_relic && type <= Item_Type.RankFrame1001_10000_relic;
    }

    public static bool IsMiniGameRewardItem(this Item_Type type)
    {
        return type >= Item_Type.RankFrame1_miniGame && type <= Item_Type.RankFrame1001_10000_miniGame;
    }

    public static int GetRandomIdx(List<float> inputDatas)
    {
        float total = 0;

        for (int i = 0; i < inputDatas.Count; i++)
        {
            total += inputDatas[i];
        }

        float pivot = UnityEngine.Random.Range(0f, 1f) * total;

        for (int i = 0; i < inputDatas.Count; i++)
        {
            if (pivot < inputDatas[i])
            {
                return i;
            }
            else
            {
                pivot -= inputDatas[i];
            }
        }
        return 0;
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static Rect GetWorldBounds(this BoxCollider2D boxCollider2D)
    {
        float worldRight = boxCollider2D.transform.TransformPoint(boxCollider2D.offset + new Vector2(boxCollider2D.size.x * 0.5f, 0)).x;
        float worldLeft = boxCollider2D.transform.TransformPoint(boxCollider2D.offset - new Vector2(boxCollider2D.size.x * 0.5f, 0)).x;

        float worldTop = boxCollider2D.transform.TransformPoint(boxCollider2D.offset + new Vector2(0, boxCollider2D.size.y * 0.5f)).y;
        float worldBottom = boxCollider2D.transform.TransformPoint(boxCollider2D.offset - new Vector2(0, boxCollider2D.size.y * 0.5f)).y;

        return new Rect(
            worldLeft,
            worldBottom,
            worldRight - worldLeft,
            worldTop - worldBottom
            );
    }

    public static string ListToString(List<string> list)
    {
        return String.Join(", ", list.ToArray());
    }
    public static List<string> StringToList(string data)
    {
        return data.Split(',').ToList();
    }

    public static DateTime ConvertFromUnixTimestamp(double timestamp)
    {
        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        return origin.AddSeconds(timestamp + 1620000000f);
    }

    public static double ConvertToUnixTimestamp(DateTime date)
    {
        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        TimeSpan diff = date.ToUniversalTime() - origin;
        return diff.TotalSeconds - 1620000000f;
    }

    public static int GetWeekNumber(DateTime currentDate)
    {
        DateTime startDate = new DateTime(2021, 1, 1); //기준일

        Calendar calenderCalc = CultureInfo.CurrentCulture.Calendar;

        return calenderCalc.GetWeekOfYear(currentDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday) - calenderCalc.GetWeekOfYear(startDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
    }

    #region BigFloat
    private static string[] goldUnitArr = new string[] { "", "만", "억", "조", "경", "해", "자", "양", "구", "간","정","재","극" };
    private static int p = (int)Mathf.Pow(10, 4);
    private static List<int> numList = new List<int>();
    private static List<string> numStringList = new List<string>();
    private static string zeroString = "0";
    public static string ConvertBigNum(float data)
    {
#if UNITY_EDITOR
        bool isUnderZero = data < 0;
        if (data < 0)
        {
            data *= -1f;
        }
#endif

        if (data == 0f)
        {
            return zeroString;
        }

        System.Numerics.BigInteger value = (System.Numerics.BigInteger)data;

        numList.Clear();
        numStringList.Clear();

        do
        {
            numList.Add((int)(value % p));
            value /= p;
        }
        while (value >= 1);

        string retStr = "";

        if (numList.Count >= 3)
        {
            for (int i = numList.Count - 1; i >= numList.Count - 2; i--)
            {
                if (numList[i] == 0) continue;

                numStringList.Add(numList[i] + goldUnitArr[i]);
            }

            for (int i = 0; i < numStringList.Count; i++)
            {
                retStr += numStringList[i];
            }
#if UNITY_EDITOR
            if (isUnderZero)
            {
                return "-" + retStr;
            }
#endif
            return retStr;
        }
        else
        {
            for (int i = 0; i < numList.Count; i++)
            {
                if (numList[i] == 0) continue;
                retStr = numList[i] + goldUnitArr[i] + retStr;
            }
#if UNITY_EDITOR
            if (isUnderZero)
            {
                return "-" + retStr;
            }
#endif
            return retStr;
        }

    }
    #endregion

    public static void RestartApplication()
    {
#if UNITY_IOS
        PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "네트워크 연결이 끊겼습니다.\n앱을 종료합니다.",confirmCallBack:()=>
        {
            Application.Quit();
        });

        return;
#endif
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

    public static bool HasBadWord(string text)
    {
        var tableData = TableManager.Instance.BadWord.dataArray;

        for (int i = 0; i < tableData.Length; i++)
        {
            string compareValue = tableData[i].Text;
            if (text.IndexOf(compareValue, System.StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                return true;
            }
        }

        return false;
    }


}
