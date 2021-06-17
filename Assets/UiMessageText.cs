using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiMessageText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private Animator animator;

    private static string fadeAnimName = "Fade";


    [SerializeField]
    private Image costumeIcon;

    public void Initialize(string description, bool isSystem)
    {
        costumeIcon.gameObject.SetActive(isSystem == false);

        animator.SetTrigger(fadeAnimName);

        if (isSystem)
        {
            this.description.SetText(description);
        }
        else
        {
            if (description.Contains($"{CommonString.ChatSplitChar}"))
            {
                var split = description.Split(CommonString.ChatSplitChar);

                if (split.Length > 0)
                {
                    int costumeIdx = int.Parse(split[0]);
                    costumeIcon.gameObject.SetActive(true);
                    costumeIcon.sprite = CommonUiContainer.Instance.GetCostumeThumbnail(costumeIdx);
                }
                else
                {
                    costumeIcon.gameObject.SetActive(false);
                }


                if (split.Length > 1)
                {
                    this.description.SetText(description.Split(CommonString.ChatSplitChar)[1]);
                }
            }
            else 
            {
                costumeIcon.gameObject.SetActive(false);
                this.description.SetText(description);
            }
           
        }
    }
}
