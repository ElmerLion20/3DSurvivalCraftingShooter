using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ToolSO")]
public class ToolSO : ScriptableObject {

    public enum Harvestable {
        None,
        Tree,
        Stone,
        Plants,
    }

    [Header("Tool")]
    public string nameString;
    public Transform prefab;
    public Sprite sprite;

    [Header("Stats")]
    public float damage;
    public float swingSpeed;
    public float durability;
    public float reach;
    public Harvestable harvestable;

    [Header("SFX")]
    public GameObject hitParticle;
    public AudioClip hitSound;

    

}
