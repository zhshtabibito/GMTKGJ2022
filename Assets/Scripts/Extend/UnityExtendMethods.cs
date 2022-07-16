using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityExtendMethods
{
    public static GameObject FindChildGameObject(this GameObject obj, string childName)
    {
        Transform[] children = obj.GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            if (child.name == childName)
                return child.gameObject;
        }

        Debug.LogWarning($"{childName} not found in parent {obj.name}");
        return null;
    }

    public static T GetOrAddComponent<T>(this Transform t) where T : Component
    {
        T component = t.GetComponent<T>();
        if (component == null)
            component = t.gameObject.AddComponent<T>();

        return component;
    }

    public static T GetOrAddComponentInChildren<T>(this Transform t, string childName) where T : Component
    {
        GameObject childObj = t.gameObject.FindChildGameObject(childName);

        if (childObj == null)
            return null;
        return childObj.transform.GetOrAddComponent<T>();
    }
    public static void PanelAppearance(this Transform t, bool on_off, bool active = false)
    {
        CanvasGroup group = t.GetOrAddComponent<CanvasGroup>();
        int value = on_off == true ? 1 : 0;

        //射线检测
        group.blocksRaycasts = on_off;
        //交互
        group.interactable = on_off;
        //透明度
        group.alpha = value;

        t.gameObject.SetActive(on_off || active);
    }
}
