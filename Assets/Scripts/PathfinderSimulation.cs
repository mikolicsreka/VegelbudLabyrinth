using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * Ez az osztály tartalmazza az összes algoritmust, csak miközben fut, színezi a csempéket. Ez fut a Testing Ground színhelyen.
 */
public class PathfinderSimulation : MonoBehaviour
{
    [SerializeField] Waypoint startWaypoint;
    Waypoint endWaypoint;
    AlgorithmSettings algorithmSettings;

    Vector2Int[] directions = {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };


    Dictionary<Vector2Int, Waypoint> grid = new Dictionary<Vector2Int, Waypoint>();
    Queue<Waypoint> queue = new Queue<Waypoint>();
    Waypoint searchCenter; //current search centre
    [SerializeField] bool isRunning = false;

    private List<Waypoint> path = new List<Waypoint>();
    string algorithmName;

    //Singleton pattern
    static PathfinderSimulation mInstance;

    public static PathfinderSimulation Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject temp = new GameObject();
                temp.name = "Infinite_TowerFactory";
                mInstance = temp.AddComponent<PathfinderSimulation>();
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
        else
        {
            algorithmName = "BFS";
        }


        LoadBlocks();
    }

    //Színezése a csempék felső felének
    private void SetTopColor(Waypoint waypoint, Color color)
    {
        if (waypoint)
        {
            Transform obj = waypoint.transform.Find("Top");
            var cubeRenderer = obj.GetComponent<MeshRenderer>();
            cubeRenderer.material.color = color;
        }

    }

    //végpont beállítása
    public void SetEndWaypoint(Waypoint waypoint)
    {
        if (isRunning)
        {
            return;
        }

        SetTopColor(startWaypoint, Color.green);
        ResetColors();
        ResetWaypoints();

        if (!waypoint.Equals(startWaypoint))
        {
            endWaypoint = waypoint;
            SetTopColor(endWaypoint, Color.red);

        }

        if (algorithmName == "BFS")
        {
            GetBFSPath();
        }
        else if (algorithmName == "DIJKSTRA")
        {
            path = GetDijkstraPath();
        }
        else
        {
            path = GetAstarPath();
        }
    }


    //reset, ha új végpontot állítunk az algoritmusnak
    public void ResetWaypoints()
    {
        foreach (var item in grid)
        {
            item.Value.isExplored = false;
            item.Value.isPlaceable = true;
            item.Value.exploredFrom = null;

            
        }
        //isRunning = false;
        queue = new Queue<Waypoint>();
        path = null;
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();



    }

    //resetnél a színek visszaállítása.
    public void ResetColors()
    {
        foreach (var item in grid)
        {
            if (!item.Value.Equals(startWaypoint))
            {
                SetTopColor(item.Value, Color.white);
            }

        }
    }


    public void GetBFSPath()
    {

        StartCoroutine(CalculatePath());
    }


    private IEnumerator CalculatePath()
    {
        // LoadBlocks();
        StartCoroutine(BreadthFirstSearch());
        yield return new WaitUntil(() => isRunning == false);

        CreatePath();


    }

    private void SetAsPath(Waypoint waypoint)
    {
        path.Add(waypoint);
    }

    //A legrövidebb út beállítása
    private void CreatePath()
    {

        path = new List<Waypoint>();
        SetAsPath(endWaypoint);

        SetTopColor(endWaypoint, Color.green);
        Waypoint previous = endWaypoint.exploredFrom;


        while (previous != startWaypoint)
        {

            //SetAsPath(previous);
            SetTopColor(previous, Color.green);

            previous = previous.exploredFrom;


        }

        SetAsPath(startWaypoint);
        SetTopColor(startWaypoint, Color.green);


        path.Reverse();

        foreach (var item in path)
        {

            SetTopColor(item, Color.green);
        }
    }

    private IEnumerator BreadthFirstSearch()
    {
        isRunning = true;

        queue.Enqueue(startWaypoint);
        while (queue.Count > 0 && isRunning)
        {

            searchCenter = queue.Dequeue();
            searchCenter.isExplored = true;

            yield return new WaitForSecondsRealtime(0.1f);
            //searchCenter.SetTopColor(Color.green);
            //we found the endpoint?
            if (searchCenter == endWaypoint)
            {
                isRunning = false;
                break;
            }

            ExploreNeighbours();
        }
    }

    private void ExploreNeighbours()
    {

        if (!isRunning) { return; }

        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighbourCoordinates = searchCenter.GetGridPos() + direction;

            if (grid.ContainsKey(neighbourCoordinates))
            {
                SetTopColor(searchCenter, Color.blue);

                QueueNewNeighbours(neighbourCoordinates);
            }

        }
    }

    private void QueueNewNeighbours(Vector2Int neighbourCoordinates)
    {


        Waypoint neighbour = grid[neighbourCoordinates];
        if (neighbour.isExplored || queue.Contains(neighbour))
        {

        }
        else
        {

            queue.Enqueue(neighbour);
            neighbour.exploredFrom = searchCenter;


            SetTopColor(neighbour, Color.magenta);
            // yield return new WaitForSecondsRealtime(0.3f);
            SetTopColor(neighbour.exploredFrom, Color.blue);

        }
    }


    private void LoadBlocks()
    {
        var waypoints = FindObjectsOfType<Waypoint>();
        foreach (Waypoint waypoint in waypoints)
        {
            var gridPos = waypoint.GetGridPos();
            if (grid.ContainsKey(gridPos))
            {
                Debug.LogWarning("Overlapping block " + waypoint);
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

    private List<Waypoint> GetDijkstraPath()
    {
        // List<Waypoint> pathD = DijkstraShortestPath(startWaypoint, endWaypoint);
        StartCoroutine(DijkstraShortestPath(startWaypoint, endWaypoint));
        path.Reverse();


       

        return path;
    }

    List<Waypoint> InitNeighbours(Waypoint point)
    {
        List<Waypoint> neighbours = new List<Waypoint>();
        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighbourCoordinates = point.GetGridPos() + direction;

            if (grid.ContainsKey(neighbourCoordinates))
            {
                Waypoint neighbour = grid[neighbourCoordinates];
                if (neighbour.isExplored)
                {

                }
                else
                {
                    neighbours.Add(neighbour);
                    neighbour.exploredFrom = point;
                }
            }

        }
        return neighbours;

    }

    //DIJKSTRa
    public IEnumerator DijkstraShortestPath(Waypoint start, Waypoint end) //List<Waypoint>
    {
        print("startDIjkstra");
        isRunning = true;
        // The final path
        List<Waypoint> shortestPath = new List<Waypoint>();
        // Previous nodes in optimal path from source
        Dictionary<Waypoint, float> vertexQueue = new Dictionary<Waypoint, float>();
        // The list of unvisited nodes
        List<Waypoint> unvisited = new List<Waypoint>();
        // The calculated distances, set all to Infinity at start, except the start Node
        Dictionary<Waypoint, float> distances = new Dictionary<Waypoint, float>();

        // Previous nodes in optimal path from source
        Dictionary<Waypoint, Waypoint> previous = new Dictionary<Waypoint, Waypoint>();

        foreach (var waypoint in grid)
        {
            Waypoint node = new Waypoint();
            node = waypoint.Value;
            unvisited.Add(node);

            // Setting the node distance to Infinity
            distances.Add(node, float.MaxValue);

        }

        float distance = 0;
        Waypoint current = new Waypoint();

        //add the starting node by default to the searching queue
        vertexQueue.Add(start, 0f);

        while (vertexQueue.Count() > 0)
        {
            distance = vertexQueue.First().Value;
            current = vertexQueue.First().Key;
            vertexQueue.Remove(current);

            SetTopColor(current, Color.blue);
            yield return new WaitForSecondsRealtime(0.02f);

            //get all the neighbors of the province
            List<Waypoint> neighbors = InitNeighbours(current);

            foreach (var neigh in neighbors)
            {
                Waypoint node = neigh;



                float currentDistance = 1; // minden node tavolsaga 1

                float distanceThroughNode = distance + currentDistance;
                //look for shorter path alternatives
                if (distanceThroughNode < distances[neigh])
                {
                    if (vertexQueue.ContainsKey(neigh)) vertexQueue.Remove(neigh);
                    distances[neigh] = distanceThroughNode;

                    SetTopColor(node, Color.magenta);
                    yield return new WaitForSecondsRealtime(0.02f);
                    previous[neigh] = current;
                    vertexQueue.Add(neigh, distances[neigh]);
                }
            }

        }

        Waypoint check = end;

        //build the shortest path indices list
        while (true)
        {
            if (check != start)
            {
                check.isPlaceable = false;
                shortestPath.Add(check);
                check = previous[check];
            }
            else
            {
                check.isPlaceable = false;
                shortestPath.Add(check);

                break;
            }
        }

        path = shortestPath;
        foreach (var item in path)
        {
            SetTopColor(item, Color.green);
        }
        print("endDIjkstra");

        isRunning = false;

        //return the shortest path
        //return shortestPath;
    }

    private List<Waypoint> GetAstarPath()
    {
        StartCoroutine(AStarSearch(startWaypoint, endWaypoint));
       
        return path;
    }

    private IEnumerator AStarSearch(Waypoint start, Waypoint end)
    {
        isRunning = true;
        Dictionary<Waypoint, Waypoint> cameFrom = new Dictionary<Waypoint, Waypoint>();
        Dictionary<Waypoint, float> costSoFar = new Dictionary<Waypoint, float>();
        var frontier = new PriorityQueue<Waypoint>();
        frontier.Enqueue(start, 0f);

        cameFrom.Add(start, start); // is set to start, None in example
        costSoFar.Add(start, 0f);

        while (frontier.Count > 0f)
        {
            // Get the Location from the frontier that has the lowest
            // priority, then remove that Location from the frontier
            Waypoint current = frontier.Dequeue();
            SetTopColor(current, Color.blue);
            yield return new WaitForSecondsRealtime(0.09f);

            // If we're at the goal Location, stop looking.
            if (current.Equals(end)) break;


            // Neighbors will return a List of valid tile Locations
            // that are next to, diagonal to, above or below current
            var neighbours = InitNeighbours(current);
            foreach (var neighbor in neighbours)
            {

                // If neighbor is diagonal to current, graph.Cost(current,neighbor)
                // will return Sqrt(2). Otherwise it will return only the cost of
                // the neighbor, which depends on its type, as set in the TileType enum.
                // So if this is a normal floor tile (1) and it's neighbor is an
                // adjacent (not diagonal) floor tile (1), newCost will be 2,
                // or if the neighbor is diagonal, 1+Sqrt(2). And that will be the
                // value assigned to costSoFar[neighbor] below.
                float newCost = costSoFar[current] + 1; // 1 = graph.Cost(current, neighbor) mert itt minden 1-be kerül, egymás mellett vannak...

                // If there's no cost assigned to the neighbor yet, or if the new
                // cost is lower than the assigned one, add newCost for this neighbor
                if (!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor])
                {

                    // If we're replacing the previous cost, remove it
                    if (costSoFar.ContainsKey(neighbor))
                    {
                        costSoFar.Remove(neighbor);
                        cameFrom.Remove(neighbor);
                        SetTopColor(current, Color.green);
                        yield return new WaitForSecondsRealtime(0.02f);
                    }

                    SetTopColor(current, Color.magenta);
                    yield return new WaitForSecondsRealtime(0.0f);

                    costSoFar.Add(neighbor, newCost);
                    cameFrom.Add(neighbor, current);
                    float priority = newCost + Heuristic(neighbor, end);
                    frontier.Enqueue(neighbor, priority);
                }

            }
        }
        // todo do it külön fgvbe 
        // getpath::
        List<Waypoint> pathA = new List<Waypoint>();
        Waypoint currentOfList = end;
        // path.Add(current);

        while (!currentOfList.Equals(start))
        {
            if (!cameFrom.ContainsKey(currentOfList))
            {
                MonoBehaviour.print("cameFrom does not contain current.");
                yield break;
            }

            currentOfList.isPlaceable = false;
            pathA.Add(currentOfList);
            currentOfList = cameFrom[currentOfList];
        }
        pathA.Add(start);
        pathA.Reverse();
        path = pathA;
        foreach (var item in path)
        {
            SetTopColor(item, Color.green);
        }
        isRunning = false;

    }

    static public float Heuristic(Waypoint a, Waypoint b)
    {
        return (Mathf.Abs(a.transform.position.x - b.transform.position.x) + Mathf.Abs(a.transform.position.z - b.transform.position.z)) / 10;
    }
}
