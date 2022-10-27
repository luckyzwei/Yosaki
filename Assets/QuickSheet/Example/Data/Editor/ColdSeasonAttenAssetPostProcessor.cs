using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
public class ColdSeasonAttenAssetPostprocessor : AssetPostprocessor 
{
    private static readonly string filePath = "Assets/06.Table/ColdSeasonAtten.xlsx";
    private static readonly string assetFilePath = "Assets/06.Table/ColdSeasonAtten.asset";
    private static readonly string sheetName = "ColdSeasonAtten";
    
    static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets) 
        {
            if (!filePath.Equals (asset))
                continue;
                
            ColdSeasonAtten data = (ColdSeasonAtten)AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ColdSeasonAtten));
            if (data == null) {
                data = ScriptableObject.CreateInstance<ColdSeasonAtten> ();
                data.SheetName = filePath;
                data.WorksheetName = sheetName;
                AssetDatabase.CreateAsset ((ScriptableObject)data, assetFilePath);
                //data.hideFlags = HideFlags.NotEditable;
            }
            
            //data.dataArray = new ExcelQuery(filePath, sheetName).Deserialize<ColdSeasonAttenData>().ToArray();		

            //ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
            //EditorUtility.SetDirty (obj);

            ExcelQuery query = new ExcelQuery(filePath, sheetName);
            if (query != null && query.IsValid())
            {
                data.dataArray = query.Deserialize<ColdSeasonAttenData>().ToArray();
                ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
                EditorUtility.SetDirty (obj);
            }
        }
    }
}
