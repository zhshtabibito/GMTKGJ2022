using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGrid : MonoBehaviour
{
    [HideInInspector]
    public Vector2Int gridIndex = new Vector2Int(0, 0);
    private bool _isDestructed = false;

    public bool isDestructed { get { return _isDestructed; }}

    public virtual bool isWalkable(bool isPlayer=true)
    {
        if (isPlayer)
            return !isDestructed;
        return true;
    }

    public virtual void onEnter(bool isPlayer)
    {
        // 进入时触发格子表现
        if (isPlayer)
            Debug.LogFormat("player enter grid: {0}", gridIndex);
        else
            Debug.LogFormat("monster enter grid: {0}", gridIndex);
    }

    public virtual void onLeave(bool isPlayer)
    {
        // 退出时触发格子表现（fall when target.tag  == player）
        if (isPlayer)
        {
            _isDestructed = true;
            Debug.LogFormat("player leave grid: {0}", gridIndex);
        }
        else
            Debug.LogFormat("monster leave grid: {0}", gridIndex);
    }

    public virtual bool needOperator()
    {
        // 伙伴类型返回true
        return false;
    }

    public virtual bool needOperand()
    {
        // 装备类型返回true
        return false;
    }

    public virtual string GetFunctionalData()
    {
        return "";
    }

    public virtual int Settle(int operand)
    {
        throw new System.NotImplementedException();
    }

    public virtual int Settle(int operand, Vector2Int helpGridIndex)
    {
        throw new System.NotImplementedException();
    }

    // 读/配表数据相关
    public virtual string GetString()
    {
        throw new System.NotImplementedException();
    }

    public virtual void ParseString(string gridData)
    {
        throw new System.NotImplementedException();
    }
}
