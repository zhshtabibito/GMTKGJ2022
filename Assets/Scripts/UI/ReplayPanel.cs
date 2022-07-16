using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplayPanel : BasePanel
{
    static readonly string Path = "Prefabs/UI/NextLevelPanel";

    public ReplayPanel() : base(new PanelInfo(Path))
    {

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
}
