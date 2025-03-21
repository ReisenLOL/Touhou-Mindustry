using UnityEngine;

[CreateAssetMenu(fileName = "New Object Price", menuName = "Scripts/ObjectPrice")]
public class ObjectPrice : ScriptableObject
{
    [SerializeField] cost price;
    [System.Serializable]
    public struct cost
    {
        //stupid way time
        public string[] resourceName;
        public int[] amount;
        public string objectDescription;
    }
    public string[] GetResourceName() => price.resourceName;
    public int[] GetAmount() => price.amount;
    public string GetDescription => price.objectDescription;
}
