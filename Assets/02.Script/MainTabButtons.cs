using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainTabButtons : MonoBehaviour
{
    [SerializeField]
    public GameObject popupPrefab;

    private GameObject popupObject;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnClickButton);
    }

    public void OnClickButton()
    {
        if (popupPrefab == null)
        {
            PopupManager.Instance.ShowConfirmPopup(CommonString.Notice, "업데이트 예정 입니다.", null);
            return;
        }

        if (popupObject == null)
        {
            popupObject = Instantiate<GameObject>(popupPrefab, InGameCanvas.Instance.transform);
        }
        else
        {
            popupObject.SetActive(true);
        }

       // UiStatus.Instance.transform.SetAsLastSibling();
    }

}
