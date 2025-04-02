using Core.Extensions;
using UnityEngine;

public class EnergyNode : MonoBehaviour
{
    [SerializeField] float range;
    [SerializeField] LayerMask BuildingLayer;
    [SerializeField] GameObject linkObject;
    private ObjectStats objectStats;
    void Start()
    {
        objectStats = GetComponent<ObjectStats>();
        Collider2D[] poweredBuildings = DetectBuildings();
        for (int i = 0; i < poweredBuildings.Length; i++)
        {
            ObjectStats poweredBuildingStats = poweredBuildings[i].gameObject.GetComponent<ObjectStats>();
            if (poweredBuildingStats.usesEnergy == true && poweredBuildingStats.gameObject != this.gameObject && poweredBuildingStats.connectedToEnergyNode == false)
            {
                poweredBuildingStats.connectedToEnergyNode = true;
                GameObject newLink = Instantiate(linkObject);
                newLink.transform.parent = transform;
                newLink.transform.position = transform.position;
                newLink.transform.Lookat2D(poweredBuildings[i].transform.position);
                float linkLength = Vector3.Distance(transform.position, poweredBuildings[i].transform.position);
                newLink.transform.localScale = new Vector2(linkLength, linkObject.transform.localScale.y);
                newLink.transform.Translate(Vector2.right * linkLength / 2);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (objectStats.refreshBuildings)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            Collider2D[] poweredBuildings = DetectBuildings();
            for (int i = 0; i < poweredBuildings.Length; i++)
            {
                ObjectStats poweredBuildingStats = poweredBuildings[i].gameObject.GetComponent<ObjectStats>();
                if (poweredBuildingStats.usesEnergy == true && poweredBuildingStats.gameObject != this.gameObject && poweredBuildingStats.connectedToEnergyNode == false)
                {
                    poweredBuildingStats.connectedToEnergyNode = true;
                    GameObject newLink = Instantiate(linkObject);
                    newLink.transform.parent = transform;
                    newLink.transform.position = transform.position;
                    newLink.transform.Lookat2D(poweredBuildings[i].transform.position);
                    float linkLength = Vector3.Distance(transform.position, poweredBuildings[i].transform.position);
                    newLink.transform.localScale = new Vector2(linkLength, linkObject.transform.localScale.y);
                    newLink.transform.Translate(Vector2.right * linkLength / 2);
                }
            }
            objectStats.refreshBuildings = false;
        }
    }
    private Collider2D[] DetectBuildings()
    {
        return Physics2D.OverlapCircleAll(transform.position, range, BuildingLayer);
    }
}
