using UnityEngine;

public class WizardBullet : MonoBehaviour
{
    [SerializeField] GameObject hitImpact;    
    
    private void OnCollisionEnter(Collision collision)
    {
        HealthSystem healthsystem = collision.gameObject.GetComponent<HealthSystem>();
        if (healthsystem != null)
        {
            healthsystem.Damage(15);
            Debug.Log($"hit {collision.gameObject.name}");
            GameObject impact = Instantiate(hitImpact, gameObject.transform.position, Quaternion.identity);
            Destroy(impact, 1f);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Did not hit");
            Destroy(gameObject);
        }
    }
}
