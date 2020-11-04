using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A játék megnyerésénél felugró menü.
/// </summary>
public class WinMenu : MonoBehaviour
{
    EnemySpawner enemySpawner;
    [SerializeField] GameObject levelWinMenu;
    [SerializeField] GameObject gameWinMenu;
    private int numberOfLevels = 3;
    Player player;

    [SerializeField] AudioClip winSFX;
    bool isLevelWon = false;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }
    public void LoadNextLevel()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        player = FindObjectOfType<Player>();
        player.Reset();
        //player.AddScore(300);

    }
    public void LoadMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    private void OnLevelWasLoaded()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    public void TryAgain()
    {

        player.Reset();
        gameWinMenu.SetActive(false);
        SceneManager.LoadScene("Level_1");
        Time.timeScale = 1f;
        isLevelWon = false;
    }

    void Win()
    {
        if (winSFX != null)
        {
            GetComponent<AudioSource>().PlayOneShot(winSFX);
        }


        //If we are at the last level, the game is absolutely won, so:
        if (SceneManager.GetActiveScene().name == ("Level_" + numberOfLevels))
        {
            //Show the absolute game winner canvas
            gameWinMenu.SetActive(true);
            Time.timeScale = 0f;

        }
        else
        {
            levelWinMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    void Update()
    {
        if (enemySpawner.IsLevelWon() && !isLevelWon)
        {
            isLevelWon = true;
            Win();
        }
    }
}
