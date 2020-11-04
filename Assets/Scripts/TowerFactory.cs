using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A tornyok lerakását kezeli az alap játékmódban. Mindig van egy ilyen a színhelyen, egy üres játékobjektumon.
/// Ennek az objektumnak a gyermekobjektumai lesznek a tornyok.
/// </summary>
public class TowerFactory : MonoBehaviour
{
    [SerializeField] Tower towerPrefab;

    int towerLimit;
    [SerializeField] Transform towerParentTransform;

    Queue<Tower> towerQueue = new Queue<Tower>();

    Player player;

    //Singleton pattern
    static TowerFactory mInstance;

    public static TowerFactory Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject temp = new GameObject();
                temp.name = "TowerFactory";
                mInstance = temp.AddComponent<TowerFactory>();
            }
            return mInstance;
        }
    }
    void Start()
    {
        player = FindObjectOfType<Player>();

        towerLimit = player.towers.Count;
    }

    public Tower GetTowerPrefab()
    {
        return towerPrefab;
    }

    void Update()
    {

        towerLimit = player.towers.Count;
    }

    //tornyok megfelelő szintre upgradelése.
    public void RefreshTowerData(int number)
    {
        
        var towerList = this.GetComponentsInChildren<Tower>();
        if (towerList[number])
        {
            towerList[number].Upgrade();
        }
        
    }

    //torony hozzáadása építésre
    public void AddTower(Tile basetile)
    {
        int numTowers = towerQueue.Count;

        if (numTowers < towerLimit)
        {
            InstantiateNewTower(basetile);
        }
        else
        {
            // MoveExistingTower(basetile);
        }
    }

    //új torony létrehozása
    private void InstantiateNewTower(Tile baseWaypoint)
    {

        var newTower = Instantiate(towerPrefab, baseWaypoint.transform.position, Quaternion.identity);
        newTower.transform.parent = towerParentTransform;
        newTower.name = towerQueue.Count.ToString();
        newTower.GetComponent<AudioSource>().volume = FindObjectOfType<Music_Player>().soundEffectsVol;
        int numberInLine = FindObjectsOfType<Tower>().Length;

       
        for (int i = 0; i < player.towers[towerQueue.Count].level; i++)
        {

            RefreshTowerData(towerQueue.Count);

            newTower.level = player.towers[towerQueue.Count].level;



            newTower.id = towerQueue.Count;
        }
        
        
        baseWaypoint.isPlaceable = false;
        newTower.baseWaypoint = baseWaypoint;
        towerQueue.Enqueue(newTower);

    }

    //private void MoveExistingTower(Tile newBaseWaypoint)
    //{
    //    Tower oldTower = towerQueue.Dequeue();
    //    oldTower.baseWaypoint.isPlaceable = true;
    //    newBaseWaypoint.isPlaceable = false;

    //    oldTower.baseWaypoint = newBaseWaypoint;
    //    oldTower.transform.position = newBaseWaypoint.transform.position;

    //    towerQueue.Enqueue(oldTower);
    //}
}
