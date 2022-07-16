using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BaseGridEditor : Editor
{
    BaseGrid grid;

    private void OnEnable()
    {
        grid = target as BaseGrid;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("打印坐标"))
        {
            Debug.LogFormat("当前格子的地图数据坐标：{0}", grid.gridIndex);
        }
        EditorGUILayout.EndHorizontal();
    }
}
