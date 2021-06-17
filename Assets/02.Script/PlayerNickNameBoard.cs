using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNickNameBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro nameText;

    private void Awake()
    {
        nameText.SetText(PlayerData.Instance.NickName);
    }

}
