using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ToolManager : MonoBehaviour {

    public static ToolManager Instance { get; private set; }

    [SerializeField] private List<ToolSO> startingTools;
    [SerializeField] private Transform toolHolder;
    [SerializeField] private ResourceTypeSO metalResourceType;

    private List<ToolSO> activeTools;
    private Dictionary<ToolSO, Transform> activeToolTransform;
    private Dictionary<ToolSO, float> currentToolDurability;

    private int currentToolIndex;
    private ToolSO activeToolSO;
    private Animator animator;
    private bool isUsing;

    private float getItemsTimer;

    private void Awake() {
        Instance = this;
    }

    private void Start() {

        currentToolDurability = new Dictionary<ToolSO, float>();
        activeToolTransform = new Dictionary<ToolSO, Transform>();
        activeTools = new List<ToolSO>();

        getItemsTimer = 0f;
        GameInput.Instance.OnUsePerformed += GameInput_OnUsePerformed;

        foreach (ToolSO toolSO in startingTools) {
            AddNewTool(toolSO);
            Debug.Log("Added: " + toolSO.name + " to tool list");
        }
        SwitchTool(0);
    }

    private void GameInput_OnUsePerformed(object sender, System.EventArgs e) {
        if (!isUsing && activeToolSO != activeTools[0]) {
            isUsing = true;
            animator.SetTrigger("Use");
        } 
    }

    private void Update() {
        
        HandleToolSwitching();

        if (isUsing) {
            getItemsTimer += Time.deltaTime;
            if (getItemsTimer > activeToolSO.swingSpeed) {
                getItemsTimer = 0f;
                isUsing = false;
                Use();
            }
        }

        if (Input.GetKeyDown(KeyCode.V)) {
            RemoveTool(activeTools[1]);
        }
        if (Input.GetKeyDown(KeyCode.B)) {
            AddNewTool(startingTools[2]);
        }
    }

    private void HandleToolSwitching() {
        
            float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
         if (scrollWheel != 0f) {
             int newIndex = currentToolIndex + (scrollWheel > 0f ? 1 : -1);

             newIndex = Mathf.Clamp(newIndex, 0, activeTools.Count - 1);

            SwitchTool(newIndex);
         }

        if (activeTools.Count < 1) { return; }
        if (Input.GetKeyDown(KeyCode.Alpha0)) { SwitchTool(0); }
        if (activeTools.Count < 2) { return; }
        if (Input.GetKeyDown(KeyCode.Alpha1)) { SwitchTool(1); }
        if (activeTools.Count < 3) { return; }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { SwitchTool(2); }
        if (activeTools.Count < 4) { return; }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { SwitchTool(3); }
        if (activeTools.Count < 5) { return; }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { SwitchTool(4); }
        if (activeTools.Count < 6) { return; }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { SwitchTool(5); }
        if (activeTools.Count < 7) { return; }
        if (Input.GetKeyDown(KeyCode.Alpha6)) { SwitchTool(6); }
        if (activeTools.Count < 8) { return; }
        if (Input.GetKeyDown(KeyCode.Alpha7)) { SwitchTool(7); }
        if (activeTools.Count < 9) { return; }
        if (Input.GetKeyDown(KeyCode.Alpha8)) { SwitchTool(8); }

    }

    private void SwitchTool(int newToolIndex) {
        if (activeTools.Count < 1) { return; }
        activeToolTransform[activeTools[currentToolIndex]].gameObject.SetActive(false);

        currentToolIndex = newToolIndex;
        activeToolSO = activeToolTransform[activeTools[currentToolIndex]].transform.GetComponent<ToolTypeHolder>().toolSO;

        animator = activeToolTransform[activeTools[currentToolIndex]].GetComponent<Animator>();
        activeToolTransform[activeTools[currentToolIndex]].gameObject.SetActive(true);
        Debug.Log("Switched to: " + activeToolSO.name);
    }

    private void Use() {
        RaycastHit hitInfo;
        bool objectInfront = Physics.Raycast(activeToolTransform[activeToolSO].position, Camera.main.transform.forward, out hitInfo, activeToolSO.reach);
        if (!objectInfront) { return; }

        HealthSystem targetHealthSystem = hitInfo.transform.GetComponent<HealthSystem>();
        if (targetHealthSystem == null) { return; }

        ResourceTypeHolder resourceHolder = hitInfo.transform.GetComponent<ResourceTypeHolder>();
        

        if (targetHealthSystem != null && resourceHolder == null) {
            
            targetHealthSystem.Damage(activeToolSO.damage);

            currentToolDurability[activeToolSO] -= 1;

            return;
        }

        
        if (resourceHolder != null && targetHealthSystem != null) {
            if (resourceHolder.resourceTypeSO.harvestabilityLevel == activeToolSO.harvestable) {
                ResourceTypeSO targetResourceType = resourceHolder.resourceTypeSO;
                ResourceManager.Instance.AddResource(targetResourceType, 1);
                targetHealthSystem.Damage(1);

                AddMetalChance(resourceHolder);
                if (activeToolSO.hitSound != null) {
                    SoundManager.Instance.PlaySound(activeToolSO.hitSound, hitInfo.point, 0.5f);
                }
                

                if (targetHealthSystem.IsDead()) {
                    Destroy(hitInfo.transform.gameObject);
                    GameObject destroyparticle = Instantiate(targetResourceType.destroyParticle,hitInfo.point,Quaternion.identity);
                    destroyparticle.gameObject.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
                    if(resourceHolder.resourceTypeSO.harvestabilityLevel == ToolSO.Harvestable.Tree)
                    {
                        Destroy(hitInfo.transform.gameObject);
                        GameObject treedestroyparticle = Instantiate(targetResourceType.destroyParticle, hitInfo.point + new Vector3(0f, 5f, -5f), Quaternion.identity);
                        destroyparticle.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                    }
                }

                currentToolDurability[activeToolSO] -= 1;
                GameObject particle = Instantiate(activeToolSO.hitParticle, hitInfo.point, Quaternion.identity);
                Destroy(particle, 1f);
            }
        }

        if (currentToolDurability[activeToolSO] <= 0) {
            RemoveTool(activeToolSO);
        }
        
    }

    public void AddNewTool(ToolSO toolToAdd) {
        if (activeTools.Contains(toolToAdd)) {
            currentToolDurability[toolToAdd] = toolToAdd.durability;
            return; 
        }
        Debug.Log("Adding: " + toolToAdd.name + " to tool list");
        activeTools.Add(toolToAdd);
        currentToolDurability.Add(toolToAdd, toolToAdd.durability);
        Transform newToolTransform = Instantiate(toolToAdd.prefab, toolHolder);
        activeToolTransform.Add(toolToAdd, newToolTransform);
        
        SwitchTool(activeTools.Count - 1);
    }

    public void RemoveTool(ToolSO toolSO) {
        if (activeTools.Count < 1) { return; }
        if (activeToolSO == toolSO) {
            currentToolIndex = currentToolIndex - 1;
            activeToolSO = activeTools[currentToolIndex];
            SwitchTool(currentToolIndex);
        }
        
        activeTools.Remove(toolSO);
        currentToolDurability.Remove(toolSO);
        Destroy(activeToolTransform[toolSO].gameObject);
        activeToolTransform.Remove(toolSO);
    }

    private void AddMetalChance(ResourceTypeHolder targetResourceHolder) {
        if (targetResourceHolder.resourceTypeSO.harvestabilityLevel == ToolSO.Harvestable.Stone) {
            if (Random.Range(0, 5) == 0) {
                ResourceManager.Instance.AddResource(metalResourceType, 1);
            }
        }
    }

    public float GetCurrentToolDurability() {
        return currentToolDurability[activeToolSO];
    }

    public float GetCurrentToolMaxDurability() {
        return activeToolSO.durability;
    }

}
