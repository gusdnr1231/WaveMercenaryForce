using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PoolManagerSO))]
public class CustomPoolManagerSO : Editor
{
    private PoolManagerSO _manager;
    private string customPath = "Assets/08SO/Pool/Items"; // Default path

    private void OnEnable()
    {
        _manager = target as PoolManagerSO;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // Text field to input the custom path
        customPath = EditorGUILayout.TextField("Pool Items Path", customPath);

        if (GUILayout.Button("Get All Pool Item"))
        {
            UpdatePoolingItems();
        }
    }

    private void UpdatePoolingItems()
    {
        List<PoolingItemSO> loadedItems = new List<PoolingItemSO>();

        string[] assetGUIDS = AssetDatabase.FindAssets("", new[] { customPath });

        foreach (string guid in assetGUIDS)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            PoolingItemSO item = AssetDatabase.LoadAssetAtPath<PoolingItemSO>(assetPath);

            if (item != null)
            {
                loadedItems.Add(item);
            }
        }

        _manager.poolingItems = loadedItems;

        EditorUtility.SetDirty(_manager);
        AssetDatabase.SaveAssets();
    }
}
