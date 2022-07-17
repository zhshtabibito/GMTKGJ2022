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
            panelManager.Push(new ToastPanel("If the UPWARD number is greater, you can defeat the enemy!\n Try HOLD SHIFT key and move!"));
        else if (LevelNum == 2)
            panelManager.Push(new ToastPanel("First get the WEAPON, then the FRIEND"));
        else if (LevelNum == 5)
            panelManager.Push(new ToastPanel("Weapon and Friend can only help for ONCE!"));
    }
}
