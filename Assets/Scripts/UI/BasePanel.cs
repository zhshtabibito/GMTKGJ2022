using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����UI���ĸ���
/// ������object����������
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
    /// Prefab transform, ���ڽ�ջά���ֵ�ʱ��ֵ
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
    /// ���ڳ�ʼ��, ��UI����֮ǰֻ��ִ��һ�Σ���OnEnter��
    /// </summary>
    protected virtual void InitEvent() { }

    /// <summary>
    /// ÿ���Ƴ�һ�����ִ�еĲ���
    /// </summary>
    public virtual void OnEnter()
    {
        ActivePanel.PanelAppearance(true);
        // Debug.Log(ActivePanel.parent);
        ActivePanel.SetSiblingIndex(ActivePanel.parent.childCount - 1);
        if (!Info.Init)
        {
            InitEvent();
            Info.Init = true;
        }
    }

    /// <summary>
    /// ÿ��ʧȥ����ʱִ�еĲ���
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
