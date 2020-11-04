using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/*
 * Dijkstra algoritmusát megvalósító szkript. 
 *  source:
 *  https://forum.unity.com/threads/dijkstras-algorithm-for-my-car.642043/
 *  https://stackoverflow.com/questions/50468903/c-sharp-shortest-path-algorithm-options
 */
namespace Assets.Scripts
{
    class Dijkstra
    {
        Dictionary<Vector2Int, Waypoint> grid = new Dictionary<Vector2Int, Waypoint>();
        Waypoint startWaypoint, endWaypoint;
        private List<Waypoint> path = new List<Waypoint>();

        Vector2Int[] directions = {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };
        public Dijkstra(Dictionary<Vector2Int, Waypoint> _grid, Waypoint _start, Waypoint _end)
        {
            grid = _grid;
            startWaypoint = _start;
            endWaypoint = _end;
        }

        public List<Waypoint> GetPath()
        {
            path = DijkstraShortestPath(startWaypoint, endWaypoint);
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
        public List<Waypoint> DijkstraShortestPath(Waypoint start, Waypoint end)
        {

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

                //get all the neighbors of the province
                List<Waypoint> neighbors = InitNeighbours(current);

                foreach (var neigh in neighbors)
                {
                    Waypoint node = neigh;

                    float currentDistance = node.cost; // minden node tavolsaga 1

                    float distanceThroughNode = distance + currentDistance;
                    //look for shorter path alternatives
                    if (distanceThroughNode < distances[neigh])
                    {
                        if (vertexQueue.ContainsKey(neigh)) vertexQueue.Remove(neigh);
                        distances[neigh] = distanceThroughNode;
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

            //return the shortest path
            return shortestPath;
        }
    }
}
