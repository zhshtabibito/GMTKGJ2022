using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGrid : BaseGrid
{
    // 0=空地 1=围墙 2=不可破坏障碍 3=可破坏障碍
    private int _terrainType;

    public override void SetInfo(Vector2Int gridIndex, string gridData)
    {
        base.SetInfo(gridIndex, gridData);
        if (_terrainType == 2 || _terrainType == 3)
            transform.Translate(0, 0.5f, 0);
    }

    public override bool isWalkable(bool isPlayer = true)
    {
        if (isPlayer)
            return _terrainType <= 0 && !isDestructed;
        return true;
    }

    public override int Settle(int number)
    {
        return number;
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
    public void RemoveHinder()
    {
        if (_terrainType == 3)
        {
            transform.Translate(0, -0.5f, 0);
            _terrainType = 0;
        }
        // do some performance here
    }
}
