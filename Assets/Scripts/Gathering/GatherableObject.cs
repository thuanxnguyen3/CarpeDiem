using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherableObject : MonoBehaviour
{
    public GatherDataSO[] gatherDatas;
    public int hits;
    public ItemSO[] preferredTools;
    public int toolMultiplier = 2;

    private void Update()
    {
        if (hits <= 0)
            Destroy(gameObject);
    }

    public void Gather (ItemSO toolUsed, InventoryManager inventory)
    {
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
