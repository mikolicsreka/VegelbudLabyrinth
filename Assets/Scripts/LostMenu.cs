using UnityEngine;
using UnityEngine.SceneManagement;

/*
 *  A felugró menü, ha veszítünk. 
 * 
 */

public class LostMenu : MonoBehaviour
{
    Player player;
    

    public void LoadMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void TryAgain()
    {
        player = FindObjectOfType<Player>();
        if (player !=null)
        {
            player.Reset();
        }
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Start()
    {
        player = FindObjectOfType<Player>();
    }
}
