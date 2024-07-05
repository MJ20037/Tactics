using UnityEngine;
using UnityEditor;

public class GridEditorWindow : EditorWindow
{
    private ObstacleData gridData;

    [MenuItem("Tools/Grid Editor")]
    public static void ShowWindow()
    {
        GetWindow<GridEditorWindow>("Grid Editor");
    }

    private void OnGUI()
    {
        if (gridData == null)
        {
            if (GUILayout.Button("Load Obstacle Data"))
            {
                string path = EditorUtility.OpenFilePanel("Select Obstacle Data", "Assets", "asset");
                if (!string.IsNullOrEmpty(path))
                {
                    path = "Assets" + path.Substring(Application.dataPath.Length);
                    gridData = AssetDatabase.LoadAssetAtPath<ObstacleData>(path);
                }
            }
        }
        else
        {
            for (int y = 0; y < 10; y++)
            {
                GUILayout.BeginHorizontal();
                for (int x = 0; x < 10; x++)
                {
                    int index = y * 10 + x;
                    gridData.obstacle[index] = GUILayout.Toggle(gridData.obstacle[index], "");
                }
                GUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Save"))
            {
                EditorUtility.SetDirty(gridData);
                AssetDatabase.SaveAssets();
            }

            if (GUILayout.Button("Clear"))
            {
                for (int i = 0; i < gridData.obstacle.Length; i++)
                {
                    gridData.obstacle[i] = false;
                }
                EditorUtility.SetDirty(gridData);
                AssetDatabase.SaveAssets();
            }
        }
    }
}

