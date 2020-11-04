using System.Collections.Generic;
using UnityEngine;

/*
 * A szélességi keresést megvalósító osztály.
 * 
 */
namespace Assets.Scripts
{
    class BreadthFirstSearch
    {

        Vector2Int[] directions = {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };

        Dictionary<Vector2Int, Waypoint> grid = new Dictionary<Vector2Int, Waypoint>();
        Waypoint startWaypoint, endWaypoint;

        Queue<Waypoint> queue = new Queue<Waypoint>();
        private List<Waypoint> path = new List<Waypoint>();
        Waypoint searchCenter; //current search centre
        bool isRunning = true;

        public BreadthFirstSearch(Dictionary<Vector2Int, Waypoint> _grid, Waypoint _start, Waypoint _end)
        {
            grid = _grid;
            startWaypoint = _start;
            endWaypoint = _end;

        }

        public List<Waypoint> GetPath()
        {
            BreadthFirstSearchAlgorithm();
            CreatePath();

            return path;
        }

        private void CreatePath()
        {
            SetAsPath(endWaypoint);



            Waypoint previous = endWaypoint.exploredFrom;
            while (previous != startWaypoint)
            {

                SetAsPath(previous);
                previous = previous.exploredFrom;
            }

            SetAsPath(startWaypoint);

            path.Reverse();



        }

        private void SetAsPath(Waypoint waypoint)
        {
            path.Add(waypoint);
            if (path == null)
            {
          
            }
            if (waypoint == null)
            {

            }

            waypoint.isPlaceable = false;
        }


        private void BreadthFirstSearchAlgorithm()
        {
            queue.Enqueue(startWaypoint);
            while (queue.Count > 0 && isRunning)
            {
                searchCenter = queue.Dequeue();
                searchCenter.isExplored = true;
                //searchCenter.SetTopColor(Color.green);
                //we found the endpoint?
                if (searchCenter == endWaypoint)
                {
                    isRunning = false;

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
            }
        }

    }
}
