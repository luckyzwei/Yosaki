using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiRecommendButton : MonoBehaviour
{
    
    

    [SerializeField]
    private TextMeshProUGUI nickName;

    public void IncreaseRecommendCount()
    {
        PartyRaidManager.Instance.NetworkManager.SendRecommend(nickName.text.ToString());

        this.gameObject.SetActive(false);
    }
}
