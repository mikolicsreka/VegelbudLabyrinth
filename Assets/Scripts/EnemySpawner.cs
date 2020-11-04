using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Az alap játékmódban a támadók létrehozásáért, ellenséges hullámok irányításáért és  minden ehhez kapcsolodó dologért felelős osztály.
 * source: https://wiki.unity3d.com/index.php/Enemy_Spawner
 *         https://answers.unity.com/questions/342665/how-to-change-enemy-spawn-wave-script-to-start-new.html
 */
public class EnemySpawner : MonoBehaviour
{
    [Range(0.1f, 120f)]
    [SerializeField]
    float secondsBetweenSpawns = 2f;
    [SerializeField] EnemyMovement enemyPrefab; //igy csak enemy lehet
    [SerializeField] Text spawnedEnemiesText;
    int numberOfEnemies;


    [SerializeField] GameObject upgradeMenu;
    [SerializeField] Transform enemyParentTransform;

    bool isSpawning = false;

    [SerializeField] Text countDownText;
    [SerializeField] GameObject waveCountDownGabeObject;
    [SerializeField] GameObject waveInProgressGameObject;

    //-----------------------------------
    // All the Enums
    //-----------------------------------
    // The different Enemy levels
    public enum EnemyLevels
    {
        Easy,
        Medium,
        Hard,
        Boss
    }
    //---------------------------------
    // End of the Enums
    //---------------------------------
    // Enemy level to be spawnedEnemy
    public EnemyLevels enemyLevel = EnemyLevels.Easy;

    //----------------------------------
    // Enemy Prefabs
    //----------------------------------
    [SerializeField] public GameObject EasyEnemy;
    [SerializeField] public GameObject MediumEnemy;
    [SerializeField] public GameObject HardEnemy;
    [SerializeField] public GameObject BossEnemy;
    private Dictionary<EnemyLevels, GameObject> Enemies = new Dictionary<EnemyLevels, GameObject>(4);
    //----------------------------------
    // End of Enemy Prefabs
    //----------------------------------

    //----------------------------------
    // Enemies and how many have been created and how many are to be created
    //----------------------------------
    private int totalEnemy = 10;
    private int numEnemy = 0;
    private int spawnedEnemy = 0;
    //----------------------------------
    // End of Enemy Settings
    //----------------------------------

    // The ID of the spawner
    private int SpawnID;
    //----------------------------------
    // Different Spawn states and ways of doing them
    //----------------------------------
    private bool waveSpawn = false;
    public bool Spawn = true;
    //public SpawnTypes spawnType = SpawnTypes.Normal;
    // timed wave controls
    private float waveTimer = 10.0f;
    private float timeTillWave = 0.0f;
    //Wave controls
    int totalWaves = 5;
    private int numWaves = 0;

    private bool isLevelWon = false;
    //----------------------------------
    // End of Different Spawn states and ways of doing them
    //----------------------------------

    AlgorithmSettings settings;

    //Singleton pattern
    static EnemySpawner mInstance;

