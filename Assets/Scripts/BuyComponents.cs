using System;
using UnityEngine;
/*
 *  Az alap játékmódban a vásárlómenüben lévő 'BuyComponent'et működteti. Vagyis a tornyok vásárlását.
 * 
 */
public class BuyComponents : MonoBehaviour
{
    Player player;
    UpgradeMenu upgradeMenu;
    int buyPrice = 30;

    void Start()
    {
        player = FindObjectOfType<Player>();
        upgradeMenu = FindObjectOfType<UpgradeMenu>();
    }

    //Torony vásárlás a boltból.
    public void Buy()
    {
       
        int score = player.GetScore();
        
        int name = Convert.ToInt32(this.name);
        if(score >= buyPrice)
        {
            player.AddTower();
            player.ChangeScore(-buyPrice);
            upgradeMenu.AddTowerUpdater(name);
            GetComponent<AudioSource>().Play();
        }

        

    }
}
