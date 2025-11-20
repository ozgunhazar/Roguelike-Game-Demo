using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {
    
    public int Width;
    public int Height;
    public int foodCountMin;
    public int foodCountMax;
    public Tile[] GroundTiles;
    public Tile[] WallTiles;
    
    private Tilemap m_Tilemap;
    private CellData[,] m_BoardData;

    private Grid m_Grid;
    private List<Vector2Int> m_EmptyCellsList;
    
    public WallObject WallPrefab;
    public ExitCellObject ExitPrefab;
    public CellObject EnemyPrefab;
    public FoodObject[] FoodPrefabs;

    public class CellData {
        public bool Passable;
        public CellObject ContainedObject;
    }
    
    
    public void Init() {
        
        int boardSize = GameManager.Instance.m_CurrentLevel / 5;
        Width = 8 + (boardSize * 2);
        Height = 8 + (boardSize * 2);
        
        m_Tilemap = GetComponentInChildren<Tilemap>();
        m_Grid = GetComponentInChildren<Grid>();

        m_EmptyCellsList = new List<Vector2Int>();

        m_BoardData = new CellData[Width, Height];
        

        for (int y = 0; y < Height; ++y) {
            for (int x = 0; x < Width; ++x) {

                Tile tile;
                m_BoardData[x, y] = new CellData();
                
                if (x == 0 || y == 0 || x == Width - 1 || y == Height - 1) {
                    tile = WallTiles[Random.Range(0, WallTiles.Length)];
                    m_BoardData[x, y].Passable = false;
                } else {
                    tile = GroundTiles[Random.Range(0, GroundTiles.Length)];
                    m_BoardData[x, y].Passable = true;
                    
                    m_EmptyCellsList.Add(new Vector2Int(x, y));
                }
                m_Tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }

        m_EmptyCellsList.Remove(new Vector2Int(1, 1));

        Vector2Int endCoord = new Vector2Int(Width - 2, Height - 2);
        AddObject(Instantiate(ExitPrefab), endCoord);
        m_EmptyCellsList.Remove(endCoord);
        
        GenerateWall();
        GenerateFood();
        GenerateEnemy();

    }

    public Vector3 CellToWorld(Vector2Int cellIndex) {

        return m_Grid.GetCellCenterWorld((Vector3Int)cellIndex);

    }

    public CellData GetCellData(Vector2Int cellIndex) {

        if (cellIndex.x < 0 || cellIndex.x >= Width || cellIndex.y < 0 || cellIndex.y >= Height) {
            return null;
        }

        return m_BoardData[cellIndex.x, cellIndex.y];
    }

    void AddObject(CellObject obj, Vector2Int coord) {

        CellData data = m_BoardData[coord.x, coord.y];
        obj.transform.position = CellToWorld(coord);
        data.ContainedObject = obj;
        obj.Init(coord);

    }

    void GenerateWall() {

        int wallCount = Random.Range(3, 6);

        for (int i = 0; i < wallCount; ++i) {

            int randomIndex = Random.Range(0, m_EmptyCellsList.Count);
            Vector2Int coord = m_EmptyCellsList[randomIndex];
            
            m_EmptyCellsList.RemoveAt(randomIndex);
            WallObject newWall = Instantiate(WallPrefab);
            AddObject(newWall, coord);
        }

    }
    
    void GenerateFood() {

        int totalfoodCount = Random.Range(foodCountMin,foodCountMax) + GameManager.Instance.m_CurrentLevel / 2;
        int meatCount = Random.Range(1, totalfoodCount);
        int pieCount = totalfoodCount - meatCount;

        for (int i = 0; i < meatCount; ++i) {

            int randomIndex = Random.Range(0, m_EmptyCellsList.Count);
            Vector2Int coord = m_EmptyCellsList[randomIndex];
      
            m_EmptyCellsList.RemoveAt(randomIndex);
            FoodObject newFood = Instantiate(FoodPrefabs[0]);
            AddObject(newFood, coord);
        }
        
        for (int i = 0; i < pieCount; ++i) {

            int randomIndex = Random.Range(0, m_EmptyCellsList.Count);
            Vector2Int coord = m_EmptyCellsList[randomIndex];
      
            m_EmptyCellsList.RemoveAt(randomIndex);
            FoodObject newFood = Instantiate(FoodPrefabs[1]);
            AddObject(newFood, coord);
        }

    }
    
    void GenerateEnemy() {

        int enemySpawnRange = GameManager.Instance.m_CurrentLevel / 5;
        int minEnemy = 1 + (enemySpawnRange * 2);
        int maxEnemy = 4 + (enemySpawnRange * 2);
        
        int enemyCount = Random.Range(minEnemy,maxEnemy);

        for (int i = 0; i < enemyCount; ++i) {

            int randomIndex = Random.Range(0, m_EmptyCellsList.Count);
            Vector2Int coord = m_EmptyCellsList[randomIndex];
      
            m_EmptyCellsList.RemoveAt(randomIndex);
            CellObject newEnemy = Instantiate(EnemyPrefab);
            AddObject(newEnemy, coord);

        }

    }
    

    public void SetCellTile(Vector2Int cellIndex, Tile tile) {
        
        m_Tilemap.SetTile(new Vector3Int(cellIndex.x, cellIndex.y, 0), tile);
        
    }

    public Tile GetCellTile(Vector2Int cellIndex) {

        return m_Tilemap.GetTile<Tile>(new Vector3Int(cellIndex.x, cellIndex.y, 0));

    }


    public void Clean() {

        if (m_BoardData == null) {
            return;
        }

        for (int y = 0; y < Height; ++y) {
            for (int x = 0; x < Width; ++x) {
                var cellData = m_BoardData[x, y];

                if (cellData.ContainedObject != null) {
                    Destroy(cellData.ContainedObject.gameObject);
                }
                
                SetCellTile(new Vector2Int(x, y), null);
            }
            
        }
        
        
    }
    
    
}
