using UnityEngine;

/*
 * A végtelen játékmódban a bolt.
 */
public class Infinite_Shop : MonoBehaviour
{

    Infinite_TowerFactory towerFactory;

    public Tower greenTower;
    public Tower blueTower;
    public Tower redTower;

    void Start()
    {
        towerFactory = FindObjectOfType<Infinite_TowerFactory>();
    }

    public void SelectGreenTurret()
    {
        
        towerFactory.SelectTurretToBuild(greenTower);
    }

    public void SelectBlueTurret()
    {
       
        towerFactory.SelectTurretToBuild(blueTower);
    }

    public void SelectRedTurret()
    {

        towerFactory.SelectTurretToBuild(redTower);
    }
    

}
