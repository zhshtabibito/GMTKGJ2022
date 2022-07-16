using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[DisallowMultipleComponent]
public class CharacterBase : MonoBehaviour
{
    public Vector3 Coordinate
    {
        get { return transform.localPosition; }
        set { transform.localPosition = value; }
    }
    
    public int diceUp = 1;

    public int diceFront = 2;

    public int diceRight = 3;
    
    public int[] diceValues = new int[6]{1,2,3,4,5,6};    // dice每一面的实际值
    public int currentDiceValue => diceValues[diceUp - 1];

    protected void UpdateByKey(char key)
    {
        var coord = Coordinate;
        int oldDiceUp = diceUp;
        switch (key)
        {
            case 'w':
                coord.x--;
                Coordinate = coord;
                diceUp = diceFront;
                diceFront = 7 - oldDiceUp;
                break;
            case 's':
                coord.x++;
                Coordinate = coord;
                diceUp = 7 - diceFront;
                diceFront = oldDiceUp;
                break;
            case 'a':
                coord.z--;
                Coordinate = coord;
                diceUp = diceRight;
                diceRight = 7 - oldDiceUp;
                break;
            case 'd':
                coord.z++;
                Coordinate = coord;
                diceUp = 7 - diceRight;
                diceRight = oldDiceUp;
                break;
        }
    }
}