using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NextLevelPanel : BasePanel
{
    public int turnCount;
    static readonly string Path = "Prefabs/UI/NextLevelPanel";

    public NextLevelPanel(int c) : base(new PanelInfo(Path))
    {
        turnCount = c;
    }

    protected override void InitEvent()
    {
        ActivePanel.GetOrAddComponentInChildren<Button>("PlayBtn").onClick.AddListener(() =>
        {
            OnBtnPlay();
        });
    }

    private void OnBtnPlay()
    {
        LevelScene s = (LevelScene)GameRoot.Instance.scene;
        GameRoot.Instance.LoadScene(new LevelScene(s.LevelNum+1));
    }

    public override void OnEnter()
    {
        base.OnEnter();
        GameObject.Find("TurnCntText").GetComponent<TMP_Text>().text = $"You've Rolled: {turnCount}";
    }

    public override void OnChange(BasePanel newPanel)
    {
        base.OnChange(newPanel);
        turnCount = ((NextLevelPanel)newPanel).turnCount;

    }


}
