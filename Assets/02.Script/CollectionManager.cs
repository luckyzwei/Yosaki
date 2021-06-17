using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;
using UniRx;

public class CollectionManager : Singleton<CollectionManager>
{
    private HashSet<string> updatedCollectionKeys = new HashSet<string>();
    public CollectionServerData GetCollectionData(string key, bool forChange)
    {
        if (forChange)
        {
            if (updatedCollectionKeys.Contains(key) == false)
            {
                updatedCollectionKeys.Add(key);
            }
        }

        return DatabaseManager.collectionTable.TableDatas[key];
    }

    public void SyncToServer()
    {
        Param defultValues = new Param();

        var table = TableManager.Instance.EnemyTable.dataArray;

        var tableDatas = DatabaseManager.collectionTable.TableDatas;

        for (int i = 0; i < table.Length; i++)
        {
            if (table[i].Usecollection == false) continue;
            if (table[i].Ishardenemy == true) continue;
            if (updatedCollectionKeys.Contains(table[i].Collectionkey) == false) continue;

            string key = table[i].Collectionkey;
            defultValues.Add(key, $"{tableDatas[key].idx},{tableDatas[key].level.Value},{tableDatas[key].amount.Value}");
        }

        SendQueue.Enqueue(Backend.GameData.Update, CollectionTable.tableName, CollectionTable.Indate, defultValues, bro =>
         {
#if UNITY_EDITOR
             if (bro.IsSuccess() == false)
             {
                 Debug.LogError($"Sync collection failed");
                 return;
             }
             else
             {
                 Debug.LogError($"Sync collection complete");
             }
#endif
         });



        updatedCollectionKeys.Clear();
    }

    public void SyncToServerForce()
    {
        Param defultValues = new Param();

        var table = TableManager.Instance.EnemyTable.dataArray;

        var tableDatas = DatabaseManager.collectionTable.TableDatas;

        for (int i = 0; i < table.Length; i++)
        {
            if (table[i].Usecollection == false) continue;
            if (table[i].Ishardenemy == true) continue;
            if (updatedCollectionKeys.Contains(table[i].Collectionkey) == false) continue;

            string key = table[i].Collectionkey;
            defultValues.Add(key, $"{tableDatas[key].idx},{tableDatas[key].level.Value},{tableDatas[key].amount.Value}");
        }

        var bro = Backend.GameData.Update(CollectionTable.tableName, CollectionTable.Indate, defultValues);

#if UNITY_EDITOR
        if (bro.IsSuccess() == false)
        {
            Debug.LogError($"Sync collection failed");
            return;
        }
        else
        {
            Debug.LogError($"Sync collection complete");
        }
#endif

        updatedCollectionKeys.Clear();
    }
}
