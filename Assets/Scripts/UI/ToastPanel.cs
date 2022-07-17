using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToastPanel : BasePanel
{
    public string content;
    static readonly string Path = "Prefabs/UI/ToastBtn";

    public ToastPanel(string s = "") : base(new PanelInfo(Path))
    {
        content = s;
    }

    protected override void InitEvent()
    {
        ActivePanel.GetComponent<Button>().onClick.AddListener(() =>
        {
            Pop();
        });

    }

    
    public override void OnEnter()
    {
        base.OnEnter();
        GameObject.Find("ToastText").GetComponent<TMP_Text>().text = content;
    }

    public override void OnChange(BasePanel newPanel)
    {
        base.OnChange(newPanel);
        content = ((ToastPanel)newPanel).content;
        GameObject.Find("ToastText").GetComponent<TMP_Text>().text = content;

    }
}
