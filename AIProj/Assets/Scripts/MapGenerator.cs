using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// pathfinding breaks when goal in corner



// generates the tilemap, handles state
public class MapGenerator : MonoBehaviour
{
    [Range(10, 50)]
    public int width;
    [Range(10, 50)]
    public int height;    
    public GameObject basePrefab;
    // public int highlightOpacity;
    public int wallTraversalCost;

    public Color groundColor;
    public Color wallColor;
    public Color turretColor;
    public Color spawnColor;
    public Color objectiveColor;

    public GameObject ground;
    public GameObject wall;
    public GameObject turret;
    public GameObject spawn;
    public GameObject objective;

    public static GameObject damageEffect;
    public static TileType currentSelection;
    public static Tile[,] tiles;
    public static Stage stage;
    public static int mapScale;
    Node[,] nodes;
    Node[,] nodes_CanBreak;

   
    int typeLength = 5;

    // Djikstra pathfinder;
    D2 pathfinder;

    // Use this for initialization
    void Start()
    {
        stage = Stage.placeTiles;
        currentSelection = TileType.ground;
        tiles = new Tile[height, width];
        nodes = new Node[height, width];
        nodes_CanBreak = (Node[,])nodes.Clone();
        Turret.turrets = new HashSet<Transform>();
        Wall.walls = new HashSet<Transform>();
        Enemy.enemies = new HashSet<Transform>();
        damageEffect = (GameObject)Resources.Load("Damage");

        GenerateTileMap();
        InitializeData();

        // pathfinder = new Djikstra();
        pathfinder = new D2();

        mapScale = Mathf.Max(width, height);
        Camera.main.transform.position = new Vector3(width / 2f - 0.5f, mapScale, height / 2f - 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if(stage == Stage.defend){  SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
            else { InitRun(); }
        }

        switch (stage)
        {
            case Stage.placeTiles:
                UpdateBuild();
                break;
            case Stage.defend:
                UpdateRun();
                break;
        }
    }

    // initializes the tilemap
    void GenerateTileMap()
    {
        float s = 0.9f; // tile size
        basePrefab.transform.localScale = new Vector3(s, s, s);

        GameObject tileObj;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                tileObj = Instantiate(basePrefab, new Vector3(x, 0, y), Quaternion.identity);
                tiles[y, x] = tileObj.GetComponent<Tile>();
                tiles[y, x].Set(TileType.ground, new Index(x, y), tileObj);

                nodes[y, x] = new Node(1, new Index(x, y));
                nodes_CanBreak[y, x] = new Node(1, new Index(x, y));
            }
        }
    }

    // makes a 2d array of nodes given conditions
    Node[,] GenerateNodeMap(bool canBreakWalls)
    {
        Node[,] tempNodes = canBreakWalls ? (Node[,])nodes_CanBreak.Clone() : (Node[,])nodes.Clone(); // shouldn't matter (?)       

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Node current = tempNodes[y, x];
                current.nextNodes.Clear();

                if (y > 0)          { TryAddNode(current, tempNodes[y - 1, x], canBreakWalls); }
                if (y < height - 1) { TryAddNode(current, tempNodes[y + 1, x], canBreakWalls); }
                if (x > 0)          { TryAddNode(current, tempNodes[y, x - 1], canBreakWalls); }
                if (x < width - 1)  { TryAddNode(current, tempNodes[y, x + 1], canBreakWalls); }
            }
        }
        return tempNodes;
    }

    void TryAddNode(Node baseNode, Node nextNode, bool canBreakWalls)
    {
        TileType t = tiles[nextNode.idx.y, nextNode.idx.x].type;
        if (t == TileType.turret || ((! canBreakWalls) && t == TileType.wall)) { return; } // don't add

        if (t == TileType.wall) { nextNode.traversalCost = wallTraversalCost; }

        baseNode.nextNodes.Add(nextNode);
    }

    // makes type -> color & GameObject dictionaries
    void InitializeData()
    {
        Tile.colors = new Dictionary<TileType, Color>();
        Tile.colors.Add(TileType.ground, groundColor);
        Tile.colors.Add(TileType.wall, wallColor);
        Tile.colors.Add(TileType.turret, turretColor);
        Tile.colors.Add(TileType.spawn, spawnColor);
        Tile.colors.Add(TileType.objective, objectiveColor);

        Tile.objs = new Dictionary<TileType, GameObject>();
        Tile.objs.Add(TileType.ground, ground);
        Tile.objs.Add(TileType.wall, wall);
        Tile.objs.Add(TileType.turret, turret);
        Tile.objs.Add(TileType.spawn, spawn);
        Tile.objs.Add(TileType.objective, objective);
    }
  
    void InitBuild()
    {

    }

    // initializes run stage
    void InitRun()
    {
        if (stage == Stage.defend) { return; }

        stage = Stage.defend;

        nodes = GenerateNodeMap(false); // ToDo: generate second nodemap using first for better performance
        nodes_CanBreak = GenerateNodeMap(true);

        Node objectiveNode, objectiveNode_CanBreak;
        objectiveNode = objectiveNode_CanBreak = null;

        Turret.turrets.Clear();
        Wall.walls.Clear();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (tiles[y, x].type == TileType.objective)
                {
                    objectiveNode = nodes[y, x];
                    objectiveNode_CanBreak = nodes_CanBreak[y, x];
                    break;
                }
            }
        }

        TileType type;
        GameObject obj;
        Vector3 pos;
        Tile tile;

        // instantiate all
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {               
                tile = tiles[y, x];
                type = tile.type;
                obj = null;
                Tile.objs.TryGetValue(type, out obj);
                if(null == obj) { continue; } // do not instantiate

                obj = Instantiate(obj, tile.transform.position, Quaternion.identity);
                             
                switch (type) 
                {
                    case TileType.ground:
                        break;
                    case TileType.wall:
                        obj.GetComponent<Wall>().Set(new Index(x, y));
                        Wall.walls.Add(tile.transform);
                        break;
                    case TileType.turret:
                        Turret.turrets.Add(obj.transform);
                        break;
                    case TileType.spawn:
                        // THIS IS GROSS
                        List<Index> tryNodeMap = pathfinder.Pathfind(nodes[y, x], objectiveNode, nodes);
                        if (null == tryNodeMap)
                        {
                            tryNodeMap = pathfinder.Pathfind(nodes_CanBreak[y, x], objectiveNode_CanBreak, nodes_CanBreak);
                            if (null == tryNodeMap) { Debug.LogError("null sec time"); }
                        }
                        obj.GetComponent<Spawner>().SetValues(tryNodeMap);
                        break;                        
                    case TileType.objective:
                        break;
                    default:
                        return;                   
                }
           
                tile.gameObject.SetActive(false);      
            }
        }
    }

    // updates build stage
    void UpdateBuild()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            currentSelection++;
            if ((int)currentSelection >= typeLength) { currentSelection = 0; }
        }

        if (Input.mouseScrollDelta.y < 0)
        {
            currentSelection--;
            if (currentSelection < 0) { currentSelection = (TileType)(typeLength - 1); }
        }
    }

    void UpdateRun()
    {

    }
}



