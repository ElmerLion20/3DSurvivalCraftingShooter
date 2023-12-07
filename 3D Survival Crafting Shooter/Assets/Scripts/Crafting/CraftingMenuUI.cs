using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Runtime.CompilerServices;

public class CraftingMenuUI : MonoBehaviour {
    public static CraftingMenuUI Instance { get; private set; }

    [SerializeField] private List<RecipeSO> recipeSOList;
    [SerializeField] private GameObject craftingRecipeButtonTemplate;
    [SerializeField] private Transform textAddon;
    [SerializeField] private Transform imageAddon;
    [SerializeField] private Transform recipeContainer;
    [SerializeField] private Transform resourceContainer;
    [SerializeField] private Transform resourceTemplate;

    public Dictionary<GameObject, RecipeSO> craftButtonRecipeMap = new Dictionary<GameObject, RecipeSO>();
    private Dictionary<ResourceTypeSO, Transform> resourceTypeTransformUI = new Dictionary<ResourceTypeSO, Transform>();
    private bool craftingOpen;
    private Transform background;

    private void Awake() {
        Instance = this;

        craftingRecipeButtonTemplate.SetActive(false);
        background = transform.GetChild(0);

        foreach (RecipeSO recipeSO in recipeSOList) {
            if (!craftButtonRecipeMap.ContainsValue(recipeSO)) {
                CreateCraftingRecipe(recipeSO);
            }
        }

        AddResourceBar();

    }

    

    private void Start() {
        resourceContainer.gameObject.SetActive(false);
        resourceTemplate.gameObject.SetActive(false);
        recipeContainer.gameObject.SetActive(false);
        background.gameObject.SetActive(false);
        craftingOpen = false;
        
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E) || craftingOpen && Input.GetKeyDown(KeyCode.Escape)) {
            if (!craftingOpen) {
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0f;

                foreach (RecipeSO recipeSO in recipeSOList) {
                    if (!craftButtonRecipeMap.ContainsValue(recipeSO)) {
                        CreateCraftingRecipe(recipeSO);
                    }
                }
                craftingOpen = true;
                recipeContainer.gameObject.SetActive(true);
                background.gameObject.SetActive(true);
                resourceContainer.gameObject.SetActive(true);
            } else if (craftingOpen){
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1f;
                recipeContainer.gameObject.SetActive(false);
                background.gameObject.SetActive(false);
                resourceContainer.gameObject.SetActive(false);
                craftingOpen = false;
                
            }
            

        }
    }

    private void CreateCraftingRecipe(RecipeSO recipe) {
        GameObject craftingButton = Instantiate(craftingRecipeButtonTemplate, recipeContainer);
        Transform itemGrid = craftingButton.transform.GetChild(0).transform.Find("ItemGrid");
        craftingButton.SetActive(true);


        foreach (RecipeInputOutput inputItem in recipe.input) {
            if (inputItem.amount > 1) {
                Transform text = Instantiate(textAddon, itemGrid);
                text.GetComponent<TextMeshProUGUI>().text = inputItem.amount.ToString();
            }

            Transform resourceImage = Instantiate(imageAddon, itemGrid);
            resourceImage.GetComponent<Image>().sprite = inputItem.resourceType.sprite;

            if (recipe.input.Length > 1) {
                Transform plusText = Instantiate(textAddon, itemGrid);
                plusText.GetComponent<TextMeshProUGUI>().text = "+";
            }
        }

        Transform equalsText = Instantiate(textAddon, itemGrid);
        equalsText.GetComponent<TextMeshProUGUI>().text = "=";

        if (recipe.output.resourceType == null) {
            Transform resultToolImage = Instantiate(imageAddon, itemGrid);
            resultToolImage.GetComponent<Image>().sprite = recipe.toolSO.sprite;
        } else {
            if (recipe.output.amount > 1) {
                Transform text = Instantiate(textAddon, itemGrid);
                text.GetComponent<TextMeshProUGUI>().text = recipe.output.amount.ToString();

            }

            Transform resultImage = Instantiate(imageAddon, itemGrid);
            resultImage.GetComponent<Image>().sprite = recipe.output.resourceType.sprite;
        }

        

        craftingButton.transform.GetChild(0).GetComponent<CraftingButton>().Initialize(recipe);

        craftButtonRecipeMap.Add(craftingButton, recipe);

    }

    private void AddResourceBar() {
        foreach (ResourceTypeSO resourceType in Resources.Load<ResourceTypeSOList>(typeof(ResourceTypeSOList).Name).list) {
            Transform resource = Instantiate(resourceTemplate, resourceContainer);
            resource.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = ResourceManager.Instance.GetResourceAmount(resourceType).ToString();
            resource.transform.Find("Image").GetComponent<Image>().sprite = resourceType.sprite;
            resourceTypeTransformUI.Add(resourceType, resource);
        }
    }

    public void UpdateResourceUI(ResourceTypeSO resourceTypeSO) {
        resourceTypeTransformUI[resourceTypeSO].Find("Text").GetComponent<TextMeshProUGUI>().text = ResourceManager.Instance.GetResourceAmount(resourceTypeSO).ToString();
    }

}
