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
        gameManager = FindFirstObjectByType<GameManager>();
        buildableObjects = gameManager.GetComponent<GameManager>().buildList;
        for (int i = 0; i < buildingCategories.Length; i++)
        {
            GameObject newButton = Instantiate(templateButton, transform);
            newButton.SetActive(true);
            TextMeshProUGUI newButtonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
            newButtonText.text = buildingCategories[i];
            newButtonText.autoSizeTextContainer = true;
            newButton.GetComponent<Button>().onClick.AddListener(() => SelectCategory(newButtonText.text));
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
                GameObject newButton = Instantiate(templateButton, categoryContainerUI.gameObject.transform);
                newButton.SetActive(true);
                TextMeshProUGUI newButtonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
                newButtonText.text = buildableObjects[i].name;
                newButtonText.autoSizeTextContainer = true;
                newButton.GetComponent<Button>().onClick.AddListener(() => gameManager.SetBuildMode(newButtonText.text));
                newButton.GetComponent<Button>().onClick.AddListener(() => gameManager.SetSelection(newButtonText.text));
                newButton.GetComponent<Image>().sprite = buildableObjects[i].GetComponent<SpriteRenderer>().sprite;
                newButtonText.gameObject.SetActive(false);
            }
        }
        currentCategory = category;
    }
}
