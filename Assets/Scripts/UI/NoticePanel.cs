using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticePanel : BasePanel
{
    static readonly string Path = "Prefabs/UI/NoticePanel";

    public NoticePanel() : base(new PanelInfo(Path))
    {

    }

    protected override void InitEvent()
    {
        ActivePanel.GetOrAddComponentInChildren<Button>("CloseBtn").onClick.AddListener(() =>
        {
            Pop();
        });
        ActivePanel.GetOrAddComponentInChildren<Button>("LeaveBtn").onClick.AddListener(() =>
        {
            GameRoot.Instance.LoadScene(new StartScene());
        });
        ActivePanel.GetOrAddComponentInChildren<Button>("PlayBtn").onClick.AddListener(() =>
        {
            LevelScene s = (LevelScene)GameRoot.Instance.scene;
            GameRoot.Instance.LoadScene(new LevelScene(s.LevelNum));
        });

    }

    private void OnBtnPlay()
    {
        
    }
}
