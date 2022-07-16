using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonsterController : CharacterBase
{
    public string route = "";
    private int currentRouteIndex = 0;

    public void Move()
    {
        if (string.IsNullOrEmpty(route))
            return;
        
        var currentKey = route[currentRouteIndex % route.Length];
        UpdateByKey(currentKey);
        currentRouteIndex++;
    }

}