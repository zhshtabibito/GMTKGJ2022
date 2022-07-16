using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelPanel : BasePanel
{
    static readonly string Path = "Prefabs/UI/HudAll";

    public LevelPanel() : base(new PanelInfo(Path))
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        PlayerController player = GameObject.Find("Player").GetComponent<PlayerController>();

        player.frontText = ActivePanel.Find("Num(1)").GetComponent<TMP_Text>();
        player.leftText = ActivePanel.Find("Num(2)").GetComponent<TMP_Text>();
        player.upText = ActivePanel.Find("Num(3)").GetComponent<TMP_Text>();
        player.rightText = ActivePanel.Find("Num(4)").GetComponent<TMP_Text>();
        player.backText = ActivePanel.Find("Num(5)").GetComponent<TMP_Text>();
        player.bottomText = ActivePanel.Find("Num(6)").GetComponent<TMP_Text>();

    }
}
