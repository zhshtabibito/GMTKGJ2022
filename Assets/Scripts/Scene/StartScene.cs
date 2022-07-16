using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : SceneInfo
{
    /// <summary>
    /// ¿ªÊ¼³¡¾°
    /// </summary>
    public StartScene()
    {
        SceneName = "Start";
    }

    public override void OnEnter()
    {
        panelManager.Push(new StartPanel());
    }
}
