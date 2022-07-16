using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionalGrid : BaseGrid
{
    [SerializeField]
    private bool _hasOperator;  // 药水和装备填true
    [SerializeField]
    private bool _hasOperand;  // 药水和伙伴填true
    [SerializeField]
    char _operator;
    [SerializeField]
    int _operand;

    public override bool needOperator()
    {
        return !_hasOperator;
    }

    public override bool needOperand()
    {
        return !_hasOperand;
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

    public override int Settle(int number)
    {
        return number;
    }

    public override void ParseString(string gridData)
    {
        var data = gridData.Split('}');
        _hasOperator = data[0].Length > 1;
        if (_hasOperator)
            _operator = data[0][1];
        _hasOperand = data[1].Length > 1;
        if (_hasOperand)
            _operand = (int)float.Parse(data[1].Substring(1));
    }
}
