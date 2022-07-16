using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPanel : BasePanel
{
    static readonly string Path = "Prefabs/UI/LevelPanel";

    public LevelPanel() : base(new PanelInfo(Path))
    {

    }
}
