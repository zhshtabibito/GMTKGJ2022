using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Equip
{
    public char _operator = ' '; // +-*/
    public int attackDistanceType;  // 0-剑 1-弓
    public List<Vector2Int> attackRelativeGrids = new List<Vector2Int>();  // 相对坐标列表
}

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

    GameMap map;
    private Camera camera;
    private Equip[] equips = new Equip[6]{null, null, null, null, null, null};
    private int[] friends = new int[6]{0,0,0,0,0,0};
    private int turnCount;
     bool pause;
    private Record record;
    private List<BaseGrid> coloredGrids = new List<BaseGrid>();

    public AudioClip GroundFall;
    public AudioClip ColdWeapen;
    public AudioClip HotWeapon;
    public AudioClip GetItem;
    public AudioClip Move;
    private AudioSource Audio;
    public Transform body;
    public Transform weaponSword;
    public Transform weaponBow;
    public Transform weaponGunzi;
    public Transform weaponGun;

    class Record
    {
        public Vector3 coordinate;
        public int diceUp;
        public int diceFront;
        public int diceRight;
        public int[] diceValues;
        public Quaternion diceRotation;
        public GameObject stillGo;
    }

    void Start()
    {
        map = FindObjectOfType<GameMap>();
        camera = Camera.main;
        foreach(var i in equips)
        RefreshDiceHintUI();

        Audio = GetComponent<AudioSource>();
    }

    private float lastTime;
    // Update is called once per frame
    void Update()
    {
        if (lastTime < 0.5f)
        {
            lastTime += Time.deltaTime;
            return;
        }

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
            body.forward = Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.S)|| Input.GetKeyDown(KeyCode.DownArrow))
        {
            nextGrid = map.GetGrid((int) Coordinate.x + 1, (int)Coordinate.z);
            key = 's';
            body.forward = Vector3.right;
        }
        else if (Input.GetKeyDown(KeyCode.A)|| Input.GetKeyDown(KeyCode.LeftArrow))
        {
            nextGrid = map.GetGrid((int) Coordinate.x, (int)Coordinate.z - 1);
            key = 'a';
            body.forward = Vector3.back;
        }
        else if (Input.GetKeyDown(KeyCode.D)|| Input.GetKeyDown(KeyCode.RightArrow))
        {
            nextGrid = map.GetGrid((int) Coordinate.x, (int)Coordinate.z + 1);
            key = 'd';
            body.forward = Vector3.forward;
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
            record.diceRotation = dice.localRotation;
            record.stillGo = Instantiate(gameObject);
            Destroy(record.stillGo.GetComponent<PlayerController>());
            var renders = GetComponentsInChildren<Renderer>();
            var block = new MaterialPropertyBlock();
            var colorId = Shader.PropertyToID("_Color");
            foreach (var r in renders)
            {
                r.GetPropertyBlock(block);
                block.SetColor(colorId, new Color(1,1,1,0.5f));
                r.SetPropertyBlock(block);
            }
            RefreshDiceHintUI();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            RestoreRecord();
        }

        if (nextGrid != null && nextGrid.isWalkable())
        {
            if (record == null)
            {
                var lastGrid = map.GetGrid((int) Coordinate.x, (int) Coordinate.z);
                lastGrid.onLeave(true);
            }
            UpdateByKey(key);
            lastTime = 0;
            validTurn = true;
            if (record == null)
            {
                nextGrid.onEnter(true);
                var func = nextGrid.GetComponentInChildren<GridFunction>();

                Audio.PlayOneShot(GroundFall);
                Audio.PlayOneShot(Move);

                if (func != null)
                {
                    ProcessGridFunction(func, nextGrid);
                }
            }
        }

        if (validTurn)
        {
            
            //camera.transform.position = new Vector3(transform.position.x + 4.5f, camera.transform.position.y, camera.transform.position.z);
            RefreshDiceHintUI();
            RefreshEquipFriendUI();

            if (record == null) // 没有使用技能时
            {
                foreach (var g in coloredGrids)
                {
                    g.UnColorAttackRange();
                }
                coloredGrids = GetAllAttackRangeGrids();
                bool defeatAll = true;
                bool fail = false;
                turnCount++;
                foreach (var g in coloredGrids)
                {
                    g.RemoveHinder();
                    g.ColorAttackRange();
                }
                foreach (var monster in FindObjectsOfType<MonsterController>())
                {
                    monster.Move();
                    bool inAttackRange = false;
                    foreach (var g in coloredGrids)
                    {
                        if (monster.Coordinate.x == g.gridIndex.x && monster.Coordinate.z == g.gridIndex.y)
                        {
                            inAttackRange = true;
                            break;
                        }
                    }
                    if ((monster.Coordinate == Coordinate || inAttackRange) && !monster.hasDefeat)
                    {
                        PlayState("棒子");
                        if (monster.Battle(currentDiceValue))
                        {
                            diceValues[diceUp - 1] = diceUp;
                            equips[diceUp - 1] = null;
                            friends[diceUp - 1] = 0;
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
                    LeanTween.delayedCall(gameObject, 1, () => PanelManager.Instance.Push(new ReplayPanel(turnCount)));
                }
                else if (defeatAll)// 通关
                {
                    pause = true;
                    LeanTween.delayedCall(gameObject, 1, () => PanelManager.Instance.Push(new NextLevelPanel(turnCount)));
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

    private void RestoreRecord()
    {
        if (record == null)
            return;
        diceFront = record.diceFront;
        diceUp = record.diceUp;
        diceRight = record.diceRight;
        diceValues = record.diceValues;
        Coordinate = record.coordinate;
        dice.localRotation = record.diceRotation;
        Destroy(record.stillGo);
        record = null;
        var renders = GetComponentsInChildren<Renderer>();
        var block = new MaterialPropertyBlock();
        var colorId = Shader.PropertyToID("_Color");
        foreach (var r in renders)
        {
            r.GetPropertyBlock(block);
            block.SetColor(colorId, Color.white);
            r.SetPropertyBlock(block);
        }

        RefreshDiceHintUI();
    }

    private void ProcessGridFunction(GridFunction func, BaseGrid nextGrid)
    {
        if (func.functionState == 0)
        {
            diceValues[diceUp - 1] = nextGrid.Settlement(currentDiceValue, string.Empty);
            upTipText.text = func.functionOperator.ToString() + func.functionOperand.ToString();
            func.Performance();
            LeanTween.delayedCall(gameObject, 1, () => upTipText.text = "");
        }
        else if (func.functionState == 1) //装备
        {
            if (friends[diceUp - 1] == 0) // 仅当没有伙伴时
            {
                equips[diceUp - 1] = new Equip();
                equips[diceUp - 1]._operator = func.functionOperator;
                equips[diceUp - 1].attackDistanceType = func.attackDistanceType;
                equips[diceUp - 1].attackRelativeGrids = func.attackRelativeGrids;
                func.Performance();
            }
        }
        else if (func.functionState == 2)
        {
            if (equips[diceUp - 1] != null && friends[diceUp - 1] == 0) // 仅当有装备没有伙伴时
            {
                diceValues[diceUp - 1] = nextGrid.Settlement(currentDiceValue, equips[diceUp - 1]._operator.ToString());
                friends[diceUp - 1] = func.functionOperand;
                func.Performance();
                upTipText.text = equips[diceUp - 1]._operator.ToString() + friends[diceUp - 1].ToString();
                LeanTween.delayedCall(gameObject, 1, () => upTipText.text = "");
            }
        }
    }

    void UseEquip()
    {
        if (equips[diceUp - 1] == null)
        {
            PlayState("walk");
            weaponSword.gameObject.SetActive(false);
            weaponBow.gameObject.SetActive(false);
            weaponGunzi.gameObject.SetActive(false);
            weaponGun.gameObject.SetActive(false);
            return;
        }

        int type = equips[diceUp - 1].attackDistanceType;
        
        weaponSword.gameObject.SetActive(type == 0);
        weaponBow.gameObject.SetActive(type == 1);
        weaponGunzi.gameObject.SetActive(type == 2);
        weaponGun.gameObject.SetActive(type == 3);
        
        switch (type)
        {
            case 0:
                PlayState("棒子");
                Audio.PlayOneShot(ColdWeapen);
                break;
            case 1:
                PlayState("射箭");
                Audio.PlayOneShot(ColdWeapen);
                break;
            case 2:
                PlayState("棒子");
                Audio.PlayOneShot(ColdWeapen);
                break;
            case 3:
                PlayState("开枪");
                Audio.PlayOneShot(HotWeapon);
                break;
        }
        
    }

    List<BaseGrid> GetAllAttackRangeGrids()
    {
        UseEquip();
        
        var results = new List<BaseGrid>();
        if (equips[diceUp - 1] == null || equips[diceUp - 1].attackRelativeGrids == null ||
            equips[diceUp - 1].attackRelativeGrids.Count == 0)
            return results;
        
        foreach (var g in equips[diceUp - 1].attackRelativeGrids)
        {
            var grid = map.GetGrid((int) Coordinate.x + g.x, (int) Coordinate.z + g.y);
            if (grid != null)
            {
                results.Add(grid);
            }
        }

        return results;
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
        GameObject.Find("HudAll")?.GetComponent<EquipHud>()?.Refresh(equips[diceUp - 1], friends[diceUp - 1]);        
    }
    
    void OnApplicationFocus(bool hasFocus)
    {
        RestoreRecord();
    }
}
