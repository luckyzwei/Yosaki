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

    //[SerializeField]
    //private GameObject touchMask;

    [SerializeField]
    private Image storyImage;

    [SerializeField]
    private List<Sprite> storySprites;

    [SerializeField]
    private List<string> storyTexts;

    private bool setSkip = false;

    private int currentIdx = 0;

    private Coroutine textingRoutine;

    private TextingState textingState = TextingState.Playing;

    private enum TextingState
    {
        Playing, End
    }

    private bool isEndIdx()
    {
        return currentIdx == storySprites.Count - 1;
    }

    private new void Awake()
    {
        base.Awake();

        currentIdx = 0;
    }

    public void SetNextDialog()
    {
        storyImage.sprite = storySprites[currentIdx];

        setSkip = false;

        rootObject.SetActive(true);

        if (textingRoutine != null)
        {
            StopCoroutine(textingRoutine);
        }

        textingRoutine = StartCoroutine(TextingRoutine());
    }

    public void SkipText()
    {
        if (isEndIdx())
        {
            EndDialog();
            return;
        }

        if (setSkip == true)
        {
            currentIdx++;
            SetNextDialog();
        }
        else
        {
            SoundManager.Instance.PlayButtonSound();
            setSkip = true;
        }
    }

    private IEnumerator TextingRoutine()
    {
        textingState = TextingState.Playing;

        dialogText.SetText(string.Empty);

        WaitForSeconds textingDelay = new WaitForSeconds(0.02f);

        int textCount = storyTexts[currentIdx].Length;
        int currentTextIdx = 0;

        string message = string.Empty;

        while (currentTextIdx < textCount)
        {

            if (setSkip)
            {
                dialogText.SetText(storyTexts[currentIdx]);
                break;
            }

            message += storyTexts[currentIdx][currentTextIdx];
            dialogText.SetText(message);
            currentTextIdx++;
            yield return textingDelay;
        }

        textingState = TextingState.End;
        setSkip = true;
    }

    public void EndDialog()
    {
        if (textingRoutine != null)
        {
            StopCoroutine(textingRoutine);
        }

        if (textingState == TextingState.Playing)
        {
            dialogText.SetText(storyTexts[storyTexts.Count - 1]);
            textingState = TextingState.End;
            return;
        }

        rootObject.SetActive(false);
        currentIdx = 0;
    }
}
