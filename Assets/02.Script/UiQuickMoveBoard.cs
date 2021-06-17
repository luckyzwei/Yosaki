using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiQuickMoveBoard : MonoBehaviour
{
    [SerializeField]
    private UiQuickMoveThemaSet uiQuickMoveThemaSet;

    [SerializeField]
    private Transform themaSetparent;

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var e = TableManager.Instance.StageMapData.GetEnumerator();

        Dictionary<int, List<StageMapData>> datas = new Dictionary<int, List<StageMapData>>();

        while (e.MoveNext())
        {

            if (datas.ContainsKey(e.Current.Value.Mapthema) == false)
            {
                datas.Add(e.Current.Value.Mapthema, new List<StageMapData>());
            }

            datas[e.Current.Value.Mapthema].Add(e.Current.Value);
        }

        var e2 = datas.GetEnumerator();

        while (e2.MoveNext())
        {
            var themaSet = Instantiate<UiQuickMoveThemaSet>(uiQuickMoveThemaSet, themaSetparent);
            themaSet.Initialize(e2.Current.Value);
        }
    }

}
