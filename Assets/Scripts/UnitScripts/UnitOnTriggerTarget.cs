using UnityEngine;

public class UnitOnTriggerTarget : MonoBehaviour
{
    private UnitTargetter unitTargetter;
    private void Start()
    {
        unitTargetter = gameObject.GetComponentInParent<UnitTargetter>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((!unitTargetter.isEnemy && other.gameObject.CompareTag("Unit")) || unitTargetter.isEnemy && (other.gameObject.CompareTag("Building") || other.CompareTag("Unit") || other.CompareTag("Player")))
        {
            unitTargetter.targetList.Add(other);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (unitTargetter.targetList.Contains(other))
        {
            unitTargetter.targetList.Remove(other);
        }
    }
}
