using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour {
// hej
    public static CraftingManager Instance { get; private set; }

    private ResourceManager resourceManager;

    private List<ResourceTypeSO> availableResources;

    private void Awake() {
        Instance = this;
    }
    private void Start() {
        resourceManager = ResourceManager.Instance;
    }

    public void CraftRecipe(RecipeSO recipeSO) {
        
        availableResources = new List<ResourceTypeSO>();
        foreach (RecipeInputOutput input in recipeSO.input) {
            if (resourceManager.GetResourceAmount(input.resourceType) >= input.amount) {
                availableResources.Add(input.resourceType);
            } else {
                Debug.Log("Too poor");
                break;
            }
        }

        

        if (availableResources.Count == recipeSO.input.Length) {
            if (recipeSO.output.resourceType == null && recipeSO.toolSO != null) {
                Debug.Log("Crafting tool instead of item");
                ToolManager.Instance.AddNewTool(recipeSO.toolSO);
                foreach (RecipeInputOutput input in recipeSO.input) {
                    resourceManager.AddResource(input.resourceType, -input.amount);
                }
                return;
            }
            Debug.Log("Craft recipe!");
            resourceManager.AddResource(recipeSO.output.resourceType, recipeSO.output.amount);
            foreach (RecipeInputOutput input in recipeSO.input) {
                resourceManager.AddResource(input.resourceType, -input.amount);
            }
        }
    }
   
}
