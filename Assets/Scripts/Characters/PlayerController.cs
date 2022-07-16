using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : CharacterBase
{

    public TMP_Text upText;
    public TMP_Text bottomText;
    public TMP_Text rightText;
    public TMP_Text leftText;
    public TMP_Text frontText;
    public TMP_Text backText;
    public GameMap map;

    // Update is called once per frame
    void Update()
    {
        var pos = Coordinate;
        bool validTurn = false;    // TODO: 判断所有无效输入
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            var grid = map.GetGrid((int) Coordinate.x - 1, (int)Coordinate.z);
            if (grid != null && grid.isWalkable())
            {
                UpdateByKey('w');
                validTurn = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.S)|| Input.GetKeyDown(KeyCode.DownArrow))
        {
            var grid = map.GetGrid((int) Coordinate.x + 1, (int)Coordinate.z);
            if (grid != null && grid.isWalkable())
            {
                UpdateByKey('s');
                validTurn = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.A)|| Input.GetKeyDown(KeyCode.LeftArrow))
        {
            var grid = map.GetGrid((int) Coordinate.x, (int)Coordinate.z - 1);
            if (grid != null && grid.isWalkable())
            {
                UpdateByKey('a');
                validTurn = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D)|| Input.GetKeyDown(KeyCode.RightArrow))
        {
            var grid = map.GetGrid((int) Coordinate.x, (int)Coordinate.z + 1);
            if (grid != null && grid.isWalkable())
            {
                UpdateByKey('d');
                validTurn = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            validTurn = true;
        }


        if (validTurn)
        {
//            if (pos.x == 1 && pos.z == 1)
//            {
//                diceValues[diceUp - 1] *= 2;
//            }
//            if (pos.x == 1 && pos.z == 2)
//            {
//                diceValues[diceUp - 1] += 2;
//            }
//

            foreach (var monster in FindObjectsOfType<MonsterController>())
            {
                monster.Move();
            }
        }

        
        RefreshDiceHintUI();

    }

    void RefreshDiceHintUI()
    {
        if (upText == null)
        {
            return;
        }

        upText.text = diceValues[diceUp-1].ToString();
        frontText.text = diceValues[diceFront-1].ToString();
        rightText.text = diceValues[diceRight-1].ToString();
        bottomText.text = diceValues[7-diceUp-1].ToString();
        leftText.text = diceValues[7-diceRight-1].ToString();
        backText.text = diceValues[7-diceFront-1].ToString();
    }

}