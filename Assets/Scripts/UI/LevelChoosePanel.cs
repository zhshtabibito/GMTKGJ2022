using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelChoosePanel : BasePanel
{
    static readonly string Path = "Prefabs/UI/LevelPanel";

    public LevelChoosePanel() : base(new PanelInfo(Path))
    {

    }

    protected override void InitEvent()
    {
        ActivePanel.GetOrAddComponentInChildren<Button>("LevelBtn(1)").onClick.AddListener(() =>
        {
            GameRoot.Instance.LoadScene(new LevelScene(1));
        });
        ActivePanel.GetOrAddComponentInChildren<Button>("LevelBtn(2)").onClick.AddListener(() =>
        {
            GameRoot.Instance.LoadScene(new LevelScene(2));
        });
        ActivePanel.GetOrAddComponentInChildren<Button>("LevelBtn(3)").onClick.AddListener(() =>
        {
            GameRoot.Instance.LoadScene(new LevelScene(3));
        });
        ActivePanel.GetOrAddComponentInChildren<Button>("LevelBtn(4)").onClick.AddListener(() =>
        {
            GameRoot.Instance.LoadScene(new LevelScene(4));
        });
        ActivePanel.GetOrAddComponentInChildren<Button>("LevelBtn(5)").onClick.AddListener(() =>
        {
            GameRoot.Instance.LoadScene(new LevelScene(5));
        });
        ActivePanel.GetOrAddComponentInChildren<Button>("LevelBtn(6)").onClick.AddListener(() =>
        {
            GameRoot.Instance.LoadScene(new LevelScene(6));
        });
        ActivePanel.GetOrAddComponentInChildren<Button>("LevelBtn(7)").onClick.AddListener(() =>
        {
            GameRoot.Instance.LoadScene(new LevelScene(7));
        });
        ActivePanel.GetOrAddComponentInChildren<Button>("LevelBtn(8)").onClick.AddListener(() =>
        {
            GameRoot.Instance.LoadScene(new LevelScene(8));
        });
        ActivePanel.GetOrAddComponentInChildren<Button>("LevelBtn(9)").onClick.AddListener(() =>
        {
            GameRoot.Instance.LoadScene(new LevelScene(9));
        });
        ActivePanel.GetOrAddComponentInChildren<Button>("LevelBtn(10)").onClick.AddListener(() =>
        {
            GameRoot.Instance.LoadScene(new LevelScene(10));
        });
        ActivePanel.GetOrAddComponentInChildren<Button>("CloseBtn").onClick.AddListener(() =>
        {
            Pop();
        });
    }

    public override void OnEnter()
    {
        base.OnEnter();

        int SL = Mathf.Min(SavingManager.Level, 10);
        for (int i = 1; i <= SL; i++)
        {
            GameObject.Find($"LevelBtn({i})").GetComponent<Button>().interactable = true;
        }
        for (int i = SL+1; i <= 10; i++)
        {
            GameObject.Find($"LevelBtn({i})").GetComponent<Button>().interactable = false;
        }

    }

}
