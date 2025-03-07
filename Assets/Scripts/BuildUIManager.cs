using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildUIManager : MonoBehaviour
{
    private GameObject[] buildableObjects;
    private GameManager gameManager;
    [SerializeField] GameObject templateButton;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        buildableObjects = gameManager.GetComponent<GameManager>().buildList;
        for (int i = 0; i < buildableObjects.Length; i++)
        {
            GameObject newButton = Instantiate(templateButton);
            newButton.SetActive(true);
            newButton.transform.SetParent(this.gameObject.transform);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = buildableObjects[i].name;
            newButton.GetComponent<Button>().onClick.AddListener(() => gameManager.SetBuildMode(newButton.GetComponentInChildren<TextMeshProUGUI>().text));
            newButton.GetComponent<Button>().onClick.AddListener(() => gameManager.SetSelection(newButton.GetComponentInChildren<TextMeshProUGUI>().text));
        }
    }

    void Update()
    {
        
    }
}
