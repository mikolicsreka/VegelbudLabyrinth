using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A tornyok felett megjelenő menüt (UIt) kezeli.
/// </summary>
public class TowerUI : MonoBehaviour
{
    // Start is called before the first frame update

    InfiniteLevelController levelController;
    Text[] texts;
    void Start()
    {
        Canvas thisCanvas = this.gameObject.GetComponent<Canvas>();
        thisCanvas.worldCamera = FindObjectOfType<Camera>();

        levelController = FindObjectOfType<InfiniteLevelController>();

        texts = GetComponentsInChildren<Text>();

        Tower tower = this.GetComponentInParent<Tower>();
        SetUpgradePrice(tower);
        this.gameObject.GetComponent<AudioSource>().volume = FindObjectOfType<Music_Player>().soundEffectsVol;

    }
    public void Upgrade()
    {
        Tower tower = this.GetComponentInParent<Tower>();
        if (levelController.score >= tower.upgradePrice)
        {
            levelController.score -= tower.upgradePrice;


            tower.Upgrade();

            SetUpgradePrice(tower);
            GetComponent<AudioSource>().Play();

        }

    }

    public void Sell()
    {
        Tower tower = this.GetComponentInParent<Tower>();
        StartCoroutine(SellMusicPlayAndDestroy(tower));
        tower.baseWaypoint.isPlaceable = true;
        levelController.score += 50;


    }

    void SetUpgradePrice(Tower tower)
    {
        foreach (var text in texts)
        {
            if (text.name == "UpgradePriceText")
            {
                text.text = tower.upgradePrice + "$";
            }
        }
    }

    IEnumerator SellMusicPlayAndDestroy(Tower tower)
    {
        GetComponent<AudioSource>().Play();
        yield return new WaitForSecondsRealtime(GetComponent<AudioSource>().clip.length);
        Destroy(tower.gameObject);
    }
}
