using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiAutoRevive : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI reviveRemainSecText;

    private ObscuredInt autoReviveSec = 10;

    public static bool autoRevive = false;

    public static Vector3 spawnPos;

    [SerializeField]
    private GameObject popupParent;


    // Start is called before the first frame update
    void Start()
    {
        spawnPos = PlayerMoveController.Instance.transform.position;

        if (SettingData.ShakeCamera.Value == 1)
        {
            StartCoroutine(ReviveRoutine());
        }
    }

    private IEnumerator ReviveRoutine()
    {
        reviveRemainSecText.SetText($"{autoReviveSec}초 후에 자동으로 부활");

        while (autoReviveSec > 0)
        {
            autoReviveSec--;
            yield return new WaitForSeconds(1.0f);
            reviveRemainSecText.SetText($"{autoReviveSec}초 후에 자동으로 부활");
        }

        GameObject.Destroy(popupParent);
        autoRevive = true;
        GameManager.Instance.LoadNormalField();
    }

}
