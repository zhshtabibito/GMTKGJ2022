using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndPanel : BasePanel
{
    static readonly string Path = "Prefabs/UI/EndPanel";

    public EndPanel(string s = "") : base(new PanelInfo(Path))
    {

    }

    protected override void InitEvent()
    {
        ActivePanel.GetOrAddComponentInChildren<Button>("CloseBtn").onClick.AddListener(() =>
        {
            Pop();
        });

    }

    public override void OnEnter()
    {
        base.OnEnter();

    }
}
