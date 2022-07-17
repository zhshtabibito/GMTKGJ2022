using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneInfo
{
    public string SceneName;

    protected PanelManager panelManager;

    public SceneInfo()
    {
        panelManager = new PanelManager();
        GameRoot.Instance.Initialize(panelManager);
    }

    public virtual void OnEnter()
    {
        GameRoot.Instance.StartCoroutine("HideMask");
    }

    public virtual void OnUpdate()
    {

    }

    public virtual void OnExit()
    {
        panelManager.PopAll();
    }


}
