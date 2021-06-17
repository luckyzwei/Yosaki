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
                  type == Item_Type.costume3;
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
                type == Item_Type.BlueStone ||
                type == Item_Type.MagicStone ||
                type == Item_Type.Feather ||
                type == Item_Type.Ticket;
    }
    
    public static bool IsStatusItem(this Item_Type type)
    {
        return type == Item_Type.Memory;
    }

    public static bool IsPercentStat(this StatusType type)
    {
        return
            type != StatusType.MoveSpeed &&
            type != StatusType.DamBalance &&
            type != StatusType.IntAdd &&
            type != StatusType.Hp &&
            type != StatusType.Mp;
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
        return origin.AddSeconds(timestamp);
    }

    public static double ConvertToUnixTimestamp(DateTime date)
    {
        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        TimeSpan diff = date.ToUniversalTime() - origin;
        return Math.Floor(diff.TotalSeconds);
    }

    public static int GetWeekNumber(DateTime currentDate)
    {
        DateTime startDate = new DateTime(2021, 1, 1); //기준일

        Calendar calenderCalc = CultureInfo.CurrentCulture.Calendar;

        return calenderCalc.GetWeekOfYear(currentDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday) - calenderCalc.GetWeekOfYear(startDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
    }

    #region BigFloat
    static string[] arrDecimal = { "", "만", "억", "조", "경", "해", "자", "양" };
    static string zero = "0";
    static string sharp = "#";
    static string line = "-";
    public static string ConvertBigFloat(float data)
    {
        var displayNum = string.Empty;
        if (data == 0f)
        {
            return zero;
        }

        if (data < 0f)
        {
            data = -data;
            var goldDouble = (double)data;
            int stringLength;

            if (goldDouble.ToString(sharp).Length % 4 != 0)
            {
                stringLength = goldDouble.ToString(sharp).Length + 4 - goldDouble.ToString(sharp).Length % 4;
            }
            else
            {
                stringLength = goldDouble.ToString(sharp).Length;
            }

            var sNum = goldDouble.ToString(sharp).PadLeft(stringLength);
            var j = 0;
            for (var i = 0; i < sNum.Length >> 2 && j < 2; i++)
            {
                j++;
                var part = sNum.Substring(i << 2, 4);
                var stringFormat = int.Parse(part);
                if (stringFormat == 0)
                {
                    continue;
                }

                displayNum += stringFormat + arrDecimal[(sNum.Length >> 2) - i - 1];
                // if (sNum.Length >> 2 - 1 != i) displayNum += empty;
            }

            displayNum = line + displayNum.TrimEnd();
        }
        else
        {
            var goldDouble = (double)(decimal)data;
            int stringLength;

            if (goldDouble.ToString(sharp).Length % 4 != 0)
            {
                stringLength = goldDouble.ToString(sharp).Length + 4 - goldDouble.ToString(sharp).Length % 4;
            }
            else
            {
                stringLength = goldDouble.ToString(sharp).Length;
            }

            var sNum = goldDouble.ToString(sharp).PadLeft(stringLength);
            var j = 0;
            for (var i = 0; i < sNum.Length >> 2 && j < 2; i++)
            {
                j++;
                var part = sNum.Substring(i << 2, 4);
                var stringFormat = int.Parse(part);
                if (stringFormat == 0)
                {
                    continue;
                }

                displayNum += stringFormat + arrDecimal[(sNum.Length >> 2) - i - 1];
                // if (sNum.Length >> 2 - 1 != i) displayNum += empty;
            }

            displayNum = displayNum.TrimEnd();
        }

        return displayNum;
    }
    #endregion

    public static void RestartApplication()
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
