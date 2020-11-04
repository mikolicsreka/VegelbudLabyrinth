using System.Collections.Generic;
using UnityEngine;

/*
 * A végtelen játékmódban a tornyok lerakásáért felelős osztály.
 * 
 */

public class Infinite_TowerFactory : MonoBehaviour
{
    [SerializeField] int towerLimit = 5;
    [SerializeField] Tower towerPrefab;
    [SerializeField] Transform towerParentTransform;

    Queue<Tower> towerQueue = new Queue<Tower>();

    public Tower greenTurretPrefab;
    public Tower blueTurretPrefab;
    public Tower redTurretPrefab;

    private Tower turretToBuild;
    private Tile selectedTile;

    //Singleton pattern
    static Infinite_TowerFactory mInstance;

    public static Infinite_TowerFactory Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject temp = new GameObject();
                temp.name = "Infinite_TowerFactory";
                mInstance = temp.AddComponent<Infinite_TowerFactory>();
            }
            return mInstance;
        }
    }


    //Torony létrehozása
    public void InstantiateNewTower(Tile baseWaypoint)
    {
        if (FindObjectOfType<InfiniteLevelController>().score < turretToBuild.buyPrice)
        {
            Debug.Log("No money for turret building");
            return;
        }

        FindObjectOfType<InfiniteLevelController>().score -= turretToBuild.buyPrice;

        var newTower = Instantiate(turretToBuild, baseWaypoint.transform.position, Quaternion.identity);
        newTower.transform.parent = towerParentTransform;
        baseWaypoint.isPlaceable = false;
        newTower.baseWaypoint = baseWaypoint;
        baseWaypoint.isPlaceable = false;
        newTower.GetComponent<AudioSource>().volume = FindObjectOfType<Music_Player>().soundEffectsVol;
        towerQueue.Enqueue(newTower);
    }

    public bool CanBuild{ get { return turretToBuild != null; } } // variable ami megmondja hogy egy masik null-e

    //Tile van e kiválasztva (Azt kattintott legutoljára  a játékos)
    public void SelectTile(Tile tile)
    {
        selectedTile = tile;
        turretToBuild = null;
    } 

    //Vagy torony van-e kiválasztva (Azt kattintott-e utoljára)
    public void SelectTurretToBuild(Tower turret)
    {
        turretToBuild = turret;
        selectedTile = null;
    }
}
