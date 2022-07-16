using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameMap : MonoBehaviour
{
    public TextAsset mapDataFile; // 待解析的.txt数据文件
    public TerrainGrid[] terrainGridPrefabs = new TerrainGrid[4];
    public FunctionalGrid[] functionalGridPrefabs = new FunctionalGrid[3];
    public GameObject playerPrefab;
    public List<GameObject> monsterPrefabList;
    private GameObject[,] _basegrids;

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
        
    }
}
