using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DialogCharacterView;

[System.Serializable]
public class DialogInfo
{
    public DialogCharcterType charcterType;
    public string message;
}

public class DialogManager : SingletonMono<DialogManager>
{
    [SerializeField]
    private List<DialogInfo> dialogInfo = new List<DialogInfo>();

    private List<DialogInfo> currentDialog;

    private int currentDialogIdx = 0;

    [SerializeField]
    private DialogCharacterView firstView;

    [SerializeField]
    private DialogCharacterView secondView;

    [SerializeField]
    private GameObject rootObject;

    public void StartDialog()
    {
        currentDialog = dialogInfo;
        currentDialogIdx = 0;
        rootObject.SetActive(true);
        NextDialog();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && rootObject.activeInHierarchy == true)
        {
            SoundManager.Instance.PlayButtonSound();
            NextDialog();
        }
    }

    private void NextDialog()
    {
        if (currentDialogIdx >= currentDialog.Count)
        {
            EndDialog();
            return;
        }

        DialogCharcterType characterType = currentDialog[currentDialogIdx].charcterType;

        if (characterType == DialogCharcterType.BeforeLuccy || characterType == DialogCharcterType.CurrentLuccy || characterType == DialogCharcterType.LeftManager)
        {
            if (firstView.state == State.Texting)
            {
                firstView.SetEnd();
                return;
            }
        }
        else
        {
            if (secondView.state == State.Texting)
            {
                secondView.SetEnd();
                return;
            }
        }

        firstView.gameObject.SetActive(characterType == DialogCharcterType.BeforeLuccy || characterType == DialogCharcterType.CurrentLuccy);
        secondView.gameObject.SetActive(characterType == DialogCharcterType.Manager);

        if (characterType == DialogCharcterType.BeforeLuccy || characterType == DialogCharcterType.CurrentLuccy)
        {
            firstView.Initialize(currentDialog[currentDialogIdx], WhenTextingEnd);
        }
        else
        {
            secondView.Initialize(currentDialog[currentDialogIdx], WhenTextingEnd);
        }
    }

    private void WhenTextingEnd()
    {
        currentDialogIdx++;

        if (currentDialogIdx == 9)
        {
            PopupManager.Instance.ShowWhiteEffect();
        }
    }

    private void EndDialog()
    {
        rootObject.SetActive(false);
    }
}
