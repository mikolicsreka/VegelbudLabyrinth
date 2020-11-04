using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// A Tile objektumot megvalósító osztály. 
/// </summary>
public class Tile : MonoBehaviour
{

    public bool isPlaceable = true;

    AlgorithmSettings settings;

    Infinite_TowerFactory endlessTowerFactory;

    private Color startColor;
    private Renderer renderer;

    [SerializeField] Color hoverColor = Color.grey;

    void Start()
    {
        settings = FindObjectOfType<AlgorithmSettings>();
        try
        {
            renderer = GetComponent<Renderer>();
            startColor = renderer.material.color;
        }
        catch (System.Exception)
        {

            // Not have renderer
        }


        try
        {
            endlessTowerFactory = FindObjectOfType<Infinite_TowerFactory>();
        }
        catch (System.Exception ex)
        {

            Debug.LogError(ex);
        }
    }

    /// <summary>
    /// Kattintásra tornyot épít.
    /// </summary>
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                //Debug.Log(EventSystem.current.currentSelectedGameObject.gameObject.name);
                return;
            }

            if (isPlaceable ) //  && settings.GetIsTowerPlacementTime()
            {
                if (SceneManager.GetActiveScene().name == "EndlessLevels")
                {
                    if (!endlessTowerFactory.CanBuild)
                    {
                        return;
                    }

                    FindObjectOfType<Infinite_TowerFactory>().InstantiateNewTower(this);



                }
                else
                {
                    if (settings.GetIsTowerPlacementTime())
                    {
                        FindObjectOfType<TowerFactory>().AddTower(this);
                    }
                    
                }
            }
            else
            {


                print("blocked");
            }

        }

    }

    /// <summary>
    /// egér ráhúzásra színezés
    /// </summary>
    private void OnMouseEnter()
    {
        if (FindObjectOfType<Infinite_TowerFactory>() != null)
        {


            if (renderer == null)
            {
                return;
            }
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (!endlessTowerFactory.CanBuild)
            {
                return;
            }

            renderer.material.color = startColor * 0.5f;

        }


    }

    private void OnMouseExit()
    {
        if (renderer == null)
        {
            return;
        }
        renderer.material.color = startColor;
    }
}
