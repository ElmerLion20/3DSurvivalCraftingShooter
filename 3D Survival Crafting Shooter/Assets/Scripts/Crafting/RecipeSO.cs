using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/RecipeSO")]


public class RecipeSO : ScriptableObject {
    public RecipeInputOutput[] input;
    public RecipeInputOutput output;
    public ToolSO toolSO;

}
