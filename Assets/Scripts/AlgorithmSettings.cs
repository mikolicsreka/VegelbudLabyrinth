using UnityEngine;
/*
 * Hordozza a MainMenu beállításaiban beállított algoritmus beállításokat, 
 * illetve az ellenséges hullámok közötti torony lerakásának lehetségességét.
 */
public class AlgorithmSettings : MonoBehaviour
{
    string algorithmName;

    static AlgorithmSettings mInstance;

    bool isTowerPlacementTime = true;


    //Singleton
    public static AlgorithmSettings Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject go = new GameObject();
                go.name = "AlgorithmSettings";
                mInstance = go.AddComponent<AlgorithmSettings>();


            }

            return mInstance;
        }
    }

    public void SetIsTowerPlacementTime(bool isTime)
    {
        isTowerPlacementTime = isTime;
    }

    public bool GetIsTowerPlacementTime()
    {
        return isTowerPlacementTime;
    }

    public void SetAlgorithmName(string name)
    {
        algorithmName = name;
    }

    public string GetAlgorithmName()
    {
        if (algorithmName != null)
        {
            return algorithmName;
        }
        else
        {
            return algorithmName = "BFS"; 
        }
        
    }


    void Start()
    {
        DontDestroyOnLoad(this);
    }

}
