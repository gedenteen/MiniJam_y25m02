using UnityEngine;

public class DontDestroyCanvas : MonoBehaviour
{
    public static DontDestroyCanvas instance {get; private set;} = null;

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
