using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
    private GameObject rootObject;

    [SerializeField]
    private TextMeshProUGUI dialogText;

    [SerializeField]
    private Button endInput;

    [SerializeField]
    private GameObject touchMask;

    private string storyDialog = "스토리스토리스토리스토리스토리스토리스토리스토리스토리스토리스토리스토리스토리스토리스토리스토리스토리스토리스토리스토리스토리스토리스토리스토리스토리스토리스토리스토리스토리스토리스토리스토리스토리스토리스토리스토리";

    private bool setSkip = false;

    public void StartDialog()
    {
        setSkip = false;

        touchMask.SetActive(true);

        rootObject.SetActive(true);

        endInput.interactable = false;

        StartCoroutine(TextingRoutine());
    }

    public void SkipText() 
    {
        SoundManager.Instance.PlayButtonSound();
        setSkip = true;
    }

    private IEnumerator TextingRoutine()
    {
        dialogText.SetText(string.Empty);

        WaitForSeconds textingDelay = new WaitForSeconds(0.03f);

        int textCount = storyDialog.Length;
        int currentIdx = 0;

        string message = string.Empty;

        while (currentIdx < textCount)
        {

            if (setSkip)
            {
                dialogText.SetText(storyDialog);
                break;
            }

            message += storyDialog[currentIdx];
            dialogText.SetText(message);
            currentIdx++;
            yield return textingDelay;
        }

        yield return null;

        SetEnd();
    }

    private void SetEnd()
    {
        touchMask.SetActive(false);
        endInput.interactable = true;
    }


    public void EndDialog()
    {
        rootObject.SetActive(false);
    }
}
