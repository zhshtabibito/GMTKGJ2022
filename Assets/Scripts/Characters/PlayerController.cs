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
    public TMP_Text upTipText;
    public TMP_Text downTipText;
    public Image equipImg;
    public Image friendImg;
    
    GameMap map;
    private Camera camera;
    private char[] equips = new char[6]{' ',' ',' ',' ',' ',' ',};
    private int[] friends = new int[6]{0,0,0,0,0,0};
    private int turnCount;
    private bool pause;
    private Record record;

    class Record
    {
        public Vector3 coordinate;
        public int diceUp;
        public int diceFront;
        public int diceRight;
        public int[] diceValues;
    }


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
        else if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            record = new Record();
            record.diceFront = diceFront;
            record.diceUp = diceUp;
            record.diceRight = diceRight;
            record.diceValues = diceValues;
            record.coordinate = Coordinate;
            RefreshDiceHintUI();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            diceFront = record.diceFront;
            diceUp = record.diceUp;
            diceRight = record.diceRight;
            diceValues = record.diceValues;
            Coordinate = record.coordinate;
            record = null;
            RefreshDiceHintUI();
        }

        if (nextGrid != null && nextGrid.isWalkable())
        {
            if (record == null)    // 没有使用技能时
            {
                var lastGrid = map.GetGrid((int) Coordinate.x, (int)Coordinate.z);
                lastGrid.onLeave(true);
                nextGrid.onEnter(true);
                var func = nextGrid.GetComponentInChildren<GridFunction>();
                if (func != null)
                {
                    if (func.functionState == 0)
                    {
                        diceValues[diceUp - 1] = nextGrid.Settlement(currentDiceValue, string.Empty);
                        Destroy(func.gameObject);
                        upTipText.text = func.functionOperator.ToString() + func.functionOperand.ToString();
                        LeanTween.delayedCall(gameObject, 1, () => upTipText.text = "");
                    }
                    else if (func.functionState == 1)//装备
                    {
                        equips[diceUp - 1] = func.functionOperator;
                        Destroy(func.gameObject);
                    }
                    else if (func.functionState == 2)
                    {
                        if (equips[diceUp - 1] != ' ' && friends[diceUp - 1] == 0)
                        {
                            diceValues[diceUp - 1] = nextGrid.Settlement(currentDiceValue, equips[diceUp - 1].ToString());
                            friends[diceUp - 1] = func.functionOperand;
                            Destroy(func.gameObject);
                            upTipText.text = equips[diceUp - 1].ToString() + friends[diceUp - 1].ToString();
                            LeanTween.delayedCall(gameObject, 1, () => upTipText.text = "");
                        }
                    }
                }
            }
            UpdateByKey(key);
            validTurn = true;
        }

        if (validTurn)
        {
            //camera.transform.position = new Vector3(transform.position.x + 4.5f, camera.transform.position.y, camera.transform.position.z);
            RefreshDiceHintUI();
            RefreshEquipFriendUI();
            if (record == null) // 没有使用技能时
            {
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
                            downTipText.text = "↓↓↓";
                            LeanTween.delayedCall(gameObject, 1, () => downTipText.text = "");
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
                if (fail)// 失败，次数turnCount
                {
                    pause = true;
                    PanelManager.Instance.Push(new ReplayPanel(turnCount));
                }
                else if (defeatAll)// 通关
                {
                    pause = true;
                    PanelManager.Instance.Push(new NextLevelPanel(turnCount));
                }
                else// 判断无路可走
                {
                    var up = map.GetGrid((int) Coordinate.x - 1, (int)Coordinate.z);
                    var down = map.GetGrid((int) Coordinate.x + 1, (int)Coordinate.z);
                    var left = map.GetGrid((int) Coordinate.x, (int)Coordinate.z-1);
                    var right = map.GetGrid((int) Coordinate.x, (int)Coordinate.z+1);

                    bool canWalk = (up != null && up.isWalkable()) || (down != null && down.isWalkable()) ||
                                   (left != null && left.isWalkable()) || (right != null && right.isWalkable());
                    if (!canWalk)// 失败，次数turnCount
                    {
                        pause = true;
                        PanelManager.Instance.Push(new ReplayPanel(turnCount));
                    }
                }
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
        GameObject.Find("HudAll").GetComponent<EquipHud>().Refresh(equips[diceUp - 1], friends[diceUp - 1]);        
    }
}
