using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABBManager : SingletonMono<ABBManager>
{
    private Stack<AbbObject> abbObjects = new Stack<AbbObject>();

    public void ResetAbb()
    {
        abbObjects.Clear();
    }

    public void Register(AbbObject gameObject)
    {
        abbObjects.Push(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseObject();
        }
    }

    private void CloseObject()
    {
        if (abbObjects.Count > 0)
        {
            var abbObj = PopObject();

            if (abbObj != null)
            {
                abbObj.gameObject.SetActive(false);
            }
        }
        else
        {
            GameEscapeManager.Instance.WhenEscapeInputReceived();
        }
    }

    //꺼질때
    public void WhenObjectDisable(AbbObject gameObject)
    {
        //등록된게 나다.
        if (abbObjects.Count > 0 && abbObjects.Peek() == gameObject)
        {
            PopObject();
        }
    }

    public AbbObject PopObject()
    {
        if (abbObjects.Count > 0)
        {
            return abbObjects.Pop();
        }
        else
        {
            return null;
        }
    }




}
