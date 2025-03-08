using UnityEngine;

[CreateAssetMenu(fileName = "New Resource List", menuName = "Scripts/ResourceList")]
public class ResourceList : ScriptableObject
{
    public string[] resourceType;
    public string GetResourceType(int index) => resourceType[index];
}
