using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearGrid : BaseGrid
{
    public string gridData;

    protected override void Start()
    {
        base.Start();
    }

    public virtual void onEnter(bool isPlayer)
    {
        // 进入时触发格子表现
        base.onEnter(isPlayer);
        var gameMap = GetComponentInParent<GameMap>();
        if (gameMap)
            foreach(var gridIndex in relatedGrids)
            {
                BaseGrid targetGrid = gameMap.GetGrid(gridIndex.x, gridIndex.y);
                targetGrid.UnLock();
            }
    }

    // 读/配表数据相关
    public override void ParseString(string gridData)
    {
        this.gridData = gridData;
        relatedGrids = new List<Vector2Int>();
        var gridDataIndexList = gridData.Split(")");
        foreach (var gridDataIndex in gridDataIndexList)
        {
            if (gridDataIndex.Length <= 2) continue;
            var gridDataIntList = gridDataIndex[1..].Split(';');
            relatedGrids.Add(new Vector2Int(
                int.Parse(gridDataIntList[0])+gridIndex.x,
                int.Parse(gridDataIntList[1])+gridIndex.y));
        }
    }

    public override void Lock()
    {
        // 屏蔽Lock
    }

    public override void UnLock()
    {
        // 屏蔽Unlock
    }
}
