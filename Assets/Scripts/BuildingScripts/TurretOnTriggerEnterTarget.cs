using UnityEngine;

public class TurretOnTriggerEnterTarget : MonoBehaviour
{
    private TurretController turretController;
    private void Start()
    {
        turretController = gameObject.GetComponentInParent<TurretController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Unit") && other.GetComponent<UnitStats>().isEnemy)
        {
            turretController.enemyList.Add(other);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (turretController.enemyList.Contains(other))
        {
            turretController.enemyList.Remove(other);
        }
    }
}
