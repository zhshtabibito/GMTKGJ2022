using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonsterController : CharacterBase
{
    public string route = "";
    

    public enum Operater
    {
        Greater,
        Equal,
        Less
    }

    public Operater operater;
    public int number;
    public bool isDice = false;


    public bool hasDefeat { get; private set; }
    private int currentRouteIndex = 0;

    public void Move()
    {
        if (hasDefeat)
            return;
        if (string.IsNullOrEmpty(route))
            return;
        
        var currentKey = route[currentRouteIndex % route.Length];
        UpdateByKey(currentKey);
        currentRouteIndex++;
        PlayWalk();
    }

    public bool Battle(int playerNum)
    {
        PlayAttack();
        bool win = false;
        switch (operater)
        {
            case Operater.Greater:
                win = isDice? playerNum > currentDiceValue : playerNum > number;
                break;
            case Operater.Less:
                win = isDice? playerNum < currentDiceValue : playerNum < number;
                break;
            case Operater.Equal:
                win = isDice? playerNum == currentDiceValue : playerNum == number;
                break;
        }

        if (win)
        {
            hasDefeat = true;
            LeanTween.delayedCall(2f, () => Destroy(gameObject));
        }

        return win;
    }

    private void Start()
    {
        GetComponentInChildren<TMP_Text>().text = $">{number}";
        GetComponentInChildren<TMP_Text>().color = Color.red;
    }
}