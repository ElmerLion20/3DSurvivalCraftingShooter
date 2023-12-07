using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingButton : MonoBehaviour {
    private RecipeSO recipe;

    public void Initialize(RecipeSO recipeSO) {
        recipe = recipeSO;
        Button button = GetComponent<Button>();
        if (button != null) {
            button.onClick.AddListener(CraftRecipe);
        }
    }

    private void CraftRecipe() {
        CraftingManager.Instance.CraftRecipe(recipe);
    }
}

