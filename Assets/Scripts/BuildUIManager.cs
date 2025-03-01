using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildUIManager : MonoBehaviour
{
    public GameObject[] buildableObjects;
    [SerializeField] GameObject templateButton;
    void Start()
    {
        for (int i = 0; i < buildableObjects.Length; i++)
        {
            GameObject newButton = Instantiate(templateButton);
            newButton.transform.SetParent(this.gameObject.transform);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = buildableObjects[i].name;
        }
    }

    void Update()
    {
        
    }
}
