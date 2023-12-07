using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceGainUI : MonoBehaviour {

    public static ResourceGainUI Instance { get; private set; }

    [SerializeField] private Transform resourceGainContainer;
    [SerializeField] private Transform resourceGainTemplate;

    private Dictionary<ResourceTypeSO, ResourceDisplay> resourceDisplays = new Dictionary<ResourceTypeSO, ResourceDisplay>();
    private List<ResourceTypeSO> resourcesToRemove = new List<ResourceTypeSO>();
    private Dictionary<ResourceTypeSO, int> resourceAmounts = new Dictionary<ResourceTypeSO, int>();

    private int maxItemsInContainer = 3;
    private float fadeDuration = 3f;

    private class ResourceDisplay {
        public Transform Transform;
        public Animator Animator;
        public float LastUpdatedTime;
        public float Timer = 0f;
        public bool IsFadingOut;

        public ResourceDisplay(Transform transform, Animator animator) {
            Transform = transform;
            Animator = animator;
            LastUpdatedTime = Time.time;
        }
    }

    private void Awake() {
        Instance = this;
        resourceGainTemplate.gameObject.SetActive(false);
    }

    private void Update() {
        foreach (var resourceType in resourcesToRemove) {
            RemoveResourceDisplay(resourceType);
        }
        resourcesToRemove.Clear();

        foreach (var kvp in resourceDisplays) {
            if (kvp.Value.IsFadingOut) {
                UpdateFadeOut(kvp.Key, kvp.Value);
            }
        }
        RemoveOldestResourceIfNeeded();
    }

    public void ShowResourceGain(ResourceTypeSO resourceTypeSO, int amount) {
        if (resourceDisplays.TryGetValue(resourceTypeSO, out ResourceDisplay display)) {
            display.LastUpdatedTime = Time.time;
            // Update existing display
            resourceAmounts[resourceTypeSO] += amount;
            display.Transform.Find("Text").GetComponent<TextMeshProUGUI>().text = (resourceAmounts[resourceTypeSO] > 0 ? "+" : "") + resourceAmounts[resourceTypeSO].ToString();
            display.Timer = 0f;
            display.IsFadingOut = false;
            display.Animator.SetBool("FadeOut", false);
        } else {
            // Create new display
            if (resourceDisplays.Count >= maxItemsInContainer) {
                StartFadingOutOldestResource();
            }
            resourceAmounts[resourceTypeSO] = amount; // Initialize the amount for new resource
            CreateResourceDisplay(resourceTypeSO, amount);
        }
    }

    private void CreateResourceDisplay(ResourceTypeSO resourceTypeSO, int amount) {
        Debug.Log("Creating Resource Display");
        if (resourceDisplays.ContainsKey(resourceTypeSO)) {
            resourceDisplays[resourceTypeSO].Transform.Find("Text").GetComponent<TextMeshProUGUI>().text = (amount > 0 ? "+" : "") + amount;
            Debug.Log("Already contains: " + resourceTypeSO.nameString);
            return;
        }
        Transform resourceGainTransform = Instantiate(resourceGainTemplate, resourceGainContainer);
        Animator animator = resourceGainTransform.GetComponent<Animator>();

        resourceGainTransform.gameObject.SetActive(true);
        resourceGainTransform.Find("Image").GetComponent<Image>().sprite = resourceTypeSO.sprite;
        resourceGainTransform.Find("Text").GetComponent<TextMeshProUGUI>().text = (amount > 0 ? "+" : "") + amount;

        ResourceDisplay newDisplay = new ResourceDisplay(resourceGainTransform, animator);
        resourceDisplays.Add(resourceTypeSO, newDisplay);
    }

    private void StartFadingOutOldestResource() {
        var firstResourceType = new List<ResourceTypeSO>(resourceDisplays.Keys)[0];
        ResourceDisplay display = resourceDisplays[firstResourceType];
        display.IsFadingOut = true;
        display.Animator.SetBool("FadeOut", true);
        Debug.Log($"Started fading out resource: {firstResourceType.nameString}");
    }

    private void UpdateFadeOut(ResourceTypeSO resourceTypeSO, ResourceDisplay display) {
        display.Timer += Time.deltaTime;
        if (display.Timer >= fadeDuration) {
            display.Animator.SetBool("FadeOut", true);
            if (!display.Animator.GetBool("FadeOut")) {
                display.Animator.SetBool("FadeOut", true);
                Debug.Log($"Setting FadeOut true for {resourceTypeSO.nameString}");
            }
            if (!resourcesToRemove.Contains(resourceTypeSO)) {
                resourcesToRemove.Add(resourceTypeSO);
                Debug.Log($"Added {resourceTypeSO.nameString} to resourcesToRemove");
            }
        }
    }

    private void RemoveResourceDisplay(ResourceTypeSO resourceTypeSO) {
        if (resourceDisplays.TryGetValue(resourceTypeSO, out ResourceDisplay display)) {
            Debug.Log("Removing Resource Display: " + resourceTypeSO.nameString);
            display.Timer = 0f; // Reset the timer
            display.Animator.SetBool("FadeOut", false); // Reset the fade out animation
            Destroy(display.Transform.gameObject);
            resourceDisplays.Remove(resourceTypeSO);
        }
    }

    private void RemoveOldestResourceIfNeeded() {
        ResourceTypeSO oldestResourceType = null;
        float oldestTime = float.MaxValue;

        foreach (var kvp in resourceDisplays) {
            float timeSinceUpdate = Time.time - kvp.Value.LastUpdatedTime;
            if (timeSinceUpdate > 3f && kvp.Value.LastUpdatedTime < oldestTime) {
                oldestTime = kvp.Value.LastUpdatedTime;
                oldestResourceType = kvp.Key;
            }
        }

        if (oldestResourceType != null) {
            RemoveResourceDisplay(oldestResourceType);
        }
    }


    /*public static ResourceGainUI Instance { get; private set; }

    [SerializeField] private Transform resourceGainContainer;
    [SerializeField] private Transform resourceGainTemplate;

    private Dictionary<ResourceTypeSO, int> currentItemsInContainer;
    private Dictionary<ResourceTypeSO, Transform> currentItemsInContainerTransforms;
    private Dictionary<ResourceTypeSO, float> currentResourceTimers = new Dictionary<ResourceTypeSO, float>();
    private Dictionary<ResourceTypeSO, bool> resourceActiveTimer = new Dictionary<ResourceTypeSO, bool>();
    private Dictionary<ResourceTypeSO, Coroutine> fadeOutCoroutines = new Dictionary<ResourceTypeSO, Coroutine>();
    private Dictionary<ResourceTypeSO, Animator> resourceTypeAnimators = new Dictionary<ResourceTypeSO, Animator>();

    private int maxItemsInContainer = 3;
    private Animator animator;

    private float timer;
    private float timerMax = 2f;
    private float deleteResourceTimerMax = 2.5f;

    private void Awake() {
        Instance = this;
        currentItemsInContainerTransforms = new Dictionary<ResourceTypeSO, Transform>();
        currentItemsInContainer = new Dictionary<ResourceTypeSO, int>();

        resourceGainTemplate.gameObject.SetActive(false);
    }

    private void Update() {
        foreach (ResourceTypeSO resourceTypeSO in currentItemsInContainer.Keys) {
            if (resourceActiveTimer[resourceTypeSO]) {
                currentResourceTimers[resourceTypeSO] += Time.deltaTime;
            }
            if (currentResourceTimers[resourceTypeSO] > deleteResourceTimerMax) {
                resourceActiveTimer[resourceTypeSO] = false;
                StartRemoveResourceVisual(resourceTypeSO);
            }
            
        }

        if (currentItemsInContainer.Count > 0) {
            timer += Time.deltaTime;
            if (timer > timerMax) {
                timer = 0f;
            }
        }
    }

    public void ShowResourceGain(ResourceTypeSO resourceTypeSO, int amount) {

        if (resourceTypeAnimators.ContainsKey(resourceTypeSO)) {
            resourceTypeAnimators[resourceTypeSO].SetBool("FadeOut", false);
        }
        if (fadeOutCoroutines.ContainsKey(resourceTypeSO)) {
            StopCoroutine(fadeOutCoroutines[resourceTypeSO]);
        }

        string plusMinusText = " ";
        if (amount > 0) {
            plusMinusText = "+";
        } else if (amount < 0) {
            plusMinusText = "";
        }
        
        if (currentItemsInContainer.Count >= maxItemsInContainer) {
            
        }

        if (currentItemsInContainer.ContainsKey(resourceTypeSO)) {
            currentItemsInContainer[resourceTypeSO] += amount;

            currentItemsInContainerTransforms[resourceTypeSO].Find("Text").GetComponent<TextMeshProUGUI>().text =  plusMinusText + (currentItemsInContainer[resourceTypeSO]).ToString();
            currentResourceTimers[resourceTypeSO] = 0f;
        }

        if (!currentItemsInContainer.ContainsKey(resourceTypeSO)) {
            resourceActiveTimer[resourceTypeSO] = true;
            Transform resourceGainTransform = Instantiate(resourceGainTemplate, resourceGainContainer);
            Animator animator = resourceGainTransform.GetComponent<Animator>();

            resourceGainTransform.Find("Image").GetComponent<Image>().sprite = resourceTypeSO.sprite;
            resourceGainTransform.Find("Text").GetComponent<TextMeshProUGUI>().text = plusMinusText + amount.ToString();
            

            currentItemsInContainer.Add(resourceTypeSO, amount);
            currentItemsInContainerTransforms.Add(resourceTypeSO, resourceGainTransform);
            currentResourceTimers.Add(resourceTypeSO, 0f);
            resourceTypeAnimators.Add(resourceTypeSO, animator);

            resourceGainTransform.gameObject.SetActive(true);
        }

    }

    private void StartRemoveResourceVisual(ResourceTypeSO resourceType) {
        resourceTypeAnimators[resourceType].SetBool("FadeOut", true);

        fadeOutCoroutines[resourceType] = StartCoroutine(WaitForAnimationEnd(currentItemsInContainerTransforms[resourceType], resourceType)); ;
    }

    public void RemoveResourceFromContainer(ResourceTypeSO resourceType) {
        currentItemsInContainer.Remove(resourceType);
        currentItemsInContainerTransforms.Remove(resourceType);
        currentResourceTimers.Remove(resourceType);
        resourceActiveTimer.Remove(resourceType);
        resourceTypeAnimators.Remove(resourceType);
        Destroy(currentItemsInContainerTransforms[resourceType]);
    }

    private IEnumerator WaitForAnimationEnd(Transform resourceGainTransform, ResourceTypeSO resourceType) {
        Animator animator = resourceGainTransform.GetComponent<Animator>();
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);

        RemoveResourceFromContainer(resourceType);
    }*/

}
