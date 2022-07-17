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
            panelManager.Push(new ToastPanel("��1�صĽ̳̣����Ŀ�д"));
        else if (LevelNum == 2)
            panelManager.Push(new ToastPanel("��2�صĽ̳̣����Ŀ�д"));
        else if (LevelNum == 4)
            panelManager.Push(new ToastPanel("��4�صĽ̳̣����Ŀ�д"));
    }
}
