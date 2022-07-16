using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameMap : MonoBehaviour
{
    public TextAsset mapDataFile; // 待解析的.txt数据文件
    public GameObject[] terrainGridPrefabs = new GameObject[4];
    public GameObject[] functionPrefabs = new GameObject[3];
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
        if (!mapDataFile) return;
        ClearChildrens();
        var lines = mapDataFile.text.Split("\n");
        // 生成地块实例
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Length == 0) continue;
            var gridDatas = lines[i].Split(',');
            for (int j = 0; j < gridDatas.Length; j++)
            {
                // 生成地块Object
                int gridTerrainPrefabNumber = 0;
                if (gridDatas[j].Length >= 1 && gridDatas[j][0] >= '0' && gridDatas[j][0] <= '3')
                {
                    gridTerrainPrefabNumber = int.Parse(gridDatas[j]);
                }
                var terrainObject = Instantiate(terrainGridPrefabs[gridTerrainPrefabNumber], transform);
                terrainObject.transform.Translate(new Vector3(gridSize[0] * i, 0, gridSize[1] * j));
                // 添加普通地块Component(todo: addFunctionalGrids)
                var terrainComponent = terrainObject.AddComponent<TerrainGrid>();
                terrainComponent.SetInfo(new Vector2Int(i, j), gridTerrainPrefabNumber.ToString());
                // 创建角色
                GameObject avatar = null;
                if (gridDatas[j][0] == 'X')
                {
                    avatar = Instantiate(playerPrefab);
                    avatar.AddComponent<PlayerController>();
                }

                if (gridDatas[j][0] == 'Y')
                {
                    int monsterInd = int.Parse(gridDatas[j].Substring(1));
                    if (monsterInd >= monsterPrefabList.Count)
                        Debug.LogWarningFormat("[GameMapImporter] unable to find monster prefab {0}", monsterInd);
                    else
                    {
                        avatar = Instantiate(monsterPrefabList[monsterInd]);
                        avatar.AddComponent<MonsterController>();
                    }
                }
                if (avatar != null)
                {
                    avatar.transform.Translate(new Vector3(gridSize[0] * i, 1, gridSize[1] * j));
                }
            }
        }
    }

    private void ClearChildrens()
    {
        if (this.transform.childCount > 0)
            for (int i = 0; i < this.transform.childCount; i++)
            {
                DestroyImmediate(this.transform.GetChild(i).gameObject);
            }
    }
}
