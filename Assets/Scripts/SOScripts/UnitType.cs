using UnityEngine;

[CreateAssetMenu(fileName = "New Unit Type", menuName = "Scripts/UnitType")]
public class UnitType : ScriptableObject
{
    public GameObject projectile;
    public float damageDealt;
    public float fireRate;
    public float range;
    public float speed;
}
