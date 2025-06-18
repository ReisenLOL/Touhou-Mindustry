using UnityEngine;

public class SetNewCore : MonoBehaviour
{
    public static SetNewCore instance;
    public string selectedCore;
    void Awake()
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
