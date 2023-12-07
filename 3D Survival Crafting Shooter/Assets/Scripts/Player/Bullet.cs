using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour {
    [SerializeField] Rigidbody m_rigidbody;

    private WeaponSO activeWeaponSO;

    float m_bulletSpeed;

    public float BulletSpeed { get => m_bulletSpeed; }



    public void Fire(float bulletSpeed) {
        m_bulletSpeed = bulletSpeed;
        m_rigidbody.AddForce(transform.forward * BulletSpeed, ForceMode.Impulse);
    }

    public void SetWeaponSO(WeaponSO weaponSO) {
        activeWeaponSO = weaponSO;
    }

    private void OnCollisionEnter(Collision collision) {
        
        if (collision.gameObject.GetComponent<HealthSystem>() != null) {
            if (collision.gameObject.GetComponent<ThirdPersonShooterController>() == null
            && collision.gameObject.GetComponent<Bullet>() == null) {
                collision.gameObject.GetComponent<HealthSystem>().Damage(activeWeaponSO.damage);
            }
            
        } 

        if (collision.gameObject.GetComponent<Bullet>() == null) {
            Destroy(gameObject);
        }

        
    }
}
