using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGrid : MonoBehaviour
{
    //[HideInInspector]
    public Vector2Int gridIndex = new Vector2Int(0, 0);
    private bool _isDestructed = false;
    public GridFunction _function;
    public bool isLocked = false;
    public List<Vector2Int> relatedGrids;

    protected virtual void Start()
    {
        _function = GetComponentInChildren<GridFunction>();
    }

    public bool isDestructed { get { return _isDestructed; }}

    public virtual void SetInfo(Vector2Int gridIndex, string gridData)
    {
        this.gridIndex = gridIndex;
        this.ParseString(gridData);
    }

    public virtual bool isWalkable(bool isPlayer=true)
    {
        if (isPlayer)
            return !isDestructed && !isLocked;
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
            this.transform.Translate(0, -0.5f, 0);
            Debug.LogFormat("player leave grid: {0}", gridIndex);
        }
        else
            Debug.LogFormat("monster leave grid: {0}", gridIndex);
    }

    public virtual string GetFunctionalData()
    {
        // 先忽略它
        return "";
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

    public virtual string BeforeSettlement(out int needFunctionState)
    {
        string currentFunctionData = "";
        if (_function)
        {
            needFunctionState = _function.needFunctionState;
            currentFunctionData = _function.AsHelper();
        }
        else
        {
            needFunctionState = -1;
        }
        return currentFunctionData;
    }

    public virtual int Settlement(int avatarOperand, string helperData)
    {
        if (_function)
        {
            int result = _function.Settlement(avatarOperand, helperData);
            
            _function = null;
            return result;
        }
        else
            return avatarOperand;
    }

    public virtual void Lock()
    {
        if (!isLocked && !_isDestructed)
        {
            transform.Translate(0, 0.5f, 0);
            isLocked = true;
        }
    }

    public virtual void UnLock()
    {
        if (isLocked)
        {
            transform.Translate(0, -0.5f, 0);
            isLocked = false;
        }
    }

    public virtual void RemoveHinder() { }
}
