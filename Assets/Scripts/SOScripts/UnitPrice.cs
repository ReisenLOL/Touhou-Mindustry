using UnityEngine;

[CreateAssetMenu(fileName = "New Resource List", menuName = "Scripts/UnitPrice")]
public class UnitPrice : ScriptableObject
{
    public string[] resourceName;
    public int[] amount;
    public string[] GetResourceName() => resourceName;
    public int[] GetAmount() => amount;
}
