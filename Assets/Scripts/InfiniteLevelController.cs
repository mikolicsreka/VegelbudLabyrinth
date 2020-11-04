using UnityEngine;
using UnityEngine.UI;

/*
 * A végtelen pályás módot kezeli.
 */

public class InfiniteLevelController : MonoBehaviour
{

    Infinite_EnemySpawner enemySpawner;
    MazeGenerator mazeGenerator;
    bool isSpawning;

    //if they let in this number of enemies, you lost.
    int HP = 10;
    //number of enemies killed
    [Header("Player's money")]
    public int score = 200;
    [SerializeField] Text scoreText;
    [SerializeField] Text HPtext;
    [SerializeField] GameObject nextLevelMenu;
    [SerializeField] GameObject lostLevelMenu;
    [SerializeField] GameObject numOfLevelsText;


    private int numOfLevels = 0;
    private int winningScore = 200;

    //Singleton pattern
    static InfiniteLevelController mInstance;

    public static InfiniteLevelController Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject temp = new GameObject();
                temp.name = "InfiniteLevelController";
                mInstance = temp.AddComponent<InfiniteLevelController>();
            }
            return mInstance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
 
        enemySpawner = FindObjectOfType<Infinite_EnemySpawner>();
        mazeGenerator = FindObjectOfType<MazeGenerator>();

        scoreText.text = "$" + score.ToString();
        HPtext.text = "" + HP.ToString();

        GenerateLevel();
        

    }

    //A játék szüneteltetésének visszahozása
    private void OnLevelWasLoaded()
    {
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "$" + score.ToString();

        var enemies = FindObjectsOfType<EnemyMovement>();

        if (enemies.Length == 0 && !enemySpawner.GetIsSpawningTime())
        {
            SpawnControl(false);
        }

        if (HP <=0)
        {
            lostLevelMenu.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }

    //Szint generálása
    public void GenerateLevel()
    {
        DestroyTowersPlaced();
        mazeGenerator.CreateMazeWithPathfinder();
        
        StartCoroutine(enemySpawner.Spawn());
        AddScore(winningScore);
        nextLevelMenu.SetActive(false);
        // Increment number of levels
        numOfLevels++;

    }


    //Ha nincs spawning, a következő szintet betölti
    public void SpawnControl(bool isSpawning)
    {
        if (!isSpawning)
        {
            
            numOfLevelsText.GetComponent<TMPro.TextMeshProUGUI>().text = numOfLevels.ToString();
            
            nextLevelMenu.SetActive(true);
        }
    }

    //ONLY FOR DEV BUILD!!!!!!!!
    public void DestroyEnemies()
    {
        var enemies = enemySpawner.GetComponentsInChildren<EnemyMovement>();
        foreach (var item in enemies)
        {
            Destroy(item.gameObject);
        }
    }    
    public void DestroyTowersPlaced()
    {
        var towers = FindObjectsOfType<Tower>();
        foreach (var tower in towers)
        {
            if (tower.gameObject.name != "Player")
            {
                Destroy(tower.gameObject);
            }
           
        }
    }


    public void TickOneHP()
    {
        HP--;
        HPtext.text = "HP: " + HP.ToString();
    }

    public int GetHP()
    {
        return HP;
    }

    public void AddScorePerEnemy()
    {
        score += enemySpawner.GetPricePerEnemy();
    }

    public void AddScore(int _score)
    {
        score += _score;
    }

    public int GetScore()
    {
        return score;
    }

}
