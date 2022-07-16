using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartPanel : BasePanel
{
    static readonly string Path = "Prefabs/UI/StartPanel";

    private Transform Chapters;

    public StartPanel() : base(new PanelInfo(Path))
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
        // TODO: load, if new, directly load level1
        GameRoot.Instance.LoadScene(new LevelScene(1));
    }

    public override void OnEnter()
    {
        base.OnEnter();

    }

}
