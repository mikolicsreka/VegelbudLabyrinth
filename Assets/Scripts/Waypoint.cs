using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// A Waypointokat megvalósító osztály. (Klikk event, stb.)
/// </summary>


/*
 * public is okay because this is a data class
 */
public class Waypoint : MonoBehaviour
{


    public bool isExplored = false; //BFS
    public Waypoint exploredFrom;
    public bool isPlaceable = true;
    const int gridSize = 10; // = block size
    [SerializeField] public int cost = 1;
    Scene scene;
    PathfinderSimulation simulator;




    void Start()
    {
        scene = SceneManager.GetActiveScene();
        if (scene.name == "Extras")
        {
            simulator = FindObjectOfType<PathfinderSimulation>();
        }
    }


    public Vector2Int GetGridPos()
    {
        return new Vector2Int(
            Mathf.RoundToInt(transform.position.x / gridSize),
            Mathf.RoundToInt(transform.position.z / gridSize));
    }


    public int GetGridSize()
    {
        return gridSize;
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())

                if (scene.name == "Extras")
                {

                    simulator.SetEndWaypoint(this);

                }

        }
    }

}
