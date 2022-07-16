using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticePanel : BasePanel
{
    static readonly string Path = "Prefabs/UI/NextLevelPanel";

    public NoticePanel() : base(new PanelInfo(Path))
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
        GameRoot.Instance.LoadScene(new StartScene());
    }
}
