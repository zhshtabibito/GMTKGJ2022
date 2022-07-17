using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReplayPanel : BasePanel
{
    public int turnCount;
    static readonly string Path = "Prefabs/UI/ReplayPanel";

    public ReplayPanel(int c) : base(new PanelInfo(Path))
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
        GameRoot.Instance.LoadScene(new LevelScene(s.LevelNum));
    }

    public override void OnEnter()
    {
        base.OnEnter();
        GameObject.Find("TurnCntText").GetComponent<TMP_Text>().text = $"You've Rolled: \n\n< color =#FFE6B5>{turnCount}</color>";
    }

    public override void OnChange(BasePanel newPanel)
    {
        base.OnChange(newPanel);
        turnCount = ((NextLevelPanel)newPanel).turnCount;

    }
}
