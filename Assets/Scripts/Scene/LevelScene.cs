using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScene : SceneInfo
{
    public int LevelNum;
    // Start is called before the first frame update
    public LevelScene(int n = 0)
    {
        LevelNum = n;
        if (LevelNum == 0)
        {
            SceneName = "test";
        }
        else
        {
            SceneName = $"Level_{LevelNum}";
        }


    }

    public override void OnEnter()
    {
        panelManager.Push(new LevelPanel());
    }
}
