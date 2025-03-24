using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildUIManager : MonoBehaviour
{
    private GameObject[] buildableObjects;
    private GameManager gameManager;
    [SerializeField] GameObject templateButton;
    [SerializeField] GameObject categoryContainerUI;
    [SerializeField] string[] buildingCategories;
    [SerializeField] Image[] categoryImages;
    private string currentCategory;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        buildableObjects = gameManager.GetComponent<GameManager>().buildList;
        for (int i = 0; i < buildingCategories.Length; i++)
        {
            GameObject newButton = Instantiate(templateButton);
            newButton.SetActive(true);
            newButton.transform.SetParent(this.gameObject.transform);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = buildingCategories[i];
            newButton.GetComponent<Button>().onClick.AddListener(() => SelectCategory(newButton.GetComponentInChildren<TextMeshProUGUI>().text));
        }
    }
    public void SelectCategory(string category)
    {
        if (currentCategory == category || !categoryContainerUI.activeSelf )
        {
            categoryContainerUI.SetActive(!categoryContainerUI.activeSelf);
        }
        if (categoryContainerUI.activeSelf)
        {
            if (categoryContainerUI.transform.childCount > 0)
            {
                for (int i = 0; i < categoryContainerUI.transform.childCount; i++)
                {
                    Destroy(categoryContainerUI.transform.GetChild(i).gameObject);
                }
            }
            for (int i = 0; i < buildableObjects.Length; i++)
            {
                if (buildableObjects[i].GetComponent<ObjectStats>().category != category)
                {
                    continue;
                }
                GameObject newButton = Instantiate(templateButton);
                newButton.SetActive(true);
                newButton.transform.SetParent(categoryContainerUI.gameObject.transform);
                newButton.GetComponentInChildren<TextMeshProUGUI>().text = buildableObjects[i].name;
                newButton.GetComponent<Button>().onClick.AddListener(() => gameManager.SetBuildMode(newButton.GetComponentInChildren<TextMeshProUGUI>().text));
                newButton.GetComponent<Button>().onClick.AddListener(() => gameManager.SetSelection(newButton.GetComponentInChildren<TextMeshProUGUI>().text));
            }
        }
        currentCategory = category;
    }
}
