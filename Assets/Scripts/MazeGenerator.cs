using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * A végtelen pályás mód labirintus generálója.
 * source: https://forum.unity.com/threads/quick-maze-generator.173370/
 */

public class MazeGenerator : MonoBehaviour
{
    public int width, height;
    public Material tileMaterial;
    public Material waypointMaterial;
    private int[,] Maze;
    //private List<Vector3> pathMazes = new List<Vector3>();
    private Stack<Vector2> _tiletoTry = new Stack<Vector2>();
    private List<Vector2> offsets = new List<Vector2> { new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(-1, 0) };
    private System.Random rnd = new System.Random();
    //private int _width, _height;
    private Vector2 _currentTile;

    bool isGeneratedExists;
    Pathfinder pathFinder;

    [SerializeField] GameObject SpawnPlacePrefab_RED;
    [SerializeField] GameObject SpawnPlacePrefab_BLUE;

    GameObject instantiatedSPawnPlace_RED;
    GameObject instantiatedSPawnPlace_BLUE;
 
    public Vector2 CurrentTile
    {
        get { return _currentTile; }
        private set
        {
            if (value.x < 1 || value.x >= this.width - 1 || value.y < 1 || value.y >= this.height - 1)
            {
                throw new ArgumentException("CurrentTile must be within the one tile border all around the maze");
            }
            if (value.x % 2 == 1 || value.y % 2 == 1)
            { _currentTile = value; }
            else
            {
                throw new ArgumentException("The current square must not be both on an even X-axis and an even Y-axis, to ensure we can get walls around all tunnels");
            }
        }
    }

