using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[DisallowMultipleComponent]
public class CharacterBase : MonoBehaviour
{
    public Transform dice;
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
                PlayIdle();
                if (dice)
                {
                    dice.Rotate(0,0,90, Space.World);
                }
                break;
            case 's':
                coord.x++;
                Coordinate = coord;
                diceUp = 7 - diceFront;
                diceFront = oldDiceUp;
                PlayWalk();
                if (dice)
                {
                    dice.Rotate(0, 0, -90, Space.World);
                }
                break;
            case 'a':
                coord.z--;
                Coordinate = coord;
                diceUp = diceRight;
                diceRight = 7 - oldDiceUp;
                PlayWalk();
                if (dice)
                {
                    dice.Rotate(-90, 0, 0, Space.World);
                }
                break;
            case 'd':
                coord.z++;
                Coordinate = coord;
                diceUp = 7 - diceRight;
                diceRight = oldDiceUp;
                PlayAttack();
                if (dice)
                {
                    dice.Rotate(90, 0, 0, Space.World);
                }
                break;
        }
    }
    public void PlayState(string state)
    {
        GetComponentInChildren<Animator>()?.Play(state);
    }
    public void PlayIdle()
    {
        GetComponentInChildren<Animator>()?.ResetTrigger("attack");
        GetComponentInChildren<Animator>()?.ResetTrigger("walk");
        GetComponentInChildren<Animator>()?.SetTrigger("idle");
    }
    public void PlayWalk()
    {GetComponentInChildren<Animator>()?.ResetTrigger("idle");
        GetComponentInChildren<Animator>()?.ResetTrigger("attack");
        GetComponentInChildren<Animator>()?.SetTrigger("walk");
    }
    public void PlayAttack()
    {
        GetComponentInChildren<Animator>()?.ResetTrigger("idle");
        GetComponentInChildren<Animator>()?.ResetTrigger("walk");
        GetComponentInChildren<Animator>()?.SetTrigger("attack");
    }
}