using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameMap : MonoBehaviour
{
    public TextAsset mapDataFile; // 待解析的.txt数据文件
    public Vector2 gridSize = new Vector2(0, 0);
    public GameObject[] terrainGridPrefabs = new GameObject[4];
    public GameObject[] functionPrefabs = new GameObject[3];
    public GameObject playerPrefab;
    public List<GameObject> monsterPrefabList;
    private BaseGrid[,] _basegrids;

    public BaseGrid GetGrid(int x, int z)
    {
        if (x < 0 || x > _basegrids.GetLength(0)) return null;
        if (z < 0 || z > _basegrids.GetLength(1)) return null;
        return _basegrids[x, z].GetComponent<BaseGrid>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ImportGameMapData()
    {
        if (!mapDataFile) return;
        ClearChildrens();
        var lines = mapDataFile.text.Split("\n");
        List<List<BaseGrid>> grids = new List<List<BaseGrid>>();
        // 生成地块实例
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Length == 0) continue;
            grids.Add(new List<BaseGrid>());
            var gridDatas = lines[i].Split(',');
            for (int j = 0; j < gridDatas.Length; j++)
            {
                int gridTerrainPrefabNumber = 0;
                if (gridDatas[j].Length == 1 && gridDatas[j][0] >= '0' && gridDatas[j][0] <= '3')
                    gridTerrainPrefabNumber = int.Parse(gridDatas[j]);
                var terrainObject = Instantiate(terrainGridPrefabs[gridTerrainPrefabNumber], transform);
                terrainObject.transform.Translate(new Vector3(gridSize[0] * i, 0, gridSize[1] * j));
                var terrainComponent = terrainObject.AddComponent<TerrainGrid>();
                terrainComponent.SetInfo(new Vector2Int(i, j), gridTerrainPrefabNumber.ToString());
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
