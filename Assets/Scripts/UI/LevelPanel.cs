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
        PlayerController player = GameObject.Find("Player(Clone)").GetComponent<PlayerController>();

        player.frontText = GameObject.Find("Num(1)").GetComponent<TMP_Text>();
        player.leftText = GameObject.Find("Num(2)").GetComponent<TMP_Text>();
        player.upText = GameObject.Find("Num(3)").GetComponent<TMP_Text>();
        player.rightText = GameObject.Find("Num(4)").GetComponent<TMP_Text>();
        player.backText = GameObject.Find("Num(5)").GetComponent<TMP_Text>();
        player.bottomText = GameObject.Find("Num(6)").GetComponent<TMP_Text>();

    }
}
