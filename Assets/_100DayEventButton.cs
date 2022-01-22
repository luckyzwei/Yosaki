using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _100DayEventButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(ServerData.attendanceServerTable.Attendance100AllReceived() == false);
    }
}
