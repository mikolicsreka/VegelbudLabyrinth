using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 *  A főmenü szkriptje. 
 */

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button PlayButton;
    [SerializeField] GameObject GameSaveAvailable;
    Player player;
    public Text savePlaceText;

    [SerializeField] GameObject InformationPanel;


    void Start()
    {
        if (SaveSystem.SaveExists())
        {
            PlayButton.gameObject.SetActive(false);
            GameSaveAvailable.SetActive(true);
        }
        else
        {
            PlayButton.gameObject.SetActive(true);
            GameSaveAvailable.SetActive(false);
        }

        player = Player.Instance;
        AlgorithmSettings algorithmSettings = AlgorithmSettings.Instance;
        DontDestroyOnLoad(player);
        DontDestroyOnLoad(algorithmSettings);

        savePlaceText.text = "A játék mentését, ha van, a következő helyen találod: \n" + Application.persistentDataPath + "/player.fun";
    }

    private void Update()
    {
        if (InformationPanel.activeSelf == true && Input.GetKeyDown(KeyCode.Escape))
        {
            InformationPanel.SetActive(false);

        }
    }

    void OnLevelWasLoaded()
    {
        Time.timeScale = 1f;
    }

    public void PlayGame()
    {
        player.Reset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        //SceneManager.LoadScene("Level_3");
    }

    public void ContinueGame()
    {
        player.LoadPlayer();
        Debug.Log(player.level);
        SceneManager.LoadScene(player.level);
    }

    public void LoadEndlessLevels()
    {
        SceneManager.LoadScene("EndlessLevels");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void LoadExtras()
    {
        SceneManager.LoadScene("Extras");
    }

    public void HelpButton()
    {
        bool isactive = InformationPanel.activeSelf;
        InformationPanel.SetActive(!isactive);
    }
}
