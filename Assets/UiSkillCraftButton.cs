using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiSkillCraftButton : MonoBehaviour
{
    [SerializeField]
    private GameObject popupObject;

    public void OnClickButton() 
    {
        int currentSkillGachaLevel = UiGachaPopup.GachaLevel(UserInfoTable.gachaNum_Skill);

        if (currentSkillGachaLevel < 9) 
        {
            PopupManager.Instance.ShowAlarmMessage("기술 뽑기 레벨 10부터 이용 가능합니다.");
            return;
        }

        popupObject.gameObject.SetActive(true);
    }
}
