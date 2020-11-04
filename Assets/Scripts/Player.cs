using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A játékos adatait hordozza.
/// </summary>
public class Player : MonoBehaviour
{
    public int level;
    public int score;
    public string scene;
    public bool fromSave;

    public int qualityLevel = 6;
    
    

    [SerializeField] public  List<Tower> towers = new List<Tower>();
    // public Tower towerPrefab;
    Tower tower;

    public int numOfTowers = 0;
    public int startingTowerNum = 3;

    static Player mInstance;

    public static Player Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject temp = new GameObject();
                temp.name = "Player";
                mInstance = temp.AddComponent<Player>();
            }
            return mInstance;
        }
    }


    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        
        level = 1;
        score = 0;

        scene = SceneManager.GetActiveScene().name;

        for (int i = 0; i < startingTowerNum; i++)
        {

            AddTower();

        }

       
       
    }

    private void OnLevelWasLoaded(int level)
    {
        if(level > 0 && SceneManager.GetSceneByBuildIndex(level).name != "Extras")
        {
            SetLevel(level);
        }

        QualitySettings.SetQualityLevel(qualityLevel);

    }

    public void AddScore(int scorePerHit)
    {
        score += scorePerHit;
    }

    public void SetLevel(int level_)
    {
        level = level_;
    }

    public int GetScore()
    {
        return score;
    }

    public void ChangeScore(int change)
    {
        score += change;
    }

    public void AddTower()
    {
        tower = gameObject.AddComponent<Tower>();
        tower.id = towers.Count;
        tower.Reset();
        towers.Add(tower);
        numOfTowers++;
    }

    public void SavePlayer()
    {

        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        level = data.level;   
        scene = data.scene;
        score = data.score;
        int numOfTowers_ = data.towerLevels.Count;
        fromSave = true;
        numOfTowers = 0;
        
        for (int i = 0 ; i < numOfTowers_; i++)
        {
            AddTower();
            Tower tower = towers[i];
            tower.level = data.towerLevels[i];
        }

    }

    public void Reset()
    {
        this.score = 0;
        this.numOfTowers = 3;
        towers.Clear();
        for (int i = 0; i < 3; i++)
        {
            tower = gameObject.AddComponent<Tower>();
            tower.id = i;
            towers.Add(tower);

        }

    }
}
