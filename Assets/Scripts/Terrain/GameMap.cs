using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameMap : MonoBehaviour
{
    public TextAsset mapDataFile; // 待解析的.txt数据文件
    public Vector3 terrainGridRatio = new Vector3(1.0f, 0, 0);
    public GameObject[] terrainGridPrefabs = new GameObject[4];  // 空白地形，前三个随机，第四个是墙
    public GameObject[] functionPrefabs = new GameObject[3];  // 0-药水 1-普通装备 2-伙伴
    public GameObject[] weaponPrefabs = new GameObject[4];  // 0-剑 1-弓 2-棍 3-枪(其实可以和普通装备合到一个prefab里，看美术资源情况再调整)
    public GameObject removeableHinder;
    public GameObject unremovableHinder;
    public GameObject gearButtonPrefab;
    public GameObject playerPrefab;
    public List<GameObject> monsterPrefabList;
    public bool createAvatar = false;
    [HideInInspector]
    public Vector2 sizeTotal = new Vector2Int(0, 0);
    public Vector2 gridSize = new Vector2(1, 1);
    private Dictionary<int, Dictionary<int, BaseGrid>> _basegrids;

    public BaseGrid GetGrid(int x, int z)
    {
        if (x < 0 || x > _basegrids.Count) return null;
        if (z < 0 || z > _basegrids[0].Count) return null;
        return _basegrids[x][z];
    }

    // Start is called before the first frame update
    void Start()
    {
        _basegrids = new Dictionary<int, Dictionary<int, BaseGrid>>();
        foreach (var grid in GetComponentsInChildren<BaseGrid>())
        {
            if (!_basegrids.ContainsKey(grid.gridIndex.x))
                _basegrids[grid.gridIndex.x] = new Dictionary<int, BaseGrid>();
            _basegrids[grid.gridIndex.x][grid.gridIndex.y] = grid;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public Vector2Int GetGridIndexByWorldPosition(float x, float z)
    {
        return new Vector2Int(
            (int)((transform.position.x - x) / gridSize[0]),
            (int)((transform.position.z - z) / gridSize[1])
            );
    }

    public Vector2 GetGridCenterWorldPosition(int x, int z)
    {
        return new Vector2(
            x * gridSize[0] + 0.5f,
            z * gridSize[1] + 0.5f
            );
    }

    public void ImportGameMapData()
    {
#if UNITY_EDITOR
        if (!mapDataFile) return;
        ClearChildrens();
        var lines = mapDataFile.text.Split("\n");
        List<Vector2Int> needLockGrids = new List<Vector2Int>();
        List<List<BaseGrid>> createdGrids = new List<List<BaseGrid>>();
        // 生成地块实例
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Length == 0) continue;
            var gridDatas = lines[i].Split(',');
            createdGrids.Add(new List<BaseGrid>());
            for (int j = 0; j < gridDatas.Length; j++)
            {
                // 读取地块类型
                int gridTerrainTypeNumber = 0;
                if (gridDatas[j].Length >= 1 && gridDatas[j][0] >= '0' && gridDatas[j][0] <= '3')
                {
                    gridTerrainTypeNumber = int.Parse(gridDatas[j]);
                }
                else if (gridDatas[j].Contains(')') && !gridDatas[j].Contains('}'))
                    gridTerrainTypeNumber = 4;
                // 生成地块Object
                var ratio = Random.Range(0, terrainGridRatio.x + terrainGridRatio.y + terrainGridRatio.z);
                GameObject terrainObject = null;
                if (gridTerrainTypeNumber == 1)
                {
                    terrainObject = PrefabUtility.InstantiatePrefab(terrainGridPrefabs[3], transform) as GameObject;
                }
                else if (ratio <= terrainGridRatio[0] || gridTerrainTypeNumber >= 2)
                {
                    terrainObject = PrefabUtility.InstantiatePrefab(terrainGridPrefabs[0], transform) as GameObject;
                }
                else if (ratio <= terrainGridRatio[1] + terrainGridRatio[0])
                {
                    terrainObject = PrefabUtility.InstantiatePrefab(terrainGridPrefabs[1], transform) as GameObject;
                }
                else
                {
                    terrainObject = PrefabUtility.InstantiatePrefab(terrainGridPrefabs[2], transform) as GameObject;
                }
                // terrainObject.transform.Rotate(new Vector3(0, 90 * (int)Random.Range(0, 4), 0), Space.Self);
                terrainObject.transform.Translate(new Vector3(gridSize[0] * i, 0, gridSize[1] * j), Space.World);
                // 添加地块component
                BaseGrid terrainComponent = null;
                if (gridTerrainTypeNumber < 4)
                {
                    terrainComponent = terrainObject.AddComponent<TerrainGrid>();
                    terrainComponent.SetInfo(new Vector2Int(i, j), gridTerrainTypeNumber.ToString());
                }
                else
                {
                    terrainComponent = terrainObject.AddComponent<GearGrid>();
                    terrainComponent.SetInfo(new Vector2Int(i, j), gridDatas[j]);
                    needLockGrids.AddRange(terrainComponent.relatedGrids);
                    // 机关类型头顶加一个小prefab
                    GameObject childButton = PrefabUtility.InstantiatePrefab(gearButtonPrefab, terrainObject.transform) as GameObject;
                    childButton.transform.Translate(0, 0.75f, 0);
                }
                createdGrids[i].Add(terrainComponent);
                // 创建角色
                if (createAvatar) {
                    GameObject avatar = null;
                    if (gridDatas[j][0] == 'X')
                    {
                        avatar = PrefabUtility.InstantiatePrefab(playerPrefab) as GameObject;
                        avatar.AddComponent<PlayerController>();
                    }

                    if (gridDatas[j][0] == 'Y')
                    {
                        int monsterInd = int.Parse(gridDatas[j].Substring(1));
                        if (monsterInd >= monsterPrefabList.Count)
                            Debug.LogWarningFormat("[GameMapImporter] unable to find monster prefab {0}", monsterInd);
                        else
                        {
                            avatar = PrefabUtility.InstantiatePrefab(monsterPrefabList[monsterInd]) as GameObject;
                            avatar.AddComponent<MonsterController>();
                        }
                    }
                    if (avatar)
                    {
                        avatar.transform.Translate(i, 1, j);
                        if (gridDatas[j][0] == 'Y')
                            avatar.transform.RotateAround(avatar.transform.position, new Vector3(0, 1, 0), 90);
                    }
                }
                // 生成障碍object
                GameObject obstacle = null;
                if (gridTerrainTypeNumber == 2)
                {
                    obstacle = PrefabUtility.InstantiatePrefab(unremovableHinder, terrainObject.transform) as GameObject;
                }
                else if (gridTerrainTypeNumber == 3)
                {
                    obstacle = PrefabUtility.InstantiatePrefab(removeableHinder, terrainObject.transform) as GameObject;
                }
                if (obstacle)
                    obstacle.transform.Translate(0, 0.75f, 0);
                // 生成环境物体Object
                if (gridDatas[j].Contains('}'))
                {
                    var data = gridDatas[j].Split('}');
                    bool hasOperator = data[0].Length > 1;
                    bool hasOperand = data[1].Length > 1;
                    GameObject functionObject = null;
                    int functionType = 0;
                    if (hasOperator && !hasOperand) functionType = 1;
                    else if (!hasOperator && hasOperand) functionType = 2;
                    if (data.Length > 2 && data[2].Length > 1)
                    {  // 武器实例化
                        int weaponType = 0;
                        switch (data[2][1])
                        {
                            case '*':
                                weaponType = 1;
                                break;
                            case '-':
                                weaponType = 2;
                                break;
                            case '/':
                                weaponType = 3;
                                break;
                        }
                        functionObject = PrefabUtility.InstantiatePrefab(weaponPrefabs[weaponType], terrainObject.transform) as GameObject;
                    }
                    else  // 一般装备实例化
                        functionObject = PrefabUtility.InstantiatePrefab(functionPrefabs[functionType], terrainObject.transform) as GameObject;
                    functionObject.transform.Translate(0, 1, 0);
                    // 添加环境物体功能Component
                    var gridFunction = functionObject.AddComponent<GridFunction>();
                    gridFunction.SetInfo(functionType, gridDatas[j]);
                    // 调整伙伴朝向
                    if (gridFunction.functionState == 2)
                        functionObject.transform.RotateAround(functionObject.transform.position, new Vector3(0, 1, 0), 90);
                }
            }
        }
        foreach (var needLockGridIndex in needLockGrids)
            createdGrids[needLockGridIndex.x][needLockGridIndex.y].Lock();
#endif
    }

    private void ClearChildrens()
    {
        if (this.transform.childCount > 0)
            for (int i = this.transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(this.transform.GetChild(i).gameObject);
            }
    }
}
