using TMPro;
using UnityEngine;

public class InfoUI : MonoBehaviour
{
    private ObjectStats selectedObject;
    [SerializeField] GameManager gameManager;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI inputText;
    public void ShowDescription()
    {
        gameObject.SetActive(true);
        selectedObject = gameManager.selection.GetComponent<ObjectStats>();
        if (selectedObject == null)
        {
            Debug.LogWarning("WHAT IS THAT");
            return;
        }
        nameText.text = selectedObject.name;
        string[] resourceToDisplay = selectedObject.price.GetResourceName();
        int[] amountToDisplay = selectedObject.price.GetAmount();
        string costDisplay = "";
        for (int i = 0; i < resourceToDisplay.Length; i++)
        {
            costDisplay += resourceToDisplay[i] + ": " + amountToDisplay[i] + "    ";
        }
        costText.text = costDisplay;
        descriptionText.text = selectedObject.price.GetDescription;
        string inputDisplay = "Inputs\n";
        string[] resourceInputToDisplay = selectedObject.resourceTypeInput;
        int[] inputAmountToDisplay = selectedObject.inputAmount;
        if (resourceInputToDisplay.Length > 0)
        {
            for (int i = 0; i < resourceInputToDisplay.Length; i++)
            {
                inputDisplay += resourceInputToDisplay[i] + ": " + inputAmountToDisplay[i] + "    ";
            }
            inputText.text = inputDisplay;
        }
    }
    public void hideDescription()
    {
        gameObject.SetActive(false);
    }
}
