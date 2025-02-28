using UnityEngine;

public class DontDestroyAudio : MonoBehaviour
{
    public static DontDestroyAudio instance {get; private set;} = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