    public static EnemySpawner Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject temp = new GameObject();
                temp.name = "EnemySpawner";
                mInstance = temp.AddComponent<EnemySpawner>();
            }
            return mInstance;
        }
    }

    public float GetWaveTimer()
    {
        return waveTimer;
    }
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(RepeatedlySpawnEnemies());
        //spawnedEnemiesText.text = "Enemies: " + numberOfEnemies.ToString();

        // sets a random number for the id of the spawner
        Spawn = true;
        SpawnID = UnityEngine.Random.Range(1, 500);
        Enemies.Add(EnemyLevels.Easy, EasyEnemy);
        Enemies.Add(EnemyLevels.Boss, BossEnemy);
        Enemies.Add(EnemyLevels.Medium, MediumEnemy);
        Enemies.Add(EnemyLevels.Hard, HardEnemy);

        settings = FindObjectOfType<AlgorithmSettings>();
    }
    void OnLevelWasLoaded()
    {
        Time.timeScale = 1;
    }

    void Update()
    {
        if (Spawn)
        {

            // checks if the number of waves is bigger than the total waves
            if (numWaves <= totalWaves)
            {

                SetEnemiesForWaves();

                // Increases the timer to allow the timed waves to work
                if (!isSpawning)
                {
                    //TIMED WAVE
                    if (!upgradeMenu.activeSelf)
                    {
                        settings.SetIsTowerPlacementTime(true);
                    }

                    waveInProgressGameObject.SetActive(false);
                    waveCountDownGabeObject.SetActive(true);
                    timeTillWave += Time.deltaTime;
                    int countdown = System.Convert.ToInt32(waveTimer - timeTillWave);
                    countDownText.text = countdown.ToString();



                }
                if (waveSpawn && !isSpawning)
                {
                    //spawns an enemy
                    //spawnEnemy();
                    StartCoroutine(RepeatedlySpawnEnemy());
                    waveSpawn = false;
                }
                // checks if the time is equal to the time required for a new wave
                if (timeTillWave >= waveTimer)
                {

                    SetUpgradeMenuActive(false);
                    // enables the wave spawner
                    waveSpawn = true;
                    // sets the time back to zero
                    timeTillWave = 0.0f;
                    // increases the number of waves
                    numWaves++;
                    // A hack to get it to spawn the same number of enemies regardless of how many have been killed
                    numEnemy = 0;
                }
                if (numEnemy >= totalEnemy)
                {
                    // diables the wave spawner
                    waveSpawn = false;
                }
            }
            else
            {
                //   Spawn = false;

                isLevelWon = true;
            }
        }
    }

    public bool IsLevelWon()
    {
        return isLevelWon;
    }


    //Ellenségek típusának beállítása szintenként
    private void SetEnemiesForWaves()
    {
        if (numWaves == 1 || numWaves == 0)
        {
            enemyLevel = EnemyLevels.Easy;
            totalEnemy = 5;
            waveTimer = 10.0f;

        }
        else if (numWaves == 2)
        {
            enemyLevel = EnemyLevels.Easy;
            totalEnemy = 10;
        }
        else if (numWaves == 3)
        {
            enemyLevel = EnemyLevels.Medium;
            totalEnemy = 10;
            waveTimer = 12.0f;
        }
        else if (numWaves == 4)
        {
            enemyLevel = EnemyLevels.Hard;
            totalEnemy = 6;
            waveTimer = 14.0f;
        }
        else if (numWaves == 5)
        {
            enemyLevel = EnemyLevels.Boss;
            totalEnemy = 5;
        }
    }

    private void spawnEnemy()
    {
        GameObject Enemy = (GameObject)Instantiate(Enemies[enemyLevel], gameObject.transform.position, Quaternion.identity);
        Enemy.transform.parent = enemyParentTransform;
        Enemy.SendMessage("setName", SpawnID);
       
        Enemy.GetComponent<AudioSource>().volume = FindObjectOfType<Music_Player>().soundEffectsVol;
        // Increase the total number of enemies spawned and the number of spawned enemies
        numEnemy++;
        spawnedEnemy++;
    }

    // Call this function from the enemy when it "dies" to remove an enemy count
    public void killEnemy(int sID)
    {
        // if the enemy's spawnId is equal to this spawnersID then remove an enemy count
        if (SpawnID == sID)
        {
            numEnemy--;
        }
    }

    // returns the Time Till the Next Wave, for a interface, ect.
    public float TimeTillWave
    {
        get
        {
            return timeTillWave;
        }
    }

    //Folyamatos enemy spawn a hullámokban
    IEnumerator RepeatedlySpawnEnemy()
    {

        settings.SetIsTowerPlacementTime(false);
        isSpawning = true;
        waveCountDownGabeObject.SetActive(false);
        waveInProgressGameObject.SetActive(true);

        for (int i = 0; i < totalEnemy; i++)
        {
            var newEnemy = Instantiate(Enemies[enemyLevel], transform.position, Quaternion.identity); //spawn
            newEnemy.transform.parent = enemyParentTransform;

            newEnemy.SendMessage("setName", SpawnID);
            // Increase the total number of enemies spawned and the number of spawned enemies
            numEnemy++;
            spawnedEnemy++;

            yield return new WaitForSeconds(secondsBetweenSpawns);
        }

        yield return new WaitUntil(() => this.GetComponentsInChildren<EnemyDamage>().Length == 0);
        if (numWaves == totalWaves)
        {
            // After last wave, the win menu should be immediate,  so:
            numWaves++;
 
        }
        else
        {
            SetUpgradeMenuActive(true);
            isSpawning = false;
            SetUpgradeMenuActive(true);

            yield return new WaitUntil(() => upgradeMenu.activeSelf == false);
            SetUpgradeMenuActive(false);
            settings.SetIsTowerPlacementTime(true);
        }


    }

    //Felkapcsolja az upgrade menut
    private void SetUpgradeMenuActive(bool isActive)
    {
        upgradeMenu.SetActive(isActive);
        settings.SetIsTowerPlacementTime(!isActive);

    }

}
