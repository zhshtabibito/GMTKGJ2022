using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelPanel : BasePanel
{
    static readonly string Path = "Prefabs/UI/HudAll";

    public LevelPanel() : base(new PanelInfo(Path))
    {

    }

    protected override void InitEvent()
    {
        ActivePanel.GetOrAddComponentInChildren<Button>("BackBtn").onClick.AddListener(() =>
        {
            Push(new ReplayPanel(0));
        });
    }


    public override void OnEnter()
    {
        base.OnEnter();
        // PlayerController player = GameObject.Find("Player(Clone)").GetComponent<PlayerController>();
        PlayerController player = GameObject.Find("Player").GetComponent<PlayerController>();

        player.frontText = GameObject.Find("Num(1)").GetComponent<TMP_Text>();
        player.leftText = GameObject.Find("Num(2)").GetComponent<TMP_Text>();
        player.upText = GameObject.Find("Num(3)").GetComponent<TMP_Text>();
        player.rightText = GameObject.Find("Num(4)").GetComponent<TMP_Text>();
        player.backText = GameObject.Find("Num(5)").GetComponent<TMP_Text>();
        player.bottomText = GameObject.Find("Num(6)").GetComponent<TMP_Text>();
        player.upTipText = GameObject.Find("NumUp(1)").GetComponent<TMP_Text>();
        player.downTipText = GameObject.Find("NumDown(1)").GetComponent<TMP_Text>();
        player.upTipText.text = "";
        player.downTipText.text = "";

        MonsterController[] ML = GameObject.FindObjectsOfType<MonsterController>();
        int MCnt = ML.Length;
        string goal = ML[0].number.ToString();
        for (int i = 1; i < MCnt; i++)
        {
            goal += (", " + ML[i].ToString());
        }

        GameObject.Find("BossValue").GetComponentInChildren<TMP_Text>().text = "GOAL: > <color=#FFE6B5>" + goal + "</color>";

    }
}
