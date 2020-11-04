using System.Collections.Generic;
using UnityEngine;


/*
 *  Az A* algoritmust megvalósító osztály.
 *  Source:
 *  https://gist.github.com/keithcollins/307c3335308fea62db2731265ab44c06
 *  https://gist.github.com/DanBrooker/1f8855367ae4add40792
 *  http://www.redblobgames.com/pathfinding/a-star/implementation.html#csharp
 */
namespace Assets.Scripts
{
    class AStar
    {
        Vector2Int[] directions = {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
        };

        //kezdő és végpont
        private Waypoint startWaypoint, endWaypoint;
        //a map, mint beolvasott csempetérkép (grid)
        Dictionary<Vector2Int, Waypoint> grid = new Dictionary<Vector2Int, Waypoint>();
        //a kapott út
        List<Waypoint> path = new List<Waypoint>();

        public AStar(Dictionary<Vector2Int, Waypoint> _grid, Waypoint _start, Waypoint _end)
        {
            grid = _grid;
            startWaypoint = _start;
            endWaypoint = _end;
        }

        public List<Waypoint> GetPath()
        {
            path = AStarSearch(startWaypoint, endWaypoint);
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

        private List<Waypoint> AStarSearch(Waypoint start, Waypoint end)
        {
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
                    float newCost = costSoFar[current] + neighbor.cost; // 1 = graph.Cost(current, neighbor) mert itt minden 1-be kerül, egymás mellett vannak...

                    // If there's no cost assigned to the neighbor yet, or if the new
                    // cost is lower than the assigned one, add newCost for this neighbor
                    if (!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor])
                    {

                        // If we're replacing the previous cost, remove it
                        if (costSoFar.ContainsKey(neighbor))
                        {
                            costSoFar.Remove(neighbor);
                            cameFrom.Remove(neighbor);
                        }

                        costSoFar.Add(neighbor, newCost);
                        cameFrom.Add(neighbor, current);
                        float priority = newCost + Heuristic(neighbor, end);
                        frontier.Enqueue(neighbor, priority);
                    }

                }
            }

            List<Waypoint> path = new List<Waypoint>();
            Waypoint currentOfList = end;

            while (!currentOfList.Equals(start))
            {
                if (!cameFrom.ContainsKey(currentOfList))
                {
                    return new List<Waypoint>();
                }

                currentOfList.isPlaceable = false;
                path.Add(currentOfList);
                currentOfList = cameFrom[currentOfList];
            }
            path.Add(start);
            path.Reverse();
            return path;
        }

        static public float Heuristic(Waypoint a, Waypoint b)
        {
            return (Mathf.Abs(a.transform.position.x - b.transform.position.x) + Mathf.Abs(a.transform.position.z - b.transform.position.z)) / 10;
        }
    }
}
