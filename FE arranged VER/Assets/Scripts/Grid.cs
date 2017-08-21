using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{

    public static Grid instance;

    public GameObject[,] matrixG;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public TerrainType[] walkableRegions;
    public LayerMask walkableMask;//error was annoying so it's public
    Dictionary<int, int> walkableRegionDictionary = new Dictionary<int, int>();

    public Node[,] grid;

    float nodeDiameter;
    public int gridSizeX, gridSizeY;


    [SerializeField] float randomizer;
    public int[,] objectAxis;
    public int[,] fieldRec;

    float randomvalue;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        matrixG = new GameObject[gridSizeX, gridSizeY];
        objectAxis = new int[gridSizeX, gridSizeY];
        fieldRec = new int[gridSizeX, gridSizeY];

        foreach(TerrainType region in walkableRegions)
        {
            walkableMask.value |= region.terrainMask.value;//
            walkableRegionDictionary.Add((int)Mathf.Log(region.terrainMask.value,2),region.terrainPenalty);
        }

        CreateGrid();
    }

    void CreateGrid()
    {
        GameObject prefab = (GameObject)Resources.Load("Objects/GrassTile");
        GameObject prefabW = (GameObject)Resources.Load("Objects/Wall");
        GameObject prefabM = (GameObject)Resources.Load("Objects/Mountain");
        GameObject stageObject = GameObject.FindWithTag("Stage");

        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                randomvalue = Random.value;
                if (randomvalue < randomizer)
                {
                    Vector3 tile_pos = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                    GameObject instant_objectW = Instantiate(prefabW, tile_pos, Quaternion.identity);
                    instant_objectW.transform.parent = stageObject.transform;
                    matrixG[x, y] = instant_objectW;
                    objectAxis[x, y] = -10;
                    fieldRec[x, y] = 1; //Wall
                }
                else if (randomizer < randomvalue && randomvalue < randomizer +0.1)
                {
                    Vector3 tile_pos = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                    GameObject instant_objectS = Instantiate(prefabM, tile_pos, Quaternion.identity);
                    instant_objectS.transform.parent = stageObject.transform;
                    matrixG[x, y] = instant_objectS;
                    objectAxis[x, y] = -3;
                    fieldRec[x, y] = 2; //Swamp
                }
                else if (randomizer + 0.1 < randomvalue)
                {
                    Vector3 tile_pos = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                    GameObject instant_object = Instantiate(prefab, tile_pos, Quaternion.identity);
                    instant_object.transform.parent = stageObject.transform;
                    matrixG[x, y] = instant_object;
                    objectAxis[x, y] = -1;
                    fieldRec[x, y] = 0; //Grass
                }

                //Checking if it is walkable or not through collide
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius-0.01f, unwalkableMask));

                int movementPenalty = 0;

                if(walkable)
                {
                    Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);
                    RaycastHit hit;
                    if(Physics.Raycast(ray, out hit, 100, walkableMask))
                    {
                        walkableRegionDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                    }
                }

                grid[x, y] = new Node(walkable, worldPoint, x, y,movementPenalty);
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {

        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        { if (x == 0)
                continue;
            int checkX = node.gridX + x;
            if (checkX >= 0 && checkX < gridSizeX)
            {
                neighbours.Add(grid[checkX, node.gridY]);
            }
        }
        for (int y = -1; y <= 1; y++)
        {
            if (y == 0)
                continue;
            int checkY = node.gridY + y;
            if (checkY >= 0 && checkY < gridSizeY)
            {
                neighbours.Add(grid[node.gridX, checkY]);
            }
        }
        /*
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        */
        return neighbours;
    }


    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    Pathfinding path_f;
    public List<Node> path;

    //Remove the old current/first node from the path
    public void MoveNextTile()
    {
        path_f = GetComponent<Pathfinding>();

        if (path == null)
            return;
        path_f.seeker.position = path[0].worldPosition;
        path.RemoveAt(0);
        

    }

    void OnDrawGizmos()
    {

        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if (path != null)
                    if (path.Contains(n))
                        Gizmos.color = Color.black;
                Gizmos.DrawWireCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }

    [System.Serializable]
    public class TerrainType
    {
        public LayerMask terrainMask;
        public int terrainPenalty;
    }

    
}
