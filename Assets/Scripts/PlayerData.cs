using System.Collections.Generic;
using UnityEngine.SceneManagement;

/// <summary>
/// A mentéshez a játékos nyers adatainak osztálya.
/// </summary>
[System.Serializable]
public  class PlayerData
{
    public int level;
    public int score;
    public string scene;

    public List<int> towerLevels;
    public int numTowers;

    public PlayerData(Player player)
    {
        level = SceneManager.GetActiveScene().buildIndex;
        score = player.score;
        scene = player.scene;
        

        numTowers = player.towers.Count;
        towerLevels = new List<int>();

        for (int i = 0; i < player.towers.Count; i++)
        {
            towerLevels.Add(player.towers[i].level);
        }
    }

}
