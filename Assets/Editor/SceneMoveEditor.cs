using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class SceneMoveEditor : MonoBehaviour
{
    [MenuItem("SceneMove/MainScene &1")]
    static void MainScene()
    {
        EditorSceneManager.OpenScene("Assets/01.Scene/PreScene_Sin.unity");
        Debug.Log("MainSceneMove");
    }

    [MenuItem("SceneMove/InGameScene &2")]
    static void InGameScene()
    {
        EditorSceneManager.OpenScene("Assets/01.Scene/GameScene_Sin.unity");
        Debug.Log("InGameSceneMove");
    }
}