    private static MazeGenerator instance;
    public static MazeGenerator Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
       
    }

    public void CreateMazeWithPathfinder()
    {

        DeleteChildren();


        if (!isGeneratedExists)
        {
            var childrenWaypoint = this.GetComponentsInChildren<Waypoint>();

            GenerateMaze();
            isGeneratedExists = true;


            for (int i = 0; i < 10; i++)
            {
                string line = "";
                for (int j = 0; j < 10; j++)
                {
                    line += " " + Maze[j, i].ToString();
                }



            }
            pathFinder = FindObjectOfType<Pathfinder>();

            pathFinder.Reset();

            Waypoint[] waypoints = FindObjectsOfType<Waypoint>();
            pathFinder.endWaypoint = waypoints[0];
            pathFinder.startWaypoint = waypoints[waypoints.Length - 1];

            InstantiateSpawnOnObjects(pathFinder);

            List<Waypoint> path = pathFinder.GetPath();
        }

    }

    void InstantiateSpawnOnObjects(Pathfinder pathFinder)
    {
        instantiatedSPawnPlace_RED = Instantiate(SpawnPlacePrefab_BLUE, pathFinder.endWaypoint.transform.position + new Vector3(0,5,0), Quaternion.identity);
        instantiatedSPawnPlace_BLUE = Instantiate(SpawnPlacePrefab_RED, pathFinder.startWaypoint.transform.position + new Vector3(-10, 5, 0), Quaternion.identity);
    }

    void DeleteChildren()
    {
        var childrenWaypoint = this.GetComponentsInChildren<Waypoint>();
        var childrenTiles = this.GetComponentsInChildren<Tile>();


        DestroyImmediate(instantiatedSPawnPlace_RED);
        DestroyImmediate(instantiatedSPawnPlace_BLUE);

        foreach (var item in childrenWaypoint)
        {

            DestroyImmediate(item.gameObject);
        }    

        foreach (var item in childrenTiles)
        {
            DestroyImmediate(item.gameObject);
        }

        isGeneratedExists = false;
    }

    void GenerateMaze()
    {
        Maze = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Maze[x, y] = 1;
            }
        }
        CurrentTile = Vector2.one;
        _tiletoTry.Push(CurrentTile);
        Maze = CreateMaze();
        Tile ptype = null;
        Waypoint temp = null;


        for (int i = 0; i <= Maze.GetUpperBound(0); i++)
        {
            for (int j = 0; j <= Maze.GetUpperBound(1); j++)
            {
                if (Maze[i, j] == 1)
                {
                    ptype = GameObject.CreatePrimitive(PrimitiveType.Cube).AddComponent<Tile>();


                    //set size 10x
                    var s = ptype.transform.localScale;
                    float cubeScale = 9.0f;
                    ptype.transform.localScale += new Vector3(s.x * cubeScale, s.y * cubeScale, s.z * cubeScale);

                    ptype.transform.position = new Vector3(i * ptype.transform.localScale.x,  0, j * ptype.transform.localScale.z);
                    if (tileMaterial != null)
                    {
                        ptype.GetComponent<Renderer>().material = tileMaterial;

                    }

                    ptype.transform.parent = transform;
                }
                else if (Maze[i, j] == 0)
                {

                    //pathMazes.Add(new Vector3(i, j, 0));
                    temp = GameObject.CreatePrimitive(PrimitiveType.Cube).AddComponent<Waypoint>();
                    //set size 10x
                    var s = temp.transform.localScale;
                    float cubeScale = 9.0f;
                    temp.transform.localScale += new Vector3(s.x * cubeScale, s.y * cubeScale, s.z * cubeScale);
                    //
                    temp.transform.position = new Vector3(i * temp.transform.localScale.x,  0, j * temp.transform.localScale.y);
                    temp.transform.parent = transform;

                    if (tileMaterial != null)
                    {
                        temp.GetComponent<Renderer>().material = waypointMaterial;

                    }

                }

            }
        }
    }

    public int[,] CreateMaze()
    {
        //local variable to store neighbors to the current square
        //as we work our way through the maze
        List<Vector2> neighbors;
        //as long as there are still tiles to try
        while (_tiletoTry.Count > 0)
        {
            //excavate the square we are on
            Maze[(int)CurrentTile.x, (int)CurrentTile.y] = 0;


            //get all valid neighbors for the new tile
            neighbors = GetValidNeighbors(CurrentTile);

            //if there are any interesting looking neighbors
            if (neighbors.Count > 0)
            {
                //remember this tile, by putting it on the stack
                _tiletoTry.Push(CurrentTile);
                //move on to a random of the neighboring tiles
                CurrentTile = neighbors[rnd.Next(neighbors.Count)];
            }
            else
            {
                //if there were no neighbors to try, we are at a dead-end
                //toss this tile out
                //(thereby returning to a previous tile in the list to check).
                CurrentTile = _tiletoTry.Pop();
            }
        }

        return Maze;
    }
    /// <summary>
    /// Get all the prospective neighboring tiles
    /// </summary>
    /// <param name="centerTile">The tile to test</param>
    /// <returns>All and any valid neighbors</returns>
    private List<Vector2> GetValidNeighbors(Vector2 centerTile)
    {

        List<Vector2> validNeighbors = new List<Vector2>();

        //Check all four directions around the tile
        foreach (var offset in offsets)
        {
            //find the neighbor's position
            Vector2 toCheck = new Vector2(centerTile.x + offset.x, centerTile.y + offset.y);

            //make sure the tile is not on both an even X-axis and an even Y-axis
            //to ensure we can get walls around all tunnels
            if (toCheck.x % 2 == 1 || toCheck.y % 2 == 1)
            {
                //if the potential neighbor is unexcavated (==1)
                //and still has three walls intact (new territory)

                    try
                    {
                        if (Maze[(int)toCheck.x, (int)toCheck.y] == 1 && HasThreeWallsIntact(toCheck))
                        {
                            //add the neighbor
                            validNeighbors.Add(toCheck);
                        }
                    }
                    catch (Exception)
                    {

                        Debug.Log("hiba");
                    }


                UnityEngine.Random.Range(0, 2);


            }
        }

        return validNeighbors;
    }


    /// <summary>
    /// Counts the number of intact walls around a tile
    /// </summary>
    /// <param name="Vector2ToCheck">The coordinates of the tile to check</param>
    /// <returns>Whether there are three intact walls (the tile has not been dug into earlier.</returns>
    private bool HasThreeWallsIntact(Vector2 Vector2ToCheck)
    {
        int intactWallCounter = 0;

        //Check all four directions around the tile
        foreach (var offset in offsets)
        {
            //find the neighbor's position
            Vector2 neighborToCheck = new Vector2(Vector2ToCheck.x + offset.x, Vector2ToCheck.y + offset.y);

            //make sure it is inside the maze, and it hasn't been dug out yet
            if (IsInside(neighborToCheck) && Maze[(int)neighborToCheck.x, (int)neighborToCheck.y] == 1)
            {
                intactWallCounter++;
            }
        }

        //tell whether !!!!two walls are intact
        return intactWallCounter == 3;

    }

    private bool IsInside(Vector2 p)
    {
        return p.x >= 0 && p.y >= 0 && p.x < width && p.y < height;
    }

    public int[,] GetMaze()
    {
        return Maze;
    }
}