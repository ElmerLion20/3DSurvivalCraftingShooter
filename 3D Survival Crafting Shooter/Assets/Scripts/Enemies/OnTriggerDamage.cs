using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerDamage : MonoBehaviour
{
    public GameObject gameobject;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HealthSystem>())
        {
            gameobject = other.gameObject;
            other.GetComponent<HealthSystem>().Damage(10);
        }
    }

}
