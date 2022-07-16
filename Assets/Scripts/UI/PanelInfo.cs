using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI����, �洢UI�������Լ�·��
/// </summary>
public class PanelInfo
{
    /// <summary>
    /// UI����
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// UI·��
    /// </summary>
    public string Path { get; private set; }
    /// <summary>
    /// �Ƿ�ִ�й�Init()
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
