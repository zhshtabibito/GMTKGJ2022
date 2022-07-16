using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 所有UI面板的父类
/// 不进行object创建和销毁
/// </summary>
public abstract class BasePanel
{
    /// <summary>
    /// Panel info class
    /// </summary>
    public PanelInfo Info { get; private set; }
    /// <summary>
    /// whether OnExit if clicked out of panel when on the top of ui stack
    /// </summary>
    public bool isExitIfClicked = false;

    /// <summary>
    /// Prefab transform, 会在进栈维护字典时赋值
    /// </summary>
    public Transform ActivePanel { get; set; }
    protected PanelManager panelManager;

    public BasePanel(PanelInfo info)
    {
        this.Info = info;
    }

    /// <summary>
    /// Invoked when PanelManager.Push
    /// </summary>
    public void Initialize(PanelManager manager)
    {
        panelManager = manager;
    }

    /// <summary>
    /// 用于初始化, 在UI销毁之前只会执行一次，在OnEnter中
    /// </summary>
    protected virtual void InitEvent() { }

    /// <summary>
    /// 每次推出一个面板执行的操作
    /// </summary>
    public virtual void OnEnter()
    {
        ActivePanel.PanelAppearance(true);
        ActivePanel.SetSiblingIndex(ActivePanel.parent.childCount - 1);
        if (!Info.Init)
        {
            InitEvent();
            Info.Init = true;
        }
    }

    /// <summary>
    /// 每次失去激活时执行的操作
    /// especially when a new panel get over
    /// </summary>
    public virtual void OnPause()
    {
        ActivePanel.GetOrAddComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public virtual void OnResume()
    {
        ActivePanel.GetOrAddComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public virtual void OnExit(bool isDestroy = false)
    {
        panelManager?.DestroyUI(Info, isDestroy);
    }

    public virtual void OnChange(BasePanel newPanel) { }

    public void Push(BasePanel newPanel)
    {
        panelManager?.Push(newPanel);
    }

    public void Pop()
    {
        panelManager?.Pop();
    }
}
