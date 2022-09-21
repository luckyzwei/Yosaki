using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UiGumgiMountainSoul : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI abilDescription;

    [SerializeField]
    private TextMeshProUGUI abilValue;

    [SerializeField]
    private WeaponView weaponView;


    void Start()
    {
        abilDescription.SetText($"처치 {PlayerStats.gumgiSoulDivideNum}마리당 검기의 능력치가 {PlayerStats.gumgiSoulAbilValue * 100f}% 강화 됩니다!");

        //?? 단게 검기 능력치 +??% 강화됨

        ServerData.equipmentTable.TableDatas[EquipmentTable.Weapon_View].AsObservable().Subscribe(e =>
        {
            if (e == -1)
            {
                e = 0;
            }

            weaponView.Initialize(TableManager.Instance.WeaponData[e], null);
        }).AddTo(this);

        ServerData.goodsTable.GetTableData(GoodsTable.Fw).AsObservable().Subscribe(e =>
        {
            var grade = (int)(ServerData.userInfoTable.TableDatas[UserInfoTable.gumGiSoulClear].Value / PlayerStats.gumgiSoulDivideNum);

            abilValue.SetText($"{grade}단계 검기 능력치 +{PlayerStats.GetGumgiAbilAddValue() * 100f}% 강화됨");

        }).AddTo(this);
    }
}
