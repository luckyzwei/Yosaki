using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RewardPopupManager : SingletonMono<RewardPopupManager>
{
    [SerializeField]
    MainTabButtons mainTabButtons;

    private void Start()
    {
        mainTabButtons = this.gameObject.GetComponent<MainTabButtons>();
    }

    public void OnclickButton()
    {
        mainTabButtons.OnClickButton();
    }
    // Start is called before the first frame update



}
