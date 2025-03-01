using UnityEngine;

public class MineResource : MonoBehaviour
{
    public string minedResource;
    private GameManager gameManager;
    private float _time;
    public float miningSpeed;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        if (_time >= miningSpeed)
        {
            if (minedResource == "Resource")
            {
                gameManager.resources++;
            }
        }
    }
}
