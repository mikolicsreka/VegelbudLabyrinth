using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Az alap játékmód boltját megjelenítő, kezelő osztály.
/// </summary>
public class UpgradeMenu : MonoBehaviour
{
    Player player;
    [SerializeField] GameObject towerBlueprint;
    [SerializeField] GameObject noneBlueprint;

    [SerializeField] GameObject levelOfTower;
    [SerializeField] Text scoreText;
    [SerializeField] Text countDownText;
    float countDown;

    bool isSetup = false;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>(); //így hogy csak 1 db van belole
        UpdateScore();
    }    
    void OnLevelWasLoaded()
    {
        player = FindObjectOfType<Player>(); //így hogy csak 1 db van belole
        UpdateScore();
    }

    List<GameObject> towerUpdaters = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        if (!isSetup)
        {
            //Mind a 8 négyzet konstruálása, az első ciklusban a már megvásároltaké
            //A második ciklusban a még megvásárolhatóké
            for (int i = 0; i < player.towers.Count; i++)
            {
                
                GameObject temp = Instantiate(towerBlueprint);
                var texts = towerBlueprint.GetComponentsInChildren<Text>();
                //Text level = towerBlueprint.GetComponentInChildren<Text>();
                //level.text = ""  + (i + 1).ToString()  +  ". Level " + player.towers[i].level.ToString();

                foreach (var text in texts)
                {
                    if (text.name == "Number")
                    {
                        text.text = (i + 1).ToString();
                    }
                    if (text.name == "Level")
                    {
                        text.text = "Level " + player.towers[i].level.ToString();
                    }
                }

                Text priceComp = towerBlueprint.GetComponentInChildren<Button>().GetComponentInChildren<Text>();
                priceComp.text = "UPGRADE(" + player.towers[i].GetUpgradePrice().ToString() + ")";

                temp.name = i.ToString();
                temp.transform.SetParent(this.transform, false);
                temp.GetComponent<AudioSource>().volume = FindObjectOfType<Music_Player>().soundEffectsVol;
                towerUpdaters.Add(temp);

            }
            for (int i = player.towers.Count; i < 8; i++)
            {
                

                GameObject temp = Instantiate(noneBlueprint);
                temp.name = i.ToString();
                temp.transform.SetParent(this.transform, false);
                temp.GetComponent<AudioSource>().volume = FindObjectOfType<Music_Player>().soundEffectsVol;

                //Text priceComp = towerBlueprint.GetComponentInChildren<Button>().GetComponentInChildren<Text>();
                //priceComp.text = "BUY(" + player.towers[i].GetBuyPrice().ToString() + ")";

                towerUpdaters.Add(temp);
            }

            isSetup = true;
        }


        UpdateLevel();

        UpdateScore();

    }

    private void UpdateLevel()
    {
        for (int i = 0; i < player.towers.Count; i++)
        {
            if (towerUpdaters[i].GetComponentInChildren<Text>())
            {
                //Text text = towerUpdaters[i].GetComponentInChildren<Text>();
                //text.text = "" + (i + 1).ToString() + "Level: " + player.towers[i].level;

                var texts = towerUpdaters[i].GetComponentsInChildren<Text>();

                foreach (var text in texts)
                {
                    if (text.name == "Number")
                    {
                        text.text = (i + 1).ToString();
                    }

                    if (text.name == "Level")
                    {
                        text.text = "Level " + player.towers[i].level;
                    }
                }

                Text priceComp = towerUpdaters[i].GetComponentInChildren<Button>().GetComponentInChildren<Text>();
                priceComp.text = "UPGRADE(" + player.towers[i].GetUpgradePrice() + ")";
            }
        }

    }

    void UpdateScore()
    {
        scoreText.text = player.GetScore().ToString();
    }


    // ha nem sorba vesszuk, akkkor bugos
    public void AddTowerUpdater(int number)
    {

        Destroy(towerUpdaters[player.towers.Count - 1]);
       

        GameObject temp = Instantiate(towerBlueprint);
        //Text level = towerBlueprint.GetComponentInChildren<Text>();
        //level.text = "" + (player.towers.Count - 1).ToString() + "Level " + player.towers[player.towers.Count - 1].level.ToString();

        var texts = towerBlueprint.GetComponentsInChildren<Text>();

        foreach (var text in texts)
        {
            if (text.name == "Number")
            {
                text.text = (player.towers.Count - 1).ToString();
            }

            if (text.name == "Level")
            {
                text.text = "Level " + player.towers[player.towers.Count - 1].level.ToString();
            }
        }


        temp.name = (player.towers.Count - 1).ToString();
        temp.transform.SetParent(this.transform, false);
        temp.transform.SetSiblingIndex(player.towers.Count-1);
        

        towerUpdaters[player.towers.Count - 1] = temp;
        
    }

    public void SetCountingDown(float time)
    {
        countDownText.text = (countDown - time).ToString();

    }

    public void SetCountDownValue(float value)
    {
        countDown = value;
    }
}
