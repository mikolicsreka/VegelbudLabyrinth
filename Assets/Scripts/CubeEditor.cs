using UnityEngine;

/*
 * Edit módot segítő szkript, hogy könnyebb legyen lerakni a cubeokat egymás mellé. 
 * 
 */

[ExecuteInEditMode]
[SelectionBase]
[RequireComponent(typeof(Waypoint))]
public class CubeEditor : MonoBehaviour
{
    

    Waypoint waypoint;

    private void Awake()
    {
        waypoint = GetComponent<Waypoint>();
    }
    // Start is called before the first frame update
    // in-out playmode
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SnapToGrid();

        UpdateLabel();

    }

    private void SnapToGrid()
    {
        int gridsize = waypoint.GetGridSize();
        transform.position = new Vector3(
                    waypoint.GetGridPos().x * gridsize ,
                    0f,
                    waypoint.GetGridPos().y *gridsize
                    );
    }

    private void UpdateLabel()
    {

        int gridSize = waypoint.GetGridSize();
        TextMesh textMesh = GetComponentInChildren<TextMesh>();
        string labelText = waypoint.GetGridPos().x + "," + waypoint.GetGridPos().y;
        textMesh.text = labelText;

        gameObject.name = labelText;
    }
}
