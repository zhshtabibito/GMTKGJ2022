using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager
{
    public static PanelManager Instance;

    private Dictionary<string, GameObject> dictUI;
    private Dictionary<string, BasePanel> dictPanel;
    private Stack<BasePanel> panelStack;

    public Transform CanvasObj;
    public Transform BlackMask;
    private RawImage BlackMaskCpn;

    public PanelManager()
    {
        dictUI = new Dictionary<string, GameObject>();
        dictPanel = new Dictionary<string, BasePanel>();
        panelStack = new Stack<BasePanel>();

        // BlackMaskCpn = BlackMask.GetComponent<RawImage>();
    }

    public BasePanel GetPeek()
    {
        return panelStack.Peek();
    }

    public GameObject GetSingleUI(PanelInfo panel)
    {
        if (dictUI.ContainsKey(panel.Path))
        {
            return dictUI[panel.Path];
        }

        if(CanvasObj==null)
            CanvasObj = GameObject.Find("Canvas").transform;

        GameObject obj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(panel.Path), CanvasObj);
        obj.name = panel.Name;
        dictUI.Add(panel.Path, obj);
        return obj;
    }

    public void DestroyUI(PanelInfo panel, bool isDestroy = false)
    {
        if (dictUI.ContainsKey(panel.Path))
        {
            if (isDestroy)
            {
                GameObject.Destroy(dictUI[panel.Path]);
                dictUI.Remove(panel.Path);
                dictPanel.Remove(panel.Path);
            }
            else
            {
                dictUI[panel.Path].transform.PanelAppearance(false);
            }
        }
    }

    public BasePanel Push(BasePanel newPanel)
    {
        if (panelStack.Count > 0)
            panelStack.Peek().OnPause();
        // 维护字典
        if (!dictPanel.ContainsKey(newPanel.Info.Path))
        {
            dictPanel.Add(newPanel.Info.Path, newPanel);
            GameObject obj = GetSingleUI(newPanel.Info);
            newPanel.ActivePanel = obj.transform;
            newPanel.Initialize(this);
        }
        else
        {
            // 刷新字典中的panel，不add new panal
            BasePanel p = dictPanel[newPanel.Info.Path];
            p.OnChange(newPanel);
            newPanel = p;
        }
        newPanel.OnEnter();
        // Push
        if (panelStack.Count > 0)
        {
            // 防止连续推送重复的面板
            if (panelStack.Peek().Info.Path != newPanel.Info.Path)
                panelStack.Push(newPanel);
            else
                return panelStack.Peek();
        }
        else
            panelStack.Push(newPanel);

        return newPanel;
    }

    public void Pop()
    {
        if (panelStack.Count > 0)
            panelStack.Pop().OnExit();
        if (panelStack.Count > 0)
            panelStack.Peek().OnResume();
    }

    public void PopAll()
    {
        var values = new List<BasePanel>(dictPanel.Values);
        while (values.Count > 0)
        {
            values[0].OnExit(true);
            values.RemoveAt(0);
        }
    }

    public bool isMouseInPanel(BasePanel panel)
    {
        // https://blog.csdn.net/kun1234567/article/details/78684104
        RectTransform rect = panel.ActivePanel.GetComponent<RectTransform>();
        Rect spaceRect = rect.rect;
        spaceRect.x = spaceRect.x * rect.lossyScale.x + rect.position.x;
        spaceRect.y = spaceRect.y * rect.lossyScale.y + rect.position.y;
        spaceRect.width = spaceRect.width * rect.lossyScale.x;
        spaceRect.height = spaceRect.height * rect.lossyScale.y;
        if (spaceRect.Contains(Input.mousePosition))
            return true;
        else
            return false;
    }


}
