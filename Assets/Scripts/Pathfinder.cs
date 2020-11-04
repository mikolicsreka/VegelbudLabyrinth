using Assets.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * Minden színhelyen található egy pathfinder. Beolvassa a gridet (a csempetérképet), majd meghívja az algoritmus
 * beállítások szerinti algoritmus szkriptjét, amely kiszámolja a legrövidebb utat.
 */
public class Pathfinder : MonoBehaviour
{
    [SerializeField] public Waypoint startWaypoint, endWaypoint;
    AlgorithmSettings algorithmSettings;


    Dictionary<Vector2Int, Waypoint> grid = new Dictionary<Vector2Int, Waypoint>();


    private List<Waypoint> path = new List<Waypoint>();
    string algorithmName;

    //Singleton pattern
    static Pathfinder mInstance;

    public static Pathfinder Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject temp = new GameObject();
                temp.name = "Pathfinder";
                mInstance = temp.AddComponent<Pathfinder>();
            }
            return mInstance;
        }
    }

    void Start()
    {
        algorithmSettings = FindObjectOfType<AlgorithmSettings>();
        if (algorithmSettings)
        {
            algorithmName = algorithmSettings.GetAlgorithmName();
        }

    }

    //Az út kiszámíttatása
    public List<Waypoint> GetPath()
    {
        if (path.Count == 0)
        {
            LoadBlocks();

            if (algorithmName == "BFS" || algorithmName == null)
            {
                BreadthFirstSearch BFS = new BreadthFirstSearch(grid, startWaypoint, endWaypoint);
                path = BFS.GetPath();


            }
            else if (algorithmName == "DIJKSTRA")
            {

                Dijkstra dijkstra = new Dijkstra(grid, startWaypoint, endWaypoint);
                path = dijkstra.GetPath();


            }
            else if (algorithmName == "ASTAR")
            {
                AStar astar = new AStar(grid, startWaypoint, endWaypoint);
                path = astar.GetPath();
            }

        }
        return path;
    }
    private void LoadBlocks()
    {
        var waypoints = FindObjectsOfType<Waypoint>();
        foreach (Waypoint waypoint in waypoints)
        {
            var gridPos = waypoint.GetGridPos();


            if (grid.ContainsKey(gridPos))
            {
                Debug.LogWarning("Overlapping block " + waypoint.GetGridPos());
               
            }
            else
            {
                grid.Add(waypoint.GetGridPos(), waypoint);
            }
            
        }
    }

    public Waypoint GetEndWaypoint()
    {
        return endWaypoint;
    }


    public void Reset()
    {
        grid.Clear();

        path = null;
        path = new List<Waypoint>();
        startWaypoint = null;
        endWaypoint = null;
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
    }


}


