using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Hány towerünk maradt még? A játékban megjelenített textet kezeli.
/// </summary>
public class TowersLeftText : MonoBehaviour
{
    Player player;
    TowerFactory towerFactory;
    [SerializeField] Text towersLeftText;
    // Start is called before the first frame update
    void Start()
    {
        towerFactory = FindObjectOfType<TowerFactory>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        int towersLeft = player.towers.Count - towerFactory.GetComponentsInChildren<Tower>().Length;
        towersLeftText.text = towersLeft.ToString();
    }
}
