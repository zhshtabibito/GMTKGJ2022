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

        SavingManager.Instance.SaveGame(LevelNum);

    }

    public override void OnEnter()
    {
        base.OnEnter();
        panelManager.Push(new LevelPanel());
        if(LevelNum == 1)
            panelManager.Push(new ToastPanel("第1关的教程，子文快写"));
        else if (LevelNum == 2)
            panelManager.Push(new ToastPanel("第2关的教程，子文快写"));
        else if (LevelNum == 4)
            panelManager.Push(new ToastPanel("第4关的教程，子文快写"));
    }
}
