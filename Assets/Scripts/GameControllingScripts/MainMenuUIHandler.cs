using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUIHandler : MonoBehaviour
{
    private GameObject[] buildableObjects;
    [SerializeField] string[] coreTypes;
    [SerializeField] GameObject templateButton;
    [SerializeField] GameObject coreContainerUI;
    [SerializeField] Image[] coreImages;
    [SerializeField] GameObject StartQuitUI;
    [SerializeField] GameObject CoreSelect;
    private string currentCategory;
    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
    public void EnterCoreChooser()
    {
        StartQuitUI.SetActive(false);
        CoreSelect.SetActive(true);
    }
    public void BackButton()
    {
        CoreSelect.SetActive(false);
        StartQuitUI.SetActive(true);
    }
    public void StartGame(string selectedCore)
    {
        SceneManager.LoadScene(1);
    }
    void Start()
    {
        for (int i = 0; i < coreTypes.Length; i++)
        {
            GameObject newButton = Instantiate(templateButton);
            newButton.SetActive(true);
            newButton.transform.SetParent(coreContainerUI.transform);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = coreTypes[i];
            newButton.GetComponent<Button>().onClick.AddListener(() => StartGame(newButton.GetComponentInChildren<TextMeshProUGUI>().text));
        }
    }
}
