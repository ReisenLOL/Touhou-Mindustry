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
    [SerializeField] GameObject mainMenuButtons;
    [SerializeField] private GameObject titleText;
    [SerializeField] GameObject coreSelect;
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
        mainMenuButtons.SetActive(false);
        titleText.SetActive(false);
        coreSelect.SetActive(true);
    }
    public void BackButton()
    {
        coreSelect.SetActive(false);
        mainMenuButtons.SetActive(true);
        titleText.SetActive(true);
    }
    public void StartGame(string selectedCore)
    {
        FindAnyObjectByType<SetNewCore>().selectedCore = selectedCore;
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
