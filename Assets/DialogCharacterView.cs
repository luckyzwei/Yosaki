using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using TMPro;
using System;

public enum DialogCharcterType
{
    BeforeLuccy, CurrentLuccy, Manager, LeftManager
}

public class DialogCharacterView : MonoBehaviour
{
    public enum State
    {
        Texting, End
    }

    [SerializeField]
    private SkeletonGraphic skeletonGraphic;

    [SerializeField]
    private List<SkeletonDataAsset> characterList;

    [SerializeField]
    private TextMeshProUGUI jobText;

    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private TextMeshProUGUI dialogText;

    private DialogInfo dialogInfo;

    public State state { get; private set; } = State.End;

    private Action whenTextingEnd;

    private void SetCharacterSpine(int idx)
    {
        skeletonGraphic.Clear();
        skeletonGraphic.skeletonDataAsset = characterList[idx];
        skeletonGraphic.Initialize(true);
        skeletonGraphic.SetMaterialDirty();
    }

    public void Initialize(DialogInfo dialogInfo, Action whenTextingEnd)
    {
        nameText.SetText(CommonString.GetDialogTextName(dialogInfo.charcterType));

        SetCharacterSpine((int)dialogInfo.charcterType);

        this.whenTextingEnd = whenTextingEnd;

        this.dialogInfo = dialogInfo;

        state = State.Texting;

        dialogText.SetText(string.Empty);

        if (textingRoutine != null)
        {
            StopCoroutine(textingRoutine);
        }

        textingRoutine = StartCoroutine(TextingRoutine());
    }

    private Coroutine textingRoutine;

    private IEnumerator TextingRoutine()
    {
        WaitForSeconds textingDelay = new WaitForSeconds(0.03f);

        int textCount = dialogInfo.message.Length;
        int currentIdx = 0;

        string message = string.Empty;

        while (currentIdx < textCount)
        {
            message += dialogInfo.message[currentIdx];
            dialogText.SetText(message);
            currentIdx++;
            yield return textingDelay;
        }

        SetEnd();
    }

    public void SetEnd()
    {
        if (textingRoutine != null)
        {
            StopCoroutine(textingRoutine);
        }

        dialogText.SetText(dialogInfo.message);

        state = State.End;

        whenTextingEnd.Invoke();

    }
}
