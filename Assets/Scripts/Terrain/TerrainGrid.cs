using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGrid : BaseGrid
{
    [SerializeField]
    // 0=空地 1=围墙 2=不可破坏障碍 3=可破坏障碍
    private int _terrainType;

    public override bool isWalkable(bool isPlayer = true)
    {
        if (isPlayer)
            return _terrainType <= 0 && base.isWalkable(isPlayer);
        return true;
    }

    public override string GetString()
    {
        return _terrainType.ToString();
    }

    public override void ParseString(string gridData)
    {
        _terrainType = (int)float.Parse(gridData);
    }

    // 特供方法：移除障碍
    public override void RemoveHinder()
    {
        if (_terrainType == 3)
        {
            _terrainType = 0;
            foreach (var render in GetComponentsInChildren<MeshRenderer>())
                render.enabled = false;
        }
    }

    public override void Lock()
    {
        if (_terrainType == 0) base.Lock();
    }

    public override void UnLock()
    {
        if (_terrainType == 0) base.UnLock();
    }
}
