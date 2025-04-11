using UnityEngine;
[CreateAssetMenu(fileName = "New Ammo Type", menuName = "Scripts/AmmoType")]

public class AmmoType : ScriptableObject
{
    public string ammoResource;
    public int bulletCount;
    public float damage;
    public bool isFlak;
    public float spread;
}
