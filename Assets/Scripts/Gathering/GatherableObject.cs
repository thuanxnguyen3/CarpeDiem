using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherableObject : MonoBehaviour
{
    public enum DeathType { Destroy, EnablePhysics}
    public DeathType deathType;

    public GatherDataSO[] gatherDatas;
    public int hits;
    public ItemSO[] preferredTools;
    public int toolMultiplier = 2;

    bool hasDied;

    private void Update()
    {
        if (hits <= 0 && !hasDied)
        {
            if (deathType == DeathType.Destroy)
                Destroy(gameObject);
            else if(deathType == DeathType.EnablePhysics)
            {
                if (GetComponent<Rigidbody>() != null)
                {
                    GetComponent<Rigidbody>().isKinematic = false;
                    GetComponent<Rigidbody>().useGravity = true;

                    GetComponent<Rigidbody>().AddTorque(Vector3.right * 50);
                    Destroy(gameObject, 10f);
                }
                else
                    Destroy(gameObject);
            }

            hasDied = true;
        }
    }

    public void Gather (ItemSO toolUsed, InventoryManager inventory)
    {
        if (hits <= 0)
            return;

        bool usingRightTool = false;

        // check for tool
        if (preferredTools.Length > 0)
        {
            for (int i = 0; i < preferredTools.Length; i++)
            {
                if (preferredTools[i] == toolUsed)
                {
                    usingRightTool = true;
                    break;
                }
            }
        }

        // gather
        int selectedGatherData = Random.Range(0, gatherDatas.Length);

        inventory.AddItem(gatherDatas[selectedGatherData].item, gatherDatas[selectedGatherData].amount);

        hits--;

    }
}
