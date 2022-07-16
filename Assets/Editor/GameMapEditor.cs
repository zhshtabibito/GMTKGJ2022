using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;


[CustomEditor(typeof(GameMap))]
public class GameMapEditor : Editor
{
    GameMap gameMap;

    private void OnEnable()
    {
        gameMap = target as GameMap;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("导入地图数据"))
        {
            gameMap.ImportGameMapData();
        }
        EditorGUILayout.EndHorizontal();
    }
}
