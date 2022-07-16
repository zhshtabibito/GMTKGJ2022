using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : CharacterBase
{

    public TMP_Text upText;
    public TMP_Text bottomText;
    public TMP_Text rightText;
    public TMP_Text leftText;
    public TMP_Text frontText;
    public TMP_Text backText;
    public Image equipImg;
    public Image friendImg;
    GameMap map;
    private Camera camera;
    private char[] equips = new char[6]{' ',' ',' ',' ',' ',' ',};
    private int[] friends = new int[6]{0,0,0,0,0,0};
    private int turnCount;
    private bool pause;

    void Start()
    {
        map = FindObjectOfType<GameMap>();
        camera = Camera.main;
        RefreshDiceHintUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (pause)
        {
            return;
        }

        bool validTurn = false;
        BaseGrid nextGrid = null;
        char key = ' ';
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            nextGrid = map.GetGrid((int) Coordinate.x - 1, (int)Coordinate.z);
            key = 'w';
        }
        else if (Input.GetKeyDown(KeyCode.S)|| Input.GetKeyDown(KeyCode.DownArrow))
        {
            nextGrid = map.GetGrid((int) Coordinate.x + 1, (int)Coordinate.z);
            key = 's';
        }
        else if (Input.GetKeyDown(KeyCode.A)|| Input.GetKeyDown(KeyCode.LeftArrow))
        {
            nextGrid = map.GetGrid((int) Coordinate.x, (int)Coordinate.z - 1);
            key = 'a';
        }
        else if (Input.GetKeyDown(KeyCode.D)|| Input.GetKeyDown(KeyCode.RightArrow))
        {
            nextGrid = map.GetGrid((int) Coordinate.x, (int)Coordinate.z + 1);
            key = 'd';
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            validTurn = true;
        }
        if (nextGrid != null && nextGrid.isWalkable())
        {
            var lastGrid = map.GetGrid((int) Coordinate.x, (int)Coordinate.z);
            lastGrid.onLeave(true);
            UpdateByKey(key);
            validTurn = true;
            nextGrid.onEnter(true);
            var func = nextGrid.GetComponentInChildren<GridFunction>();
            if (func != null)
            {
                if (func.functionState == 0)
                {
                    diceValues[diceUp - 1] = nextGrid.Settlement(currentDiceValue, string.Empty);
                }
                else if (func.functionState == 1)//装备
                {
                    if (friends[diceUp - 1] == 0)
                    {
                        equips[diceUp - 1] = func.functionOperator;
                    }
                    else
                    {
                        diceValues[diceUp - 1] = nextGrid.Settlement(currentDiceValue, friends[diceUp - 1].ToString());
                        friends[diceUp - 1] = 0;
                    }
                }
                else if (func.functionState == 2)
                {
                    if (equips[diceUp - 1] == ' ')
                    {
                        friends[diceUp - 1] = func.functionOperand;
                    }
                    else
                    {
                        diceValues[diceUp - 1] = nextGrid.Settlement(currentDiceValue, equips[diceUp - 1].ToString());
                        equips[diceUp - 1] = ' ';
                    }
                }
            }
        }

        if (validTurn)
        {
            camera.transform.position = new Vector3(transform.position.x + 4.5f, camera.transform.position.y, camera.transform.position.z);
            RefreshDiceHintUI();
            RefreshEquipFriendUI();
            bool defeatAll = true;
            bool fail = false;
            turnCount++;
            foreach (var monster in FindObjectsOfType<MonsterController>())
            {
                monster.Move();
                if (monster.Coordinate == Coordinate && !monster.hasDefeat)
                {
                    if (monster.Battle(currentDiceValue))
                    {
                        diceValues[diceUp - 1] = diceUp;
                    }
                    else
                    {
                        fail = true;
                        break;
                    }
                }

                if (!monster.hasDefeat)
                {
                    defeatAll = false;
                }
            }

            if (fail)
            {
                // 失败，次数turnCount
                pause = true;
                PanelManager.Instance.Push(new ReplayPanel(turnCount));
            }
            else if (defeatAll)
            {
                // 通关
                pause = true;
                PanelManager.Instance.Push(new NextLevelPanel(turnCount));
            }
        }

        
        
        
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

    void RefreshEquipFriendUI()
    {

        switch (equips[diceUp - 1])
        {
            case ' ':    // 代表没有装备
                break;
            default:
                break;
        }
        

        switch (friends[diceUp - 1])
        {
            case 0:    // 代表没有伙伴
                break;
            default:
                break;
        }
        
    }
}
