using UnityEngine;

public class MinedResourceType : ResourceType
{
    public string type; // I AM SO STUPID x2
    public ResourceList resourceList;
    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        for (int i = 0; i < resourceList.resourceType.Length; i++)
        {
            if (type == resourceList.GetResourceType(i))
            {
                spriteRenderer.color = resourceList.GetResourceColor(i);
                return;
            }
        }
        Debug.Log("Theres no color for that resource, dummy!");
    }
}
