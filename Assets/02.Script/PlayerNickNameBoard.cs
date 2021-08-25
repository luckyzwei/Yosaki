using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class PlayerNickNameBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro nameText;

    private void Awake()
    {
        nameText.SetText(PlayerData.Instance.NickName);

        Subscibe();
    }

    private void Subscibe()
    {
        PlayerData.Instance.whenNickNameChanged.AsObservable().Subscribe(e =>
        {
            nameText.SetText(e);
        }).AddTo(this);
    }
}
