using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Flags]
public enum ManagerDescriptionType
{
    //blackDragon = 2,//너도 용이니..? ★
    //costumeGachaDesc = 4,//보석으로 능력치를 뽑을수 있습니다.★
    //bonusDungeonDescription = 8,//★
    //bossContentsDescription = 16,//★
    //infinityTowerDescription = 32,//★
    //monsterCollection = 64,// 처치한 몬스터의 영혼을 흡수하면 능력치가 영구적으로 상승 ★
    //skillBoardDescription = 128,//★
    //potionDescription = 256,//★
    //dailyPassDescription = 512,//★
    //statusDescription = 1024//★
}

public class UiManagerDescription : SingletonMono<UiManagerDescription>
{
    [SerializeField]
    private GameObject rootObject;

    [SerializeField]
    private TextMeshProUGUI descriptionText;

    [SerializeField]
    private Button skipButton;

    private Coroutine textingRoutine;

    public void SetManagerDescription(ManagerDescriptionType type)
    {
        //이기능X
        //
        return;
        //
        var savedFlags = (ManagerDescriptionType)ServerData.userInfoTable.GetTableData(UserInfoTable.managerDescriptionFlags).Value;

        if (savedFlags.IsSet(type) == true)
        {
            return;
        }

        savedFlags |= type;

        ServerData.userInfoTable.UpData(UserInfoTable.managerDescriptionFlags, (float)savedFlags, false);

        rootObject.SetActive(true);


        if (textingRoutine != null)
        {
            StopCoroutine(textingRoutine);
        }

        textingRoutine = StartCoroutine(TextingRoutine(GetDescription(type)));
    }

    private IEnumerator TextingRoutine(string description)
    {
        skipButton.interactable = false;

        descriptionText.SetText(string.Empty);

        WaitForSeconds textingDelay = new WaitForSeconds(0.03f);

        int textCount = description.Length;
        int currentIdx = 0;

        string message = string.Empty;

        while (currentIdx < textCount)
        {
            message += description[currentIdx];
            descriptionText.SetText(message);
            currentIdx++;
            yield return textingDelay;
        }

        textingRoutine = null;

        skipButton.interactable = true;
    }


    private void WhenAppearAnimEnd()
    {

    }

    public void OnClickSkipButton()
    {
        rootObject.SetActive(false);
    }

    private string GetDescription(ManagerDescriptionType type)
    {
        switch (type)
        {
            //case ManagerDescriptionType.blackDragon:
            //    return "까마귀.? 너는 왜 이름이 용이니..?";
            //    break;
            //case ManagerDescriptionType.costumeGachaDesc:
            //    return "이곳에선 외형을 바꿀수 있습니다.\n그리고 보석을 사용해 능력치를 재설정할 수 있습니다.";
            //    break;
            //case ManagerDescriptionType.bonusDungeonDescription:
            //    return "괴씸한 도둑 고블린들을 처치하고 보석을 되찾으세요";
            //    break;
            //case ManagerDescriptionType.bossContentsDescription:
            //    return "루시님의 힘을 나눠갖은 악마들과 전투를 해보세요.\n아참..포션은 사용할 수 없습니다.";
            //    break;
            //case ManagerDescriptionType.infinityTowerDescription:
            //    return "이곳에선 정예 몬스터들이 등장합니다.\n제한시간 안에 전부 처치하면 클리어 됩니다.\n포션은 사용 불가합니다.";
            //    break;
            //case ManagerDescriptionType.monsterCollection:
            //    return "이곳에서는 처치한 몬스터의 영혼을 흡수하고,\n영구적으로 능력치를 얻을수 있습니다.";
            //    break;
            //case ManagerDescriptionType.skillBoardDescription:
            //    return "이곳에서는 스킬을 배우고, 레벨업할 수 있습니다.";
            //    break;
            //case ManagerDescriptionType.potionDescription:
            //    return "이곳에서는 포션을 구매하고 버튼에 등록할 수 있습니다.\n포션이 자동으로 사용되는 옵션도 선택해 보세요";
            //    break;
            //case ManagerDescriptionType.dailyPassDescription:
            //    return "매일 매일 몬스터를 처치하고 보상을 받을 수 있습니다.\n다음날이 되기전에 보상을 받는걸 까먹지 마세요.";
            //    break;
            //case ManagerDescriptionType.statusDescription:
            //    return "레벨이 오르면 스텟과 스킬포인트를 얻습니다.\n스텟으로는 능력치를,스킬포인트로는 스킬 레벨을 올릴수 있습니다.";
            //    break;
        }

        return $"텍스트 미정의 {type}";
    }
}
