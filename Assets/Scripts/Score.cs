using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A játékban megjelenő score textet frissíti.
/// </summary>
public class Score : MonoBehaviour
{
    Player player;
    [SerializeField] Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>(); //így hogy csak 1 db van belole
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            scoreText.text = player.GetScore().ToString();
        }
        
    }
}
