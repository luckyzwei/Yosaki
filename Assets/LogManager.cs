using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogManager : SingletonMono<LogManager>
{
    public void SendLog(string prefix, string description)
    {
        Param param = new Param();
        param.Add(prefix, $"{description}");
        SendQueue.Enqueue(Backend.GameLog.InsertLog, "Normal", param, (callback) =>
         {
            // 이후 처리
        });
    }

    public void SendLogType(string type, string prefix, string description)
    {
        Param param = new Param();
        param.Add(prefix, $"{description}");
        SendQueue.Enqueue(Backend.GameLog.InsertLog, type, param, (callback) =>
        {
            // 이후 처리
        });
    }
}
