using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * Ez a menü ugrik fel, ha megállítjuk a játékot az Esc gombbal.
 * 
 */

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    public Button saveButton;

    Player player;

    Canvas[] uis;
    [SerializeField] Text informationText;

    string[] hints =
    {
        "Advice: Press L to lock on the camera.",
        "Advice: Use your mouse scroll to zoom in or out.",
        "Advice: You can move the camera with WASD or with dragging your mouse.",
        "Advice: Buying more towers not always the best option. Try to upgrade!",
        "Advice: If you are bored playing the basic mode, try the endless!",
        "Advice: In the endless mode, you can sell towers any time!"
    };
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();

        uis = FindObjectsOfType<Canvas>();
        try
        {
            informationText.text = hints[0];
        }
        catch (System.Exception)
        {

            //NULLEXCEPTION
        }



        try
        {
            Button btn = saveButton.GetComponent<Button>();
            btn.onClick.AddListener(TaskOnClick);
        }
        catch (System.Exception)
        {

            //NULLEXCEPTION
        }

    }

    void TaskOnClick()
    {
        Text[] texts = FindObjectsOfType<Text>();
        foreach (var text in texts)
        {
            if (text.name == "SaveText")
            {
                text.text = "Your Game is Saved!";
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Text[] texts = FindObjectsOfType<Text>();
            foreach (var text in texts)
            {
                if (text.name == "SaveText")
                {
                    text.text = "";
                }
            }

            if (GameIsPaused)
            {

                Resume();
                UnmuteAll();

            }
            else
            {
                Pause();
                ToggleAllUI(false);
                MuteAll();
            }
        }
    }

    public void Restart()
    {
        if (!player.fromSave)
        {
            player.Reset();
        }
        else
        {
            player.LoadPlayer();
        }

        this.gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Resume()
    {

        //TODO : generate random number
        if (informationText != null)
        {
            int random = Random.Range(0,6);
            try
            {
                informationText.text = hints[random];
            }
            catch (System.Exception)
            {

                //NULLEXCEPTION
            }
            
        }

        ToggleAllUI(true);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {

        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {

        SceneManager.LoadScene("StartMenu");

    }

    public void QuitGame()
    {
        print("Quit");
        Application.Quit();
    }

    private void ToggleAllUI(bool onoff)
    {

        if (uis != null)
        {
            foreach (var ui in uis)
            {
                if (ui != null)
                {

                    if (ui.name != "PauseMenu" && ui.name != "Panel")
                    {

                        ui.gameObject.SetActive(onoff);
                    }
                }


            }
        }

    }

    //Minden elem lenémítása
    private void MuteAll()
    {
        if (SceneManager.GetActiveScene().name == "EndlessLevels")
        {
            var musics = FindObjectOfType<Infinite_TowerFactory>().GetComponentsInChildren<AudioSource>();

            if (musics != null)
            {
                foreach (var item in musics)
                {
                    item.volume = 0f;
                }
            }

        }
        else if(SceneManager.GetActiveScene().name != "Extras")
        {
            var musics = FindObjectOfType<TowerFactory>().GetComponentsInChildren<AudioSource>();

            if (musics != null)
            {
                foreach (var item in musics)
                {
                    item.volume = 0f;
                }
            }

        }

    }

    //Minden elemre vissza adjuk a hangot
    private void UnmuteAll()
    {
        if (SceneManager.GetActiveScene().name == "EndlessLevels")
        {
            var musics = FindObjectOfType<Infinite_TowerFactory>().GetComponentsInChildren<AudioSource>();

            foreach (var item in musics)
            {
                item.volume = FindObjectOfType<Music_Player>().soundEffectsVol;
            }
        }
        else
        {
            try
            {
                var musics = FindObjectOfType<TowerFactory>().GetComponentsInChildren<AudioSource>();

                foreach (var item in musics)
                {
                    item.volume = FindObjectOfType<Music_Player>().soundEffectsVol;
                }
            }
            catch (System.Exception)
            {

                //NULLEXCEPTION
            }

        }

    }



}
