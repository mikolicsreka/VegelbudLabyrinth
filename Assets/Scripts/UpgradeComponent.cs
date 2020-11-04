using System;
using UnityEngine;

/// <summary>
/// Az alap játékmód boltjában a fejlesztéseket kezelő osztály.
/// </summary>
public class UpgradeComponent : MonoBehaviour
{
    Player player;
    TowerFactory towerFactory;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        towerFactory = FindObjectOfType<TowerFactory>();
    }

    // Update is called once per frame
    public void Upgrade()
    {
        int score = player.GetScore();
        int name = Convert.ToInt32(this.name);
        int price = player.towers[name].GetUpgradePrice();

        if (score >= price && player.towers[name].id == name )
        {

            /*
             Upgradenel megvaltozik a price, azert elobb kell levonni aztan upgradelni !
             */
            player.ChangeScore(-price);

            player.towers[name].Upgrade();
            //tower ingame
            towerFactory.RefreshTowerData(name);

            GetComponent<AudioSource>().Play();
        }
        

        

    }
}
