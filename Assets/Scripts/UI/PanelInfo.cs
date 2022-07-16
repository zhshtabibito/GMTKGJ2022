using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI类型, 存储UI的名称以及路径
/// </summary>
public class PanelInfo
{
    /// <summary>
    /// UI名称
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// UI路径
    /// </summary>
    public string Path { get; private set; }
    /// <summary>
    /// 是否执行过Init()
    /// </summary>
    public bool Init = false;

    public PanelInfo(string path)
    {
        Path = path;
        Name = Path.Substring(Path.LastIndexOf('/') + 1);
    }

    public override string ToString()
    {
        return $"name : {Name} , path : {Path}";
    }
}
