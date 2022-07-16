using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridFunction: MonoBehaviour
{
    public char functionOperator;
    public int functionOperand;
    public int functionState;  // 0-药水 1-装备 2-伙伴
    public int needFunctionState
    {
        get
        {
            if (functionState <= 0)
                return 0;
            return 3-functionState;
        }
    }
    private void Start()
    {
    }

    public void SetInfo(int functionState, string functionOperatorString, string functionOperandString)
    {
        this.functionState = functionState;
        this.functionOperator = functionOperatorString.Length > 0 ? functionOperatorString[0] : '.';
        this.functionOperand = functionOperandString.Length > 0 ? int.Parse(functionOperandString) : 0;
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
