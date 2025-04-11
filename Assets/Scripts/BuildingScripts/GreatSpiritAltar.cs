using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GreatSpiritAltar : CoreController
{
    List<GameObject> improvedMines = new List<GameObject>();
    void Start()
    {
        objectStats = GetComponent<ObjectStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (objectStats.refreshBuildings == true)
        {
            Transform[] buildingList = GameObject.Find("Mine").GetComponentsInChildren<Transform>(); //perhaps i should make a empty game object for each building category
            if (buildingList.Length != improvedMines.Count)
            {
                for (int i = 0; i < buildingList.Length; i++)
                {
                    for (int j = 0; j < improvedMines.Count; j++)
                    {
                        if (buildingList[i].gameObject != improvedMines[j])
                        {
                            buildingList[i].gameObject.GetComponent<MineResource>().miningSpeed *= 1.25f;
                            improvedMines.Add(buildingList[i].gameObject);
                        }
                    }
                }
            }
        }
    }
}
