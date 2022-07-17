using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridFunction: MonoBehaviour
{
    public char functionOperator;
    public int functionOperand;
    public int functionState;  // 0-药水 1-装备 2-伙伴
    public int attackDistanceType;  // 0=+(剑) 1=*(弓) 2=-(棍) 3=/(枪)
    public List<Vector2Int> attackRelativeGrids = new List<Vector2Int>();  // 相对坐标列表
    public int needFunctionState
    {
        get
        {
            if (functionState <= 0)
                return 0;
            return 3-functionState;
        }
    }

    public void SetInfo(int functionState, string functionData)
    {
        var data = functionData.Split('}');
        this.functionState = functionState;
        functionOperator = data[0].Length > 1 ? data[0][1] : '.';
        functionOperand = data[1].Length > 1 ? int.Parse(data[1][1..]) : 0;
        if (data.Length > 2 && data[2].Length > 1)
        {
            switch (data[2][1])
            {
                case '+':
                    attackDistanceType = 0;
                    break;
                case '*':
                    attackDistanceType = 1;
                    break;
                case '-':
                    attackDistanceType = 2;
                    break;
                default:
                    attackDistanceType = 3;
                    break;
            }
            attackRelativeGrids.Clear();
            var relativeGridDataList = data[2][2..].Split(')');
            foreach(var relativeGridData in relativeGridDataList)
            {
                if (relativeGridData.Length <= 2) continue;
                var indsData = relativeGridData[1..].Split(';');
                attackRelativeGrids.Add(new Vector2Int(int.Parse(indsData[0]), int.Parse(indsData[1])));
            }
        }
    }

    public string AsHelper()
    {
        if (functionState == 1) return functionOperator.ToString();
        if (functionState == 2) return functionOperand.ToString();
        return "";
    }

    public int Settlement(int avatarOperand, string Helper)
    {
        int result = avatarOperand;
        switch (functionState)
        {
            case 0:
                result = SettleHelper(functionOperator, avatarOperand, functionOperand);
                break;
            case 1:
                result = SettleHelper(functionOperator, avatarOperand, int.Parse(Helper));
                break;
            case 2:
                result = SettleHelper(Helper[0], avatarOperand, functionOperand);
                break;
        }
        return result;
    }

    static int SettleHelper(char operator1, int operand1, int operand2)
    {
        if (operator1 == '+')
            return operand1 + operand2;
        if (operator1 == '-')
            return operand1 - operand2;
        if (operator1 == '*')
            return operand1 * operand2;
        if (operator1 == '/')
            return operand1 / operand2;
        return 0;
    }
}
