using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/*
 * A végtelen pályás módon az ellenségek spawnolásáért felelős osztály.
 */
public class Infinite_EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private float spawnDelay = 2.0f;

    [SerializeField]
    public GameObject[] enemyPrefab;

    [SerializeField]
    private GameObject enemies; //parent object


    private bool spawn;
    private int spawned;
    [SerializeField] int maxSpawnNumber = 20;

    private AudioSource audioSource;

    bool isSpawningTime = true;
    InfiniteLevelController levelController;

    int pricePerEnemy = 25;

    [SerializeField] Text numberOfEnemies;

    //Singleton pattern
    static Infinite_EnemySpawner mInstance;

    public static Infinite_EnemySpawner Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject temp = new GameObject();
                temp.name = "Infinite_EnemySpawner";
                mInstance = temp.AddComponent<Infinite_EnemySpawner>();
            }
            return mInstance;
        }
    }
    private void Start()
    {
        levelController = FindObjectOfType<InfiniteLevelController>();
        numberOfEnemies.text = (maxSpawnNumber - spawned).ToString();
    }


    public IEnumerator Spawn()
    {
        while (true)
        {
            int randomValue = Random.Range(0, 2);

            isSpawningTime = true;

            Instantiate(enemyPrefab[randomValue], gameObject.transform.position, Quaternion.identity, enemies.transform);

            // audioSource.PlayOneShot(spawnSFX);

            spawned++;
            numberOfEnemies.text = (maxSpawnNumber - spawned).ToString();
            // UpdateScore();

            yield return new WaitForSeconds(spawnDelay);

            if (spawned >= maxSpawnNumber)
            {
                isSpawningTime = false;
                spawned = 0;
                
                break;
            }
        }
    }

    public bool GetIsSpawningTime()
    {
        return isSpawningTime;
    }

    public int GetPricePerEnemy()
    {
        return pricePerEnemy;
    }

}
