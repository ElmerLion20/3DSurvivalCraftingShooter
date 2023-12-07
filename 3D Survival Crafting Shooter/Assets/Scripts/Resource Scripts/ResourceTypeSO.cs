using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ResourceTypeSO")]
public class ResourceTypeSO : ScriptableObject {

    public string nameString;
    public Sprite sprite;
    public ToolSO.Harvestable harvestabilityLevel;
    public GameObject destroyParticle;

}
