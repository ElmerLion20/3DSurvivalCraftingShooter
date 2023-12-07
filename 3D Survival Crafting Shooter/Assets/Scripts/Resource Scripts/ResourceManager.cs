using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {
    // fix
    public static ResourceManager Instance { get; private set; }

    private Dictionary<ResourceTypeSO, int> resourceTypeAmount;

    private void Awake() {
        Instance = this;

        resourceTypeAmount = new Dictionary<ResourceTypeSO, int>();

        ResourceTypeSOList resourceTypeSOList = Resources.Load<ResourceTypeSOList>(typeof(ResourceTypeSOList).Name);

        foreach (ResourceTypeSO resourceTypeSO in resourceTypeSOList.list) {
            resourceTypeAmount[resourceTypeSO] = 0;
        }
        TestResourceAmount();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Y)) {
            ResourceTypeSOList resourceTypeSOList = Resources.Load<ResourceTypeSOList>(typeof(ResourceTypeSOList).Name);
            AddResource(resourceTypeSOList.list[0], 5);
            TestResourceAmount();
        }
        if (Input.GetKeyDown(KeyCode.U)) {
            ResourceTypeSOList resourceTypeSOList = Resources.Load<ResourceTypeSOList>(typeof(ResourceTypeSOList).Name);
            AddResource(resourceTypeSOList.list[1], 5);
            TestResourceAmount();
        }
        if (Input.GetKeyDown(KeyCode.I)) {
            ResourceTypeSOList resourceTypeSOList = Resources.Load<ResourceTypeSOList>(typeof(ResourceTypeSOList).Name);
            AddResource(resourceTypeSOList.list[2], 5);
            TestResourceAmount();
        }
    }

    private void TestResourceAmount() {
        foreach (ResourceTypeSO resourceTypeSO in resourceTypeAmount.Keys) {
            Debug.Log(resourceTypeSO.nameString + " " + resourceTypeAmount[resourceTypeSO]);
        }
    }

    public void AddResource(ResourceTypeSO resourceTypeSO, int amount) {
        resourceTypeAmount[resourceTypeSO] += amount;
        CraftingMenuUI.Instance.UpdateResourceUI(resourceTypeSO);

        ResourceGainUI.Instance.ShowResourceGain(resourceTypeSO, amount);
    }

    public int GetResourceAmount(ResourceTypeSO resourceTypeSO) {
        return resourceTypeAmount[resourceTypeSO];
    }

}
