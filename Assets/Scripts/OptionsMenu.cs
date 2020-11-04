using UnityEngine;
using UnityEngine.UI;


/*
 * A Main menüben található beállítások szkriptje.
 */
public class OptionsMenu : MonoBehaviour
{
    AlgorithmSettings algorithmSettings;
    [SerializeField] Slider volumeSlider;
    void Start()
    {
        algorithmSettings = FindObjectOfType<AlgorithmSettings>();
        QualitySettings.SetQualityLevel(6);
        Music_Player audio = FindObjectOfType<Music_Player>();
        //audio.GetComponent<AudioSource>().volume = 0.5f;
        Dropdown[] dropdown = FindObjectsOfType<Dropdown>();
        foreach (var item in dropdown)
        {
            if (item.name == "QualityDropdown")
            {
                Player player = FindObjectOfType<Player>();
                item.value = player.qualityLevel/2;
            }

            if (item.name =="AlgorithmDropdown")
            {
                string algorithmName = algorithmSettings.GetAlgorithmName();
                if (algorithmName == "BFS")
                {
                    item.value = 0;
                }
                else if (algorithmName == "DIJKSTRA")
                {
                    item.value = 1;
                }
                else
                {
                    item.value = 2;
                }
                    
            }
        }

        

        //Music_Player audio = FindObjectOfType<Music_Player>();
        volumeSlider.value = audio.GetComponent<AudioSource>().volume;

    }

    public void SetVolume(float _volume)
    {

        Music_Player audio = FindObjectOfType<Music_Player>();
        audio.volume = _volume;
        audio.soundEffectsVol = _volume / 3.33f;
        audio.GetComponent<AudioSource>().volume = _volume;

    }

    public void SetAlgorithm(int index)
    {

        if(index == 0)
        {
            algorithmSettings.SetAlgorithmName("BFS");
        }        
        if(index == 1)
        {
            algorithmSettings.SetAlgorithmName("DIJKSTRA");
        }        
        if(index == 2)
        {
            algorithmSettings.SetAlgorithmName("ASTAR");
        }
        
    }


    public void SetQuality(int qualityIndex)
    {
        Player player = FindObjectOfType<Player>();
        player.qualityLevel = qualityIndex * 2;
        QualitySettings.SetQualityLevel(qualityIndex*2);
    }
}
