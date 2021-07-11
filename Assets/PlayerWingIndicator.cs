using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class PlayerWingIndicator : MonoBehaviour
{
    private GameObject currentWingObject;

    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        ServerData.userInfoTable.GetTableData(UserInfoTable.marbleAwake).AsObservable().Subscribe(e =>
        {
            if (e == -1)
            {

            }
            else
            {
                if (currentWingObject != null)
                {
                    GameObject.Destroy(currentWingObject);
                }

                currentWingObject = Instantiate<GameObject>(CommonUiContainer.Instance.wingList[(int)e], this.transform);

                currentWingObject.transform.localPosition = Vector3.zero;
            }
        }).AddTo(this);
    }
}
