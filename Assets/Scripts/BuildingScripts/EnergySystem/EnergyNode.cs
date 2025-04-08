using Core.Extensions;
using UnityEngine;

public class EnergyNode : MonoBehaviour
{
    [SerializeField] float range;
    [SerializeField] LayerMask BuildingLayer;
    [SerializeField] GameObject linkObject;
    private ObjectStats objectStats;
    public int energyStored;
    public int energyCapacity;
    public GameObject batteryObject;

    void Start()
    {
        objectStats = GetComponent<ObjectStats>();
        ConnectToEnergyNodes();
    }

    // Update is called once per frame
    void Update()
    {
        if (objectStats.refreshBuildings)
        {
            /*for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }*/ //get this figured out
            ConnectToEnergyNodes();
            objectStats.refreshBuildings = false;
        }
    }
    private Collider2D[] DetectBuildings()
    {
        return Physics2D.OverlapCircleAll(transform.position, range, BuildingLayer);
    }
    private void ConnectToEnergyNodes()
    {
        Collider2D[] poweredBuildings = DetectBuildings();
        int connectedNodeAmount = 0;
        for (int i = 0; i < poweredBuildings.Length; i++)
        {
            ObjectStats poweredBuildingStats = poweredBuildings[i].gameObject.GetComponent<ObjectStats>();
            if (poweredBuildingStats.usesEnergy == true && poweredBuildingStats.gameObject != this.gameObject && poweredBuildingStats.connectedToEnergyNode == false)
            {
                if (poweredBuildings[i].TryGetComponent(out Battery energySource))
                {
                    batteryObject = poweredBuildings[i].gameObject;
                }
                else if (poweredBuildings[i].TryGetComponent(out EnergyNode energyNodeSource) && energyNodeSource.batteryObject != null)
                {
                    batteryObject = energyNodeSource.batteryObject;
                }
                poweredBuildingStats.connectedToEnergyNode = true;
                poweredBuildingStats.connectedEnergyNode = this;
                GameObject newLink = Instantiate(linkObject);
                newLink.transform.parent = transform;
                newLink.transform.position = transform.position;
                newLink.transform.Lookat2D(poweredBuildings[i].transform.position);
                float linkLength = Vector3.Distance(transform.position, poweredBuildings[i].transform.position);
                newLink.transform.localScale = new Vector2(linkLength, linkObject.transform.localScale.y);
                newLink.transform.Translate(Vector2.right * linkLength / 2);
                connectedNodeAmount++;
            }
        }
        if (connectedNodeAmount == 0)
        {
            objectStats.connectedToEnergyNode = false; //this is horrifying i dont know how im gonna add all of this....
        }
    }
}
