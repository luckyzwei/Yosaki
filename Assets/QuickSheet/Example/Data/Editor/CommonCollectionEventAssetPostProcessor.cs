using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
public class CommonCollectionEventAssetPostprocessor : AssetPostprocessor 
{
    private static readonly string filePath = "Assets/06.Table/CommonCollectionEvent.xlsx";
    private static readonly string assetFilePath = "Assets/06.Table/CommonCollectionEvent.asset";
    private static readonly string sheetName = "CommonCollectionEvent";
    
    static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets) 
        {
            if (!filePath.Equals (asset))
                continue;
                
            CommonCollectionEvent data = (CommonCollectionEvent)AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(CommonCollectionEvent));
            if (data == null) {
                data = ScriptableObject.CreateInstance<CommonCollectionEvent> ();
                data.SheetName = filePath;
                data.WorksheetName = sheetName;
                AssetDatabase.CreateAsset ((ScriptableObject)data, assetFilePath);
                //data.hideFlags = HideFlags.NotEditable;
            }
            
            //data.dataArray = new ExcelQuery(filePath, sheetName).Deserialize<CommonCollectionEventData>().ToArray();		

            //ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
            //EditorUtility.SetDirty (obj);

            ExcelQuery query = new ExcelQuery(filePath, sheetName);
            if (query != null && query.IsValid())
            {
                data.dataArray = query.Deserialize<CommonCollectionEventData>().ToArray();
                ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
                EditorUtility.SetDirty (obj);
            }
        }
    }
}
