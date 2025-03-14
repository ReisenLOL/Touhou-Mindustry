using UnityEngine;

[CreateAssetMenu(fileName = "New Resource List", menuName = "Scripts/ResourceList")]
public class ResourceList : ScriptableObject
{
    public string[] resourceType;
    public Color[] resourceColors;
    public string GetResourceType(int index) => resourceType[index];
    public Color GetResourceColor(int index) => resourceColors[index];
    //this is stupid, but the index for both will be the same, therefore their colors are linked. i dont know if theres a better way but i sure as hell cant find out.
}
