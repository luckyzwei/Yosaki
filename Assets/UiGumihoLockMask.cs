using UniRx;
using UnityEngine;

public class UiGumihoLockMask : MonoBehaviour
{
    void Start()
    {
        Subscribe();
    }

    private void Subscribe()
    {

        ServerData.userInfoTable.TableDatas[UserInfoTable.marbleAwake].AsObservable().Subscribe(e =>
        {
            this.gameObject.SetActive(e != 1);

        }).AddTo(this);

    }


}
