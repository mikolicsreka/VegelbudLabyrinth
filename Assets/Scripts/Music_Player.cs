using UnityEngine;

/*
 *  A hangokért felelős osztály.
 */

public class Music_Player : MonoBehaviour
{
    public float volume = 1f;
    public float soundEffectsVol =  0.3f; 
    private static Music_Player instance;
    public static Music_Player Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;//Avoid doing anything else
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Object.DontDestroyOnLoad(this.gameObject);
        GetComponent<AudioSource>().Play();
    }

}
